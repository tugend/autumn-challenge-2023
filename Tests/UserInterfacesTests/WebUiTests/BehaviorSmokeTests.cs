using FluentAssertions;
using ObjectExtensions;
using TaskExtensions;
using Tests.UserInterfacesTests.WebUiTests.Tools;
using Xunit.Abstractions;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

[Collection(nameof(UICollection))]
public sealed class BehaviorSmokeTests
{
    private readonly WebUiClient _client;

    public BehaviorSmokeTests(WebUiTestFixture fixture, ITestOutputHelper outputHelper) => _client = fixture.Inject(outputHelper).Client;

    [Fact]
    public void KillLivingCell()
    {
        // Startup
        var seed = "1";
        
        using var _ = _client.StartNewConwaysGame(seed);

        _client.ClickPauseButton();
        
        // Act
        _client
            .GetCell(0).Text.Should().Be("1", "Initial state should be alive");

        _client
            .ClickCell(0)
            .GetCell(0).Text.Should().Be("0", "Should have killed 0,0");
    }
    
    [Fact]
    public void BirthCell() 
    {
        // Startup
        var seed = """
           1 2 3
           4 0 5
           """; 
        
        using var _ = _client.StartNewConwaysGame(seed);
        _client.ClickPauseButton();
        
        // Act
        _client
            .GetCell(4).Text.Should().Be("0", "Initial state should be dead: " + _client.GetMainAsString());

        _client
            .ClickCell(4)
            .GetCell(4).Text.Should().Be("1", "Should have birthed a new cell at 2, 1");
    }

    [Fact]
    public async Task TogglePause()
    {
        // Startup
        var seed = """
           1 1 1
           1 0 0
           """;

        var turnSpeed = TimeSpan.FromMilliseconds(300);
        using var _ = _client.StartNewConwaysGame(seed, turnSpeed);
        
        // Act
        await _client
            .PauseButtonShouldBe("Pause", "initial state should be un-paused")
            .WaitForTurnOrFail(2);

        await _client
            .ClickPauseButton()
            .TurnCountShouldBe(2)
            .PauseButtonShouldBe("Continue", "click should have paused")
            .Tap(_ => Task.Delay(turnSpeed * 2))
            .Then(client => client.TurnCountShouldBe(2));
        
        await _client
            .ClickPauseButton()
            .PauseButtonShouldBe("Pause", "click should have un-paused")
            .WaitForTurnOrFail(4);
    }
    
    [Fact]
    public async Task Reset()
    {
        // Startup
        var seed = """
            0 1
            2 0
            """;
        
        using var _ = _client.StartNewConwaysGame(seed, TimeSpan.FromMilliseconds(300));
        
        // Act
        var initialState = _client
            .GetMainAsString();

        var laterState = await _client
            .WaitForTurnOrFail(2)
            .MapAsync(x => x.GetMainAsString());

        var resetState = _client
            .ClickResetButton()
            .ClickPauseButton()
            .GetMainAsString();

        // Assert
        initialState
            .Should()
            .StartWith("""
                Conways Game of Life
                Turn 1
                0
                1
                2
                0

                Pause
                Reset
                """)
            .And.NotBe(laterState)
            .And.Be(resetState);
    }
}