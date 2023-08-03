using Domain.Contracts;

namespace Domain.Models;

/// <summary>
/// A simple stateful encapsulation of a game
/// </summary>
public class Game
{
    private State _state;
    private readonly Next _next;
    private readonly Stringify _stringify;
    private readonly Seed _seed;

    private Game(Seed seed, Next next, Stringify stringify, State state) 
    {
        _seed = seed;
        _next = next;
        _stringify = stringify;
        _state = state;
    }

    public static Func<State, Game> Init(Seed seed, Stringify stringify, Next next) => state => 
        new Game(seed, next, stringify, state);

    public Game Seed((int x, int y) cell)
    {
        _state = _seed(cell, _state);
        return this;
    }

    public Game Next()
    {
        _state = _next(_state);
        return this;
    }
    
    public Game Next(State state, int turns) => 
        Enumerable
            .Range(0, turns)
            .Aggregate(this, (_1, _2) => Next());

    public override string ToString()
    {
        return _state.ToString();
    }
}