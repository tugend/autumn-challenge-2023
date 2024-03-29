﻿using Domain.Catalog;
using FluentAssertions;
using TaskExtensions;
using WebViewTests.Tools;
using Xunit.Abstractions;

namespace WebViewTests;

[Collection(nameof(ViewCollection))]
public class GameTests
{
    private readonly WebViewClient _client;

    public GameTests(WebViewTestFixture fixture, ITestOutputHelper outputHelper) =>
        _client = fixture.Inject(outputHelper).Client;
    
    [Theory]
    [InlineData(nameof(StillLife.Block))]
    [InlineData(nameof(StillLife.Beehive))]
    [InlineData(nameof(StillLife.Loaf))]
    [InlineData(nameof(StillLife.Boat))]
    [InlineData(nameof(StillLife.Tub))]
    public async Task Still(string name)
    {
        var input = StillLife.Get(name);
        
        var state = await GameAfter(input, targetTurn: 10);

        Domain.Functions.Convert(state).Should().BeEquivalentTo(input, because: $"{name} should be a still life");
    }

    [Theory]
    [InlineData(nameof(Oscillators.Blinker), 2)]
    [InlineData(nameof(Oscillators.Toad), 2)]
    [InlineData(nameof(Oscillators.Beacon), 2)]
    [InlineData(nameof(Oscillators.Pulsar), 3)]
    [InlineData(nameof(Oscillators.Pentadecathlon), 15)]
    public async Task Oscillating(string name, int period)
    {
        var input = Oscillators.Get(name);

        var firstTurn = 1;
        var state = await GameAfter(input, firstTurn, firstTurn + period, firstTurn + period + 1).ToListAsync();

        state[0].Should().Be(state[1], $"because the oscillating period was {period}");
        state[0].Should().NotBe(state[2], $"because the oscillating period was {period}");
    }
    
    private async Task<string> GameAfter(int[][] input, int targetTurn)
    {
        var width = input.First().Length;
        
        using var _ = _client.StartNewConwaysGame(input);

        await _client
            .Chain()
            .Then(x => x.WaitForTurn(targetTurn))
            .Then(x => x.ClickPauseButton());
        
        return _client.GetStateAsString(width: width, padding: 1, onlyOnesAndZeros: true);
    }
    
    private async IAsyncEnumerable<string> GameAfter(int[][] input, params int[] targetTurns)
    {
        var width = input.First().Length;
        
        using var _ = _client.StartNewConwaysGame(input, turnSpeed: TimeSpan.FromMilliseconds(600));

        foreach (var targetTurn in targetTurns)
        {
            var matchedTurn = await _client.WaitForTurn(targetTurn);
            matchedTurn.Should().Be(targetTurn);
            
            _client.ClickPauseButton();
            yield return _client.GetStateAsString(width: width, padding: 1, onlyOnesAndZeros: true);
            _client.ClickPauseButton();
        }
    }
}