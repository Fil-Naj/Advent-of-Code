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
        // if (File[startRow - 1][startCol] is '|' or '7' or 'F') Enqueue(startRow - 1, startCol); // Up
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

    /*
     * I hate it but essentially we search for gaps between pipes
     * For example, pipe 'L' creates a gap to its left and down and under
     * E.g., JL creates [space · pipe · space · pipe · space] meaning there is a gap between them
     * Iterate over all points in this new gap space. If gaps lead to the edge of the map, then not enclosed
     * Couldn't figure out best way to memoise and save computation, so we do complete BFS for every non-pipe
    */
    internal override void Part2()
    {
        Part1();
        bool[,] pipes = new bool[2 * N + 1, 2 * M + 1];
        
        // Map pipes
        for (var row = 0; row < N; row++)
        {
            for (var col = 0; col < M; col++)
            {
                if (!Pipe![row, col]) continue;
                var pipeType = File[row][col];

                var r = 2 * row + 1;
                var c = 2 * col + 1;
                pipes[r, c] = true;

                if (pipeType is '|')
                {
                    pipes[r - 1, c] = true;
                    pipes[r + 1, c] = true;
                }
                else if (pipeType is '-')
                {
                    pipes[r, c - 1] = true;
                    pipes[r, c + 1] = true;
                }
                else if (pipeType is 'L')
                {
                    pipes[r - 1, c] = true;
                    pipes[r, c + 1] = true;
                }
                else if (pipeType is 'J')
                {
                    pipes[r - 1, c] = true;
                    pipes[r, c - 1] = true;
                }
                else if (pipeType is '7')
                {
                    pipes[r + 1, c] = true;
                    pipes[r, c - 1] = true;
                }
                else if (pipeType is 'F')
                {
                    pipes[r + 1, c] = true;
                    pipes[r, c + 1] = true;
                }
            }
        }

        var area = 0L;
        for (var row = 0; row < N; row++)
        {
            for (var col = 0; col < M; col++)
            {
                if (row == 3 && col == 14)
                {

                }
                if (Pipe![row, col]) continue;

                var r = 2 * row + 1;
                var c = 2 * col + 1;

                Visited = new bool[2 * N + 1, 2 * M + 1];
                var n = 2 * N + 1;
                var m = 2 * M + 1;
                Queue<(int r, int c)> queue = new();
                queue.Enqueue((r, c));

                bool TryEnqueue(int r, int c, out bool isEnclosed)
                {
                    isEnclosed = true;
                    if (r < 0 || r >= n || c < 0 || c >= m)
                    {
                        isEnclosed = false;
                        return false;
                    }

                    if (Visited![r, c] || pipes[r, c]) return false;

                    Visited[r, c] = true;
                    queue.Enqueue((r, c));
                    return true;
                }

                var isAreaEnclosed = true;
                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();

                    TryEnqueue(node.r - 1, node.c, out var isUpEnclosed);
                    TryEnqueue(node.r + 1, node.c, out var isDownEnclosed);
                    TryEnqueue(node.r, node.c - 1, out var isRightEnclosed);
                    TryEnqueue(node.r, node.c + 1, out var isLeftEnclosed);

                    isAreaEnclosed = isAreaEnclosed && isUpEnclosed && isDownEnclosed && isRightEnclosed && isLeftEnclosed;
                }

                if (isAreaEnclosed)
                    area++;
            }
        }

        area.Dump("Part 2 Answer");
    }
}
