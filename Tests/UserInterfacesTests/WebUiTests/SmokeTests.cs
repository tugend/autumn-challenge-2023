using FluentAssertions;
using ObjectExtensions;
using Tests.UserInterfacesTests.WebUiTests.Tools;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

public sealed class SmokeTests
{
    [Fact]
    public async Task VisualTests()
    {
        // Startup
        var seed = new
        {
            turn = 0,
            grid = new [] { new [] { 1, 1, 4 },  new [] { 1, 0, 0 },  new [] { 0, 0, 0 } } 
        };
            
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        for (var i = 1; i < 10; i++)
        {
            await client.BenchmarkTurn(i);
        }   
        
        await client
            .ClickCell(0, 0)
            .Benchmark("Should have seeded 2,2");
        
        await client
            .ClickCell(2, 1)
            .Benchmark("Should have killed 2,0");
        
        await client
            .ClickResetButton()
            .Benchmark("Should be reset");
        
        await client
            .ClickPauseButton()
            .Benchmark("Should be running");
        
        await client
            .ClickPauseButton()
            .Benchmark("Should be paused");
    }

    // TODO: control initial state to avoid hardcoded init state
    [Fact]
    public async Task KillLivingCell()
    {
        // Startup
        var seed = new
        {
            turn = 0,
            grid = new [] { new [] { 1 } } 
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        client
            .ClickPauseButton()
            .GetCell(0, 0).Text.Should().Be("1", "Initial state should be alive");

        client
            .ClickCell(0, 0)
            .GetCell(0, 0).Text.Should().Be("0", "Should have killed 0,0");
    }
    
    [Fact]
    public async Task BirthCell() 
    {
        // Startup
        var seed = new
        {
            turn = 0,
            grid = new [] { new [] { 1, 2, 3 },  new [] { 4, 0, 5 } }
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed); // TODO: reuse client to improve performance?

        // Act
        client
            .ClickPauseButton()
            .GetCell(1, 2).Text.Should().Be("0", "Initial state should be dead");

        client
            .ClickCell(1, 2)
            .GetCell(1, 2).Text.Should().Be("1", "Should have birthed a new cell at 2, 1");
    }

    [Fact]
    public async Task TogglePause()
    {
        // Startup
        var seed = new
        {
            turn = 0,
            grid = new [] { new [] { 0 } } 
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        client
            .GetPauseButton().Text.Should().Be("Pause", "Initial state should be running");
            
        client
            .ClickPauseButton()
            .GetPauseButton().Text.Should().Be("Continue", "Click should pause the game");
        
        client
            .ClickPauseButton()
            .GetPauseButton().Text.Should().Be("Pause", "Click should unpause the game");
    }
    
    [Fact]
    public async Task Reset()
    {
        // Startup
        var seed = new
        {
            turn = 3,
            grid = new [] { new [] { 1, 2, 3 },  new [] { 4, 5, 6 } }
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        var initialState = client
            .ClickPauseButton()
            .GetMainAsString();

        var fourthState = await client
            .SkipToTurn(4)
            .Map(x => x.GetMainAsString());

        var resetState = client
            .ClickPauseButton()
            .ClickResetButton()
            .GetMainAsString();

        // Assert
        initialState.Should().Be("""
            Conways Game of Life
            Turn 1
            1
            2
            3
            4
            5
            6

            Continue
            Reset
            """);
        
        initialState.Should().NotBe(fourthState);
        initialState.Should().Be(resetState);
    }
}

file static class WebUiClientExtensions 
{
    public static async Task BenchmarkTurn(this WebUiClient instance, int targetTurn)
    {
        await instance.SkipToTurn(targetTurn);
        await instance.Benchmark($"Turn{targetTurn}");
    }
}