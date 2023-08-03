using Domain.Models;
using ObjectExtensions;
using static System.Environment;
using static System.Linq.Enumerable;
using static System.String;
using static Domain.Functions.Functions;

namespace Tests;

public static class StateFactory
{
    public static State Empty => new State(0, Parse("""
        0 0 0
        0 0 0
        0 0 0
        """));
    
    public static State Stable => new State(0, Parse("""
        1 0 0
        1 0 1
        0 0 1
        """));
    
    public static string SomeMatrix(int width, int height) =>
        Range(0, width)
            .Select(_ => Range(0, height).Select(_ => Random.Shared.Next(0, 5)))
            .Select(line => Join(' ', line))
            .Pipe(lines => Join(NewLine, lines));
}