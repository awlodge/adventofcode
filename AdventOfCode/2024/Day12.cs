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

    [AdventOfCode2024(12, 2)]
    public static long RunPart2()
    {
        return ParseHelpers.ParseCharGridFile(InputPath).GetTotalCost(withDiscount: true);
    }

    public static int GetTotalCost(string input)
    {
        return ParseHelpers.ParseCharGrid(input).GetTotalCost();
    }

    public static int GetDiscountedCost(string input)
    {
        return ParseHelpers.ParseCharGrid(input).GetTotalCost(withDiscount: true);
    }

    private static int GetTotalCost(this Map map, bool withDiscount = false)
    {
        return map.GetAllRegions().Sum(r => r.GetCost(withDiscount));
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

    private static int GetCost(this IEnumerable<Point> region, bool withDiscount = false)
    {
        return region.Count() * (withDiscount ? region.CountSides() : region.GetPerimeter());
    }

    private static int GetPerimeter(this IEnumerable<Point> region)
    {
        return region.Sum(p => region.GetPointPerimeter(p));
    }

    private static int GetPointPerimeter(this IEnumerable<Point> region, Point p)
    {
        return Directions.Cardinal.Count(d => region.HasSideAtPoint(p, d));
    }

    private static int CountSides(this IEnumerable<Point> region)
    {
        int count = 0;
        HashSet<Point> visited = [];

        foreach (var p in region)
        {
            CountSidesAtPoint(p);
        }

        return count;

        void CountSidesAtPoint(Point p)
        {
            foreach (var d in Directions.Cardinal.Where(x => region.HasSideAtPoint(p, x)))
            {
                Point neighbor = (d == Directions.North ||
                        d == Directions.South) ?
                    p + Directions.West : p + Directions.North;

                if (region.Contains(neighbor) && region.HasSideAtPoint(neighbor, d))
                {
                    if (!visited.Contains(neighbor))
                    {
                        CountSidesAtPoint(neighbor);
                    }
                    if (visited.Contains(p))
                    {
                        return;
                    }
                }
                else
                {
                    if (!visited.Contains(p))
                    {
                        count++;
                    }
                }
            }

            visited.Add(p);
        }
    }

    private static bool HasSideAtPoint(this IEnumerable<Point> region, Point p, Point dir) => !region.Contains(p + dir);
}
