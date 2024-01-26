using ObjectExtensions;

namespace Domain.Catalog;

public static class StillLife
{
    public const string Block = """
                                0 0 0 0
                                0 1 1 0
                                0 1 1 0
                                0 0 0 0
                                """;

    public const string Beehive = """
                                  0 0 0 0 0 0
                                  0 0 1 1 0 0
                                  0 1 0 0 1 0
                                  0 0 1 1 0 0
                                  0 0 0 0 0 0
                                  """;

    public const string Loaf = """
                               0 0 0 0 0 0
                               0 0 1 1 0 0
                               0 1 0 0 1 0
                               0 0 1 0 1 0
                               0 0 0 1 0 0
                               0 0 0 0 0 0
                               """;

    public const string Boat = """
                               0 0 0 0 0
                               0 1 1 0 0
                               0 1 0 1 0
                               0 0 1 0 0
                               0 0 0 0 0
                               """;

    public const string Tub = """
                              0 0 0 0 0
                              0 0 1 0 0
                              0 1 0 1 0
                              0 0 1 0 0
                              0 0 0 0 0
                              """;
    
    public static int[][] Get(string name) =>
        (typeof(StillLife)
            .GetField(name)?
            .GetValue(null) as string ?? throw new InvalidOperationException())
            .Map(Functions.Convert);
    
    public static IEnumerable<KeyValuePair<string, int[][]>> All =>
        new[]
        {
            nameof(Block),
            nameof(Beehive),
            nameof(Loaf),
            nameof(Boat),
            nameof(Tub)
        }.Select(name => KeyValuePair.Create(name, Get(name)));
}