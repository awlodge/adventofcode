using AdventOfCode._2024;
using System.Diagnostics;

Console.WriteLine("========================");
Console.WriteLine("Executing Day 1 (part 1)");
var sw = Stopwatch.StartNew();
var output = Day01.RunPart1();
sw.Stop();
Console.WriteLine(output);
Console.WriteLine($"Ran in {sw.Elapsed.TotalSeconds} seconds");

Console.WriteLine("========================");
Console.WriteLine("Executing Day 1 (part 2)");
sw = Stopwatch.StartNew();
output = Day01.RunPart2();
sw.Stop();
Console.WriteLine(output);
Console.WriteLine($"Ran in {sw.Elapsed.TotalSeconds} seconds");

