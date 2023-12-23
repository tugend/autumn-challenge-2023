using EnumerableExtensions;

namespace Domain;

public record Game
{
    public int Turn { get; }
    public int[][] Grid { get; }

    private Game(int turn, int[][] grid)
    {
        Turn = turn;
        Grid = grid;
    }

    public static Game Init(string initialSeed) => 
        new(1, Functions.Parse(initialSeed));
    
    public static Game Init(int turns, int[][] grid) => 
        new(turns, grid);
    
    public IEnumerable<Game> Play()
    {
        var stopCriteria = (Game xs, Game ys) => xs.Grid
            .Flatten()
            .SequenceEqual(ys.Grid.Flatten());

        var next = (Game game) => 
            new Game(game.Turn + 1, Functions.Next(game.Grid).ToArrays());

        return Functions.Play(this, next, stopCriteria);
    }
}