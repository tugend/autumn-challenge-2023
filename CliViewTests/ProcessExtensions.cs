using System.Diagnostics;

namespace CliViewTests;

public static class ProcessExtensions
{
    public static async Task<Process> Enter(this Process instance)
    {
        await instance.StandardInput.WriteLineAsync(Environment.NewLine);
        return instance;
    }
}