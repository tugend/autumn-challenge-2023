namespace EnumerableExtensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> target) =>
        target.SelectMany(xs => xs);
}