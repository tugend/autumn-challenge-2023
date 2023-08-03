using System.Collections;

namespace Domain.Models;

public record Grid(int[][] Cells) : IEnumerable<IEnumerable<(Coordinate Coordinate, int Cell)>>
{
    public (int MaxX, int MaxY) Bounds = (Cells.Length, Cells.Select(x => x.Length).FirstOrDefault(0));
    
    public bool IsInsideOf(Coordinate coordinate) =>
        0 <= coordinate.X && coordinate.X < Cells.Length 
        && 0 <= coordinate.Y && coordinate.Y < Cells.Length;
    
    public bool IsOutsideOf(Coordinate coordinate) =>
        !IsInsideOf(coordinate);
    
    public IEnumerator<IEnumerable<(Coordinate, int)>> GetEnumerator() =>
        Cells
            .Select((row, i) => row.Select((cell, j) => (new Coordinate(j, i), cell)))
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();
    
    public static implicit operator Grid(int[][] cells) => 
        new Grid(cells); 
    
    public static Grid Of(int[][] cells) => new(cells);

}