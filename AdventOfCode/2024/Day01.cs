﻿using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day01
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day01.txt");

    [AdventOfCode2024(1, 1)]
    public static long RunPart1()
    {
        ParseFile(InputPath, out var colA, out var colB);
        return CalculateDistance([.. colA], [.. colB]);
    }

    [AdventOfCode2024(1, 2)]

    public static long RunPart2()
    {
        ParseFile(InputPath, out var colA, out var colB);
        return CalculateSimilarityScore(colA, colB);
    }

    public static int CalculateDistance(string input)
    {
        Parse(input, out var colA, out var colB);
        return CalculateDistance([.. colA], [.. colB]);
    }

    public static int CalculateDistance(List<int> colA, List<int> colB)
    {
        static int distance(int a, int b) => a >= b ? a - b : b - a;
        colA.Sort();
        colB.Sort();
        return colA
            .Zip(colB)
            .Select(x => distance(x.First, x.Second))
            .Sum();
    }

    public static int CalculateSimilarityScore(string input)
    {
        Parse(input, out var colA, out var colB);
        return CalculateSimilarityScore(colA, colB);
    }

    public static int CalculateSimilarityScore(IList<int> colA, IList<int> colB)
    {
        Dictionary<int, int> colBCounter = [];
        foreach (var x in colB)
        {
            var value = colBCounter.GetValueOrDefault(x);
            colBCounter[x] = ++value;
        }

        return colA
            .Select(x => x * colBCounter.GetValueOrDefault(x))
            .Sum();
    }

    public static void Parse(string input, out IList<int> colA, out IList<int> colB)
    {
        ParseInner(input.SplitLines(), out colA, out colB);
    }

    public static void ParseFile(string path, out IList<int> colA, out IList<int> colB)
    {
        ParseInner(File.ReadLines(path), out colA, out colB);
    }

    private static void ParseInner(IEnumerable<string> input, out IList<int> colA, out IList<int> colB)
    {
        colA = [];
        colB = [];

        foreach (var line in input)
        {
            var cols = line.ParseAsInts().ToList();
            if (cols.Count != 2)
            {
                throw new ArgumentException($"Row has insufficient columns", nameof(input));
            }
            colA.Add(cols[0]);
            colB.Add(cols[1]);
        }
    }
}
