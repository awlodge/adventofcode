namespace AdventOfCode.Helpers;

internal static class NumberHelpers
{
    public static int Digits(this long y)
    {
        int digits = 1;
        while ((y /= 10) != 0)
        {
            digits++;
        }

        return digits;
    }

    public static long DropSuffix(this long x, long y)
    {
        return (long)((x - y) / Math.Pow(10, y.Digits()));
    }

    public static long[] SplitNum(this long x, int pos)
    {
        var exp = Math.Pow(10, pos);
        var suff = (long)(x % exp);
        return [(long)((x - suff) / exp), suff];
    }
}
