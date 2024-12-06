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
        return grid.GuardWalk();
    }

    public static int GuardWalk(string input)
    {
        var grid = Grid<char>.ParseCharGrid(input);
        return grid.GuardWalk();
    }

    private static int GuardWalk(this Grid<char> map) => map
        .GuardWalk(map.FindGuard(), Directions.North);

    private static Point FindGuard(this Grid<char> map) => map
        .Search()
        .First(p => map.Lookup(p) == Guard);

    private static int GuardWalk(this Grid<char> map, Point guardPosition, Point startDirection)
    {
        var direction = startDirection;
        var position = guardPosition;
        HashSet<Point> visited = [guardPosition];

        while (map.TryLookup(position + direction, out char x))
        {
            if (x == Obstacle)
            {
                direction = direction.Rotate();
            }
            position += direction;
            if (map.Contains(position))
            {
                visited.Add(position);
            }
            else
            {
                break;
            }
        }

        return visited.Count;
    }

    private static Point Rotate(this Point point) => point switch
    {
        Point(-1, 0) => Directions.East,
        Point(0, 1) => Directions.South,
        Point(1, 0) => Directions.West,
        Point(0, -1) => Directions.North,
        _ => throw new InvalidOperationException("Can only rotate cardinal directions")
    };
}
