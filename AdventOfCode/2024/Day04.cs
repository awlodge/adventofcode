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

        return Point.Directions.Count(p => wordsearch.CheckDirection(word, position, p));
    }

    private static bool CheckDirection(this Wordsearch wordsearch, string word, Point position, Point direction)
    {
        var wordArr = word.ToCharArray();
        Point pos = new(position.X, position.Y);
        for (int i = 1; i < wordArr.Length; i++)
        {
            pos = pos.Add(direction);
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

        Point[][] directions = new[]
        {
            new[] { Point.NorthEast, Point.SouthWest },
            new[] { Point.NorthWest, Point.SouthEast },
        };

        foreach (var dir in directions)
        {
            if (!wordsearch.TryLookup(position.Add(dir[0]), out var x))
            {
                return false;
            }

            if (!wordsearch.TryLookup(position.Add(dir[1]), out var y))
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

    private static int ColCount(this Wordsearch wordsearch) => wordsearch[0].Count;

    private static char Lookup(this Wordsearch wordsearch, Point p) => wordsearch[p.X][p.Y];

    private static bool TryLookup(this Wordsearch wordsearch, Point p, out char? x)
    {
        x = null;
        if (p.X < 0 || p.X >= wordsearch.Count || p.Y < 0 || p.Y >= wordsearch.ColCount())
        {
            return false;
        }

        x = wordsearch.Lookup(p);
        return true;
    }

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

    public static Point North = new(-1, 0);
    public static Point NorthEast = new(-1, 1);
    public static Point East = new(0, 1);
    public static Point SouthEast = new(1, 1);
    public static Point South = new(1, 0);
    public static Point SouthWest = new(1, -1);
    public static Point West = new(0, -1);
    public static Point NorthWest = new(-1, -1);

    public static Point[] Directions = {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    };
}