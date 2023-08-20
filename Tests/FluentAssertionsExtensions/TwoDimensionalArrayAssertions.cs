using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using static Domain.Functions;

namespace Tests.FluentAssertionsExtensions;

public class TwoDimensionalArrayAssertions<T> : ReferenceTypeAssertions<T[][], TwoDimensionalArrayAssertions<T>>
{
    public TwoDimensionalArrayAssertions(T[][] subject) : base(subject)
    {
    }

    protected override string Identifier => "T[][]";
    
    public AndConstraint<TwoDimensionalArrayAssertions<T>> BeEquivalentTo(T[][] other, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(s => s.SelectMany(row => row).SequenceEqual(other.SelectMany(row => row)))
            .FailWith($"""
                Expected
                {Stringify(Subject, 1)}
                To Equal
                {Stringify(other, 1)}

                """);
        
        return new AndConstraint<TwoDimensionalArrayAssertions<T>>(this);
    }
}