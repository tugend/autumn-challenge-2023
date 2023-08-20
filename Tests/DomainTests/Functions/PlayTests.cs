using FluentAssertions;
using static Domain.Functions;

namespace Tests.DomainTests;

public class PlayTests
{
    [Fact]
    public void Sanity()
    {
        var countingGame = new State(1);
        var next = (State state) => new State(state.Counter + 1);
        var stopWhenLastWasFiveAndNextIsSix = (State cur, State pending) => cur.Counter == 5 && pending.Counter == 6;
        var results = Play(countingGame, next, stopWhenLastWasFiveAndNextIsSix);

        results
            .Should()
            .HaveCount(5)
            .And
            .ContainInConsecutiveOrder(
                new State(1),    
                new State(2),    
                new State(3),    
                new State(4),    
                new State(5));
    }
}

file record State(int Counter);

