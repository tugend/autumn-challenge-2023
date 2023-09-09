using Domain.Models;
using EnumerableExtensions;

// ReSharper disable once CheckNamespace
namespace Domain;

public static partial class Functions
{
    public static IEnumerable<IEnumerable<int>> Next(int[][] grid) => grid
        .MatrixSelect((coordinate, cell) => AliveNeighbours(coordinate, grid).Count() switch
        {
            2 when cell > 0 => cell+1,
            3 => cell + 1,
            _ => 0
        });

    private static IEnumerable<Coordinate> AliveNeighbours(Coordinate cell, int[][] grid) =>
        from neighbour in NeighboursWithinGrid(cell, grid)
        let value = grid[neighbour.X][neighbour.Y]
        where value > 0
        select neighbour;

    private static IEnumerable<Coordinate> NeighboursWithinGrid(Coordinate cell, int[][] grid) =>
        from delta in AdjacencyMatrix()
        let neighbour = new Coordinate(cell.X + delta.Dx, cell.Y + delta.Dy)
        where neighbour.IsIn(grid)
        select neighbour;
    
    private static IEnumerable<(int Dx, int Dy)> AdjacencyMatrix() =>
        new[] { -1, 0, 1 }
            .Product(new[] { -1, 0, 1 }, (dx, dy) => (dx, dy))
            .Where(delta => delta != (0, 0));
}