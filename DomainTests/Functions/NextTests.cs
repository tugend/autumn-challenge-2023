using FluentAssertions;
using static Domain.Functions;

namespace DomainTests.Functions;

public class NextTests
{
    [Fact]
    public void Zero_Case__Preserves_Empty_Grid()
    {
        Next(Empty)
            .Should()
            .BeEquivalentTo(Empty);
    }

    [Fact]
    public void Cell_Without_Neighbours_Dies()
    {
        var input = Convert("""
            0 0 0
            0 1 0
            0 0 0
            """);

        Next(input)
            .Should()
            .BeEquivalentTo(Empty);
    }
        
    [Fact]
    public void Cells_With_Single_Neighbour_Dies()
    {
        var input = Convert("""
            1 0 0
            1 0 0
            0 0 0
            """);
    
        Next(input)
            .Should()
            .BeEquivalentTo(Empty);
    }
    
    [Fact]
    public void Cells_With_Two_Neighbours_Lives()
    {
        var input = Convert("""
            0 0 1
            0 1 0
            1 0 0
            """);
    
        Next(input)
            .Should()
            .BeEquivalentTo(Convert("""
                0 0 0
                0 2 0
                0 0 0
                """));
    }
    
    [Fact]
    public void Cells_With_Three_Neighbours_Lives()
    {
        var input = Convert("""
            0 0 0
            1 1 0
            1 1 0
            """);
        
        Next(input)
            .Should()
            .BeEquivalentTo(Convert("""
                0 0 0
                2 2 0
                2 2 0
                """));
    }

    private static int[][] Empty => Convert("""
        0 0 0
        0 0 0
        0 0 0
        """);
}