using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day17Tests
{
    private const string TestInput = @"Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0";

    [Theory]
    [InlineData(new[] { 2, 6 }, 0, 0, 9, 0, 1, 9, new int[] { })]
    [InlineData(new[] { 5, 0, 5, 1, 5, 4 }, 10, 0, 0, 10, 0, 0, new int[] { 0, 1, 2 })]
    [InlineData(new[] { 0, 1, 5, 4, 3, 0 }, 2024, 0, 0, 0, 0, 0, new int[] { 4, 2, 5, 6, 7, 7, 7, 7, 3, 1, 0 })]
    [InlineData(new[] { 1, 7 }, 0, 29, 0, 0, 26, 0, new int[] { })]
    [InlineData(new[] { 4, 0 }, 0, 2024, 43690, 0, 44354, 43690, new int[] { })]
    public void TestComputer(int[] program, int a, int b, int c,
        int expA, int expB, int expC, int[] expOutputs)
    {
        Day17.TestComputer(program.ToList(), a, b, c,
            out var outA, out var outB, out var outC)
            .Should().BeEquivalentTo(expOutputs);
        outA.Should().Be(expA);
        outB.Should().Be(expB);
        outC.Should().Be(expC);
    }

    [Fact]
    public void TestComputerInput()
    {
        Day17.TestComputer(TestInput).Should().Be("4,6,3,5,6,3,5,2,1,0");
    }

    [Fact]
    public void TestPart1()
    {
        Day17.RunPart1().Should().Be("1,3,7,4,6,4,2,3,5");
    }
}
