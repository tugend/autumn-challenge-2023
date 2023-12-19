namespace Domain.Catalog;

public static class Index
{
    public static IEnumerable<KeyValuePair<string, string>> All => Enumerable
        .Empty<KeyValuePair<string, string>>()
        .Concat(StillLife.All)
        .Concat(Oscillators.All)
        .Concat(Spaceships.All)
        .ToDictionary(x => x.Key, x => x.Value);
}