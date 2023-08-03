namespace Domain.Models;

/// <summary>
/// Board is a matrix of integers where 0 indicate a dead cell,
/// and for a positive value x, value indicates a live cell that was born x turns ago.
/// </summary>
public record State(int Turns, Grid Grid)
{
    public override string ToString() =>
        $"""
        State
        Turns: {Turns}  
        {Functions.Functions.Stringify(Grid)}
        """;
};