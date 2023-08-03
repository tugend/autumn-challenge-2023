using System.Numerics;

namespace NumberExtensions;

public static partial class NumberExtensions
{
    public static bool InRange<T>(this T target, T minInclusive, T maxInclusive) where T : INumber<T> =>
        (target - minInclusive) * (target - maxInclusive) >= (default(T) ?? throw new InvalidOperationException());
        // minInclusive <= target && target < maxInclusive;
}