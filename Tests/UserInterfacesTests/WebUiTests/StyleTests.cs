using System.Diagnostics;
using Codeuctivity.ImageSharpCompare;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Drawing;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebUi;
using static System.IO.Path;
using Size = System.Drawing.Size;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

public class StyleTests
{
    [Fact]
    public async Task SeleniumSanityCheck()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");
        using var driver = new ChromeDriver(chromeOptions);

        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        })!;
        
        await process.StandardInput.WriteLineAsync("dotnet run --project ..\\..\\..\\..\\WebUi\\WebUi.csproj");
        driver.Navigate().GoToUrl("http://localhost:5089/resources/index.html");
        driver.Manage().Window.Size = new Size(900, 900);
        
        var screenshot = driver.GetScreenshot();
        var assemblyLocation = GetDirectoryName(GetType().Assembly.Location)!;
        var testFolderPath = Join(assemblyLocation.Split(DirectorySeparatorChar).TakeWhile(x => x != "bin").ToArray());
        var filePath = Join(testFolderPath, "UserInterfacesTests", "WebUITests", "StyleTestScreenshots", "test.png");
        var diffPath = Join(testFolderPath, "UserInterfacesTests", "WebUITests", "StyleTestScreenshots", "diff.png");

        using var actual = Image.Load(screenshot.AsByteArray);
        using var expected = Image.Load(await File.ReadAllBytesAsync(filePath));
        using var calcDiff = ImageSharpCompare.CalcDiffMaskImage(actual, expected);
        await using var fileStreamDifferenceMask = File.Create("differenceMask.png");
        
        actual.Mutate(x => x.DrawImage(
            calcDiff, 
            PixelColorBlendingMode.Darken, 
            PixelAlphaCompositionMode.DestAtop, 
            0.5f));
        
        await actual.SaveAsPngAsync(diffPath);

        // screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

    }
}