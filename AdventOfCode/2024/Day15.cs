using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day15
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day15.txt");

    [AdventOfCode2024(15, 1)]
    public static long RunPart1()
    {
        ParseFile(InputPath, out var map, out var moves);
        return map.MoveAndGetGpsSum(moves);
    }

    [AdventOfCode2024(15, 2)]
    public static long RunPart2()
    {
        ParseFile(InputPath, out var map, out var moves, expand: true);
        return map.MoveAndGetGpsSum(moves);
    }

    public static int GetGpsSum(string input)
    {
        Parse(input, out var map, out var moves);
        return map.MoveAndGetGpsSum(moves);
    }

    public static int GetGpsSumExpandedInput(string input)
    {
        Parse(input, out var map, out var moves, expand: true);
        return map.MoveAndGetGpsSum(moves);
    }

    private static int MoveAndGetGpsSum(this Map map, IEnumerable<Point> moves)
    {
        var robot = map.FindRobot();
        foreach (var move in moves)
        {
            robot = map.MoveRobot(robot, move);
        }

        return map.GetGpsSum();
    }

    public static void TestMoveRobot()
    {
        ParseFile(InputPath, out var map, out var _, expand: true);
        TestMoveRobot(map);
    }

    public static void TestMoveRobot(string input)
    {
        var map = ParseHelpers.ParseCharGrid(input);
        TestMoveRobot(map);
    }

    private static void TestMoveRobot(Map map)
    {
        var robot = map.FindRobot();
        map.Print();
        while (true)
        {
            var key = Console.ReadKey();
            Point dir;
            try
            {
                dir = GetDirection(key.KeyChar);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            Console.WriteLine($"Moving robot: {key.KeyChar}");
            robot = map.MoveRobot(robot, dir);
            map.Print();
        }
    }

    private static Point FindRobot(this Map map) =>
        map.Search(x => x == '@').First();

    private static Point MoveRobot(this Map map, Point robot, Point direction)
    {
        if (TryMove(robot))
        {
            robot += direction;
        }

        return robot;

        bool TryMove(Point p, bool execute = true)
        {
            var next = p + direction;
            if (!map.Contains(next))
            {
                throw new InvalidOperationException("Escaped the grid!");
            }

            if (map.IsWall(next))
            {
                return false;
            }

            if (map.IsBox(next))
            {
                if (TryMove(next, execute))
                {
                    if (execute)
                    {
                        map.Swap(p, next);
                    }
                    return true;
                }
                return false;
            }

            if (map.IsExpandedBox(next))
            {
                if (direction == Directions.East || direction == Directions.West)
                {
                    if (TryMove(next, execute))
                    {
                        if (execute)
                        {
                            map.Swap(p, next);
                        }
                        return true;
                    }
                    return false;
                }

                var partner = map.GetPartner(next);
                if (TryMove(next, execute: false) && TryMove(partner, execute: false))
                {
                    if (execute)
                    {
                        TryMove(next, true);
                        TryMove(partner, true);
                        map.Swap(p, next);
                    }
                    return true;
                }
                return false;
            }

            if (execute)
            {
                map.Swap(p, next);
            }
            return true;
        }
    }

    private static bool IsWall(this Map map, Point p) =>
        map.Lookup(p) == '#';

    private static bool IsBox(this Map map, Point p) =>
        map.Lookup(p) == 'O';

    private static bool IsExpandedBox(this Map map, Point p)
    {
        var x = map.Lookup(p);
        return x == '[' || x == ']';
    }

    private static bool IsExpandedBoxLeft(this Map map, Point p) =>
        map.Lookup(p) == '[';

    private static Point GetPartner(this Map map, Point p)
    {
        return map.Lookup(p) switch
        {
            '[' => p + Directions.East,
            ']' => p + Directions.West,
            _ => throw new ArgumentException($"Not a box: {map.Lookup(p)}", nameof(p))
        };
    }

    private static Point GetDirection(char c) => c switch
    {
        '^' => Directions.North,
        '>' => Directions.East,
        'v' => Directions.South,
        '<' => Directions.West,
        _ => throw new ArgumentException($"'{c}' is not a valid direction", nameof(c))
    };

    private static int GetGpsCoordinate(this Point p) =>
        (100 * p.Y) + p.X;

    private static int GetGpsSum(this Map map) =>
        map.Search()
            .Where(p => map.IsBox(p) || map.IsExpandedBoxLeft(p))
            .Sum(p => p.GetGpsCoordinate());

    private static void ParseFile(string path, out Map map, out IEnumerable<Point> moves, bool expand = false)
    {
        ParseInner(File.ReadLines(path), out map, out moves, expand);
    }

    private static void Parse(string input, out Map map, out IEnumerable<Point> moves, bool expand = false)
    {
        ParseInner(input.SplitLines(), out map, out moves, expand);
    }

    private static void ParseInner(IEnumerable<string> lines, out Map map, out IEnumerable<Point> moves, bool expand = false)
    {
        var blocks = lines.SplitBlocks().ToList();
        map = expand ? blocks[0].ParseExpandedMap() : blocks[0].ParseCharGrid();
        moves = blocks[1].ParseMoves();
    }

    private static IEnumerable<Point> ParseMoves(this IEnumerable<string> lines)
    {
        return lines.SelectMany(l => l.ToCharArray()).Select(c => GetDirection(c));
    }

    private static Map ParseExpandedMap(this IEnumerable<string> lines)
    {
        return new Map(lines
            .Select(l => l
                .ToCharArray()
                .SelectMany(x => ExpandChar(x))
                .ToList())
            .ToList());

        static char[] ExpandChar(char x) => x switch
        {
            '#' => ['#', '#'],
            'O' => ['[', ']'],
            '.' => ['.', '.'],
            '@' => ['@', '.'],
            _ => throw new ArgumentException($"Invalid character {x}", nameof(x))
        };
    }
}
