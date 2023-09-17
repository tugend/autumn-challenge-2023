﻿using ObjectExtensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Size = System.Drawing.Size;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

// TODO: maybe better setup? https://stackoverflow.com/questions/18261338/get-chromes-console-log
public static class ChromiumRunner
{
    public static ChromeDriver Start(string uri)
    {
        var options = new ChromeOptions()
            .Tap(x => x.AddArgument("--headless"))
            .Tap(x => x.SetLoggingPreference(LogType.Browser, LogLevel.All));
        
        var driver = new ChromeDriver(options);
        
        driver
            .Manage()
            .Window.Size = new Size(900, 900);
        
        driver
            .Navigate()
            .GoToUrl(uri);
        
        return driver;
    }
}