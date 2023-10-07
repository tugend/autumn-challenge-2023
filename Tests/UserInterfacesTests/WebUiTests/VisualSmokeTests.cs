using Tests.UserInterfacesTests.WebUiTests.Tools;
using Xunit.Abstractions;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

[Collection(nameof(UICollection))]
public sealed class VisualSmokeTests
{
    private readonly WebUiClient _client;

    public VisualSmokeTests(WebUiTestFixture fixture, ITestOutputHelper outputHelper) => 
        _client = fixture.Inject(outputHelper).Client;

    [Fact]
    public async Task Shotgun()
    {
        // Startup
        var seed = new
        {
            turn = 0,
            grid = new[] { new[] { 1, 1, 4 }, new[] { 1, 0, 0 }, new[] { 0, 0, 0 } }
        };

        using var _ = _client.StartNewConwaysGame(nameof(Shotgun), seed, TimeSpan.FromMilliseconds(800));

        // Act
        for (var i = 1; i < 10; i++)
        {
            await _client.WaitForTurn(i);
            await _client.Benchmark($"turn-{i}");
        }
    }
    
    [Fact]
    public async Task Interactions()
    {
        var seed = new
        {
            turn = 0,
            grid = new[] { 
                new[] { 0, 2, 2 }, 
                new[] { 2, 2, 2 }, 
                new[] { 2, 2, 2 } }
        };
        
        using var _ = _client.StartNewConwaysGame(nameof(Interactions), seed, TimeSpan.FromMilliseconds(800));

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
            .Benchmark(count++ + ".should-be-paused");
    }
}