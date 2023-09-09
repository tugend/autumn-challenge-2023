namespace ObjectExtensions;

public static partial class ObjectExtensions
{
    public static TOut Map<TIn, TOut>(this TIn instance, Func<TIn, TOut> f) => 
        f(instance);
    
    public static async Task<TOut> Map<TIn, TOut>(this Task<TIn> instance, Func<TIn, TOut> f) => 
        f(await instance);
}