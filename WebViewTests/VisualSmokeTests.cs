using WebViewTests.Tools;
using Xunit.Abstractions;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace WebViewTests;

[Collection(nameof(ViewCollection))]
public sealed class VisualSmokeTests
{
    private readonly WebViewClient _client;

    public VisualSmokeTests(WebViewTestFixture fixture, ITestOutputHelper outputHelper) => 
        _client = fixture.Inject(outputHelper).Client;

    [Fact]
    public async Task Shotgun()
    {
        // Startup
        var seed = """
            1 1 4
            1 0 0
            0 0 0
            """;

        using var _ = _client.StartNewConwaysGame(seed, TimeSpan.FromMilliseconds(800));

        // Act
        for (var i = 1; i < 10; i++)
        {
            await _client.WaitForTurn(i);
            await _client.Benchmark($"turn-{i}");
        }
    }
    
    [Theory]
    [InlineData("color")]
    [InlineData("binary")]
    public async Task Interactions_Given_Theme(string theme)
    {
        var seed = """
            0 2 2
            2 2 2
            2 2 2
            """;
        
        using var _ = _client.StartNewConwaysGame(seed, TimeSpan.FromMilliseconds(800), theme);

        var count = 0;
        
        await _client
            .ClickPauseButton()
            .Benchmark(count++ + ".initial-state");
        
        await _client
            .ClickCell(0)
            .Benchmark(count++ + ".should-have-seeded-0");

        await _client
            .ClickCell(1)
            .Benchmark(count++ + ".should-have-killed-1");
        
        await _client
            .ClickCell(4)
            .Benchmark(count++ + ".should-have-killed-4");
        
        await _client
            .ClickCell(5)
            .Benchmark(count++ + ".should-have-killed-5");
        
        await _client
            .ClickResetButton()
            .Benchmark(count++ + ".should-be-reset");
        
        await _client
            .ClickPauseButton()
            .Benchmark(count++ + ".should-be-running");
        
        await _client
            .ClickPauseButton()
            .Benchmark(count + ".should-be-paused");
    }
}