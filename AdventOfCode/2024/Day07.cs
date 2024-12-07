using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day07
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day07.txt");

    public static long RunPart1()
    {
        return ParseFile(InputPath).TotalCalibrationResult();
    }

    public static long TotalCalibrationResult(string input)
    {
        return Parse(input).TotalCalibrationResult();
    }

    private static long TotalCalibrationResult(this IEnumerable<Equation> equations) =>
        equations
            .Where(eq => eq.IsValid())
            .Sum(eq => eq.Result);

    public static bool IsValid(this Equation equation) =>
        CheckEquation(equation.Result, equation.Inputs);

    private static bool CheckEquation(long result, List<int> inputs)
    {
        if (inputs.Count == 1)
        {
            return result == inputs[0];
        }

        if (result < inputs[^1])
        {
            return false;
        }

        if (CheckEquation(result - inputs[^1], inputs[..^1]))
        {
            return true;
        }

        if (result % inputs[^1] == 0 && CheckEquation(result / inputs[^1], inputs[..^1]))
        {
            return true;
        }

        return false;
    }

    private static IEnumerable<Equation> ParseFile(string path)
    {
        return File.ReadLines(path).Select(l => ParseLine(l));
    }

    public static IEnumerable<Equation> Parse(string input)
    {
        return input.SplitLines().Select(l => ParseLine(l));
    }

    private static Equation ParseLine(string line)
    {
        var parts = line.Split(':', options: StringSplitOptions.TrimEntries);
        return new Equation(Int64.Parse(parts[0]), parts[1].ParseAsInts().ToList());
    }
}

public record Equation(long Result, List<int> Inputs);