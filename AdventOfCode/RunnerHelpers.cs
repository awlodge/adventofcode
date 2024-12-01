using System.Diagnostics;

namespace AdventOfCode;

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
}
