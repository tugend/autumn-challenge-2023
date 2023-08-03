using Domain.Models;

namespace Domain.Functions;

public static partial class Functions
{
    public static readonly Func<State, State> Next = (state) =>
    {
        return state;
    };
}