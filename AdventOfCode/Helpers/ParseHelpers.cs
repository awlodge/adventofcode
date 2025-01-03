﻿namespace AdventOfCode.Helpers;

internal static class ParseHelpers
{
    public static IEnumerable<string> SplitLines(this string input, bool removeEmpty = true) => input
        .Split('\n',
            options: removeEmpty ?
                StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries :
                StringSplitOptions.TrimEntries)
        .Select(l => l.TrimEnd('\r', '\n'));

    public static IEnumerable<List<string>> SplitBlocks(this IEnumerable<string> lines)
    {
        List<string> block = [];
        foreach (var line in lines)
        {
            if (line == string.Empty)
            {
                yield return block;
                block = [];
            }
            else
            {
                block.Add(line);
            }
        }

        if (block.Count != 0)
        {
            yield return block;
        }
    }

    public static IEnumerable<int> ParseAsInts(this string input, char[]? splitChars = default) => input
        .Split(splitChars ?? [], options: StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Int32.Parse(x));

    public static IEnumerable<long> ParseAsLongs(this string input, char[]? splitChars = default) => input
        .Split(splitChars ?? [], options: StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Int64.Parse(x));

    public static Grid<char> ParseCharGrid(string input) =>
        input.SplitLines().ParseCharGrid();

    public static Grid<char> ParseCharGridFile(string path) =>
        File.ReadLines(path).ParseCharGrid();

    public static Grid<char> ParseCharGrid(this IEnumerable<string> lines)
    {
        return new Grid<char>(lines.Select(l => l.ToCharArray().ToList()).ToList());
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
