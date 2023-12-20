using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle18 : Puzzle2023<Puzzle18>
{
    private static readonly Dictionary<string, (int x, int y)> _directions = new()
    {
        { "U", (0, -1) },
        { "D", (0, 1) },
        { "L", (-1, 0) },
        { "R", (1, 0) },
    };
    internal override void Part1()
    {
        HashSet<(int x, int y)> points = [(0,0)];
        var x = 0;
        var y = 0;
        foreach (var line in File)
        {
            var instructions = line.Split(' ');
            var delta = _directions[instructions[0]];
            var numSteps = int.Parse(instructions[1]);

            for (int i = 0; i < numSteps; i++)
            {
                x += delta.x;
                y += delta.y;
                points.Add((x,y));
            }
        }
        var minX = points.Min(x => x.x);
        var minY = points.Min(x => x.y);
        var maxX = points.Max(x => x.x);
        var maxY = points.Max(x => x.y);

        var n = maxY - minY + 1;
        var m = maxX - minX + 1;
        int[,] steps = new int[n, m];
        foreach (var point in points)
        {
            steps[point.y - minY, point.x - minX] = 1;
        }
        
        var visited = new bool[n, m];
        var area = 0L;
        // BFS each point in step
        for (int row = 0; row < maxY; row++)
        {
            for (int col = 0; col < maxX; col++)
            {
                // If visited or is the outline, next spot
                if (visited[row, col] || steps[row, col] == 1) continue;
                Queue<(int r, int c)> queue = new();
                queue.Enqueue((row, col));

                var isTouchingEdge = false;
                var count = 1;
                void TryEnqueue(int r, int c)
                {
                    if (r < 0 || r >= n || c < 0 || c >= m)
                    {
                        isTouchingEdge = true;
                        return;
                    }

                    if (visited[r, c] || steps[r, c] == 1) return;

                    count++;
                    visited[r, c] = true;
                    queue.Enqueue((r, c));
                }

                while (queue.Count > 0)
                {
                    var (r, c) = queue.Dequeue();

                    TryEnqueue(r + 1, c);
                    TryEnqueue(r - 1, c);
                    TryEnqueue(r, c + 1);
                    TryEnqueue(r, c - 1);
                }

                area += isTouchingEdge ? 0 : count;
            }
        }

        (area + points.Count - 1).Dump("Part 1 Answer");
    }

    internal override void Part2()
    {
        
    }
}
