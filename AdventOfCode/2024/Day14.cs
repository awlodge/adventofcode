using AdventOfCode.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day14
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day14.txt");
    private static readonly Map Map = Map.Generate('.', 101, 103);
    private const int Time = 100;
    private static readonly bool Debug = false;

    [AdventOfCode2024(14, 1)]
    public static long RunPart1()
    {
        return ParseFile(InputPath).GetSafetyFactor(Map, Time);
    }

    [AdventOfCode2024(14, 2)]
    public static long RunPart2()
    {
        return ParseFile(InputPath).FindChristmasTree(Map);
    }

    public static int GetSafetyFactor(string input, int mapWidth, int mapHeight, int time)
    {
        return Parse(input).GetSafetyFactor(Map.Generate('.', mapWidth, mapHeight), time);
    }

    private static int FindChristmasTree(this IEnumerable<Robot> robots, Map map)
    {
        robots = robots.ToList();
        int i = 0;
        while (true)
        {
            if (robots.HasChristmasTree())
            {
                if (Debug)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Found christmas tree at {i}");
                    robots.Print(map);
                }
                return i;
            }
            else
            {
                if (Debug)
                {
                    if (i % 100 == 1)
                    {
                        Console.Write(".");
                    }
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine();
                    }
                }
            }
            i++;
            robots.AsParallel().ForAll(r => r.Move(Map, 1));
        }
    }

    public static string MoveRobot(string input, int mapWidth, int mapHeight, int time)
    {
        return Robot.Parse(input).Move(Map.Generate('.', mapWidth, mapHeight), time).ToString();
    }

    private static Robot Move(this Robot robot, Map map, int time)
    {
        var deltaX = robot.Velocity.X * time;
        var deltaY = robot.Velocity.Y * time;
        Point newPos = new(PosMod(robot.Position.X + deltaX, map.Width),
            PosMod(robot.Position.Y + deltaY, map.Height));
        robot.Position = newPos;
        return robot;
    }

    private static int GetSafetyFactor(this IEnumerable<Robot> robots, Map map, int time)
    {
        int[] counts = [0, 0, 0, 0];
        robots
            .AsParallel()
            .Select(r => r.Move(map, time))
            .ForAll(r =>
            {
                var quad = map.GetQuadrant(r.Position);
                if (quad != -1)
                {
                    Interlocked.Increment(ref counts[quad]);
                }
            });

        return counts.Aggregate(1, (acc, val) => acc * val);
    }

    private static bool HasChristmasTree(this IEnumerable<Robot> robots)
    {
        var robotPositions = robots.Select(r => r.Position).ToHashSet();
        return robotPositions.Any(p => (
            IsPoint(p)
            && IsPoint(p + Directions.SouthWest)
            && IsPoint(p + Directions.SouthEast)
        ));

        bool IsPoint(Point p) => (
            robotPositions.Contains(p + Directions.SouthWest)
            && robotPositions.Contains(p + Directions.South)
            && robotPositions.Contains(p + Directions.SouthEast)
            );
    }

    private static int GetQuadrant(this Map map, Point p)
    {
        var midWidth = map.Width / 2;
        var midHeight = map.Height / 2;

        if (p.X == midWidth || p.Y == midHeight)
        {
            return -1;
        }

        if (p.X < midWidth)
        {
            return p.Y < midHeight ? 0 : 1;
        }
        else
        {
            return p.Y < midHeight ? 2 : 3;
        }
    }

    private static int PosMod(int n, int mod)
    {
        int r = n % mod;
        return r < 0 ? r + mod : r;
    }

    private static void Print(this IEnumerable<Robot> robots, Map map)
    {
        map.Print(robots
            .Select(r => r.Position)
            .Distinct()
            .ToDictionary(p => p, p => 'X'));
    }

    private static IEnumerable<Robot> ParseFile(string path)
        => ParseInner(File.ReadLines(path));

    private static IEnumerable<Robot> Parse(string input)
        => ParseInner(input.SplitLines());

    private static IEnumerable<Robot> ParseInner(IEnumerable<string> lines)
        => lines.Select(line => Robot.Parse(line));
}

internal partial record Robot(Point Position, Point Velocity)
{
    public Point Position { get; set; } = Position;

    public Point Velocity { get; init; } = Velocity;

    [GeneratedRegex("p=(\\d+),(\\d+) v=([-\\d]+),([-\\d]+)")]
    private static partial Regex RobotRegex();

    public override string ToString() => $"p={Position.X},{Position.Y} v={Velocity.X},{Velocity.Y}";

    public static Robot Parse(string input)
    {
        var match = RobotRegex().Match(input);
        return new Robot(
            new Point(Int32.Parse(match.Groups[1].Value),
                Int32.Parse(match.Groups[2].Value)),
            new Point(Int32.Parse(match.Groups[3].Value),
                Int32.Parse(match.Groups[4].Value)));
    }
}
