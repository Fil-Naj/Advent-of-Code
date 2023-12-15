using System.Diagnostics;
using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle14 : Puzzle2023<Puzzle14>
{
    private readonly Dictionary<SlideDirection, (int row, int col)> _slideDirections = new()
    {
        { SlideDirection.North, (-1, 0) },
        { SlideDirection.South, (1, 0) },
        { SlideDirection.East,  (0, 1) },
        { SlideDirection.West,  (0, -1) },
    };

    private char[][]? Grid;
    private int Rows;
    private int Cols;

    internal override void Part1()
    {
        Grid = File.Select(r => r.ToArray()).ToArray();
        Rows = Grid.Length;
        Cols = Grid[0].Length;

        List<Rock> rocks = [];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if (Grid[i][j] != 'O') continue;

                rocks.Add(new Rock()
                {
                    Row = i,
                    Col = j,
                });
            }
        }

        foreach (var rock in rocks)
            Slide(rock, SlideDirection.North);

        var load = rocks.Sum(rock => Rows - rock.Row);

        load.Dump("Part 1 Answer");
    }

    private void Slide(Rock rock, SlideDirection direction)
    {
        var (row, col) = _slideDirections[direction];

        // Get ready to move by removing the rock from initial location
        Grid![rock.Row][rock.Col] = '.';

        while (CanSlide(rock.Row + row, rock.Col + col))
            rock.Move(row, col);

        Grid[rock.Row][rock.Col] = 'O';
    }

    private bool CanSlide(int r, int c)
    {
        // If out of bounds, then nah probs can't slide that way
        if (r < 0 || r >= Rows || c < 0 || c >= Cols) return false;

        return Grid![r][c] == '.';
    }

    internal override void Part2()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Grid = File.Select(r => r.ToArray()).ToArray();
        Rows = Grid.Length;
        Cols = Grid[0].Length;

        List<Rock> rocks = [];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if (Grid[i][j] != 'O') continue;

                rocks.Add(new Rock()
                {
                    Row = i,
                    Col = j,
                });
            }
        }

        var repetitions = 1_000_000_000;

        Dictionary<string, int> dp = [];
        for (int i = 0; i < repetitions; i++)
        {
            var key = string.Join('|', rocks.Select(r => $"{r.Row},{r.Col}"));

            if (dp.TryGetValue(key, out var reps))
            {
                // WE FOUND A LOOP!
                // When find a loop:
                // - Find out how long the loop is
                // - Find out how many loops are left in the repetitions
                // - Go to the last instance of this loop
                var loopLength = i - reps;
                dp[key] = i;
                var repsToSkip = (repetitions - i) / loopLength;
                i += repsToSkip * loopLength;
            }

            dp[key] = i;

            // We want the rocks at the top row moving UP first
            rocks = [.. rocks.OrderBy(r => r.Row)];
            foreach (var rock in rocks)
                Slide(rock, SlideDirection.North);

            // We want the rocks furthest left moving LEFT first
            rocks = [.. rocks.OrderBy(r => r.Col)];
            foreach (var rock in rocks)
                Slide(rock, SlideDirection.West);

            // We want the rocks furthest down moving DOWN first
            rocks = [.. rocks.OrderByDescending(r => r.Row)];
            foreach (var rock in rocks)
                Slide(rock, SlideDirection.South);

            // We want the rocks furthest right moving DOWN first
            rocks = [.. rocks.OrderByDescending(r => r.Col)];
            foreach (var rock in rocks)
                Slide(rock, SlideDirection.East);
        }

        foreach (var line in Grid)
            line.Dump();

        var load = rocks.Sum(rock => Rows - rock.Row);
        sw.ElapsedMilliseconds.Dump("Time (ms)");
        load.Dump("Part 2 Answer");
    }

    private class Rock
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public void Move(int r, int c)
        {
            Row += r;
            Col += c;
        }
    }

    private enum SlideDirection
    {
        North,
        South,
        East,
        West
    }
}
