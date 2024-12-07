using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day07Tests
{
    private const string TestInput = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";

    public static IEnumerable<object[]> GetTestInputForCheckEquation()
    {
        HashSet<long> validResults = [190, 3267, 292];

        foreach (var eq in Day07.Parse(TestInput))
        {
            yield return new object[] { eq, validResults.Contains(eq.Result) };
        }
    }

    [Theory]
    [MemberData(nameof(GetTestInputForCheckEquation))]
    public void TestCheckEquation(Equation equation, bool expectedResult)
    {
        equation.IsValid().Should().Be(expectedResult);
    }

    [Fact]
    public void TestTotalCalibrationResult()
    {
        Day07.TotalCalibrationResult(TestInput).Should().Be(3749);
    }

    [Fact]
    public void TestDay7Part1()
    {
        Day07.RunPart1().Should().Be(12553187650171);
    }
}
