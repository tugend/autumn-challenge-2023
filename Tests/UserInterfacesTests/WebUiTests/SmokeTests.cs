using FluentAssertions;
using ObjectExtensions;
using TaskExtensions;
using Tests.UserInterfacesTests.WebUiTests.Tools;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

public sealed class SmokeTests
{
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

        client.ClickPauseButton();
        
        // Act
        client
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

        client.ClickPauseButton();
        
        // Act
        client
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
            grid = new [] { new [] { 1, 1, 1 }, new [] { 1, 0, 0 } }
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        await client
            .TurnCountShouldBe(1)
            .PauseButtonShouldBe("Pause", "initial state should be un-paused")
            .WaitForTurnOrFail(2);

        await client
            .ClickPauseButton()
            .TurnCountShouldBe(2)
            .PauseButtonShouldBe("Continue", "click should have paused")
            .WaitForTurnOrFail(4, expectedTurn: 2);
        
        await client
            .ClickPauseButton()
            .PauseButtonShouldBe("Pause", "click should have un-paused")
            .WaitForTurnOrFail(4, expectedTurn: 4);
    }
    
    [Fact]
    public async Task Reset()
    {
        // Startup
        var seed = new
        {
            turn = 2, /* UI is 1 indexed but state is 0 indexed */
            grid = new [] { new [] { 1, 2, 3 },  new [] { 4, 5, 6 } }
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        var initialState = client
            .GetMainAsString();

        var fourthState = await client
            .WaitForTurnOrFail(4)
            .MapAsync(x => x.GetMainAsString());

        var resetState = client
            .ClickResetButton()
            .ClickPauseButton()
            .GetMainAsString();

        // Assert
        initialState
            .Should()
            .Be("""
                Conways Game of Life
                Turn 3
                1
                2
                3
                4
                5
                6

                Pause
                Reset
                """)
            .And.NotBe(fourthState)
            .And.Be(resetState);
    }
    
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
}

file static class WebUiClientExtensions 
{
    public static async Task BenchmarkTurn(this WebUiClient instance, int targetTurn)
    {
        await instance.WaitForTurn(targetTurn);
        await instance.Benchmark($"Turn{targetTurn}");
    }

    public static async Task<WebUiClient> WaitForTurnOrFail(this WebUiClient instance, int targetTurn, int? expectedTurn = null)
    {
        var endTurn = await instance.WaitForTurn(targetTurn);
        endTurn.Should().Be(expectedTurn ?? targetTurn);
        return instance;
    }
    
    public static WebUiClient TurnCountShouldBe(this WebUiClient instance, int expected, string? because = null)
    {
        instance
            .GetTurnCount()
            .Should().Be(expected, because);

        return instance;
    }
    
    public static WebUiClient PauseButtonShouldBe(this WebUiClient instance, string expected, string because)
    {
        instance
            .GetPauseButton()
            .Text.Should().Be(expected, because);

        return instance;
    }
}