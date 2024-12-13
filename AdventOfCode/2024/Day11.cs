using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day11
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day11.txt");
    private static readonly Dictionary<long, Dictionary<int, long>> CountCache = [];

    [AdventOfCode2024(11, 1)]
    public static long RunPart1()
    {
        return CountStones(File.ReadAllText(InputPath), 25);
    }

    [AdventOfCode2024(11, 2)]
    public static long RunPart2()
    {
        return CountStones(File.ReadAllText(InputPath), 75);
    }

    public static long CountStones(string input, int changeCount)
    {
        return input.ParseAsLongs().Sum(s => s.CountStones(changeCount));
    }

    private static long CountStones(this long stone, int changeCount)
    {
        if (changeCount == 0)
        {
            return 1;
        }

        if (CountCache.TryGetValue(stone, out var stoneCache) && stoneCache.TryGetValue(changeCount, out var value))
        {
            return value;
        }

        var result = stone
            .ChangeStone()
            .Sum(s => s.CountStones(changeCount - 1));
        CacheCountResult(stone, changeCount, result);
        return result;
    }

    private static void CacheCountResult(long stone, int changeCount, long result)
    {
        if (!CountCache.ContainsKey(stone))
        {
            CountCache[stone] = [];
        }
        CountCache[stone][changeCount] = result;
    }

    public static IEnumerable<long> ChangeStones(this IEnumerable<long> stones)
    {
        return stones.SelectMany(s => s.ChangeStone());
    }

    private static long[] ChangeStone(this long stone)
    {
        if (stone == 0)
        {
            return [1];
        }

        var digits = stone.Digits();
        if (digits % 2 == 0)
        {
            return stone.SplitNum(digits / 2);
        }

        return [stone * 2024];
    }
}
