using AdventOfCode.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

public static class Day14
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day14.txt");
    private static readonly MapSize Map = new(101, 103);
    private const int Time = 100;

    [AdventOfCode2024(14, 1)]
    public static long RunPart1()
    {
        return ParseFile(InputPath).GetSafetyFactor(Map, Time);
    }

    public static int GetSafetyFactor(string input, int mapWidth, int mapHeight, int time)
    {
        return Parse(input).GetSafetyFactor(new MapSize(mapWidth, mapHeight), time);
    }

    public static string MoveRobot(string input, int mapWidth, int mapHeight, int time)
    {
        return Robot.Parse(input).Move(new MapSize(mapWidth, mapHeight), time).ToString();
    }

    private static Robot Move(this Robot robot, MapSize map, int time)
    {
        var delta = robot.Velocity * time;
        Point newPos = new(PosMod(robot.Position.X + delta.X, map.Width),
            PosMod(robot.Position.Y + delta.Y, map.Height));
        return robot with { Position = newPos };
    }

    private static int GetSafetyFactor(this IEnumerable<Robot> robots, MapSize map, int time)
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

    private static int GetQuadrant(this MapSize map, Point p)
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

    private static IEnumerable<Robot> ParseFile(string path)
        => ParseInner(File.ReadLines(path));

    private static IEnumerable<Robot> Parse(string input)
        => ParseInner(input.SplitLines());

    private static IEnumerable<Robot> ParseInner(IEnumerable<string> lines)
        => lines.Select(line => Robot.Parse(line));
}

internal partial record Robot(Point Position, Point Velocity)
{
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

internal record MapSize(int Width, int Height);
