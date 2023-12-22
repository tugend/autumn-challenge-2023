using ObjectExtensions;

namespace Domain.Catalog;

public static class Spaceships
{
    public const string Glider = """
                                 0 0 0 0 0
                                 0 1 0 1 0
                                 0 0 1 1 0
                                 0 0 1 0 0
                                 """;
    
    public const string LightWeightSpaceship = """
                                 0 0 0 0 0 0 0
                                 0 0 1 1 1 1 0
                                 0 1 0 0 0 1 0
                                 0 0 0 0 0 1 0
                                 0 1 0 0 1 0 0
                                 0 0 0 0 0 0 0
                                 """;
    
    public const string MiddleWeightSpaceship = """
                                   0 0 0 0 0 0 0 0
                                   0 0 0 1 0 0 0 0
                                   0 1 0 0 0 1 0 0
                                   0 0 0 0 0 0 1 0
                                   0 1 0 0 0 0 1 0
                                   0 0 1 1 1 1 1 0
                                   0 0 0 0 0 0 0 0
                                   """;   
    
    public const string HeavyWeightSpaceship = """
                                    0 0 0 0 0 0 0 0
                                    0 0 0 1 1 0 0 0
                                    0 1 0 0 0 1 0 0
                                    0 0 0 0 0 0 1 0
                                    0 1 0 0 0 0 1 0
                                    0 0 1 1 1 1 1 0
                                    0 0 0 0 0 0 0 0
                                    """;

    public static string[][] Get(string name) =>
        (typeof(Spaceships)
            .GetField(name)?
            .GetValue(null) as string ?? throw new InvalidOperationException())
        .Map(Converter.Convert)
        .TopPadColumns("0", 5)
        .Select(row => row
            .LeftPadRow("0", 5)
            .RightPadRow("0", 20)).ToArray()
        .BottomPadColumns("0", 20);
    
    public static IEnumerable<KeyValuePair<string, string[][]>> All =>
        new[]
        {
            nameof(Glider),
            nameof(LightWeightSpaceship),
            nameof(MiddleWeightSpaceship),
            nameof(HeavyWeightSpaceship),
        }
        .Select(name => KeyValuePair.Create(name, Get(name)));
}

file static class Extensions 
{
    public static string[] LeftPadRow(this string[] row, string padding, int count) => 
        Enumerable.Repeat(padding, count).Concat(row).ToArray();
    
    public static string[] RightPadRow(this string[] row, string padding, int count) => 
        row.Concat(Enumerable.Repeat(padding, count)).ToArray();
    
    public static string[][] TopPadColumns(this string[][] columns, string padding, int count)
    {
        var createColumn = () => Enumerable.Repeat(padding, columns.First().Length).ToArray();
        var topPadding = Enumerable.Repeat(0, count).Select(_ => createColumn());
            
        return topPadding.Concat(columns).ToArray();
    }
    
    public static string[][] BottomPadColumns(this string[][] columns, string padding, int count)
    {
        var createColumn = () => Enumerable.Repeat(padding, columns.First().Length).ToArray();
        var bottomPadding = Enumerable.Repeat(0, count).Select(_ => createColumn());
            
        return columns.Concat(bottomPadding).ToArray();
    }
}