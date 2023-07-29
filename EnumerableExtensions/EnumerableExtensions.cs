namespace EnumerableExtensions;

public static class Strings
{
    public static Func<IEnumerable<T>, string> Join<T>(char separator) => target =>
        string.Join(separator, target);
        
    public static Func<IEnumerable<T>, string> Join<T>(string separator) => target =>
            string.Join(separator, target);
}