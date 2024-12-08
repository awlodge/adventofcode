namespace AdventOfCode.Helpers;

internal class Grid<T>(List<List<T>> data)
{
    private readonly List<List<T>> _data = data;

    public int ColCount => _data[0].Count;

    public int RowCount => _data.Count;

    public T Lookup(Point p) => _data[p.X][p.Y];

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

    public bool Contains(Point p) => p.X >= 0 && p.X < RowCount && p.Y >= 0 && p.Y < ColCount;

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

    public void Print(IDictionary<Point, T>? substitutes = default)
    {
        for (int i = 0; i < RowCount; i++)
        {
            var rowStr = String.Empty;
            for (int j = 0; j < RowCount; j++)
            {
                var p = new Point(i, j);
                T x = Lookup(p);
                if (substitutes?.TryGetValue(p, out var y) ?? false)
                {
                    x = y;
                }
                rowStr += x;
            }
            Console.WriteLine(rowStr);
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

    public static Point operator *(Point a, int n) => new(n * a.X, n * a.Y);

    public static Point operator *(int n, Point a) => new(n * a.X, n * a.Y);
}

internal static class Directions
{
    public static readonly Point North = new(-1, 0);
    public static readonly Point NorthEast = new(-1, 1);
    public static readonly Point East = new(0, 1);
    public static readonly Point SouthEast = new(1, 1);
    public static readonly Point South = new(1, 0);
    public static readonly Point SouthWest = new(1, -1);
    public static readonly Point West = new(0, -1);
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
}