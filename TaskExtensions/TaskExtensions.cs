namespace TaskExtensions;

public static class TaskExtensions 
{
    public static async Task<T> MapAsync<T>(this Task<T> instance, Action<T> then)
    {
        var result = await instance;
        then(result);
        return result;
    }
}