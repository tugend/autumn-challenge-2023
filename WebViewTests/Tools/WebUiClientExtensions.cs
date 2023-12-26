using FluentAssertions;

namespace WebViewTests.Tools;

public static class WebUiClientExtensions 
{
    public static async Task<WebUiClient> WaitForTurnOrFail(this WebUiClient instance, int targetTurn)
    {
        var endTurn = await instance.WaitForTurn(targetTurn);
        endTurn.Should().Be(targetTurn);
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