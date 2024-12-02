namespace AdventOfCode._2024;

using Report = IList<int>;

public static class Day02
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day02.txt");

    public static int RunPart1() => SafeCount(ParseFile(InputPath));

    public static int SafeCount(string input) => SafeCount(Parse(input));

    public static int SafeCount(IEnumerable<Report> reports) => reports.Count(r => r.IsSafe());

    public static bool IsSafe(this Report report)
    {
        if (report.Count <= 1)
        {
            return true;
        }

        if (report[1] == report[0])
        {
            return false;
        }

        var increasing = report[1] > report[0];
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
                return false;
            }

            i++;
        }

        return true;
    }

    public static IEnumerable<Report> Parse(string input) => ParseInner(input.Split('\n', options: StringSplitOptions.RemoveEmptyEntries));

    public static IEnumerable<Report> ParseFile(string path) => ParseInner(File.ReadLines(path));

    private static IEnumerable<Report> ParseInner(IEnumerable<string> lines) => lines
        .Select(line => line
            .Split(Array.Empty<char>(), options: StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Int32.Parse(x))
            .ToList());
}
