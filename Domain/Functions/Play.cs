// ReSharper disable once CheckNamespace
namespace Domain;

public static partial class Functions
{
    public static IEnumerable<T> Play<T>(T state, Func<T, T> next, Func<T, T, bool> stopCriteria) where T : class
    {
        var pending = state;
        var current = default(T);
        
        while (current == null || !stopCriteria(current, pending))
        {
            yield return pending;
            current = pending;
            pending = next(pending);
        }
    }
}