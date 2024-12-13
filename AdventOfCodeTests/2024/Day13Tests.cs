using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day13Tests
{
    private const string TestMachine1 = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400";

    private const string TestMachine2 = @"Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176";

    private const string TestMachine3 = @"Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450";

    private const string TestMachine4 = @"Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279";

    private const string TestInput = @"Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279";

    [Theory]
    [InlineData(TestMachine1, true, 280)]
    [InlineData(TestMachine2, false, null)]
    [InlineData(TestMachine3, true, 200)]
    [InlineData(TestMachine4, false, null)]
    public void TestIsWinnable(string input, bool expectedResult, int? expectedMinCost)
    {
        Day13.IsWinnable(input, out var minCost).Should().Be(expectedResult);
        minCost.Should().Be(expectedMinCost);
    }

    [Theory]
    [InlineData(TestInput, 480)]
    public void TestGetTotalTokens(string input, int expectedResult)
    {
        Day13.GetTotalTokens(input).Should().Be(expectedResult);
    }
}
