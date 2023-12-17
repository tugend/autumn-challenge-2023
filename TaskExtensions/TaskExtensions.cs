namespace TaskExtensions;

public static class TaskExtensions 
{
    public static Task<T> Chain<T>(this T instance) => 
        Task.FromResult(instance);

    public static async Task<T> Then<T>(this Task<T> instance, Func<T, Task> then)
    {
        var result = await instance;
        await then(result);
        return result;
    }
    
    public static async Task Then<T>(this Task<T> instance, Action<T> then)
    {
        var result = await instance;
        then(result);
    }
    
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> instance, Func<TIn, TOut> then)
    {
        var result = await instance;
        return then(result);
    }
}