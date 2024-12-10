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

    [AdventOfCode2024(10, 2)]
    public static long RunPart2()
    {
        return ParseHelpers.ParseIntGridFile(InputPath).GetTotalRating();
    }

    public static int GetTotalScore(string input) =>
        ParseHelpers.ParseIntGrid(input).GetTotalScore();

    public static int GetTotalRating(string input) =>
        ParseHelpers.ParseIntGrid(input).GetTotalRating();

    private static int GetTotalScore(this Map map) =>
        map.FindTrailheads().Sum(p => map.GetTrailheadScore(p));

    private static int GetTotalRating(this Map map) =>
        map.FindTrailheads().Sum(p => map.GetTrailheadRating(p));

    private static IEnumerable<Point> FindTrailheads(this Map map) =>
        map.Search(x => x == 0);

    private static int GetTrailheadScore(this Map map, Point start) =>
        map.FindTrails(start).Distinct().Count();

    private static int GetTrailheadRating(this Map map, Point start) =>
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
            });
    }
}
