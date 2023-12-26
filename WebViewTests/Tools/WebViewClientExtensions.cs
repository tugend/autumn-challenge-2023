using FluentAssertions;

namespace WebViewTests.Tools;

public static class WebViewClientExtensions 
{
    public static async Task<WebViewClient> WaitForTurnOrFail(this WebViewClient instance, int targetTurn)
    {
        var endTurn = await instance.WaitForTurn(targetTurn);
        endTurn.Should().Be(targetTurn);
        return instance;
    }
    
    public static WebViewClient TurnCountShouldBe(this WebViewClient instance, int expected, string? because = null)
    {
        instance
            .GetTurnCount()
            .Should().Be(expected, because);

        return instance;
    }
    
    public static WebViewClient PauseButtonShouldBe(this WebViewClient instance, string expected, string because)
    {
        instance
            .GetPauseButton()
            .Text.Should().Be(expected, because);

        return instance;
    }
}