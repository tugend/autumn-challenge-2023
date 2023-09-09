using FluentAssertions;
using Tests.FluentAssertionsExtensions;
using static Domain.Functions;

namespace Tests.DomainTests.Functions;

public class ParseTests
{
    [Fact]
    public void Parse_Given_Simple()
    {
        const string repr = """
            1 0 0
            0 2 0
            0 0 3
            """;

        Parse(repr)
            .Should()
            .BeEquivalentTo(new[]
            {
                new[]{ 1, 0, 0 },
                new[]{ 0, 2, 0 },
                new[]{ 0, 0, 3 },
            });
    }
        
    [Fact]
    public void Parse_Given_Extra_Whitespace()
    {
        const string repr = """
               1 0 0
            0 2 0   
                   0 0 3
            """;
        
        Parse(repr)
            .Should()
            .BeEquivalentTo(new[]
            {
                new[]{ 1, 0, 0 },
                new[]{ 0, 2, 0 },
                new[]{ 0, 0, 3 },
            });    
    }

    [Fact]
    public void Rejects_Jagged_Arrays()
    {
        const string repr = """
            1 0
            0 2 0
            0 0 3
            """;
        
        var act = () => Parse(repr);
            
        act
            .Should()
            .Throw<Exception>()
            .WithMessage("Jagged arrays are not allowed!");
    }
    
    [Fact]
    public void Rejects_Invalid_Integers()
    {
        const string repr = """
            1 0 x
            0 2 0
            0 0 3
            """;
        
        var act = () => Parse(repr);

        act
            .Should()
            .Throw<Exception>()
            .WithMessage("The input string 'x' was not in a correct format.");
    }
}