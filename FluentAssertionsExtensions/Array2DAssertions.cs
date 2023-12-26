using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using static System.Environment;
using static System.String;

namespace FluentAssertionsExtensions;

public static class Array2DAssertions
{
    public static Array2DAssertions<T> Should<T>(this T[][] instance) =>
        new(instance);
}

public class Array2DAssertions<T> : ReferenceTypeAssertions<T[][], Array2DAssertions<T>>
{
    public Array2DAssertions(T[][] subject) : base(subject)
    {
    }

    protected override string Identifier => "T[][]";

    public void BeEquivalentTo(T[][] other, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(s => s.SelectMany(row => row).SequenceEqual(other.SelectMany(row => row)))
            .FailWith($"""
                       Expected
                       {ToString(Subject)}
                       To Equal
                       {ToString(other)}

                       """);

        new AndConstraint<Array2DAssertions<T>>(this);
    }

    private static string ToString(T[][] matrix) =>
        Join(NewLine + " ", matrix.Select(line => Join(' ', line)));
}