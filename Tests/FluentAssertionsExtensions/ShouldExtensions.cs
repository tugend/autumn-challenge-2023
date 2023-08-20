namespace Tests.FluentAssertionsExtensions;

public static class ShouldExtensions 
{
    public static TwoDimensionalArrayAssertions<T> Should<T>(this T[][] instance)
    {
        return new TwoDimensionalArrayAssertions<T>(instance); 
    } 
}