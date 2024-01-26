namespace TaskExtensions;

public static partial class TaskExtensions
{
    public static Task<T> Chain<T>(this T instance) =>
        Task.FromResult(instance);
}