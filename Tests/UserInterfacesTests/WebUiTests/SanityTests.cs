using System.Diagnostics;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebUi;

// https://www.benday.com/2021/07/19/asp-net-core-integration-tests-with-selenium-webapplicationfactory/
namespace Tests.UserInterfacesTests.WebUiTests;

public class SanityTests
{
    [Fact]
    public async Task SeleniumSanityCheck()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");
        using var chromeDriver = new ChromeDriver(chromeOptions);

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
        chromeDriver.Navigate().GoToUrl("http://localhost:5089/resources/index.html");

        var mainElm = chromeDriver.FindElement(By.TagName("main")).Text;

        mainElm
            .Should()
            .Be("""
                Conways Game of Life
                turn: 1
                1 2 3

                2 3 4

                3 4 5
                """);
    }
}