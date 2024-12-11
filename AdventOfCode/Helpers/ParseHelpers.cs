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

    public static IEnumerable<long> ParseAsLongs(this string input, char[]? splitChars = default) => input
        .Split(splitChars ?? [], options: StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Int64.Parse(x));

    public static Grid<char> ParseCharGrid(string input)
    {
        return new Grid<char>(input.SplitLines().Select(l => l.ToCharArray().ToList()).ToList());
    }

    public static Grid<char> ParseCharGridFile(string path)
    {
        return new Grid<char>(File.ReadLines(path).Select(l => l.ToCharArray().ToList()).ToList());
    }

    public static Grid<int> ParseIntGrid(string input)
    {
        return new Grid<int>(input.SplitLines().Select(l => l.ToCharArray().Select(x => (int)Char.GetNumericValue(x)).ToList()).ToList());
    }

    public static Grid<int> ParseIntGridFile(string path)
    {
        return new Grid<int>(File.ReadLines(path).Select(l => l.ToCharArray().Select(x => (int)Char.GetNumericValue(x)).ToList()).ToList());
    }
}
