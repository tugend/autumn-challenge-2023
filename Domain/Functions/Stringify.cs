using ObjectExtensions;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<int[][], string> Stringify = input =>
        input
            .Select(row => string.Join(' ', row))
            .Pipe(rows => string.Join(Environment.NewLine, rows));
}