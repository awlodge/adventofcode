using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Helpers;

internal static class RunnerHelpers
{
    public static void Execute(MethodInfo method, string title)
    {
        Console.WriteLine("============================");
        Console.WriteLine($"Executing {title}");
        var sw = Stopwatch.StartNew();
        var output = method.Invoke(null, null);
        sw.Stop();
        Console.WriteLine($"Output: {output}");
        Console.WriteLine($"[Ran in {sw.Elapsed.TotalSeconds} seconds]");
        Console.WriteLine();
    }

    public static IEnumerable<(MethodInfo, AdventOfCodeAttribute?)> GetAllRunners()
    {
        var methods = typeof(AdventOfCodeAttribute)
            .Assembly
            .GetTypes()
            .SelectMany(t => t.GetMethods());

        return methods.Zip(methods.Select(x => (AdventOfCodeAttribute?)x.GetCustomAttribute(typeof(AdventOfCodeAttribute))))
            .Where(x => x.Second != null)
            .OrderBy(x => x.Second);
    }

    public static IEnumerable<(MethodInfo, AdventOfCodeAttribute?)> Filter(this IEnumerable<(MethodInfo, AdventOfCodeAttribute?)> runners, int year, int? day = default, int? part = default)
    {
        return runners.Where(x =>
        {
            AdventOfCodeAttribute a = x.Item2!;
            return (a.Year == year)
                && (day == null || (a.Day == day))
                && (part == null || (a.Part == part));
        });
    }

    public static void RunAll(this IEnumerable<(MethodInfo, AdventOfCodeAttribute?)> runners)
    {
        foreach (var x in runners)
        {
            var m = x.Item1;
            var a = x.Item2 ?? throw new ArgumentNullException(nameof(runners), $"{m.DeclaringType}.{m.Name} missing attributes");
            Execute(m, $"{a.Year} Day {a.Day} (part {a.Part})");
        }
    }
}
