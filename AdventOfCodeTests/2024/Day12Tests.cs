using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day12Tests
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

    private const string TestInput4 = @"EEEEE
EXXXX
EEEEE
EXXXX
EEEEE";

    private const string TestInput5 = @"AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA";

    [Theory]
    [InlineData(TestInput1, 140)]
    [InlineData(TestInput2, 772)]
    [InlineData(TestInput3, 1930)]
    public void TestGetTotalCost(string input, int expectedResult)
    {
        Day12.GetTotalCost(input).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput1, 80)]
    [InlineData(TestInput2, 436)]
    [InlineData(TestInput3, 1206)]
    [InlineData(TestInput4, 236)]
    [InlineData(TestInput5, 368)]
    public void TestGetDiscountedCost(string input, int expectedResult)
    {
        Day12.GetDiscountedCost(input).Should().Be(expectedResult);
    }
}