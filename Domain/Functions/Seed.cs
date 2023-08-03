using Domain.Models;
using ObjectExtensions;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<Coordinate, State, State> Seed = (target, state) =>
    {
        if (state.Grid.IsOutsideOf(target))
        {
            throw new ArgumentOutOfRangeException(nameof(target), $"{target} was out of bounds{state.Grid.Bounds}");
        }
        
        return state with
        {
            Grid = state.Grid
                .Select(row => row
                    .Select(entry => target == entry.Coordinate ? 1 : entry.Cell)
                    .ToArray())
                .ToArray()
                .Pipe(Grid.Of)
        };
    };
}