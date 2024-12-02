namespace AdventOfCode.Helpers;

internal static class ParseHelpers
{
    public static IEnumerable<string> SplitLines(this string input) => input.Split('\n', options: StringSplitOptions.RemoveEmptyEntries);

    public static IEnumerable<int> ParseAsInts(this string input) => input
        .Split(Array.Empty<char>(), options: StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Int32.Parse(x));
}
