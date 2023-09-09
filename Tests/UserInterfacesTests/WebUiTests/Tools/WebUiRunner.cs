using System.Diagnostics;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public static class WebUiRunner
{
    public static async Task<Process> Start()
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
        
        var webUiPath = RelativePaths.WebUiProgramPath();
        await process.StandardInput.WriteLineAsync($"dotnet run --project {webUiPath}");
        return process;
    }
}