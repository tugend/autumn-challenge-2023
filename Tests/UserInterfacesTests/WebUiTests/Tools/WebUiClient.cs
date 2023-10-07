using System.Diagnostics;
using System.Reflection;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using ObjectExtensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public sealed class WebUiClient
{
    private readonly ChromeDriver _driver;
    private readonly WebDriverWait _wait ;
    private ITestOutputHelper? _output;

    internal WebUiClient(ChromeDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(100));
    }

    private void PrintLogs()
    {
        _driver
            .Manage().Logs
            .GetLog(LogType.Browser).ToList()
            .ForEach(entry => _output?.WriteLine(entry.ToString()));
    }

    public LogContext StartNewConwaysGame(object? seedObject = null, TimeSpan? turnSpeed = null) =>
        StartNewConwaysGame(null, seedObject, turnSpeed);
    
    public LogContext StartNewConwaysGame(string? context, object? seedObject = null, TimeSpan? turnSpeed = null)
    {
        var seed = EncodeSeed(seedObject);
        var speed = turnSpeed ?? TimeSpan.FromMilliseconds(200);
        
        var id = Guid.NewGuid().ToString();
        var url = $"http://localhost:5089/resources/index.html?id={id}&turn-speed={speed.TotalMilliseconds}#{seed}";
        
        _driver .Navigate().GoToUrl(url);
        _wait.Until(_ => _driver.ExecuteScript("return window.conway.isMainLoopRunning"));
        
        return new LogContext(PrintLogs);
    }

    private static string EncodeSeed(object? seed)
    {
        if (seed == null) return string.Empty;
        var seedAsString = JsonConvert.SerializeObject(seed);
        var seedAsUrlComponent = UrlEncoder.Default.Encode(seedAsString);
        return seedAsUrlComponent;
    }

    public async Task Benchmark(string name)
    {
        var screenshot = _driver.GetScreenshot();
        
        // <start> Bit of hacky reflection here
        var type = _output?.GetType();
        var testMember = type?.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        var test = (XunitTest?) testMember?.GetValue(_output);
        var context = test?.DisplayName ?? throw new ApplicationException("Unknown context!");
        // <end>
        
        var benchmark = VisualBenchmark.Init($"{context}.{name}");
        if (benchmark.IsEmpty()) benchmark.SaveAsBenchmark(screenshot);
        else await benchmark.AssertBenchmarkMatches(screenshot);
    }

    public async Task<int> WaitForTurn(int targetTurn)
    {
        var timer = Stopwatch.StartNew();
        var initialTurn = GetTurnCount();
        while (GetTurnCount() < targetTurn && timer.Elapsed < TimeSpan.FromSeconds(3*(targetTurn - initialTurn)))
        {
            await Task.Delay(50);
        }

        return GetTurnCount();
    }
    
    public int GetTurnCount() => 
        _driver
            .FindElement(By.Id("turn"))
            .FindElement(By.TagName("span"))
            .Text
            .Map(int.Parse);

    public WebUiClient ClickPauseButton()
    {
        GetPauseButton()
            .Click();

        return this;
    }

    public IWebElement GetPauseButton() =>
        _driver.FindElement(By.Id("pause-btn"));
    
    public string GetMainAsString() =>
        _driver
            .FindElement(By.TagName("main"))
            .Text;

    public WebUiClient ClickCell(int flatZeroIndexedCellIndex)
    {
        try
        {
            GetCell(flatZeroIndexedCellIndex).Click();
        }
        catch (StaleElementReferenceException)
        {
            // retry in case we hit a fetch/click in the middle of a rerender
            GetCell(flatZeroIndexedCellIndex).Click();
        }

        return this;
    }

    public IWebElement GetCell(int flatZeroIndexedCellIndex) =>
        _driver
            .FindElement(By.CssSelector($"#state .life:nth-child({flatZeroIndexedCellIndex+1})"));

    public WebUiClient ClickResetButton()
    {
        _driver
            .FindElement(By.Id("reset-btn"))
            .Click();

        return this;
    }

    public void Inject(ITestOutputHelper output) => 
        _output = output;
    
    public class LogContext : IDisposable
    {
        private readonly Action _printLogs;

        public LogContext(Action PrintLogs) => 
            _printLogs = PrintLogs;

        public void Dispose() => 
            _printLogs();
    }
}