using Domain.Models;
using EnumerableExtensions;

// ReSharper disable once CheckNamespace
namespace Domain;

public static partial class Functions
{
    public static readonly Func<Coordinate, State, State> Seed = (target, state) =>
    {
        IsValidOrThrow(target, state);

        return state with
        {
            Grid = state.Grid
                .Select((row, i) => row.Select((cell, j) => target == (i, j) ? 1 : cell))
                .ToArrays()
        };
    };

    private static void IsValidOrThrow(Coordinate target, State state)
    {
        if (!target.IsNotIn(state.Grid)) return;
        
        var bounds = state.GridBounds;
        throw new ArgumentOutOfRangeException(nameof(target), $"{target} was out of bounds{bounds}");
    }
}