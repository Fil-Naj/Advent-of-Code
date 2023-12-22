using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal partial class Puzzle20 : Puzzle2023<Puzzle20>
{
    private char[][]? Grid;

    internal override void Part1()
    {
        Grid = File.Select(s => s.ToArray()).ToArray();
        var n = Grid.Length;
        var m = Grid[0].Length;

        Point? start = null;
        bool[,] rocks = new bool[n, m];
        for (int r = 0; r < Grid.Length; r++)
        {
            for (int c = 0; c < Grid[r].Length; c++)
            {
                if (Grid[r][c] == 'S')
                    start = new(r, c);
                else if (Grid[r][c] == '#')
                    rocks[r, c] = true;
            }
        }

        bool[,] stepLocations = new bool[n, m];
        var numSteps = 64;
        Queue<Point> queue = new();
        queue.Enqueue(start!);
        stepLocations[start!.Row, start.Col] = false;

        void TryEnqueue(Point p)
        {
            if (p.Row < 0 || p.Row >= n || p.Col < 0 || p.Col >= m) return;
            if (rocks[p.Row % n, p.Col % m] || stepLocations[p.Row % n, p.Col % m]) return;

            stepLocations[p.Row % n, p.Col % m] = true;
            queue.Enqueue(p);
        }

        while (numSteps-- > 0)
        {
            var plotsInStep = queue.Count;
            for (int i = 0; i < plotsInStep; i++)
            {
                var step = queue.Dequeue();
                stepLocations[step.Row % n, step.Col % m] = false;

                TryEnqueue(new(step.Row + 1, step.Col));
                TryEnqueue(new(step.Row - 1, step.Col));
                TryEnqueue(new(step.Row, step.Col + 1));
                TryEnqueue(new(step.Row, step.Col - 1));
            }
        }

        queue.Count.Dump("Part 1 Answer");
        // Grid.Dump(delimiter: null);
    }

    internal override void Part2()
    {

    }

    private class Point(int row, int col)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;

        public override bool Equals(object? obj)
        {
            if (obj is not Point point2) return false;

            return Row == point2.Row && Col == point2.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
    }
}
