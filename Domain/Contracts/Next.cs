namespace Domain.Models;

/// <summary>
/// Given a state of the game, returns a new state representing the state one turn later.
/// </summary>
public delegate State Next(State state);