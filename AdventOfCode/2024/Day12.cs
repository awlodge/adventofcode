using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<char>;

public static class Day12
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day12.txt");

    [AdventOfCode2024(12, 1)]
    public static long RunPart1()
    {
        return ParseHelpers.ParseCharGridFile(InputPath).GetTotalCost();
    }

    public static int GetTotalCost(string input)
    {
        return ParseHelpers.ParseCharGrid(input).GetTotalCost();
    }

    private static int GetTotalCost(this Map map)
    {
        return map.GetAllRegions().Sum(r => map.GetRegionCost(r));
    }

    private static IEnumerable<HashSet<Point>> GetAllRegions(this Map map)
    {
        HashSet<Point> searched = [];
        foreach (var p in map.Search())
        {
            if (!searched.Contains(p))
            {
                var region = map.GetRegion(p);
                searched.UnionWith(region);
                yield return region;
            }
        }
    }

    private static HashSet<Point> GetRegion(this Map map, Point p)
    {
        HashSet<Point> region = [p];
        Queue<Point> queue = new();
        queue.Enqueue(p);
        var x = map.Lookup(p);

        while (queue.TryDequeue(out var q))
        {
            foreach (var d in Directions.Cardinal)
            {
                var r = q + d;
                if ((!region.Contains(r)) &&
                    map.TryLookup(r, out var y) &&
                    y == x)
                {
                    region.Add(r);
                    queue.Enqueue(r);
                }
            }
        }

        return region;
    }

    private static int GetRegionCost(this Map map, IEnumerable<Point> region)
    {
        return region.Count() * region.Sum(p => map.GetPointPerimeter(p));
    }

    private static int GetPointPerimeter(this Map map, Point p)
    {
        var x = map.Lookup(p);
        return Directions.Cardinal.Count(d =>
        {
            return !(map.TryLookup(p + d, out var y) && y == x);
        });
    }
}
