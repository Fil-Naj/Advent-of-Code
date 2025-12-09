namespace AdventOfCode._2025.Solutions;

using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle04 : Puzzle2025<Puzzle04>
{
    internal override void Part1()
    {
        const int Required = 4;
        const char Roll = '@';

        var accessible = 0;
        var n = File.Length;
        var m = File[0].Length;

        int IsPaper(int r, int c)
        {
            if (r < 0 || r >= n || c < 0 || c >= m)
                return 0;

            return File[r][c] == Roll ? 1 : 0;

        }
        for (var r = 0; r < File.Length; r++)
        {
            for (var c = 0; c < File[r].Length; c++)
            {
                if (File[r][c] != Roll) continue;

                var around = IsPaper(r - 1, c)
                    + IsPaper(r + 1, c)
                    + IsPaper(r, c - 1)
                    + IsPaper(r, c + 1)
                    + IsPaper(r + 1, c + 1)
                    + IsPaper(r + 1, c - 1)
                    + IsPaper(r - 1, c - 1)
                    + IsPaper(r - 1, c + 1);

                if (around < Required)
                {
                    accessible++;
                }
            }
        }

        accessible.Dump("ans");
    }

    internal override void Part2()
    {
        const int Required = 4;
        const char Roll = '@';

        var grid = File.Select(l => l.ToCharArray()).ToArray();

        var accessible = 0;
        var n = grid.Length;
        var m = grid[0].Length;

        int IsPaper(int r, int c)
        {
            if (r < 0 || r >= n || c < 0 || c >= m)
                return 0;

            return grid[r][c] == Roll ? 1 : 0;
        }

        int removed;
        do
        {
            removed = 0;
            for (var r = 0; r < grid.Length; r++)
            {
                for (var c = 0; c < grid[r].Length; c++)
                {
                    if (grid[r][c] != Roll) continue;

                    var around = IsPaper(r - 1, c)
                        + IsPaper(r + 1, c)
                        + IsPaper(r, c - 1)
                        + IsPaper(r, c + 1)
                        + IsPaper(r + 1, c + 1)
                        + IsPaper(r + 1, c - 1)
                        + IsPaper(r - 1, c - 1)
                        + IsPaper(r - 1, c + 1);

                    if (around < Required)
                    {
                        removed++;
                        grid[r][c] = '.';
                    }
                }
            }

            accessible += removed;
        } while (removed > 0);

        accessible.Dump("ans");
    }
}
