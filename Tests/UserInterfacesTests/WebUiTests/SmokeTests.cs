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
            grid = new [] { new [] { 1, 1, 1 }, new [] { 1, 0, 0 } }
        };
        
        using var client = await WebUiClient.Init(nameof(VisualTests), seed);

        // Act
        client.GetTurnCount().Should().Be(1);

        client.GetPauseButton().Text.Should().Be("Pause", "initial state should be running");
        await client.TrySkipToTurn(2, expectedTurn: 2);

        client.ClickPauseButton();
        client.GetTurnCount().Should().Be(2);
        client.GetPauseButton().Text.Should().Be("Continue", "click should pause the game");
        await client.TrySkipToTurn(4, expectedTurn: 2);
        
        client.ClickPauseButton();
        client.GetPauseButton().Text.Should().Be("Pause", "click should unpause the game");
        await client.TrySkipToTurn(4, expectedTurn: 4);
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
            .ClickPauseButton()
            .GetMainAsString();

        var fourthState = await client
            .SkipToTurn(4)
            .MapAsync(x => x.GetMainAsString());

        var resetState = client
            .ClickResetButton()
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

                Continue
                Reset
                """)
            .And.NotBe(fourthState)
            .And.Be(resetState);
    }
}

file static class WebUiClientExtensions 
{
    public static async Task BenchmarkTurn(this WebUiClient instance, int targetTurn)
    {
        await instance.TrySkipToTurn(targetTurn);
        await instance.Benchmark($"Turn{targetTurn}");
    }

    public static async Task TrySkipToTurn(this WebUiClient instance, int targetTurn, int expectedTurn)
    {
        var endTurn = await instance.TrySkipToTurn(targetTurn);
        endTurn.Should().Be(expectedTurn);
    }

    public static async Task<WebUiClient> SkipToTurn(this WebUiClient instance, int targetTurn)
    {
        var endTurn = await instance.TrySkipToTurn(targetTurn);
        endTurn.Should().Be(targetTurn);
        return instance;
    }
}