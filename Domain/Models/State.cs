namespace Domain.Models;

/// <summary>
/// Board is a matrix of integers where 0 indicate a dead cell,
/// and for a positive value x, value indicates a live cell that was born x turns ago.
/// </summary>
public record State(int Turns, int[][] Grid)
{
    public override string ToString() =>
        $"""
        State
        Turns: {Turns}  
        {Functions.Stringify(Grid)}
        """;
    
    public (int MaxX, int MaxY) GridBounds = 
        (Grid.Length, Grid.Select(x => x.Length).FirstOrDefault(0));
}