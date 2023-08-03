using Domain.Models;
using FluentAssertions;
using ObjectExtensions;
using static Domain.Functions.Functions;
using static Tests.StateFactory;

namespace Tests.Domain;

public class SeedTests
{
    [Fact]
    public void Seed_Single()
    {
        Seed((1, 2), Empty)
            .Should()
            .BeEquivalentTo(Empty with
            {
                Grid = new[]
                {
                    new[]{ 0, 0, 0 },
                    new[]{ 0, 0, 0 },
                    new[]{ 0, 1, 0 },
                }
            });
    }
    
    [Fact]
    public void Seed_Many()
    {
        var partialSeed = ((int, int) coordinate) => 
            (State state) => Seed(coordinate, state);
        
        Empty
            .Pipe(partialSeed((1, 1)))
            .Pipe(partialSeed((2, 1)))
            .Pipe(partialSeed((0, 2)))
            .Should()
            .BeEquivalentTo(Empty with // TODO: error message is not readable
            {
                Grid = new[]
                {
                    new[]{ 0, 0, 0 },
                    new[]{ 0, 1, 1 },
                    new[]{ 1, 0, 0 },
                }
            });
    }
    
    [Fact]
    public void Reject_Given_OutOfBounds()
    {
        // TODO: try again to use 2d array int[,]
        var act = () => Seed((Empty.Grid.Bounds.MaxX, 0), Empty);
            
        act
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Coordinate { X = 3, Y = 0 } was out of bounds(3, 3) (Parameter 'target')");
    }
}