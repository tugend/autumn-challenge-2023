using Domain;
using FluentAssertions;
using ObjectExtensions;

namespace Tests.Domain;

public class StringerTests
{
    private readonly Stringer _sut = StringerFactory.Init();

    [Fact]
    public void Parse()
    {
        const string repr = """
            1 0 0
            0 2 0
            0 0 3
            """;

        repr
            .Pipe(_sut.Parse)
            .Should()
            .BeEquivalentTo(new State(
                0, 
                new[]
                {
                    new[]{ 1, 0, 0 },
                    new[]{ 0, 2, 0 },
                    new[]{ 0, 0, 3 },
                }));
    }
    
        
    [Fact]
    public void Parse_Given_Extra_Whitespace()
    {
        const string repr = """
               1 0 0
            0 2 0   
                   0 0 3
            """;
        
        repr
            .Pipe(_sut.Parse)
            .Should()
            .BeEquivalentTo(new State(
                0, 
                new[]
                {
                    new[]{ 1, 0, 0 },
                    new[]{ 0, 2, 0 },
                    new[]{ 0, 0, 3 },
                }));    
    }

    [Fact]
    public void Stringify()
    {
        var state = new State(
            0,
            new[]
            {
                new[]{ 1, 0, 0 },
                new[]{ 0, 2, 0 },
                new[]{ 0, 0, 3 },
            });

        _sut
            .Stringify(state)
            .Should().Be("""
                1 0 0
                0 2 0
                0 0 3
                """);
    }
    
    [Fact]
    public void RoundTrip()
    {
        var repr = Generate(10, 10);

        repr
            .Pipe(_sut.Parse)
            .Pipe(_sut.Stringify)
            .Pipe(_sut.Parse)
            .Pipe(_sut.Stringify)
            .Should().Be(repr);
    }
    
    [Fact]
    public void Rejects_Jagged_Arrays()
    {
        const string repr = """
            1 0
            0 2 0
            0 0 3
            """;
        
        var act = () => _sut.Parse(repr);
            
        act.Should().Throw<Exception>(); // TODO: elaborate
    }
    
    [Fact]
    public void Rejects_Invalid_Integers()
    {
        const string repr = """
            1 0 x
            0 2 0
            0 0 3
            """;
        
        var act = () => _sut.Parse(repr);
            
        act.Should().Throw<Exception>(); // TODO: elaborate
    }

    private string Generate(int columns, int rows) =>
        Enumerable
            .Range(0, columns)
            .Select(_ => Enumerable.Range(0, rows).Select(_ => Random.Shared.Next(0, 5)))
            .Select(line => string.Join(' ', line))
            .Pipe(lines => string.Join(Environment.NewLine, lines));
}