using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day03Tests
{
    private const string TestInput = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
    private const string TestInput2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

    [Theory]
    [InlineData(TestInput, 161)]
    public void TestExtractUncorrupted(string input, int expectedResult)
    {
        Day03.ExtractUncorrupted(input).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput2, 48)]
    public void TextExtractUncorruptedWithConditionals(string input, int expectedResult)
    {
        Day03.ExtractUncorruptedWithConditionals(input).Should().Be(expectedResult);
    }
}
