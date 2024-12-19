using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day16
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day16.txt");

    [AdventOfCode2024(16, 1)]
    public static long RunDay16()
    {
        return ParseHelpers.ParseCharGridFile(InputPath).GetLowestScore();
    }

    public static int GetLowestScore(string input)
    {
        return ParseHelpers.ParseCharGrid(input).GetLowestScore();
    }

    public static int CountVisited(string input)
    {
        return ParseHelpers.ParseCharGrid(input).GetVisited().Count();
    }

    private static IEnumerable<DirectedPoint> GetAllNodes(this Map map)
    {
        return map.Search()
            .Where(p => !map.IsWall(p))
            .SelectMany(p => Directions.Cardinal
                .Select(d => new DirectedPoint(p, d))
            );
    }

    private static int GetLowestScore(this Map map)
    {
        return map.ToGraph().ShortestPath(n => map.Lookup(n.Position) == 'S' && n.Direction == Directions.East,
            n => map.Lookup(n.Position) == 'E');
    }

    private static IEnumerable<Point> GetVisited(this Map map)
    {
        var forwardPaths = map.ToGraph()
            .ShortestPaths(n => map.Lookup(n.Position) == 'S' && n.Direction == Directions.East,
                n => map.Lookup(n.Position) == 'E');
        var reversePaths = map.ToGraph()
            .ShortestPaths(n => map.Lookup(n.Position) == 'E',
                n => map.Lookup(n.Position) == 'S' && n.Direction == Directions.East);
        var pathLength = forwardPaths.First(x => map.Lookup(x.Key.Position) == 'E').Value;
        return forwardPaths
            .Where(x => reversePaths.TryGetValue(x.Key, out var y) && x.Value + y == pathLength)
            .Select(x => x.Key.Position);
    }

    private static Graph<DirectedPoint> ToGraph(this Map map)
    {
        var graph = new Graph<DirectedPoint>();
        var allNodes = map.GetAllNodes().ToHashSet();
        foreach (var node in allNodes)
        {
            graph.AddNode(node, GetNeighbors(node));
        }

        return graph;

        IEnumerable<(DirectedPoint, int)> GetNeighbors(DirectedPoint n)
        {
            var forward = new DirectedPoint(n.Position + n.Direction, n.Direction);
            if (allNodes.Contains(forward))
            {
                yield return (forward, 1);
            }

            foreach (var q in n.Direction.Rotate())
            {
                var candidate = new DirectedPoint(n.Position + q, q);
                if (allNodes.Contains(candidate))
                {
                    yield return (candidate, 1001);
                }
            }
        }
    }
    private static bool IsWall(this Map map, Point p) =>
        map.Lookup(p) == '#';

    private static IEnumerable<Point> Rotate(this Point d)
    {
        if (d == Directions.North || d == Directions.South)
        {
            yield return Directions.East;
            yield return Directions.West;
        }
        else if (d == Directions.East || d == Directions.West)
        {
            yield return Directions.North;
            yield return Directions.South;
        }
        else
        {
            throw new ArgumentException($"Cannot rotate non-cardinal direction {d}", nameof(d));
        }
    }
}

internal record struct DirectedPoint(Point Position, Point Direction);
