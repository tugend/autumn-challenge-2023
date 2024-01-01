using System.Diagnostics;
using FluentAssertions;
using Polly;

namespace WebViewTests.Tools;

public static class WebViewRunner
{
    public static async Task<Process> Start()
    {
        var path = RelativePaths.WebViewProgramPath;
        var cd = Environment.CurrentDirectory;
        File.Exists(path).Should().BeTrue(because: $"{path} should exist");

        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "WebView.dll", // .exe?
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
        })!;

        await Task.Delay(300);
        if (process.HasExited)
        {
            throw new Exception("Whoops! Process failed to start!" + await process.StandardError.ReadToEndAsync());
        }

        using var client = new HttpClient();

        try
        {
            // https://stackoverflow.com/questions/55299641/can-i-combine-retry-and-fallback-polly-resilience-policies
            //
            // var fallback = Policy<IList<Value>>
            //     .Handle<TaskCanceledException>()
            //     .FallbackAsync(null as IList<Value>);
            //
            // var retry = Policy<IList<Value>>
            //     .Handle<TaskCanceledException>()
            //     .RetryAsync<IList<Value>>(3);
            //
            // var results = await fallback.WrapAsync(retry)
            //     .ExecuteAsync(() => myRestfulCall());

            // TODO: how to do this idiomatically in Polly?
            await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(10), OnRetry)
                .ExecuteAsync(() => client.GetAsync("http://localhost:5000/api/health"));
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