using AdventOfCode.Helpers;
using FluentAssertions;
using System.Reflection;

namespace AdventOfCodeTests._2024;

public class SolutionTests
{
    public static IEnumerable<object[]> GetTestData()
    {
        var solutions = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "2024/solutions.txt"))
            .Select(l => l
                .Split(',')
                .Select(x => Int64.Parse(x))
                .ToList())
            .ToList();

        for (int i = 0; i < solutions.Count; i++)
        {
            var s = solutions[i];
            for (int j = 0; j < s.Count; j++)
            {
                yield return [i + 1, j + 1, s[j]];
            }
        }
    }

    private static long InvokeRunner(int day, int part)
    {
        var method = typeof(AdventOfCodeAttribute)
            .Assembly
            .GetTypes()
            .SelectMany(t => t.GetMethods())
            .First(m =>
            {
                var a = (AdventOfCodeAttribute?)m.GetCustomAttribute(typeof(AdventOfCodeAttribute));
                return a != null && a.Year == 2024 && a.Day == day && a.Part == part;
            });

        return (long)method.Invoke(null, null)!;
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void TestSolutions(int day, int part, long expectedResult)
    {
        InvokeRunner(day, part).Should().Be(expectedResult);
    }
}