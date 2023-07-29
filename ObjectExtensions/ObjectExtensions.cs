namespace ObjectExtensions;

public static class ObjectExtensions
{
    public static TOut Pipe<TIn, TOut>(this TIn target, Func<TIn, TOut> mapper) =>
        mapper(target);
}