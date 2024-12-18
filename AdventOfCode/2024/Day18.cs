using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day18
{
    private const int GridSize = 71;
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day18.txt");

    [AdventOfCode2024(18, 1)]
    public static int RunPart1()
    {
        var obstacles = File.ReadLines(InputPath)
            .Parse()
            .Take(1024)
            .ToHashSet();

        return GetShortestPath(GridSize, GridSize, obstacles);
    }

    [AdventOfCode2024(18, 2)]
    public static string RunPart2()
    {
        var obstacles = File.ReadLines(InputPath)
            .Parse()
            .ToList();

        var p = FindFirstBlocker(GridSize, GridSize, obstacles);
        return $"{p.X},{p.Y}";
    }

    public static int GetShortestPath(string input, int width, int height, int ticks)
    {
        var obstacles = input.SplitLines()
            .Parse()
            .Take(ticks);

        return GetShortestPath(width, height, obstacles);
    }

    public static string FindFirstBlocker(string input, int width, int height)
    {
        var obstacles = input.SplitLines()
            .Parse()
            .ToList();

        var p = FindFirstBlocker(width, height, obstacles);
        return $"{p.X},{p.Y}";
    }

    private static IEnumerable<Point> Parse(this IEnumerable<string> lines)
    {
        return lines
            .Select(l => l.ParseAsInts([',']).ToArray())
            .Select(l => new Point(l[0], l[1]));
    }

    private static int GetShortestPath(int width, int height, IEnumerable<Point> obstacles)
    {
        var map = Map.Generate('.', width, height);
        var start = new Point(0, 0);
        var end = new Point(width - 1, height - 1);
        return map.GetShortestPath(start, end, obstacles);
    }

    private static Graph<Point> ToGraph(this Map map, IEnumerable<Point> obstacles)
    {
        // map.Print(obstacles.ToDictionary(k => k, _ => '#'));
        var graph = new Graph<Point>();
        foreach (var p in map.Search().Where(n => !obstacles.Contains(n)))
        {
            graph.AddNode(p, GetNeighbors(p));
        }

        return graph;

        IEnumerable<(Point, int)> GetNeighbors(Point p) => Directions.Cardinal
            .Select(d => p + d)
            .Where(q => map.Contains(q) && !obstacles.Contains(q))
            .Select(q => (q, 1));
    }

    private static int GetShortestPath(this Map map, Point start, Point end, IEnumerable<Point> obstacles)
    {
        return map.ToGraph(obstacles).ShortestPath(start, end);
    }

    private static Point FindFirstBlocker(int width, int height, List<Point> obstacles)
    {
        var map = Map.Generate('.', width, height);
        var start = new Point(0, 0);
        var end = new Point(width - 1, height - 1);
        return map.FindFirstBlocker(start, end, obstacles);
    }

    private static Point FindFirstBlocker(this Map map, Point start, Point end, List<Point> obstacles)
    {
        var midPoint = Enumerable.Range(0, obstacles.Count)
            .ToList()
            .BinarySearch<int>(n => map.GetShortestPath(start, end, obstacles.Take(n).ToHashSet()) == -1);

        return obstacles[midPoint];
    }
}
