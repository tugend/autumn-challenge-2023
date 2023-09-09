namespace TaskExtensions;

public static class TaskExtensions 
{
    public static async Task<T> Then<T>(this Task<T> instance, Action<T> then)
    {
        var result = await instance;
        then(result);
        return result;
    }
    
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> instance, Func<TIn, TOut> then)
    {
        var result = await instance;
        return then(result);
    }
}