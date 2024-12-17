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
        var unvisited = map.GetAllNodes()
            .ToDictionary(n => n, n => IsStart(n) ? 0 : -1);

        while (true)
        {
            var node = unvisited
                .Where(x => x.Value != -1)
                .MinBy(x => x.Value)
                .Key ?? throw new InvalidOperationException("No nodes left");

            if (map.Lookup(node.Position) == 'E')
            {
                return unvisited[node];
            }

            foreach (var (n, e) in GetNeighbors(node))
            {
                var newScore = unvisited[node] + e;
                if (unvisited[n] == -1 || newScore < unvisited[n])
                {
                    unvisited[n] = newScore;
                }
            }

            unvisited.Remove(node);
        }

        bool IsStart(DirectedPoint n) =>
            map.Lookup(n.Position) == 'S' && n.Direction == Directions.East;

        IEnumerable<(DirectedPoint, int)> GetNeighbors(DirectedPoint n)
        {
            var forward = new DirectedPoint(n.Position + n.Direction, n.Direction);
            if (unvisited.ContainsKey(forward))
            {
                yield return (forward, 1);
            }

            foreach (var q in n.Direction.Rotate())
            {
                var candidate = new DirectedPoint(n.Position + q, q);
                if (unvisited.ContainsKey(candidate))
                {
                    yield return (candidate, 1001);
                }
            }
        }
    }

    private static int GetLowestScoreOld(this Map map, Point start)
    {
        Dictionary<Point, Dictionary<Point, int>> visited = [];

        return GetLowestScoreInner(start, Directions.East, []);

        int GetLowestScoreInner(Point p, Point dir, HashSet<Point> route)
        {
            if (route.Contains(p))
            {
                return -1;
            }

            if (VisitedTryLookup(p, dir, out var prevScore))
            {
                return (int)prevScore!;
            }

            if (map.Lookup(p) == 'E')
            {
                VisitedSetValue(p, dir, 0);
                return 0;
            }

            List<int> scores = [];
            var newRoute = new HashSet<Point>(route)
            {
                p
            };
            var forward = p + dir;
            if (!map.IsWall(forward))
            {
                var score = GetLowestScoreInner(forward, dir, newRoute);
                if (score != -1)
                {
                    scores.Add(1 + score);
                }
            }

            foreach (var q in dir.Rotate())
            {
                var next = p + q;
                if (!map.IsWall(next))
                {
                    var score = GetLowestScoreInner(next, q, newRoute);
                    if (score != -1)
                    {
                        scores.Add(1001 + score);
                    }
                }
            }

            var result = scores.Count == 0 ? -1 : scores.Min();
            VisitedSetValue(p, dir, result);
            return result;
        }

        bool VisitedTryLookup(Point p, Point d, out int? s)
        {
            s = null;
            if (visited.TryGetValue(p, out var dirs) && dirs.TryGetValue(d, out var res))
            {
                s = res;
                return true;
            }
            return false;
        }

        void VisitedSetValue(Point p, Point d, int s)
        {
            if (visited.TryGetValue(p, out var dirs))
            {
                dirs[d] = s;
            }
            else
            {
                visited[p] = [];
                visited[p][d] = s;
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

internal record DirectedPoint(Point Position, Point Direction);
