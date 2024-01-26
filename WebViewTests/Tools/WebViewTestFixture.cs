using System.Diagnostics;
using JetBrains.Annotations;
using ObjectExtensions;
using OpenQA.Selenium.Chrome;
using Xunit.Abstractions;

namespace WebViewTests.Tools;

[CollectionDefinition(nameof(ViewCollection))]
public class ViewCollection : ICollectionFixture<WebViewTestFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[UsedImplicitly]
public sealed class WebViewTestFixture : IAsyncLifetime
{
    private const string TargetPort = "5002";
    private static readonly Uri BasePath = new($"http://localhost:{TargetPort}");
    private static readonly Uri GamePath = new(BasePath, "resources/index.html");
    private static readonly Uri TargetHealthEndpoint = new(BasePath, "api/health");

    private Process? _process;
    private ChromeDriver? _driver;
    private WebViewClient? _client;

    public WebViewClient Client =>
        _client ?? throw new ApplicationException("Fixture should have been initialized!");
    
    public async Task InitializeAsync()
    {
        try
        {
            Console.WriteLine("Starting web ui runner");
            _process = await WebViewRunner.Start(BasePath, TargetHealthEndpoint);

            Console.WriteLine("Starting chromium runner");
            _driver = await ChromiumRunner.Start(GamePath);
            _client = new WebViewClient(_driver, GamePath);
        }
        catch (Exception)
        {
            Dispose(_driver);
            Dispose(_process);
            throw;
        }    
    }

    public WebViewTestFixture Inject(ITestOutputHelper output) =>
        this.Tap(x => x.Client.Inject(output));

    private static void Dispose(IDisposable? disposable) =>
        disposable?.Dispose();

    private static void Dispose(Process? process) =>
        process?.Kill(entireProcessTree: true);

    public Task DisposeAsync()
    {
        Dispose(_driver);
        Dispose(_process);
        return Task.CompletedTask;
    }
}