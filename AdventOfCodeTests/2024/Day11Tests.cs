using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day11Tests
{
    [Theory]
    [InlineData(new long[] { 0, 1, 10, 99, 999 },
        new long[] { 1, 2024, 1, 0, 9, 9, 2021976, })]
    [InlineData(new long[] { 125, 17 },
        new long[] { 253000, 1, 7 })]
    [InlineData(new long[] { 253000, 1, 7 },
        new long[] { 253, 0, 2024, 14168 })]
    [InlineData(new long[] { 253, 0, 2024, 14168 },
        new long[] { 512072, 1, 20, 24, 28676032 })]
    public void TestChangeStones(long[] stones, long[] expectedResult)
    {
        stones.ChangeStones().Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData("0 1 10 99 999", 1, 7)]
    [InlineData("125 17", 6, 22)]
    [InlineData("125 17", 25, 55312)]
    public void TestCountStones(string input, int changeCount, int expectedResult)
    {
        Day11.CountStones(input, changeCount).Should().Be(expectedResult);
    }
}