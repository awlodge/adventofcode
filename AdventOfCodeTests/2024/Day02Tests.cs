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
        report.IsSafe().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 2)]
    public void TestSafeCount(string input, int expectedCount)
    {
        Day02.SafeCount(input).Should().Be(expectedCount);
    }

    [Fact]
    public void TestDay2Part1()
    {
        Day02.RunPart1().Should().Be(356);
    }
}
