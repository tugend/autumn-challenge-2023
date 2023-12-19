namespace Domain.Catalog;

public class Spaceships
{
    public const string Glider = """
                                 0 0 0 0 0
                                 0 1 0 1 0
                                 0 0 1 1 0
                                 0 0 1 0 0
                                 0 0 0 0 0
                                 """;
    
    public const string LightWeightSpaceship = """
                                 0 0 1 1 1 1 0
                                 0 1 0 0 0 1 0
                                 0 0 0 0 0 1 0
                                 0 1 0 0 1 0 0
                                 0 0 0 0 0 0 0
                                 """;
    
    public const string MiddleWeightSpaceship = """
                                   0 0 1 0 0 0 0
                                   1 0 0 0 1 0 0
                                   0 0 0 0 0 1 0
                                   1 0 0 0 0 1 0
                                   0 1 1 1 1 1 0
                                   0 0 0 0 0 0 0
                                   """;   
    
    public const string HeavyWeightSpaceship = """
                                    0 0 1 1 0 0 0
                                    1 0 0 0 1 0 0
                                    0 0 0 0 0 1 0
                                    1 0 0 0 0 1 0
                                    0 1 1 1 1 1 0
                                    0 0 0 0 0 0 0
                                    """;       

    public static IEnumerable<KeyValuePair<string, string>> All =>
        new[]
        {
            nameof(Glider),
            nameof(LightWeightSpaceship),
            nameof(MiddleWeightSpaceship),
            nameof(HeavyWeightSpaceship),
        }.Select(name => KeyValuePair.Create(name, Get(name)));
    
    public static string Get(string name) =>
        typeof(Oscillators).GetField(name)?.GetValue(null) as string ?? throw new InvalidOperationException();
}