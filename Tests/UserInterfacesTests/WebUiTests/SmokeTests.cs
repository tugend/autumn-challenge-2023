using Tests.UserInterfacesTests.WebUiTests.Tools;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

public class SmokeTests
{
    [Fact]
    public async Task VisualTests()
    {
        // Startup
        using var client = await WebUiClient.Init(nameof(VisualTests));

        // Act
        for (var i = 1; i < 10; i++)
        {
            await client.BenchmarkTurn(i);
        }    
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