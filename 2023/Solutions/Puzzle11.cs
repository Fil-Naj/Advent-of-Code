using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle11 : Puzzle2023<Puzzle11>
{
    private const char Galaxy = '#';

    internal override void Part1()
    {
        SumGalaxyPairs(2).Dump("Part 1 Answer");
    }

    internal override void Part2()
    {
        SumGalaxyPairs(1000000).Dump("Part 2 Answer");
    }

    private long SumGalaxyPairs(long expansionSize)
    {
        var n = File.Length;
        var m = File[0].Length;
        char[,] map = new char[n, m];

        var rowHasGalaxy = new bool[n];
        var colHasGalaxy = new bool[m];
        List<(int r, int c)> galaxes = [];

        for (var row = 0; row < n; row++)
        {
            for (var col = 0; col < m; col++)
            {
                map[row, col] = File[row][col];
                if (File[row][col] != Galaxy) continue;

                rowHasGalaxy[row] = true;
                colHasGalaxy[col] = true;
                galaxes.Add((row, col));
            }
        }
        var sum = 0L;
        foreach (var (r, c) in galaxes)
        {
            bool[,] visited = new bool[n, m];
            // Replace with blank space to not care about it in the next pair search
            map[r, c] = '.';

            Queue<Path> queue = new();
            visited[r, c] = true;
            queue.Enqueue(new Path(r, c, 0));

            void TryEnqueue(int row, int col, long steps)
            {
                // If out of bounds, stop
                if (row < 0 || col < 0 || row >= n || col >= m) return;

                // If been there done that, stop
                if (visited[row, col]) return;

                var isExpanded = !rowHasGalaxy[row] || !colHasGalaxy[col];
                visited[row, col] = true;
                queue.Enqueue(new Path(row, col, steps + (isExpanded ? expansionSize : 1)));
            }

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();

                // If we git galaxy, then big W lets goooo count them steps babyyyy
                if (map[path.Row, path.Col] == Galaxy)
                {
                    sum += path.Steps;
                }

                // Continue exploring
                TryEnqueue(path.Row - 1, path.Col, path.Steps); // Up
                TryEnqueue(path.Row + 1, path.Col, path.Steps); // Down
                TryEnqueue(path.Row, path.Col - 1, path.Steps); // Right
                TryEnqueue(path.Row, path.Col + 1, path.Steps); // Left
            }
        }

        return sum;
    }

    public class Path(int row, int col, long steps)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;
        public long Steps { get; set; } = steps;
    }
}
