using Domain.Models;
using FluentAssertions;
using ObjectExtensions;
using static System.Environment;
using static System.Linq.Enumerable;
using static Domain.Functions.Functions;

namespace Tests.Domain;

public class NextTests
{
    // TODO: separate state updates with trivial turn increments
    // TODO: group tests on rules

    [Fact]
    public void Zero_Case__Preserves_Empty_Grid()
    {
        Next(Empty)
            .Should()
            .BeEquivalentTo(Empty);
    }
    
    [Fact]
    public void Rule_Of_UnderPopulation__Last_Standing_Dies_Alone()
    {
        var input = Parse("""
            1 0 0
            0 0 0
            0 0 0
            """);
        
        Next(input)
            .Should()
            .BeEquivalentTo(Empty);
    }
    
    [Fact]
    public void Rule_Of_Preservation__Three_Remaining_Are_Stable_Survivors()
    {
        var input = Parse("""
            1 1 0
            1 0 0
            0 0 0
            """);
        
        Next(input)
            .Should()
            .BeEquivalentTo(input);
    }
    
    public static int[][] Empty => Parse("""
        0 0 0
        0 0 0
        0 0 0
        """);
    
    public static string SomeMatrix(int width, int height) =>
        Range(0, width)
            .Select(_ => Range(0, height).Select(_ => Random.Shared.Next(0, 5)))
            .Select(line => string.Join(' ', line))
            .Pipe(lines => string.Join(NewLine, lines));
}