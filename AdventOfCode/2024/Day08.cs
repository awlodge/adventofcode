using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day08
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day08.txt");
    private static bool Debug = false;

    public static int RunPart1() =>
        ParseHelpers.ParseCharGridFile(InputPath).CountAntiNodes();

    public static int CountAntiNodes(string input) =>
        ParseHelpers.ParseCharGrid(input).CountAntiNodes();

    private static int CountAntiNodes(this Map map) => map.FindAllAntiNodes().Count;

    private static HashSet<Point> FindAllAntiNodes(this Map map)
    {
        HashSet<Point> result = [];
        var antennae = map.FindAntennae();

        foreach (var antennaGroup in antennae.Values)
        {
            foreach (var pair in antennaGroup.Pairs())
            {
                result.UnionWith(map.FindAntiNodes(pair[0], pair[1]));
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
}
