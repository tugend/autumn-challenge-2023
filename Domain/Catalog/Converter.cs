using ObjectExtensions;

namespace Domain.Catalog;

public static class Converter
{
    public static int[][] Convert(string input) => input
        .Split(Environment.NewLine)
        .Select(line => line.Split(" ").Select(int.Parse).ToArray())
        .ToArray();

    public static string Convert(int[][] input) => input
        .Select(line => line.Select(x => x + ""))
        .Select(Join(" "))
        .Map(Join(Environment.NewLine));

    private static Func<IEnumerable<string>, string> Join(string separator) => lines =>
        string.Join(separator, lines);
}