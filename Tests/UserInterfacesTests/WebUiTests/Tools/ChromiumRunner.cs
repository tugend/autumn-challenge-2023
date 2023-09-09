using ObjectExtensions;
using OpenQA.Selenium.Chrome;
using Size = System.Drawing.Size;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public static class ChromiumRunner
{
    public static ChromeDriver Start(string uri)
    {
        var options = new ChromeOptions()
            .Tap(x => x.AddArgument("--headless"));

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