using Domain;
using static Domain.Functions;

const string initialState = """
    0 1 0
    1 0 0 
    0 0 0
    """;

var main = async (Game game) =>
{
    foreach (var round in game.Play())
    {
        Print(round);
        await Task.Delay(1000);
    }
};

Console.WriteLine("Starting ...");
await main(Game.Init(initialState));

Console.WriteLine();
Console.WriteLine("Enter key to quit.");
Console.ReadLine();
Console.WriteLine("Thanks for playing. Goodbye.");
return;

void Print(Game game)
{
    if (Console.IsOutputRedirected) Console.WriteLine();
    else Console.Clear();

    Console.WriteLine("Turns: " + game.Turn);
    Console.WriteLine("Grid");
    Console.WriteLine(Convert(game.Grid));
}

namespace CliView
{
    public class Program { }
}