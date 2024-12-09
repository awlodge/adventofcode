using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day07
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day07.txt");

    [AdventOfCode2024(7, 1)]
    public static long RunPart1()
    {
        return ParseFile(InputPath).TotalCalibrationResult();
    }

    [AdventOfCode2024(7, 2)]
    public static long RunPart2()
    {
        return ParseFile(InputPath).TotalCalibrationResultWithConcat();
    }

    public static long TotalCalibrationResult(string input)
    {
        return Parse(input).TotalCalibrationResult();
    }

    public static long TotalCalibrationResultWithConcat(string input)
    {
        return Parse(input).TotalCalibrationResultWithConcat();
    }

    private static long TotalCalibrationResult(this IEnumerable<Equation> equations) =>
        equations
            .Where(eq => eq.IsValid())
            .Sum(eq => eq.Result);

    private static long TotalCalibrationResultWithConcat(this IEnumerable<Equation> equations) =>
        equations
            .Where(eq => eq.IsValidWithConcat())
            .Sum(eq => eq.Result);

    public static bool IsValid(this Equation equation) =>
        CheckEquation(equation.Result, equation.Inputs);

    public static bool IsValidWithConcat(this Equation equation) =>
        CheckEquation(equation.Result, equation.Inputs, withConcat: true);

    private static bool CheckEquation(long result, List<int> inputs, bool withConcat = false)
    {
        if (inputs.Count == 1)
        {
            return result == inputs[0];
        }

        if (result < inputs[^1])
        {
            return false;
        }

        if (CheckEquation(result - inputs[^1], inputs[..^1], withConcat))
        {
            return true;
        }

        if (result % inputs[^1] == 0 && CheckEquation(result / inputs[^1], inputs[..^1], withConcat))
        {
            return true;
        }

        if (withConcat && result.EndsWith(inputs[^1]) && CheckEquation(result.DropSuffix(inputs[^1]), inputs[..^1], withConcat))
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

    private static bool EndsWith(this long x, int y)
    {
        return (x - y) % Math.Pow(10, y.Digits()) == 0;
    }

    private static int Digits(this int y)
    {
        int digits = 1;
        while ((y /= 10) != 0)
        {
            digits++;
        }

        return digits;
    }

    private static long DropSuffix(this long x, int y)
    {
        return (long)((x - y) / Math.Pow(10, y.Digits()));
    }
}

public record Equation(long Result, List<int> Inputs);