namespace AdventOfCode._2025.Solutions;

using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle07 : Puzzle2025<Puzzle07>
{
    private const char Splitter = '^';
    internal override void Part1()
    {
        var s = this.File[0].IndexOf('S');
        var n = this.File.Length;
        var m = this.File[0].Length;
        bool[,] grid = new bool[n, m];
        grid[0, s] = true;

        var splits = 0;
        for (var r = 0; r < n - 1; r++)
        {
            for (var c = 0; c < m; c++)
            {
                if (grid[r, c])
                {
                    // Check if splitter
                    if (this.File[r][c] == Splitter)
                    {
                        splits++;
                        grid[r + 1, c - 1] = true;
                        grid[r + 1, c + 1] = true;
                    }
                    else
                    {
                        grid[r + 1, c] = true;
                    }
                }
            }
        }

        splits.Dump("ans");
    }

    internal override void Part2()
    {
        Dictionary<(int r, int c), long> dp = [];
        var s = this.File[0].IndexOf('S');

        var n = this.File.Length;
        var m = this.File[0].Length;

        long Dfs(int r, int c)
        {
            if (r == n - 1)
            {
                return 1;
            }

            if (dp.TryGetValue((r, c), out var val))
            {
                return val;
            }

            var next = this.File[r + 1][c] == Splitter
                ? Dfs(r + 1, c + 1) + Dfs(r + 1, c - 1)
                : Dfs(r + 1, c);

            dp[(r, c)] = next;
            return next;
        }

        Dfs(0, s).Dump("ans");
    }
}
