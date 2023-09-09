namespace EnumerableExtensions;

public static class EnumerableExtensions
{
    public static T[][] ToArrays<T>(this IEnumerable<IEnumerable<T>> target) =>
        target
            .Select(xs => xs.ToArray()).ToArray();
    
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> target) =>
        target.SelectMany(xs => xs);    

    public static IEnumerable<IEnumerable<T>> MatrixSelect<T>(this IEnumerable<IEnumerable<T>> target, Func<(int X, int Y), T, T> mapping) => 
        target
            .Select((row, x) => row 
                .Select((cell, y) => mapping((x, y), cell)));
    
    public static IEnumerable<T3> Product<T1, T2, T3>(this IEnumerable<T1> xs, IEnumerable<T2> ys, Func<T1, T2, T3> combine) =>
        from x in xs
        from y in ys
        select combine(x, y);
}