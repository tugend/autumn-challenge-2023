using LanguageExt;
using static LanguageExt.Prelude;

namespace Domain;

/// <summary>
/// Board is a matrix of integers where 0 indicate a dead cell,
/// and for a positive value x, value indicates a live cell that was born x turns ago.
/// </summary>
public record State(int Turns, int[][] Grid);

/// <summary>
/// Given a coordinate (x, y) returns a new state where said cell is set to be newborn. 
/// </summary>
public record Seed(Func<(int X, int Y), State> Apply)
{
    public static implicit operator Seed(Func<(int X, int Y), State> value) => new(value);
}

/// <summary>
/// Given a state of the game, returns a new state representing the state one turn later.
/// </summary>
public record Next(Func<State, State> Apply)
{
    public static implicit operator Next(Func<State, State> value) => new(value);
}

/// <summary>
/// Returns a simple string representation of the board state for debugging purposes.
/// </summary>
public record Stringer(Func<State, string> Stringify, Func<string, State> Parse);

/// <summary>
/// A simple stateful encapsulation of a game
/// </summary>
public class Game
{
    private State _state;
    private readonly Next _next;
    private readonly Seed _seed;
    private readonly Stringer _stringer;

    private Game(Seed seed, Next next, Stringer stringer, State state) 
    {
        _seed = seed;
        _next = next;
        _stringer = stringer;
        _state = state;
    }
    
    public static Func<State, Game> Init(Seed seed, Next next, Stringer stringer) => state => 
        new Game(seed, next, stringer, state);

    public Game Apply((int x, int y) cell)
    {
        _state = _seed.Apply(cell);
        return this;
    }

    public Game Next()
    {
        _state = _next.Apply(_state);
        return this;
    }
    
    public Game Next(State state, int turns) => 
        Enumerable
            .Range(0, turns)
            .Aggregate(this, (_1, _2) => Next());

    public override string ToString()
    {
        return _stringer.Stringify(_state);
    }
}