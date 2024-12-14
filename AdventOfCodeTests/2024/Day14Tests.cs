using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day14Tests
{
    private const string TestInput = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";

    [Theory]
    [InlineData("p=2,4 v=2,-3", 11, 7, 1, "p=4,1 v=2,-3")]
    [InlineData("p=2,4 v=2,-3", 11, 7, 2, "p=6,5 v=2,-3")]
    [InlineData("p=2,4 v=2,-3", 11, 7, 3, "p=8,2 v=2,-3")]
    [InlineData("p=2,4 v=2,-3", 11, 7, 4, "p=10,6 v=2,-3")]
    [InlineData("p=2,4 v=2,-3", 11, 7, 5, "p=1,3 v=2,-3")]
    public void TestMoveRobot(string input, int mapWidth, int mapHeight, int time, string expectedResult)
    {
        Day14.MoveRobot(input, mapWidth, mapHeight, time).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(TestInput, 11, 7, 100, 12)]
    public void TestGetSafetyFactor(string input, int mapWidth, int mapHeight, int time, int expectedResult)
    {
        Day14.GetSafetyFactor(input, mapWidth, mapHeight, time).Should().Be(expectedResult);
    }
}