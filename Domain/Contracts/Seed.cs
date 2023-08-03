using Domain.Models;

namespace Domain.Contracts;

/// <summary>
/// Given a coordinate (x, y) returns a new state where said cell is set to be newborn. 
/// </summary>
public delegate State Seed((int X, int Y) coordinate, State state);