using ObjectExtensions;
using static System.Linq.Enumerable;

namespace Domain.Functions;

public static partial class Functions
{
    public static string Stringify<T>(T[][] input, int indent = 0) => 
        Stringify(input, string.Join("", Repeat("    ", indent)));
    
    private static string Stringify<T>(T[][] input, string indent) => 
        indent + input
            .Select(Join<T>(' '))
            .Pipe(Join<string>(Environment.NewLine + indent));

    private static Func<IEnumerable<T>, string> Join<T>(char separator) => row => 
        string.Join(separator, row);
    
    private static Func<IEnumerable<T>, string> Join<T>(string separator) => row => 
        string.Join(separator, row);
}