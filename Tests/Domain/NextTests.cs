using FluentAssertions;
using static Domain.Functions.Functions;
using static Tests.StateFactory;

namespace Tests.Domain;

public class NextTests
{
    [Fact]
    public void Preserve_Empty_State()
    {
        Next(Empty)
            .Should()
            .BeEquivalentTo(Empty);
    }
    
    [Fact]
    public void Preserve_Stable_State()
    {
        Next(Stable)
            .Should()
            .BeEquivalentTo(Stable);
    }
}