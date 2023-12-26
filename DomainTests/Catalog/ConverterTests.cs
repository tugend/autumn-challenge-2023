using Domain.Catalog;
using FluentAssertions;
using ObjectExtensions;

namespace DomainTests.Catalog;

public class ConverterTests
{
    [Fact]
    public void Parse()
    {
        const string input = """
            0 0 0 0
            0 1 1 0
            0 1 1 0
            0 0 0 0
            """;

        var converted = Converter.Convert(input);

        converted.Should().BeEquivalentTo(new[]
        {
            new[] { 0, 0, 0, 0 },
            new[] { 0, 1, 1, 0 },
            new[] { 0, 1, 1, 0 },
            new[] { 0, 0, 0, 0 },
        });
    }
    
    [Fact]
    public void Stringify()
    {
        var input = new[]
        {
            new[] { 0, 0, 0, 0 },
            new[] { 0, 1, 1, 0 },
            new[] { 0, 1, 1, 0 },
            new[] { 0, 0, 0, 0 },
        };

        var converted = Converter.Convert(input);

        converted.Should().BeEquivalentTo("""
            0 0 0 0
            0 1 1 0
            0 1 1 0
            0 0 0 0
            """);
    }
    
    [Fact]
    public void Convert()
    {
        const string input = StillLife.Beehive;

        var twiceConverted = input
            .Map(Converter.Convert)
            .Map(Converter.Convert);

        input.Should().Be(twiceConverted);
    }
}