using AdventOfCode.Helpers;
using FluentAssertions;

namespace AdventOfCodeTests.Helpers;

public class HelperTests
{
    [Fact]
    public void TestPairs()
    {
        int[] x = { 1, 2, 3 };
        List<List<int>> expectedResult = [[1, 2], [1, 3], [2, 3]];
        var result = x.Pairs();
        result.Should().BeEquivalentTo(expectedResult);
    }
}
