namespace AdventOfCode._2024;

using AdventOfCode.Helpers;
using Wordsearch = List<List<char>>;

public static class Day04
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day04.txt");
    private const string InputWord = "XMAS";

    public static int RunPart1()
    {
        return ParseFile(InputPath).FindWords(InputWord);
    }

    public static int FindWords(string input, string word)
    {
        return Parse(input).FindWords(word);
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

        return Point.Directions.Count(p => wordsearch.CheckDirection(word, position, p));
    }

    private static bool CheckDirection(this Wordsearch wordsearch, string word, Point position, Point direction)
    {
        var wordArr = word.ToCharArray();
        Point pos = new(position.X, position.Y);
        for (int i = 1; i < wordArr.Length; i++)
        {
            pos = pos.Add(direction);
            if (pos.X < 0 || pos.X >= wordsearch.Count || pos.Y < 0 || pos.Y >= wordsearch.ColCount())
            {
                return false;
            }

            if (wordsearch.Lookup(pos) != wordArr[i])
            {
                return false;
            }
        }

        return true;
    }

    private static int ColCount(this Wordsearch wordsearch) => wordsearch[0].Count;

    private static char Lookup(this Wordsearch wordsearch, Point p) => wordsearch[p.X][p.Y];

    private static IEnumerable<Point> Search(this Wordsearch wordsearch)
    {
        for (int i = 0; i < wordsearch.Count; i++)
        {
            for (int j = 0; j < wordsearch[i].Count; j++)
            {
                yield return new(i, j);
            }
        }
    }

    private static Wordsearch Parse(string input)
    {
        return input.SplitLines().Select(l => l.ToCharArray().ToList()).ToList();
    }

    private static Wordsearch ParseFile(string path)
    {
        return File.ReadLines(path).Select(l => l.ToCharArray().ToList()).ToList();
    }
}

internal record Point(int x, int y)
{
    public int X { get; init; } = x;
    public int Y { get; init; } = y;

    public Point Add(Point point) => new(X + point.X, Y + point.Y);

    public static Point[] Directions = {
        new(1, 0),
        new(1, 1),
        new(0, 1),
        new(-1, 1),
        new(-1, 0),
        new(-1, -1),
        new(0, -1),
        new(1, -1),
    };
}