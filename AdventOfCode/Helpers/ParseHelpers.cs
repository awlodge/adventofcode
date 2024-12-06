namespace AdventOfCode.Helpers;

internal static class ParseHelpers
{
    public static IEnumerable<string> SplitLines(this string input, bool removeEmpty = true) => input
        .Split('\n',
            options: removeEmpty ?
                StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries :
                StringSplitOptions.TrimEntries)
        .Select(l => l.TrimEnd('\r', '\n'));

    public static IEnumerable<int> ParseAsInts(this string input, char[]? splitChars = default) => input
        .Split(splitChars ?? [], options: StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Int32.Parse(x));
}
