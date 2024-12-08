using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day06
{
    private const char Obstacle = '#';
    private const char Guard = '^';

    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day06.txt");

    public static int RunPart1()
    {
        var grid = Grid<char>.ParseCharGridFile(InputPath);
        return grid.CountGuardWalk();
    }

    public static int RunPart2()
    {
        var grid = Grid<char>.ParseCharGridFile(InputPath);
        return grid.CheckAddingObstacles();
    }

    public static int GuardWalk(string input)
    {
        var grid = Grid<char>.ParseCharGrid(input);
        return grid.CountGuardWalk();
    }

    public static int CheckAddingObstacles(string input)
    {
        var grid = Grid<char>.ParseCharGrid(input);
        return grid.CheckAddingObstacles();
    }

    private static int CountGuardWalk(this Grid<char> map) => map
        .CountGuardWalk(new PosDef(map.FindGuard(), Directions.North));

    private static Point FindGuard(this Grid<char> map) => map
        .Search()
        .First(p => map.Lookup(p) == Guard);

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
        return map
            .GuardWalk(guard)
            .Select(p => p.Position)
            .Distinct()
            .Where(p => map.GuardWalk(guard, extras: [p]).CheckLoop())
            .Count();
    }

    private static IEnumerable<PosDef> GuardWalk(this Grid<char> map, PosDef guardPosition, IList<Point>? extras = default)
    {
        var direction = guardPosition.Direction;
        var position = guardPosition.Position;

        yield return guardPosition;

        while (map.TryLookup(position + direction, out char x))
        {
            if (x == Obstacle || (extras?.Contains(position + direction) ?? false))
            {
                direction = direction.Rotate();
            }
            position += direction;
            if (map.Contains(position))
            {
                yield return new PosDef(position, direction);
            }
            else
            {
                break;
            }
        }
    }

    private static Point Rotate(this Point point) => point switch
    {
        Point(-1, 0) => Directions.East,
        Point(0, 1) => Directions.South,
        Point(1, 0) => Directions.West,
        Point(0, -1) => Directions.North,
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
