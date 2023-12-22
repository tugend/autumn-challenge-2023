using ObjectExtensions;

namespace Domain.Catalog;

public static class Converter
{
    // TODO: replace stringify
    public static string[][] Convert(string input) => input
        .Split(Environment.NewLine)
        .Select(line => line.Split(" ").ToArray())
        .ToArray();

    public static string Convert(string[][] input) => input
        .Select(Join(" "))
        .Map(Join(Environment.NewLine));

    private static Func<IEnumerable<string>, string> Join(string separator) => lines =>
        string.Join(separator, lines);
}