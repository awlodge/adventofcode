using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day02Tests
{
    public const string TestInput = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

    [Theory]
    [InlineData(new[] { 7, 6, 4, 2, 1 }, true)]
    [InlineData(new[] { 1, 2, 7, 8, 9 }, false)]
    [InlineData(new[] { 9, 7, 6, 2, 1 }, false)]
    [InlineData(new[] { 1, 3, 2, 4, 5 }, false)]
    [InlineData(new[] { 8, 6, 4, 4, 1 }, false)]
    [InlineData(new[] { 1, 3, 6, 7, 9 }, true)]
    public void TestIsSafe(int[] report, bool expectedResult)
    {
        report.ToList().IsSafe().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 2)]
    public void TestSafeCount(string input, int expectedCount)
    {
        Day02.SafeCount(input).Should().Be(expectedCount);
    }

    [Theory]
    [InlineData(new[] { 7, 6, 4, 2, 1 }, true)]
    [InlineData(new[] { 1, 2, 7, 8, 9 }, false)]
    [InlineData(new[] { 9, 7, 6, 2, 1 }, false)]
    [InlineData(new[] { 1, 3, 2, 4, 5 }, true)]
    [InlineData(new[] { 8, 6, 4, 4, 1 }, true)]
    [InlineData(new[] { 1, 3, 6, 7, 9 }, true)]
    [InlineData(new[] { 3, 2, 4, 5, 6 }, true)]
    [InlineData(new[] { 1, 4, 2, 4, 6 }, true)]
    public void TestIsSafeWithProblemDampener(int[] report, bool expectedResult)
    {
        report.ToList().IsSafeWithProblemDampener().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 4)]
    public void TestSafeCountWithProblemDampener(string input, int expectedCount)
    {
        Day02.SafeCountWithProblemDampener(input).Should().Be(expectedCount);
    }
}
