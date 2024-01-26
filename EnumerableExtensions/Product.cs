namespace EnumerableExtensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T3> Product<T1, T2, T3>(this IEnumerable<T1> xs, IEnumerable<T2> ys,
        Func<T1, T2, T3> combine) =>
        from x in xs
        from y in ys
        select combine(x, y);
}