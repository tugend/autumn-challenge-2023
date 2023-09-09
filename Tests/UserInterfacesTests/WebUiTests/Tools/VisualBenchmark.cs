﻿using Codeuctivity.ImageSharpCompare;
using ObjectExtensions;
using OpenQA.Selenium;

namespace Tests.UserInterfacesTests.WebUiTests.Tools;

public class VisualBenchmark
{
    private readonly string _name;
    private readonly string _actualPath;
    private readonly string _diffPath;
    private readonly string _benchmarkPath;

    private VisualBenchmark(string name, string actualPath, string diffPath, string benchmarkPath)
    {
        _name = name;
        _actualPath = actualPath;
        _diffPath = diffPath;
        _benchmarkPath = benchmarkPath;
    }

    public static VisualBenchmark Init(string name)
    {
        var folder = RelativePaths.VisualBenchmarkPath("UserInterfacesTests", "WebUITests", "Benchmarks");
        var actualPath = Path.Join(folder, $"{name}.actual.png");
        var diffPath = Path.Join(folder, $"{name}.diff.png");
        var benchmarkPath = Path.Join(folder, $"{name}.benchmark.png");

        return new VisualBenchmark(name, actualPath, diffPath, benchmarkPath);
    }
    
    public VisualBenchmark SaveAsBenchmark(Screenshot screenshot)
    {
        screenshot.SaveAsFile(_benchmarkPath, ScreenshotImageFormat.Png);
        return this;
    }
    
    public bool IsEmpty() => 
        !File.Exists(_benchmarkPath);

    public async Task AssertBenchmarkMatches(Screenshot screenshot)
    {
        var benchmarkAsBytes = await File.ReadAllBytesAsync(_benchmarkPath);
        var screenshotAsBytes = screenshot.AsByteArray;
        
        using var expected = Image.Load(benchmarkAsBytes);
        using var actual = Image.Load(screenshotAsBytes);
        var diff = ImageSharpCompare.CalcDiff(actual, expected);

        if (diff.PixelErrorPercentage == 0)
        {
            return;
        }

        await actual
            .Tap(x => x.Mutate(CreateOverlayDiff(benchmarkAsBytes, screenshotAsBytes)))
            .Map(x => actual.SaveAsPngAsync(_diffPath));
        
        screenshot.SaveAsFile(_actualPath, ScreenshotImageFormat.Png);
        
        Assert.Fail($"Expected benchmark {_name} to match! Difference was {double.Round(diff.PixelErrorPercentage, 2)}%");
    }

    private static Action<IImageProcessingContext> CreateOverlayDiff( byte[] benchmark, byte[] screenshot) => context =>
    {
        using var expected = Image.Load(benchmark);
        using var actual = Image.Load(screenshot);
        using var diffMask = ImageSharpCompare.CalcDiffMaskImage(actual, expected);
        context.DrawImage(diffMask, PixelColorBlendingMode.Overlay, PixelAlphaCompositionMode.DestOver, 0.8f);
    };
}