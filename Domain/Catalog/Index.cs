namespace Domain.Catalog;

public static class Index
{
    public static IEnumerable<KeyValuePair<string, int[][]>> All => Enumerable
        .Empty<KeyValuePair<string, int[][]>>()
        .Concat(StillLife.All)
        .Concat(Oscillators.All)
        .Concat(Spaceships.All);
}