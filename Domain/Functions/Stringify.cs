using Domain.Models;
using ObjectExtensions;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<Grid, string> Stringify = input =>
        input
            .Select(rowEntry => rowEntry.Select(x => x.Cell))
            .Select(row => string.Join(' ', row))
            .Pipe(rows => string.Join(Environment.NewLine, rows));
}