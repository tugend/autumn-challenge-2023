namespace TaskExtensions;

public static partial class TaskExtensions
{
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> instance, Func<TIn, TOut> then)
    {
        var result = await instance;
        return then(result);
    }
}