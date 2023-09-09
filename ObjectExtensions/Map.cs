namespace ObjectExtensions;

public static partial class ObjectExtensions
{
    public static TOut Map<TIn, TOut>(this TIn instance, Func<TIn, TOut> f) => 
        f(instance);
    
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> pendingInstance, Func<TIn, TOut> f) => 
        f(await pendingInstance);
}