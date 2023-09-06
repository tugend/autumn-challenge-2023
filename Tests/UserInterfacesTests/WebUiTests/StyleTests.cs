using System.Diagnostics;
using System.Reflection;
using Codeuctivity.ImageSharpCompare;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Drawing;
using SixLabors.ImageSharp.Processing.Processors.Filters;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebUi;
using static System.IO.Path;
using static SixLabors.ImageSharp.PixelFormats.PixelAlphaCompositionMode;
using static SixLabors.ImageSharp.PixelFormats.PixelColorBlendingMode;
using Size = System.Drawing.Size;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

public class StyleTests
{
    [Fact]
    public async Task SeleniumSanityCheck()
    {
        // Startup
        using var process = await StartWebUi();
        using var driver = StartChromium();
        driver.Navigate().GoToUrl("http://localhost:5089/resources/index.html");

        // Arrange
        // Do nothing
        
        // Act
        var screenshot = driver.GetScreenshot();
        using var actual = Image.Load(screenshot.AsByteArray);
        
        // Assert
        // TODO: custom assertion with side effects
        // var result = Compare(actual, expected, diffFilePath...)
        // Assert....
        var screenshotPath = GetType().Assembly.GetSourceRootPath("UserInterfacesTests", "WebUITests", "StyleTestScreenshots");
        var expectedPath = Join(screenshotPath, "test.png");
        using var expected = Image.Load(await File.ReadAllBytesAsync(expectedPath));

        var calculatedDifference = ImageSharpCompare.CalcDiff(actual, expected);

        if (calculatedDifference.PixelErrorPercentage == 0)
        {
            true.Should().BeTrue("Benchmark image matches");
        }
        
        using var calcDiff = ImageSharpCompare.CalcDiffMaskImage(actual, expected);
        var diffPath = Join(screenshotPath, "diff.png");
        var actualPath = Join(screenshotPath, "actual.png");

        actual.Mutate(x => x.DrawImage(calcDiff, Overlay, DestOver, 0.8f));
        await actual.SaveAsPngAsync(diffPath);
        screenshot.SaveAsFile(actualPath, ScreenshotImageFormat.Png);

    }

    private static ChromeDriver StartChromium()
    {
        var driverOptions = new ChromeOptions().WithArgument("--headless");
        var driver = new ChromeDriver(driverOptions);
        
        driver.Manage().Window.Size = new Size(900, 900);
        
        return driver;
    }

    private async Task<Process> StartWebUi()
    {
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        })!;
        var webUiPath = typeof(Program).Assembly.GetSourceRootPath("WebUi.csproj");
        await process.StandardInput.WriteLineAsync($"dotnet run --project {webUiPath}");
        return process;
    }
}

public static class AssemblyExtensions
{
    public static string GetSourceRootPath(this Assembly assembly, params string[] subPath) =>
        GetDirectoryName(assembly.Location)!
            .Split(DirectorySeparatorChar)
            .TakeWhile(x => x != "bin")
            .Concat(subPath)
            .PathJoin();
}

public static class PathExtensions
{
    public static string PathJoin(this IEnumerable<string> instance) =>
        Join(instance.ToArray());
}

public static class ChromeDriverExtensions 
{
    public static ChromeOptions WithArgument(this ChromeOptions instance, string argument)
    {
        instance.AddArgument(argument);
        return instance;
    }
}