using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class TestDay12
{
    private const string TestInput1 = @"AAAA
BBCD
BBCC
EEEC";

    private const string TestInput2 = @"OOOOO
OXOXO
OOOOO
OXOXO
OOOOO";

    private const string TestInput3 = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE";

    [Theory]
    [InlineData(TestInput1, 140)]
    [InlineData(TestInput2, 772)]
    [InlineData(TestInput3, 1930)]
    public void TestGetTotalCost(string input, int expectedResult)
    {
        Day12.GetTotalCost(input).Should().Be(expectedResult);
    }
}