using System.Diagnostics;
using FluentAssertions;
using Polly;

namespace WebViewTests.Tools;

public static class WebViewRunner
{
    public static async Task<Process> Start()
    {
        var path = RelativePaths.WebViewProgramPath;

        File.Exists(path).Should().BeTrue(because: $"{path} should exist");

        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project {path}",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = true
        })!;

        await Task.Delay(300);
        if (process.HasExited)
        {
            throw new Exception("Whoops! Process failed to start!");
        }

        using var client = new HttpClient();

        try
        {
            // TODO: how to do this idiomatically in Polly?
            await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(10), OnRetry)
                .ExecuteAsync(() => client.GetAsync("http://localhost:5087/api/health"));
        }
        catch
        {
            process.Kill();
        }

        return process;
    }

    private static void OnRetry(Exception exception, TimeSpan timeSpan, int count, Context context) => 
        Console.WriteLine($"Retry {count} {exception.Message}");
}