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

namespace WebViewTests.Tools;

public sealed class WebViewClient
{
    private readonly ChromeDriver _driver;
    private readonly Uri _gamePath;
    private readonly WebDriverWait _wait ;
    private ITestOutputHelper? _output;

    internal WebViewClient(ChromeDriver driver, Uri gamePath)
    {
        _driver = driver;
        _gamePath = gamePath;
        _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(100));
    }

    private void PrintLogs()
    {
        _driver
            .Manage().Logs
            .GetLog(LogType.Browser).ToList()
            .ForEach(entry => _output?.WriteLine(entry.ToString()));
    }

    public LogContext StartNewConwaysGame(string input, TimeSpan? turnSpeed = null, string? theme = "color") =>
        StartNewConwaysGame(Domain.Functions.Convert(input), turnSpeed, theme);

    
    public LogContext StartNewConwaysGame(int[][] input, TimeSpan? turnSpeed = null, string? theme = "color")
    {
        if (!new[] { "color", "binary" }.Contains(theme))
        {
            throw new ArgumentException("Unknown theme " + theme, nameof(theme));
        }

        var seed = EncodeSeed(new { key = "Custom", value = input });
        var speed = turnSpeed ?? TimeSpan.FromMilliseconds(400);
        
        var id = Guid.NewGuid().ToString();
        var url = $"{_gamePath}?id={id}&turn-speed={speed.TotalMilliseconds}&seed={seed}&theme={theme}";
        
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

    public WebViewClient ClickPauseButton()
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

    private static string OnlyOneOrZero(string value) =>
        int.Parse(value)
            .Map(x => Math.Min(x, 1).ToString());

    public string GetStateAsString(int width, int padding, bool onlyOnesAndZeros) =>
        GetMainAsString()
            .Split(Environment.NewLine)
            .Skip(2)
            .TakeWhile(x => int.TryParse(x, out _))
            .Select(x => onlyOnesAndZeros ? OnlyOneOrZero(x) : x)
            .Chunk(width)
            .Select(xs => xs.Select(x => x.PadLeft(padding, ' ')))
            .Select(xs => string.Join(" ", xs))
            .Map(xs => string.Join(Environment.NewLine, xs));

    public WebViewClient ClickCell(int flatZeroIndexedCellIndex)
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
            .FindElement(By.CssSelector($"#state > .cell:nth-child({flatZeroIndexedCellIndex+1})"));

    public WebViewClient ClickResetButton()
    {
        _driver
            .FindElement(By.Id("reset-btn"))
            .Click();

        return this;
    }

    public void Inject(ITestOutputHelper output) => 
        _output = output;
    
    public sealed class LogContext : IDisposable
    {
        private readonly Action _printLogs;

        public LogContext(Action PrintLogs) => 
            _printLogs = PrintLogs;

        public void Dispose() => 
            _printLogs();
    }
}