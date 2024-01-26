using System.Diagnostics;

namespace CliViewTests;

public static class ApplicationTestRunner
{
    public static (Func<string> Read, Func<string, TimeSpan, Task> WaitFor) InitOutputBuffer(Process process)
    {
        var output = new List<string>();
        process.OutputDataReceived += (_, e) =>
        {
            // Prepend line numbers to each line of the output.
            if (e.Data == null) return;
            output.Add(e.Data);
        };

        process.BeginOutputReadLine();
        var read = () =>
        {
            var copy = output.ToList();
            output.Clear();
            return string.Join(Environment.NewLine, copy);
        };

        var blockUntil = async (string needle, TimeSpan timeout) =>
        {
            var source = new CancellationTokenSource(timeout);
            while(!source.Token.IsCancellationRequested)
            {
                if (output.Any(line => line.Contains(needle))) return;
                await Task.Delay(200, source.Token);
            }
        };

        return (read, blockUntil);
    }
    
    public static Process StartApplication(Type targetType) =>
        Process.Start(new ProcessStartInfo
        {
            FileName = ProgramPath(targetType),
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        })!;

    private static string ProgramPath(Type targetType)
    {
        var targetAssembly = targetType.Assembly;
        var executableName = targetAssembly.GetName().Name;
        var executablePath = Path.GetDirectoryName(targetAssembly.Location);
        var programToTest = $"{executablePath}{Path.DirectorySeparatorChar}{executableName}.exe";
        return programToTest;
    }
}