using Domain.Models;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<Coordinate, State, State> Seed = (target, state) =>
    {
        if (target.IsNotIn(state.Grid))
        {
            var bounds = state.GridBounds;
            throw new ArgumentOutOfRangeException(nameof(target), $"{target} was out of bounds{bounds}");
        }
        
        return state with
        {
            Grid = state.Grid
                .Select((row, i) => row
                    .Select((cell, j) => target == (i, j) ? 1 : cell)
                    .ToArray())
                .ToArray()
        };    };
}