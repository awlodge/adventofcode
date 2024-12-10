using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Map = Grid<int>;

public static class Day10
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day10.txt");

    [AdventOfCode2024(10, 1)]
    public static long RunPart1()
    {
        return ParseHelpers.ParseIntGridFile(InputPath).GetTotalScore();
    }

    public static int GetTotalScore(string input) =>
        ParseHelpers.ParseIntGrid(input).GetTotalScore();

    private static int GetTotalScore(this Map map) =>
        map.FindTrailheads().Sum(p => map.GetTrailheadScore(p));

    private static IEnumerable<Point> FindTrailheads(this Map map) =>
        map.Search(x => x == 0);

    private static int GetTrailheadScore(this Map map, Point start) =>
        map.FindTrails(start).Count();

    private static IEnumerable<Point> FindTrails(this Map map, Point start)
    {
        var x = map.Lookup(start);
        if (x == 9)
        {
            return [start];
        }

        return Directions.Cardinal
            .SelectMany(d =>
            {
                if (map.TryLookup(start + d, out var next) && next == x + 1)
                {
                    return map.FindTrails(start + d);
                }

                return [];
            })
            .Distinct();
    }
}
