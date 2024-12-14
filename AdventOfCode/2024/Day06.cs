using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day06
{
    private const char Obstacle = '#';
    private const char Guard = '^';

    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day06.txt");

    [AdventOfCode2024(6, 1)]
    public static long RunPart1()
    {
        var grid = ParseHelpers.ParseCharGridFile(InputPath);
        return grid.CountGuardWalk();
    }

    [AdventOfCode2024(6, 2)]
    public static long RunPart2()
    {
        var grid = ParseHelpers.ParseCharGridFile(InputPath);
        return grid.CheckAddingObstacles();
    }

    public static int GuardWalk(string input)
    {
        var grid = ParseHelpers.ParseCharGrid(input);
        return grid.CountGuardWalk();
    }

    public static int CheckAddingObstacles(string input)
    {
        var grid = ParseHelpers.ParseCharGrid(input);
        return grid.CheckAddingObstacles();
    }

    private static int CountGuardWalk(this Grid<char> map) => map
        .CountGuardWalk(new PosDef(map.FindGuard(), Directions.North));

    private static Point FindGuard(this Grid<char> map)
    {
        var guard = map
            .Search()
            .First(p => map.Lookup(p) == Guard);
        return guard;
    }

    private static int CountGuardWalk(this Grid<char> map, PosDef guard)
    {
        return map
            .GuardWalk(guard)
            .Select(p => p.Position)
            .Distinct()
            .Count();
    }

    private static int CheckAddingObstacles(this Grid<char> map) =>
        map.CheckAddingObstacles(new PosDef(map.FindGuard(), Directions.North));

    private static int CheckAddingObstacles(this Grid<char> map, PosDef guard)
    {
        int count = 0;
        map.GuardWalk(guard)
            .Skip(1)
            .Select(p => p.Position)
            .Distinct()
            .AsParallel()
            .ForAll(p =>
            {
                if (map.GuardWalk(guard, extraObstacle: p).CheckLoop())
                {
                    Interlocked.Increment(ref count);
                }
            });

        return count;
    }

    private static IEnumerable<PosDef> GuardWalk(this Grid<char> map, PosDef guardPosition, Point? extraObstacle = default)
    {
        var direction = guardPosition.Direction with { };
        var position = guardPosition.Position with { };

        yield return guardPosition;

        while (true)
        {
            var candidate = position + direction;
            if ((map.TryLookup(candidate, out var x) && x == Obstacle) || (extraObstacle == candidate))
            {
                direction = direction.Rotate();
                continue;
            }
            position = candidate;
            if (!map.Contains(position))
            {
                break;
            }

            yield return new PosDef(position, direction);
        }
    }

    private static Point Rotate(this Point point) => point switch
    {
        Point(0, -1) => Directions.East,
        Point(1, 0) => Directions.South,
        Point(0, 1) => Directions.West,
        Point(-1, 0) => Directions.North,
        _ => throw new InvalidOperationException("Can only rotate cardinal directions")
    };

    private static bool CheckLoop(this IEnumerable<PosDef> walk)
    {
        HashSet<PosDef> visited = [];
        foreach (var posdef in walk)
        {
            if (visited.Contains(posdef))
            {
                return true;
            }
            visited.Add(posdef);
        }
        return false;
    }
}

internal record PosDef(Point Position, Point Direction);
