﻿namespace AdventOfCode.Helpers;

internal class Grid<T>(List<List<T>> data)
{
    private readonly List<List<T>> _data = data;

    public int ColCount => _data[0].Count;
    public int Width => ColCount;

    public int RowCount => _data.Count;
    public int Height => RowCount;

    public T Lookup(Point p) => _data[p.Y][p.X];

    public bool TryLookup(Point p, out T? x)
    {
        x = default;
        if (!Contains(p))
        {
            return false;
        }

        x = Lookup(p);
        return true;
    }

    public bool Contains(Point p) => p.Y >= 0 && p.Y < RowCount && p.X >= 0 && p.X < ColCount;

    public IEnumerable<Point> Search()
    {
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < _data[y].Count; x++)
            {
                yield return new(x, y);
            }
        }
    }

    public IEnumerable<Point> Search(Func<T, bool> predicate)
    {
        return Search().Where(p => predicate(Lookup(p)));
    }

    public IEnumerable<Point> Walk(Point p, Point direction)
    {
        var q = p + direction;
        while (Contains(q))
        {
            yield return q;
            q += direction;
        }
    }

    public bool TryUpdate(Point p, T val)
    {
        if (Contains(p))
        {
            _data[p.Y][p.X] = val;
            return true;
        }

        return false;
    }

    public bool Swap(Point p, Point q)
    {
        if (!(Contains(p) && Contains(q)))
        {
            return false;
        }

        var pVal = Lookup(p);
        var qVal = Lookup(q);
        TryUpdate(p, qVal);
        TryUpdate(q, pVal);
        return true;
    }

    public void Print(IDictionary<Point, T>? substitutes = default)
    {
        for (int y = 0; y < RowCount; y++)
        {
            var rowStr = String.Empty;
            for (int x = 0; x < ColCount; x++)
            {
                var p = new Point(x, y);
                T val = Lookup(p);
                if (substitutes?.TryGetValue(p, out var val2) ?? false)
                {
                    val = val2;
                }
                rowStr += val;
            }
            Console.WriteLine(rowStr);
        }
    }

    public static Grid<T> Generate(T input, int width, int height)
    {
        return new Grid<T>(Enumerable.Repeat(Enumerable.Repeat(input, width).ToList(), height).ToList());
    }
}

internal record struct Point(int X, int Y)
{
    public static Point operator +(Point a) => a;
    public static Point operator -(Point a) => new(-a.X, -a.Y);

    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator -(Point a, Point b)
        => a + (-b);

    public static Point operator *(Point a, int n) => new(n * a.X, n * a.Y);

    public static Point operator *(int n, Point a) => new(n * a.X, n * a.Y);
}

internal static class Directions
{
    public static readonly Point North = new(0, -1);
    public static readonly Point NorthEast = new(1, -1);
    public static readonly Point East = new(1, 0);
    public static readonly Point SouthEast = new(1, 1);
    public static readonly Point South = new(0, 1);
    public static readonly Point SouthWest = new(-1, 1);
    public static readonly Point West = new(-1, 0);
    public static readonly Point NorthWest = new(-1, -1);

    public static readonly Point[] All = [
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    ];

    public static readonly Point[] Cardinal = [
        North,
        East,
        South,
        West,
    ];
}