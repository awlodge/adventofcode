using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day18Tests
{
    private const string TestInput = @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";

    [Theory]
    [InlineData(TestInput, 7, 7, 12, 22)]
    public void TestGetShortestPath(string input, int width, int height, int ticks, int expectedResult)
    {
        Day18.GetShortestPath(input, width, height, ticks).Should().Be(expectedResult);
    }
}