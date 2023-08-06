using Domain.Models;

namespace Domain.Functions;

public static partial class Functions
{
    public static int[][] Next(int[][] grid) => 
        grid.Select((row, x) => row
                .Select((cell, y) => 
                    NeighbourCountOf(new Coordinate(x, y), grid) switch
                    {
                        0 => 0,
                        1 => 0,
                        2 when cell > 0 => cell+1,
                        3 => cell + 1,
                        _ => 0
                    })
            .ToArray())
            .ToArray();

    private static int NeighbourCountOf(Coordinate cell, int[][] grid) =>
        new[] { -1, 0, 1 }
            .SelectMany(deltaX => new[] { -1, 0, 1 }.Select(yDelta => new Coordinate(cell.X + deltaX, cell.Y + yDelta)))
            .Count(neighbour => 
                neighbour.IsIn(grid) 
                && neighbour != cell
                && grid[neighbour.X][neighbour.Y] > 0); // TODO: update coordinate to make lookup easier


}