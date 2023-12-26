namespace EnumerableExtensions;

public static partial class EnumerableExtensions
{
    public static T[][] ToArrays<T>(this IEnumerable<IEnumerable<T>> target) =>
        target
            .Select(xs => xs.ToArray()).ToArray();
}