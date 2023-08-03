using Domain.Models;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<int[][], int[][]> Next = grid => 
        grid.Select(row => row.Select(cell => 0).ToArray()).ToArray();
    
    // TODO: grid -> neighbour-count-grid
}