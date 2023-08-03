using Domain.Models;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<string, Grid> Parse = input =>
    {
        var cells = input
            .Trim()
            .Split(Environment.NewLine)
            .Select(row => row
                .Trim()
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        if (cells.DistinctBy(row => row.Length).Count() != 1)
            throw new Exception("Jagged arrays are not allowed!");

        return Grid.Of(cells);
    };
}