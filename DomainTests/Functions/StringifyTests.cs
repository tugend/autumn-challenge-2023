using FluentAssertions;
using static Domain.Functions;

namespace DomainTests.Functions;

public class StringifyTests
{
    [Fact]
    public void Sanity()
    {
        var grid = new[]
        {
            new[]{ 1, 0, 0 },
            new[]{ 0, 2, 0 },
            new[]{ 0, 0, 3 }
        };

        Stringify(grid)
            .Should().Be(
                """
                1 0 0
                0 2 0
                0 0 3
                """);
    }
}