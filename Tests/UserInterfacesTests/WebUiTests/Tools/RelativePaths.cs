using ObjectExtensions;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public static class RelativePaths
{
    public static string VisualBenchmarkPath(params string[] subPath) =>
        TestsRootSrcPath(new [] { "Tests" }.Concat(subPath).ToArray());
    
    public static string WebUiProgramPath() =>
        TestsRootSrcPath("WebView", "WebView.csproj");

    private static string TestsRootSrcPath(params string[] subPath) =>
        Environment
            .CurrentDirectory
            .Split(Path.DirectorySeparatorChar)
            .TakeWhile(x => !x.Equals("Tests"))
            .Concat(subPath)
            .ToArray()
            .Map(Path.Join);
}