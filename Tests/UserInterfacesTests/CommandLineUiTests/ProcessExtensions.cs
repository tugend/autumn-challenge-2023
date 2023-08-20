using System.Diagnostics;

namespace Tests.UserInterfacesTests.CommandLineUiTests;

public static class ProcessExtensions
{
    public static Task<List<string>> AppendOutputBuffer(this Process instance, TimeSpan readUntil)
    {
        var output = new List<string>();
        instance.OutputDataReceived += (sender, e) =>
        {
            // Prepend line numbers to each line of the output.
            if (e.Data == null) return;
            output.Add(e.Data);
        };

        instance.BeginOutputReadLine();
        return Task.FromResult(output);
    }
    
    public static async Task<Process> Enter(this Process instance)
    {
        await instance.StandardInput.WriteLineAsync(Environment.NewLine);
        return instance;
    }
}