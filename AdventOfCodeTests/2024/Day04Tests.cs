using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day04Tests
{
    private const string TestInput = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

    [Theory]
    [InlineData(TestInput, "XMAS", 18)]
    public void TestFindWords(string input, string word, int expectedCount)
    {
        Day04.FindWords(input, word).Should().Be(expectedCount);
    }

    [Theory]
    [InlineData(TestInput, 9)]
    public void TestCountXmases(string input, int expectedCount)
    {
        Day04.CountXmases(input).Should().Be(expectedCount);
    }
}