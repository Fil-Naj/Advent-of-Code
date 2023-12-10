using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle10 : Puzzle2023<Puzzle10>
{
    private const char Start = 'S';

    private int N;
    private int M;

    // Defintions
    // x += 1 => move right
    // y += 1 => move down
    private readonly Dictionary<char, (int x, int y)[]> Directions = new()
    {
        { '|', [(0, 1), (0, -1)] },
        { '-', [(1, 0), (-1, 0)] },
        { 'L', [(1, 0), (0, -1)] },
        { 'J', [(-1, 0), (0, -1)] },
        { '7', [(-1, 0), (0, 1)] },
        { 'F', [(1, 0), (0, 1)] },
        { '.', [] }
    };

    private readonly Queue<(int r, int c)> Queue = new();
    private bool[,]? Pipe;
    private bool[,]? Visited;

    internal override void Part1()
    {
        N = File.Length;
        M = File[0].Length;

        // Find S
        int startRow = -1;
        int startCol = -1;
        for (var row = 0; row < N; row++)
        {
            for (var col = 0; col < M; col++)
            {
                if (File[row][col] == Start)
                {
                    startRow = row;
                    startCol = col;
                    break;
                }
            }
            if (startRow > -1 && startCol > -1) break;
        }

        Pipe = new bool[N,M];
        Pipe[startRow, startCol] = true;

        // Kinda ugly but eh
        if (File[startRow - 1][startCol] is '|' or '7' or 'F') Enqueue(startRow - 1, startCol); // Up
        if (File[startRow + 1][startCol] is '|' or 'L' or 'J') Enqueue(startRow + 1, startCol); // Down
        if (File[startRow][startCol - 1] is '-' or 'L' or 'F') Enqueue(startRow, startCol - 1); // Left
        if (File[startRow][startCol + 1] is '-' or '7' or 'J') Enqueue(startRow, startCol + 1); // Right

        // BFS BABYYYYY (with step counting)
        var steps = 0L;
        while (Queue.Count > 0)
        {
            var numOptions = Queue.Count;
            for (int i = 0; i < numOptions; i++)
            {
                var (r, c) = Queue.Dequeue();
                var possible = Directions[File[r][c]];
                foreach (var (x, y) in possible)
                {
                    if (Pipe[r + y, c + x]) continue;

                    Enqueue(r + y, c + x);
                }
            }
            steps++;
        }

        steps.Dump("Part 1 Answer");
    }

    private void Enqueue(int row, int col)
    {
        Queue.Enqueue((row, col));
        Pipe![row, col] = true;
    }

    internal override void Part2()
    {
        // TODO
    }
}
