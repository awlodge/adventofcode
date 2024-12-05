﻿namespace AdventOfCode.Helpers;

internal class Grid<T>(List<List<T>> data)
{
    private readonly List<List<T>> _data = data;

    public int ColCount => _data[0].Count;

    public int RowCount => _data.Count;

    public T Lookup(Point p) => _data[p.X][p.Y];

    public bool TryLookup(Point p, out T? x)
    {
        x = default;
        if (p.X < 0 || p.X >= RowCount || p.Y < 0 || p.Y >= ColCount)
        {
            return false;
        }

        x = Lookup(p);
        return true;
    }

    public IEnumerable<Point> Search()
    {
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < _data[i].Count; j++)
            {
                yield return new(i, j);
            }
        }
    }
}

internal record Point(int X, int Y)
{
    public static Point operator +(Point a) => a;
    public static Point operator -(Point a) => new(-a.X, -a.Y);

    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator -(Point a, Point b)
        => a + (-b);
}

internal static class Directions
{
    public static Point North = new(-1, 0);
    public static Point NorthEast = new(-1, 1);
    public static Point East = new(0, 1);
    public static Point SouthEast = new(1, 1);
    public static Point South = new(1, 0);
    public static Point SouthWest = new(1, -1);
    public static Point West = new(0, -1);
    public static Point NorthWest = new(-1, -1);

    public static Point[] All = [
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    ];
}