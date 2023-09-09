using System.Diagnostics;
using FluentAssertions;
using ObjectExtensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public sealed class WebUiClient : IDisposable
{
    private readonly string _testNamePrefix;
    private readonly ChromeDriver _driver;
    private readonly Process _process;
    private IEnumerable<IDisposable> _disposables => new IDisposable[] { _driver, _process};

    private WebUiClient(string testNamePrefix, ChromeDriver driver, Process process)
    {
        _testNamePrefix = testNamePrefix;
        _driver = driver;
        _process = process;
    }

    public static async Task<WebUiClient> Init(string testNamePrefix)
    {
        Process? process = null;
        ChromeDriver? driver = null;
        
        try
        {
            process = await WebUiRunner.Start();
            driver = ChromiumRunner.Start("http://localhost:5089/resources/index.html");
            return new WebUiClient(testNamePrefix, driver, process);
        }
        catch (Exception)
        {
            process?.Dispose();
            driver?.Dispose();
            throw;
        }
    }

    public async Task Benchmark(string name)
    {
        var screenshot = _driver.GetScreenshot();
        var benchmark = VisualBenchmark.Init(_testNamePrefix + "__" + name);
        if (benchmark.IsEmpty()) benchmark.SaveAsBenchmark(screenshot);
        else await benchmark.AssertBenchmarkMatches(screenshot);
    }

    private RunState GetState()
    {
        var text = _driver
            .FindElement(By.Id("pause-btn"))
            .Text;
        
        return text switch
        {
            "Continue" => RunState.Paused,
            "Pause" => RunState.Running,
            _ => throw new ArgumentOutOfRangeException(text)
        };
    }

    public async Task<WebUiClient> SkipToTurn(int targetTurn)
    {
        if (GetState() is RunState.Paused) ClickPauseButton();
        
        while (GetTurnCount() < targetTurn)
        {
            await Task.Delay(500);
        }
        
        ClickPauseButton();

        GetTurnCount()
            .Should()
            .Be(targetTurn);

        return this;
    }

    public int GetTurnCount() => 
        _driver
            .FindElement(By.Id("turn"))
            .FindElement(By.TagName("span"))
            .Text
            .Map(int.Parse);

    public void ClickPauseButton()
    {
        var pauseBtn = _driver.FindElement(By.Id("pause-btn"));
        pauseBtn.Click();
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    private enum RunState
    {
        Running, Paused
    }
}