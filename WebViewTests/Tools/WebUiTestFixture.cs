using System.Diagnostics;
using JetBrains.Annotations;
using ObjectExtensions;
using OpenQA.Selenium.Chrome;
using Xunit.Abstractions;

namespace WebViewTests.Tools;



[CollectionDefinition(nameof(UICollection))]
public class UICollection : ICollectionFixture<WebUiTestFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[UsedImplicitly]
public sealed class WebUiTestFixture : IAsyncLifetime
{
    private Process? _process;
    private ChromeDriver? _driver;
    private WebUiClient? _client;

    public WebUiClient Client => 
        _client ?? throw new ApplicationException("Fixture should have been initialized!");
    
    public async Task InitializeAsync()
    {
        try
        {
            Console.WriteLine("Starting web ui runner");
            _process = await WebUiRunner.Start();
            
            Console.WriteLine("Starting chromium runner");
            _driver = await ChromiumRunner.Start();
            _client = new WebUiClient(_driver);
        }
        catch (Exception)
        {
            Dispose(_driver);
            Dispose(_process);
            throw;
        }    
    }

    public WebUiTestFixture Inject(ITestOutputHelper output) => 
        this.Tap(x => x.Client.Inject(output));

    private static void Dispose(ChromeDriver? driver) => 
        driver?.Dispose();

    private static void Dispose(Process? process) 
    {
        process?.Kill();
        process?.Dispose();
    }
    
    public Task DisposeAsync()
    {
        Dispose(_driver);
        Dispose(_process);
        return Task.CompletedTask;
    }
}