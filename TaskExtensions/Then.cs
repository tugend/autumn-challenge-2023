namespace TaskExtensions;

public static partial class TaskExtensions
{
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
}