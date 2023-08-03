namespace ObjectExtensions;

public static partial class ObjectExtensions
{
    public static TOut Pipe<TIn, TOut>(this TIn target, Func<TIn, TOut> mapper) =>
        mapper(target);
}