using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day05Tests
{
    private const string TestInput = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";

    public static IEnumerable<object[]> GetParsedTestInputForCheckUpdates()
    {
        var parsedInput = Day05.Parse(TestInput);
        bool[] expectedResults = [
            true,
            true,
            true,
            false,
            false,
            false,
        ];

        for (int i = 0; i < expectedResults.Length; i++)
        {
            yield return new object[] { parsedInput.Rules, parsedInput.Updates[i], expectedResults[i] };
        }
    }

    [Theory]
    [MemberData(nameof(GetParsedTestInputForCheckUpdates))]
    public void TestCheckUpdate(Dictionary<int, HashSet<int>> rules, List<int> update, bool expectedResult)
    {
        update.CheckUpdate(rules).Should().Be(expectedResult);
    }

    [Fact]
    public void TestSumMiddleValidUpdates()
    {
        var parsedInput = Day05.Parse(TestInput);
        parsedInput.SumMiddleValidUpdate().Should().Be(143);
    }
}
