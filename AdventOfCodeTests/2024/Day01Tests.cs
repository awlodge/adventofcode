using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day01Tests
{
    public const string TestInput = @"3   4
4   3
2   5
1   3
3   9
3   3";

    [Theory]
    [InlineData(TestInput, 11)]
    public void TestCalculateDistance(string input, int expectedDistance)
    {
        Day01.CalculateDistance(input).Should().Be(expectedDistance);
    }

    [Theory]
    [InlineData(TestInput, 31)]
    public void TestSimilarityScore(string input, int expectedSimilarity)
    {
        Day01.CalculateSimilarityScore(input).Should().Be(expectedSimilarity);
    }
}
