using Domain.Catalog;
using FluentAssertions;
using FluentAssertionsExtensions;
using ObjectExtensions;
using static Domain.Functions;

namespace DomainTests.Functions;

public class ConvertTests
{
    [Fact]
    public void Convert_Given_Array()
    {
        var grid = new[]
        {
            new[]{ 1, 0, 0 },
            new[]{ 0, 2, 0 },
            new[]{ 0, 0, 3 }
        };

        Convert(grid)
            .Should().Be(
                """
                1 0 0
                0 2 0
                0 0 3
                """);
    }

    [Fact]
    public void Convert_Given_String()
    {
        const string repr = """
                            1 0 0
                            0 2 0
                            0 0 3
                            """;

        Convert(repr)
            .Should()
            .BeEquivalentTo(new[]
            {
                new[]{ 1, 0, 0 },
                new[]{ 0, 2, 0 },
                new[]{ 0, 0, 3 }
            });
    }

    [Fact]
    public void Convert_And_Back_Again()
    {
        const string input = StillLife.Beehive;

        var twiceConverted = input
            .Map(Convert)
            .Map(Convert);

        input.Should().Be(twiceConverted);
    }

    [Fact]
    public void Convert_Given_String_With_Extra_Whitespace()
    {
        const string repr = """
               1 0 0
            0 2 0   
                   0 0 3
            """;
        
        Convert(repr)
            .Should()
            .BeEquivalentTo(new[]
            {
                new[]{ 1, 0, 0 },
                new[]{ 0, 2, 0 },
                new[]{ 0, 0, 3 }
            });    
    }

    [Fact]
    public void Rejects_Given_Jagged_Arrays()
    {
        const string repr = """
            1 0
            0 2 0
            0 0 3
            """;
        
        var act = () => Convert(repr);
            
        act
            .Should()
            .Throw<Exception>()
            .WithMessage("Jagged arrays are not allowed!");
    }
    
    [Fact]
    public void Rejects_Given_String_With_Non_Digits()
    {
        const string repr = """
            1 0 x
            0 2 0
            0 0 3
            """;
        
        var act = () => Convert(repr);

        act
            .Should()
            .Throw<Exception>()
            .WithMessage("The input string 'x' was not in a correct format.");
    }
}