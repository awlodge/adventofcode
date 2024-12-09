using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day09Tests
{
    private const string TestInput = "2333133121414131402";

    [Theory]
    [InlineData("12345", "0..111....22222")]
    [InlineData(TestInput, "00...111...2...333.44.5555.6666.777.888899")]
    public void TestGetMemoryMap(string input, string expectedResult)
    {
        input.GetMemoryMap().MemoryMapAsString().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("12345", "022111222......")]
    [InlineData(TestInput, "0099811188827773336446555566..............")]
    public void TestCollapse(string input, string expectedResult)
    {
        input.GetMemoryMap().Collapse().MemoryMapAsString().Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("12345", 60)]
    [InlineData(TestInput, 1928)]
    public void TestCollapseChecksum(string input, long expectedResult)
    {
        input.GetMemoryMap().Collapse().Checksum().Should().Be(expectedResult);
    }

    [Fact]
    public void TestDay9Part1()
    {
        Day09.RunPart1().Should().Be(6385338159127);
    }
}