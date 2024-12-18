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

    public static int GetShortestPath(string input, int width, int height, int ticks)
    {
        var obstacles = input.SplitLines()
            .Parse()
            .Take(ticks)
            .ToHashSet();

        return GetShortestPath(width, height, obstacles);
    }

    private static IEnumerable<Point> Parse(this IEnumerable<string> lines)
    {
        return lines
            .Select(l => l.ParseAsInts([',']).ToArray())
            .Select(l => new Point(l[0], l[1]));
    }

    private static int GetShortestPath(int width, int height, HashSet<Point> obstacles)
    {
        var map = Map.Generate('.', width, height);
        var start = new Point(0, 0);
        var end = new Point(width - 1, height - 1);
        return map.GetShortestPath(start, end, obstacles);
    }

    private static int GetShortestPath(this Map map, Point start, Point end, HashSet<Point> obstacles)
    {
        // map.Print(obstacles.ToDictionary(k => k, _ => '#'));
        var graph = new Graph<Point>();
        foreach (var p in map.Search().Where(n => !obstacles.Contains(n)))
        {
            graph.AddNode(p, GetNeighbors(p));
        }

        return graph.ShortestPath(start, end);

        IEnumerable<(Point, int)> GetNeighbors(Point p) => Directions.Cardinal
            .Select(d => p + d)
            .Where(q => map.Contains(q) && !obstacles.Contains(q))
            .Select(q => (q, 1));
    }
}
