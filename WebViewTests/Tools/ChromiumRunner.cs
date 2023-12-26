using ObjectExtensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Polly;
using Size = System.Drawing.Size;

namespace WebViewTests.Tools;

public static class ChromiumRunner
{
    public static async Task<ChromeDriver> Start()
    {
        var options = new ChromeOptions()
            .Tap(x => x.AddArgument("--headless"))
            .Tap(x => x.SetLoggingPreference(LogType.Browser, LogLevel.All));
        
        var driver = new ChromeDriver(options);
        
        driver
            .Manage()
            .Window.Size = new Size(900, 900);
        
        await Policy
            .Handle<WebDriverException>()
            .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(10), OnRetry)
            .ExecuteAsync(() => Task.Run(() => driver.Navigate().GoToUrl("http://localhost:5087/resources/index.html")));
        
        return driver;
    }
    
    private static void OnRetry(Exception exception, TimeSpan timeSpan, int count, Context context) => 
        Console.WriteLine($"Retry {count} {exception.Message}");
}