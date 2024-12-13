using AdventOfCode.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

public static partial class Day13
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day13.txt");

    [GeneratedRegex("Button [AB]: X\\+(\\d+), Y\\+(\\d+)")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex("Prize: X=(\\d+), Y=(\\d+)")]
    private static partial Regex PrizeRegex();

    [AdventOfCode2024(13, 1)]
    public static long RunPart1()
    {
        return ParseMany(File.ReadLines(InputPath)).GetTotalTokens();
    }

    [AdventOfCode2024(13, 2)]
    public static long RunPart2()
    {
        return ParseMany(File.ReadLines(InputPath))
            .Select(m => m.FixInputs())
            .GetTotalTokens();
    }

    public static long GetTotalTokens(string input)
    {
        return ParseMany(input.SplitLines(removeEmpty: false)).GetTotalTokens();
    }

    public static long GetTotalTokensWithFixedInput(string input)
    {
        return ParseMany(input.SplitLines(removeEmpty: false))
            .Select(m => m.FixInputs())
            .GetTotalTokens();
    }

    public static bool IsWinnable(string input, out long? minCost)
    {
        return Parse(input.SplitLines()).IsWinnable(out minCost);
    }

    public static bool IsWinnableWithFixedInput(string input, out long? minCost)
    {
        return Parse(input.SplitLines()).FixInputs().IsWinnable(out minCost);
    }

    private static long GetTotalTokens(this IEnumerable<Machine> machines)
    {
        return machines.Sum(m => m.IsWinnable(out var x) ? (long)x! : 0);
    }

    private static bool IsWinnable(this Machine m, out long? minCost)
    {
        minCost = null;

        var num = (m.B.X * m.Prize.Y) - (m.B.Y * m.Prize.X);
        var denom = (m.B.X * m.A.Y) - (m.B.Y * m.A.X);
        if (denom == 0)
        {
            throw new InvalidOperationException("Got multiple solutions!");
        }
        if (num % denom != 0)
        {
            return false;
        }
        var a = num / denom;
        if (a < 0)
        {
            return false;
        }

        var bNum = m.Prize.X - (m.A.X * a);
        if (bNum % m.B.X != 0)
        {
            return false;
        }
        var b = bNum / m.B.X;
        if (b < 0)
        {
            return false;
        }

        minCost = (3 * a) + b;
        return true;
    }

    private static IEnumerable<Machine> ParseMany(IEnumerable<string> lines)
    {
        return lines.SplitBlocks().Select(b => Parse(b));
    }

    private static Machine Parse(IEnumerable<string> lines)
    {
        var linesList = lines.ToList();
        if (linesList.Count < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(lines), "Must have 3 lines");
        }

        var aMatch = ButtonRegex().Match(linesList[0]);
        var bMatch = ButtonRegex().Match(linesList[1]);
        var prizeMatch = PrizeRegex().Match(linesList[2]);

        MachinePoint a = new(Int64.Parse(aMatch.Groups[1].Value), Int64.Parse(aMatch.Groups[2].Value));
        MachinePoint b = new(Int64.Parse(bMatch.Groups[1].Value), Int64.Parse(bMatch.Groups[2].Value));
        MachinePoint prize = new(Int64.Parse(prizeMatch.Groups[1].Value), Int64.Parse(prizeMatch.Groups[2].Value));

        return new(prize, a, b);
    }

    private static Machine FixInputs(this Machine m) =>
        m with { Prize = new(m.Prize.X + 10000000000000, m.Prize.Y + 10000000000000) };
}

internal record Machine(MachinePoint Prize, MachinePoint A, MachinePoint B);

internal record MachinePoint(long X, long Y);
