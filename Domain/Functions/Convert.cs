using ObjectExtensions;

// ReSharper disable once CheckNamespace
namespace Domain;

public static partial class Functions
{
    public static string Convert(int[][] input) => input
        .Select(Join(' '))
        .Map(Join(Environment.NewLine));

    public static int[][] Convert(string input) => input
        .Trim()
        .Split(Environment.NewLine)
        .Select(line => line.Trim().Split(" ").Select(int.Parse).ToArray())
        .ToArray()
        .Tap(ThrowIfJagged);

    private static Func<IEnumerable<int>, string> Join(char separator) => lines =>
        string.Join(separator, lines);

    private static Func<IEnumerable<string>, string> Join(string separator) => lines =>
        string.Join(separator, lines);

    private static void ThrowIfJagged(int[][] result)
    {
        if (result.DistinctBy(row => row.Length).Count() == 1) return;
        throw new Exception("Jagged arrays are not allowed!");
    }
}