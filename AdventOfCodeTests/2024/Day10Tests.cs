using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day10Tests
{
    private const string TestInput = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";

    private const string TestInput2 = @"0123
1234
8765
9876";

    [Theory]
    [InlineData(TestInput, 36)]
    [InlineData(TestInput2, 1)]
    public void TestGetTotalScore(string input, int expectedResult)
    {
        Day10.GetTotalScore(input).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 81)]
    [InlineData(TestInput2, 16)]
    public void TestGetTotalRating(string input, int expectedResult)
    {
        Day10.GetTotalRating(input).Should().Be(expectedResult);
    }
}
