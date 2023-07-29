using EnumerableExtensions;
using ObjectExtensions;
using static System.Environment;

namespace Domain;

public static class StringerFactory
{
    public static Stringer Init() => new Stringer(Stringify, Parse);

    private static string Stringify(State input) => 
        input
            .Grid
            .Select(Strings.Join<int>(' '))
            .Pipe(Strings.Join<string>(NewLine));
    
    private static State Parse(string input)
    {
        var grid = input
            .Trim()
            .Split(NewLine)
            .Select(line => line
                .Trim()
                .Split(" ")
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        return new State(0, grid);
    }
}