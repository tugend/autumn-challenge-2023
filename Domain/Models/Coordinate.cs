namespace Domain.Models;

public record Coordinate(int X, int Y)
{
    public static implicit operator Coordinate((int X, int Y) target) => 
        new(target.X, target.Y);

    public bool IsNotIn(int[][] grid) =>
        !IsIn(grid);
    
    public bool IsIn(int[][] grid) =>
        0 <= X && X < grid.Length && 
        0 <= Y && Y < grid.FirstOrDefault(Array.Empty<int>()).Length;
}