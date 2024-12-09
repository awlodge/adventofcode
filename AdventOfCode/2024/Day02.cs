namespace AdventOfCode._2024;

using AdventOfCode.Helpers;
using Report = List<int>;

public static class Day02
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day02.txt");

    [AdventOfCode2024(2, 1)]
    public static long RunPart1() => SafeCount(ParseFile(InputPath));

    [AdventOfCode2024(2, 2)]
    public static long RunPart2() => SafeCountWithProblemDampener(ParseFile(InputPath));

    public static int SafeCount(string input) => SafeCount(Parse(input));

    public static int SafeCount(IEnumerable<Report> reports) => reports.Count(r => r.IsSafe());

    public static int SafeCountWithProblemDampener(string input) => SafeCountWithProblemDampener(Parse(input));

    public static int SafeCountWithProblemDampener(IEnumerable<Report> reports) => reports.Count(r => r.IsSafeWithProblemDampener());

    public static bool IsSafe(this Report report, bool enablePd = false)
    {
        if (report.Count <= 1)
        {
            return true;
        }

        if (report[1] == report[0])
        {
            return enablePd && report[1..].IsSafe(enablePd: false);
        }

        bool skipped = false;
        bool increasing = report[1] > report[0];
        var i = 1;
        while (i < report.Count)
        {
            var diff = report[i] - report[i - 1];
            if (!increasing)
            {
                diff = -diff;
            }

            if (diff < 1 || diff > 3)
            {
                if (enablePd && !skipped)
                {
                    if (i <= 2)
                    {
                        // Handle the cases where removing the first or second entry can still work. Eurgh.
                        if (report[1..].IsSafe(enablePd: false))
                        {
                            return true;
                        }
                        var copy = report[1..];
                        copy[0] = report[0];
                        if (copy.IsSafe(enablePd: false))
                        {
                            return true;
                        }
                    }
                    skipped = true;
                    report.RemoveAt(i);
                    continue;
                }
                else
                {
                    return false;
                }
            }

            i++;
        }

        return true;
    }

    public static bool IsSafeWithProblemDampener(this Report report) => report.IsSafe(enablePd: true);

    public static IEnumerable<Report> Parse(string input) => ParseInner(input.SplitLines());

    public static IEnumerable<Report> ParseFile(string path) => ParseInner(File.ReadLines(path));

    private static IEnumerable<Report> ParseInner(IEnumerable<string> lines) => lines
        .Select(line => line
            .ParseAsInts()
            .ToList());
}
