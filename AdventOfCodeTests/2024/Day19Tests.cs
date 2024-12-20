using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day19Tests
{
    private const string TestInput = @"r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb";

    public static IEnumerable<object[]> TestData()
    {
        bool[] expectedResults = [true, true, true, true, false, true, true, false];
        (var startPatterns, var targetPatterns) = Day19.Parse(TestInput);
        for (var i = 0; i < targetPatterns.Count; i++)
        {
            yield return new object[] { startPatterns, targetPatterns[i], expectedResults[i] };
        }
    }
    
    public static IEnumerable<object[]> TestDataCounts()
    {
        int[] expectedResults = [2, 1, 4, 6, 0, 1, 2, 0];
        (var startPatterns, var targetPatterns) = Day19.Parse(TestInput);
        for (var i = 0; i < targetPatterns.Count; i++)
        {
            yield return new object[] { startPatterns, targetPatterns[i], expectedResults[i] };
        }
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void TestViablePattern(string[] startPatterns, string targetPattern, bool expectedResult)
    {
        var d = new Day19(startPatterns);
        d.IsViablePattern(targetPattern).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 6)]
    public void TestCountViablePatterns(string input, int expectedResult)
    {
        Day19.CountViablePatterns(input).Should().Be(expectedResult);
    }
    
    [Theory]
    [MemberData(nameof(TestDataCounts))]
    public void TestCountPatternOptions(string[] startPatterns, string targetPattern, int expectedResult)
    {
        var d = new Day19(startPatterns);
        d.FindPatternOptions(targetPattern).Should().HaveCount(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 16)]
    public void TestTotalPatternOptions(string input, int expectedResult)
    {
        Day19.CountTotalPatternOptions(input).Should().Be(expectedResult);
    }
}
