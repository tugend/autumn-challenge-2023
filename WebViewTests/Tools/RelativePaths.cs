using ObjectExtensions;

namespace WebViewTests.Tools;

public static class RelativePaths
{
    public static string VisualBenchmarkPath(params string[] subPath) =>
        TestsRootSrcPath(subPath);
    
    public static string WebViewProgramPath => // TODO: target dll included in assembly instead, no reason to double compile
        TestsRootSrcPath("WebView", "WebView.csproj");

    private static string TestsRootSrcPath(params string[] subPath) =>
        Environment
            .CurrentDirectory
            .Split(Path.DirectorySeparatorChar)
            .TakeWhile(x => !x.Equals("WebViewTests"))
            .Concat(subPath)
            .ToArray()
            .Map(Path.Join);
}