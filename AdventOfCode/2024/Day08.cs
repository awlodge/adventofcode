using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day08
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day08.txt");
    private static readonly bool Debug = false;

    [AdventOfCode2024(8, 1)]
    public static long RunPart1() =>
        ParseHelpers.ParseCharGridFile(InputPath).CountAntiNodes();

    [AdventOfCode2024(8, 2)]
    public static long RunPart2() =>
        ParseHelpers.ParseCharGridFile(InputPath).CountAntiNodes(withRH: true);

    public static int CountAntiNodes(string input) =>
        ParseHelpers.ParseCharGrid(input).CountAntiNodes();

    public static int CountAntiNodesWithRH(string input) =>
        ParseHelpers.ParseCharGrid(input).CountAntiNodes(withRH: true);

    private static int CountAntiNodes(this Map map, bool withRH = false) => map.FindAllAntiNodes(withRH).Count;

    private static HashSet<Point> FindAllAntiNodes(this Map map, bool withRH = false)
    {
        HashSet<Point> result = [];
        var antennae = map.FindAntennae();

        foreach (var antennaGroup in antennae.Values)
        {
            foreach (var pair in antennaGroup.Pairs())
            {
                var antiNodes = withRH ?
                    map.FindAntiNodesRH(pair[0], pair[1]) :
                    map.FindAntiNodes(pair[0], pair[1]);
                result.UnionWith(antiNodes);
            }
        }

        if (Debug)
        {
            Dictionary<Point, char> subs = [];
            foreach (var p in result)
            {
                subs[p] = '#';
            }
            map.Print(subs);
        }

        return result;
    }

    private static bool IsAntenna(this char x) => Char.IsLetterOrDigit(x);

    private static Dictionary<char, HashSet<Point>> FindAntennae(this Map map)
    {
        Dictionary<char, HashSet<Point>> result = [];
        foreach (var p in map.Search())
        {
            var x = map.Lookup(p);
            if (x.IsAntenna())
            {
                var y = result.GetValueOrDefault(x, []);
                y.Add(p);
                result[x] = y;
            }
        }

        return result;
    }

    private static HashSet<Point> FindAntiNodes(this Map map, Point a, Point b)
    {
        var step = b - a;
        Point[] candidates = [b + step, a - step];
        return candidates.Where(p => map.Contains(p)).ToHashSet();
    }

    private static HashSet<Point> FindAntiNodesRH(this Map map, Point a, Point b)
    {
        HashSet<Point> result = [];
        var step = b - a;
        Point[] startPoints = [b, a];
        foreach (var startPoint in startPoints)
        {
            var p = startPoint;
            while (map.Contains(p))
            {
                result.Add(p);
                p += step;
            }

            step = -step;
        }

        return result;
    }
}
