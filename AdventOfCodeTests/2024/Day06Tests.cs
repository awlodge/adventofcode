﻿using AdventOfCode._2024;
using FluentAssertions;

namespace AdventOfCodeTests._2024;

public class Day06Tests
{
    private const string TestInput = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

    [Fact]
    public void TestGuardWalk()
    {
        Day06.GuardWalk(TestInput).Should().Be(41);
    }

    [Fact]
    public void TestDay6Part1()
    {
        Day06.RunPart1().Should().Be(4580);
    }

    [Fact]
    public void TestCheckAddingObstacles()
    {
        Day06.CheckAddingObstacles(TestInput).Should().Be(6);
    }
}