namespace EnumerableExtensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> MatrixSelect<T>(this IEnumerable<IEnumerable<T>> target,
        Func<(int X, int Y), T, T> mapping) =>
        target
            .Select((row, x) => row
                .Select<T, T>((cell, y) => mapping((x, y), cell)));
}