using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day11
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day11.txt");

    [AdventOfCode2024(11, 1)]
    public static long RunDay11()
    {
        return CountStones(File.ReadAllText(InputPath), 25);
    }

    public static int CountStones(string input, int changeCount)
    {
        return input.ParseAsLongs().RepeatChangeStones(changeCount).Count();
    }

    public static IEnumerable<long> RepeatChangeStones(this IEnumerable<long> stones, int count)
    {
        for (int i = 0; i < count; i++)
        {
            stones = stones.ChangeStones();
        }

        return stones;
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
