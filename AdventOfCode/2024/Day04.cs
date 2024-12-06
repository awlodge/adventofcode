namespace AdventOfCode._2024;

using AdventOfCode.Helpers;
using Wordsearch = Helpers.Grid<char>;

public static class Day04
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day04.txt");
    private const string InputWord = "XMAS";

    public static int RunPart1()
    {
        return ParseFile(InputPath).FindWords(InputWord);
    }

    public static int RunPart2()
    {
        return ParseFile(InputPath).CountXmases();
    }

    public static int FindWords(string input, string word)
    {
        return Parse(input).FindWords(word);
    }

    public static int CountXmases(string input)
    {
        return Parse(input).CountXmases();
    }

    private static int FindWords(this Wordsearch wordsearch, string word)
    {
        return wordsearch.Search().Sum(p => wordsearch.FindWordsAt(word, p));
    }

    private static int FindWordsAt(this Wordsearch wordsearch, string word, Point position)
    {
        if (wordsearch.Lookup(position) != word[0])
        {
            return 0;
        }

        return Directions.All.Count(p => wordsearch.CheckDirection(word, position, p));
    }

    private static bool CheckDirection(this Wordsearch wordsearch, string word, Point position, Point direction)
    {
        var wordArr = word.ToCharArray();
        var pos = position with { };
        for (int i = 1; i < wordArr.Length; i++)
        {
            pos += direction;
            if (!wordsearch.TryLookup(pos, out var x))
            {
                return false;
            }

            if (x != wordArr[i])
            {
                return false;
            }
        }

        return true;
    }

    private static int CountXmases(this Wordsearch wordsearch)
    {
        return wordsearch.Search().Count(p => wordsearch.CheckXmasAt(p));
    }

    private static bool CheckXmasAt(this Wordsearch wordsearch, Point position)
    {
        if (wordsearch.Lookup(position) != 'A')
        {
            return false;
        }

        Point[][] directions =
        [
            [Directions.NorthEast, Directions.SouthWest],
            [Directions.NorthWest, Directions.SouthEast],
        ];

        foreach (var dir in directions)
        {
            if (!wordsearch.TryLookup(position + dir[0], out var x))
            {
                return false;
            }

            if (!wordsearch.TryLookup(position + dir[1], out var y))
            {
                return false;
            }

            if (!((x == 'M' && y == 'S') || (x == 'S' && y == 'M')))
            {
                return false;
            }
        }

        return true;
    }

    private static Wordsearch Parse(string input) => Wordsearch.ParseCharGrid(input);

    private static Wordsearch ParseFile(string path) => Wordsearch.ParseCharGridFile(path);
}
