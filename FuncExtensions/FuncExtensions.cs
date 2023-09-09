namespace FuncExtensions;

public static class FuncExtensions
{
    public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> instance, T1 arg1) =>
        arg2 => instance.Invoke(arg1, arg2);
}