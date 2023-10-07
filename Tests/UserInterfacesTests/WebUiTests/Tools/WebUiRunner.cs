using System.Diagnostics;
using Polly;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public static class WebUiRunner
{
    public static async Task<Process> Start()
    {
        var webUiPath = RelativePaths.WebUiProgramPath();

        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project {webUiPath}",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = true
        })!;
        
        using var client = new HttpClient();
        
        await Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(10), OnRetry)
            .ExecuteAsync(() => client.GetAsync("http://localhost:5089/api/health"));
        
        return process;
    }

    private static void OnRetry(Exception exception, TimeSpan timeSpan, int count, Context context) => 
        Console.WriteLine($"Retry {count} {exception.Message}");
}