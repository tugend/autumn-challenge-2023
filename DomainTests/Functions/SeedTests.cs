using Domain.Models;
using FluentAssertions;
using ObjectExtensions;
using static Domain.Functions;

namespace DomainTests.Functions;

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
                        new[]{ 0, 0, 1 },
                        new[]{ 0, 0, 0 }
                    }
                });
    }

    [Fact]
    public void Seed_Many()
    {
        var partialSeed = ((int, int) coordinate) => 
            (State state) => Seed(coordinate, state);
        
        Empty
            .Map(partialSeed((1, 1)))
            .Map(partialSeed((2, 1)))
            .Map(partialSeed((0, 2)))
            .Should()
            .BeEquivalentTo(Empty with
            {
                Grid = new[]
                {
                    new[]{ 0, 0, 1 },
                    new[]{ 0, 1, 0 },
                    new[]{ 0, 1, 0 }
                }
            });
    }
    
    [Fact]
    public void Reject_Given_OutOfBounds()
    {
        
        var act = () => Seed((Empty.GridBounds.MaxX, 0), Empty);
            
        act
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Coordinate { X = 3, Y = 0 } was out of bounds(3, 3) (Parameter 'target')");
    }

    private static State Empty => new(0, Parse("""
                                               0 0 0
                                               0 0 0
                                               0 0 0
                                               """));
}