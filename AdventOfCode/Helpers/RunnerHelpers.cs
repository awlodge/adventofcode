using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Helpers;

internal static class RunnerHelpers
{
    public static void Execute<T>(Func<T> action, string title)
    {
        Console.WriteLine("============================");
        Console.WriteLine($"Executing {title}");
        var sw = Stopwatch.StartNew();
        T output = action();
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

    public static void RunAll(this IEnumerable<(MethodInfo, AdventOfCodeAttribute?)> runners)
    {
        foreach (var x in runners)
        {
            var m = x.Item1;
            var a = x.Item2 ?? throw new ArgumentNullException(nameof(runners), $"{m.DeclaringType}.{m.Name} missing attributes");
            Execute((Func<long>)Delegate.CreateDelegate(typeof(Func<long>), m),
                $"{a.Year} Day {a.Day} (part {a.Part})");
        }
    }
}
