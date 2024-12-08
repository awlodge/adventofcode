using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day08Tests
{
    private const string TestInput = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............";

    [Fact]
    public void TestCountAntiNodes()
    {
        Day08.CountAntiNodes(TestInput).Should().Be(14);
    }

    [Fact]
    public void TestDay8Part1()
    {
        Day08.RunPart1().Should().Be(344);
    }

    [Fact]
    public void TestCountAntiNodesWithRH()
    {
        Day08.CountAntiNodesWithRH(TestInput).Should().Be(34);
    }

    [Fact]
    public void TestDay8Part2()
    {
        Day08.RunPart2().Should().Be(1182);
    }
}
