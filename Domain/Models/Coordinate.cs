namespace Domain.Models;

public record Coordinate(int X, int Y)
{
    public static implicit operator Coordinate((int X, int Y) target) => new Coordinate(target.X, target.Y);
};