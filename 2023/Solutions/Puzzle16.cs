using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle16 : Puzzle2023<Puzzle16>
{
    private static readonly Dictionary<Direction, (int x, int y)> _directions = new()
    {
        { Direction.Up, (0, -1) },
        { Direction.Down, (0, 1) },
        { Direction.Left, (-1, 0) },
        { Direction.Right, (1, 0) },
    };

    private static char[][] Grid {  get; set; }
    private static int N { get; set; }
    private static int M { get; set; }

    internal override void Part1()
    {
        Grid = File.Select(r => r.ToArray()).ToArray();
        N = Grid.Length;
        M = Grid[0].Length;
        FindEnergisedStartingFrom(0, 0, Direction.Right).Dump("Part 1 Answer");
    }

    private long FindEnergisedStartingFrom(int x, int y, Direction direction)
    {
        bool[,] numVisits = new bool[N, M];
        HashSet<(int x, int y, Direction direction)> currentPathVisited = [];
        HashSet<(int x, int y, Direction direction)> globalVisited = [];

        numVisits[y, x] = true;
        currentPathVisited.Add((x, y, direction));
        globalVisited.Add((x, y, direction));

        var energised = 1L;
        void Dfs(int x, int y, Direction direction)
        {
            foreach (var path in Move(x, y, direction))
            {
                if (path.X < 0 || path.X >= M || path.Y < 0 || path.Y >= N) continue;

                var option = (path.Y, path.X, path.direction);
                if (currentPathVisited.Contains(option) || globalVisited.Contains(option)) continue;

                if (!numVisits[path.Y, path.X])
                    energised++;

                currentPathVisited.Add(option);
                globalVisited.Add(option);
                numVisits[path.Y, path.X] = true;
                Dfs(path.X, path.Y, path.direction);
                currentPathVisited.Remove(option);
            }
        }

        Dfs(x, y, direction);

        return energised;
    }

    public List<(int X, int Y, Direction direction)> Move(int x, int y, Direction direction)
    {
        var (xMove, yMove) = _directions[direction];

        // Do the move
        switch (Grid[y][x])
        {
            case '.':
                return [(x + xMove, y + yMove, direction)];

            case '-':
                if (direction is Direction.Right or Direction.Left)
                {
                    return [(x + xMove, y, direction)];
                }

                return [(x + 1, y, Direction.Right), (x - 1, y, Direction.Left)];
            case '|':
                if (direction is Direction.Up or Direction.Down)
                {
                    return [(x, y + yMove, direction)];
                }
                return [(x, y - 1, Direction.Up), (x, y + 1, Direction.Down)];
            case '/':
                if (direction is Direction.Left)
                {
                    return [(x, y + 1, Direction.Down)];
                }
                else if (direction is Direction.Right)
                {
                    return [(x, y - 1, Direction.Up)];
                }
                else if (direction is Direction.Up)
                {
                    return [(x + 1, y, Direction.Right)];
                }
                else
                {
                    return [(x - 1, y, Direction.Left)];
                }
            case '\\':
                if (direction is Direction.Left)
                {
                    return [(x, y - 1, Direction.Up)];
                }
                else if (direction is Direction.Right)
                {
                    return [(x, y + 1, Direction.Down)];
                }
                else if (direction is Direction.Up)
                {
                    return [(x - 1, y, Direction.Left)];
                }
                else
                {
                    return [(x + 1, y, Direction.Right)];
                }
            default:
                return [];
        }
    }

    internal override void Part2()
    {
        Grid = File.Select(r => r.ToArray()).ToArray();
        N = Grid.Length;
        M = Grid[0].Length;

        var maxVal = 0L;

        // Top going down
        for (int i = 0; i < M; i++)
            maxVal = Math.Max(maxVal, FindEnergisedStartingFrom(i, 0, Direction.Down));

        // Botoom going up
        for (int i = 0; i < M; i++)
            maxVal = Math.Max(maxVal, FindEnergisedStartingFrom(i, N - 1, Direction.Up));

        // Left going right
        for (int i = 0; i < N; i++)
            maxVal = Math.Max(maxVal, FindEnergisedStartingFrom(0, i, Direction.Right));

        // Right going left
        for (int i = 0; i < N; i++)
            maxVal = Math.Max(maxVal, FindEnergisedStartingFrom(M - 1, i, Direction.Left));

        maxVal.Dump("Part2 Answer");

    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
