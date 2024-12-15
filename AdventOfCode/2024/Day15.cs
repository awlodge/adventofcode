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

    public static int GetGpsSum(string input)
    {
        Parse(input, out var map, out var moves);
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

    public static void TestMoveRobot(string input)
    {
        var map = ParseHelpers.ParseCharGrid(input);
        var robot = map.FindRobot();
        map.Print();
        while (true)
        {
            var key = Console.ReadKey();
            Point? dir;
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
            robot = map.MoveRobot(robot, dir!);
            map.Print();
        }
    }

    private static Point FindRobot(this Map map) =>
        map.Search(x => x == '@').First();

    private static Point MoveRobot(this Map map, Point robot, Point direction)
    {
        List<Point> boxes = [];
        foreach (var p in map.Walk(robot, direction))
        {
            if (map.IsWall(p))
            {
                return robot;
            }

            if (map.IsBox(p))
            {
                boxes.Add(p);
            }
            else
            {
                var space = p;
                for (int i = boxes.Count - 1; i >= 0; i--)
                {
                    var b = boxes[i];
                    map.Swap(space, b);
                    space = b;
                }
                map.Swap(robot, space);
                return space;
            }
        }

        throw new InvalidOperationException("Escaped the grid!");
    }

    private static bool IsWall(this Map map, Point p) =>
        map.Lookup(p) == '#';

    private static bool IsBox(this Map map, Point p) =>
        map.Lookup(p) == 'O';

    private static Point GetDirection(char c) =>
        c switch
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
        map.Search().Where(p => map.IsBox(p)).Sum(p => p.GetGpsCoordinate());

    private static void ParseFile(string path, out Map map, out IEnumerable<Point> moves)
    {
        ParseInner(File.ReadLines(path), out map, out moves);
    }

    private static void Parse(string input, out Map map, out IEnumerable<Point> moves)
    {
        ParseInner(input.SplitLines(), out map, out moves);
    }

    private static void ParseInner(IEnumerable<string> lines, out Map map, out IEnumerable<Point> moves)
    {
        var blocks = lines.SplitBlocks().ToList();
        map = blocks[0].ParseCharGrid();
        moves = blocks[1].ParseMoves();
    }

    private static IEnumerable<Point> ParseMoves(this IEnumerable<string> lines)
    {
        return lines.SelectMany(l => l.ToCharArray()).Select(c => GetDirection(c));
    }
}
