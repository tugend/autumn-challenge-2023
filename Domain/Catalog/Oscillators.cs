namespace Domain.Catalog;

public static class Oscillators
{
    public const string Blinker = """
                              0 0 0 0 0
                              0 0 1 0 0
                              0 0 1 0 0
                              0 0 1 0 0
                              0 0 0 0 0
                              """;

    public const string Toad = """
                               0 0 0 1 0 0
                               0 1 0 0 1 0
                               0 1 0 0 1 0
                               0 0 1 0 0 0
                               0 0 0 0 0 0
                               """;
    
    public const string Beacon = """
                               0 0 0 0 0 0
                               0 1 1 0 0 0
                               0 1 0 0 0 0
                               0 0 0 0 1 0
                               0 0 0 1 1 0
                               """;    
    
    public const string Pulsar = """
                                 0 0 0 0 0 1 0 0 0 0 0 1 0 0 0 0 0
                                 0 0 0 0 0 1 0 0 0 0 0 1 0 0 0 0 0
                                 0 0 0 0 0 1 1 0 0 0 1 1 0 0 0 0 0
                                 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                                 0 1 1 1 0 0 1 1 0 1 1 0 0 1 1 1 0
                                 0 0 0 1 0 1 0 1 0 1 0 1 0 1 0 0 0
                                 0 0 0 0 0 1 1 0 0 0 1 1 0 0 0 0 0
                                 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                                 0 0 0 0 0 1 1 0 0 0 1 1 0 0 0 0 0
                                 0 0 0 1 0 1 0 1 0 1 0 1 0 1 0 0 0
                                 0 1 1 1 0 0 1 1 0 1 1 0 0 1 1 1 0
                                 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
                                 0 0 0 0 0 1 1 0 0 0 1 1 0 0 0 0 0
                                 0 0 0 0 0 1 0 0 0 0 0 1 0 0 0 0 0
                                 0 0 0 0 0 1 0 0 0 0 0 1 0 0 0 0 0
                                 """;  
    
    public const string Pentadecathlon = """
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 0 1 1 1 0 0 0 0
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 1 0 0 0 1 0 0 0 
                                  0 0 0 1 0 0 0 1 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 0 1 1 1 0 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 0 1 1 1 0 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 1 0 0 0 1 0 0 0 
                                  0 0 0 1 0 0 0 1 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0 
                                  0 0 0 0 1 1 1 0 0 0 0 
                                  0 0 0 0 0 0 0 0 0 0 0  
                                  0 0 0 0 0 0 0 0 0 0 0  
                                  """;      
    
    public static string Get(string name) =>
        typeof(Oscillators)
            .GetField(name)?
            .GetValue(null) as string ?? throw new InvalidOperationException();
    
    public static IEnumerable<KeyValuePair<string, string>> All =>
        new[]
        {
            nameof(Blinker),
            nameof(Toad),
            nameof(Beacon),
            nameof(Pulsar),
            nameof(Pentadecathlon),
        }.Select(name => KeyValuePair.Create(name, Get(name)));
}