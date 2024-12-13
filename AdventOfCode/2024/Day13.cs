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

    public static int GetTotalTokens(string input)
    {
        return ParseMany(input.SplitLines(removeEmpty: false)).GetTotalTokens();
    }

    public static bool IsWinnable(string input, out int? minCost)
    {
        return Parse(input.SplitLines()).IsWinnable(out minCost);
    }

    private static int GetTotalTokens(this IEnumerable<Machine> machines)
    {
        return machines.Sum(m => m.IsWinnable(out var x) ? (int)x! : 0);
    }

    private static bool IsWinnable(this Machine m, out int? minCost)
    {
        minCost = null;

        for (int a = 0; a <= 100; a++)
        {
            var num = (m.Prize.X - (m.A.X * a));
            var denom = m.B.X;
            if (num >= 0 && (num % denom == 0))
            {
                var b = num / denom;
                if ((m.A.Y * a) + (m.B.Y * b) == m.Prize.Y)
                {
                    minCost = (3 * a) + b;
                    return true;
                }
            }
        }

        return false;
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

        Point a = new(Int32.Parse(aMatch.Groups[1].Value), Int32.Parse(aMatch.Groups[2].Value));
        Point b = new(Int32.Parse(bMatch.Groups[1].Value), Int32.Parse(bMatch.Groups[2].Value));
        Point prize = new(Int32.Parse(prizeMatch.Groups[1].Value), Int32.Parse(prizeMatch.Groups[2].Value));

        return new(prize, a, b);
    }
}

internal record Machine(Point Prize, Point A, Point B);
