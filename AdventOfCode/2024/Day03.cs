using AdventOfCode.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

public static partial class Day03
{
    [GeneratedRegex("mul\\((\\d+),(\\d+)\\)")]
    private static partial Regex UncorruptedProgramRegex();

    [GeneratedRegex("(mul|do|don't)\\(((\\d+),(\\d+))?\\)")]
    private static partial Regex UncorruptedProgramWithConditionalsRegex();

    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day03.txt");

    [AdventOfCode2024(3, 1)]
    public static long RunPart1()
    {
        var reader = new StreamReader(InputPath);
        return ExtractUncorrupted(reader.ReadToEnd());
    }

    [AdventOfCode2024(3, 2)]

    public static long RunPart2()
    {
        var reader = new StreamReader(InputPath);
        return ExtractUncorruptedWithConditionals(reader.ReadToEnd());
    }

    public static int ExtractUncorrupted(string input)
    {
        var matches = UncorruptedProgramRegex().Matches(input);

        return matches.Sum(m => m.Groups.ExecuteGroup());
    }

    private static int ExecuteGroup(this GroupCollection groups)
    {
        return Int32.Parse(groups[1].Value) * Int32.Parse(groups[2].Value);
    }

    public static int ExtractUncorruptedWithConditionals(string input)
    {
        var matches = UncorruptedProgramWithConditionalsRegex().Matches(input);
        var enabled = true;
        var sum = 0;

        foreach (Match match in matches)
        {
            switch (match.Groups[1].Value)
            {
                case "mul":
                    if (enabled)
                    {
                        sum += (Int32.Parse(match.Groups[3].Value) * Int32.Parse(match.Groups[4].Value));
                    }
                    break;
                case "do":
                    enabled = true;
                    break;
                case "don't":
                    enabled = false;
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected statement: {match.Groups[0].Value}");
            }
        }

        return sum;
    }
}
