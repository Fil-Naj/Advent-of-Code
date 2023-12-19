using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle17 : Puzzle2023<Puzzle17>
{
    private const int MaxLargeCrucibleConsecutiveSteps = 3;
    private const int MaxUltraCrucibleConsecutiveSteps = 10;

    private static readonly Dictionary<Facing, (int x, int y)> _directions = new()
    {
        { Facing.Up, (0, -1) },
        { Facing.Down, (0, 1) },
        { Facing.Left, (-1, 0) },
        { Facing.Right, (1, 0) },
    };

    private static int[][]? Grid { get; set; }
    private static int N { get; set; }
    private static int M { get; set; }

    private readonly PriorityQueue<Path, Path> _pq = new();
    private readonly HashSet<string> _visited = [];

    internal override void Part1()
    {
        static bool CanEnd(Path path)
        {
            return path.Row == N - 1 && path.Col == M - 1;
        }

        PriorityFirstSearch(MoveLargeCrucible, CanEnd).Dump("Part 1 Answer");
    }

    private long PriorityFirstSearch(Func<Path, Direction, Path?> moveFunction, Func<Path, bool> canEnd, bool dumpSteps = false)
    {
        Grid = File.Select(r => r.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
        N = Grid.Length;
        M = Grid[0].Length;

        var r = new Path()
        {
            Row = 0,
            Col = 1,
            HeatLoss = Grid[0][1],
            Facing = Facing.Right,
            Consecutive = 1,
            Steps = [(0, 0), (0, 1)]
        };
        var d = new Path()
        {
            Row = 1,
            Col = 0,
            HeatLoss = Grid[1][0],
            Facing = Facing.Down,
            Consecutive = 1,
            Steps = [(0, 0), (1, 0)]
        };

        _pq.Enqueue(r, r);
        _pq.Enqueue(d, d);

        _visited.Add(r.ToKey());
        _visited.Add(d.ToKey());

        while (_pq.Count > 0)
        {
            var node = _pq.Dequeue();

            if (canEnd(node))
            {
                if (dumpSteps)
                {
                    var gridSteps = new int[N, M];
                    foreach (var (row, col) in node.Steps)
                        gridSteps[row, col] = 1;
                    gridSteps.Dump();
                }

                return node.HeatLoss;
            }

            TryEnqueue(moveFunction(node, Direction.Left));
            TryEnqueue(moveFunction(node, Direction.Right));
            TryEnqueue(moveFunction(node, Direction.Straight));
        }

        return -1;
    }

    private void TryEnqueue(Path? path)
    {
        if (path is null) return;
        if (path.Col < 0 || path.Col >= M || path.Row < 0 || path.Row >= N) return;

        if (_visited.Contains(path.ToKey())) return;

        if (path.Steps.Contains((path.Row, path.Col))) return;

        path.HeatLoss += Grid![path.Row][path.Col];
        path.Steps.Add((path.Row, path.Col));

        _visited.Add(path.ToKey());
        _pq.Enqueue(path, path);
    }

    private Path? MoveLargeCrucible(Path path, Direction direction)
    {
        if (direction is Direction.Straight)
        {
            if (path.Consecutive + 1 > MaxLargeCrucibleConsecutiveSteps) return null;

            var (x, y) = _directions[path.Facing];
            return new Path()
            {
                Row = path.Row + y,
                Col = path.Col + x,
                HeatLoss = path.HeatLoss,
                Consecutive = path.Consecutive + 1,
                Facing = path.Facing,
                Steps = new(path.Steps)
            };
        }

        if (direction is Direction.Right)
        {
            var newFacing = path.Facing switch
            {
                Facing.Up => Facing.Right,
                Facing.Down => Facing.Left,
                Facing.Left => Facing.Up,
                Facing.Right => Facing.Down,
                _ => throw new NotImplementedException(),
            };

            var (x, y) = _directions[newFacing];
            return new Path()
            {
                Row = path.Row + y,
                Col = path.Col + x,
                HeatLoss = path.HeatLoss,
                Consecutive = 1,
                Facing = newFacing,
                Steps = new(path.Steps)
            };
        }

        // Going left
        var leftFacing = path.Facing switch
        {
            Facing.Up => Facing.Left,
            Facing.Down => Facing.Right,
            Facing.Left => Facing.Down,
            Facing.Right => Facing.Up,
            _ => throw new NotImplementedException(),
        };

        var (dx, dy) = _directions[leftFacing];
        return new Path()
        {
            Row = path.Row + dy,
            Col = path.Col + dx,
            HeatLoss = path.HeatLoss,
            Consecutive = 1,
            Facing = leftFacing,
            Steps = new(path.Steps)
        };
    }

    internal override void Part2()
    {
        static bool CanEnd(Path path)
        {
            return path.Row == N - 1 && path.Col == M - 1 && path.Consecutive >= 4;
        }

        PriorityFirstSearch(MoveUltraCrucible, CanEnd).Dump("Part 2 Answer");
    }

    private Path? MoveUltraCrucible(Path path, Direction direction)
    {
        if (direction is Direction.Straight)
        {
            if (path.Consecutive + 1 > MaxUltraCrucibleConsecutiveSteps) return null;

            var (x, y) = _directions[path.Facing];
            return new Path()
            {
                Row = path.Row + y,
                Col = path.Col + x,
                HeatLoss = path.HeatLoss,
                Consecutive = path.Consecutive + 1,
                Facing = path.Facing,
                Steps = new(path.Steps)
            };
        }

        // Must move at least 4 in the same direction before it can turn
        if (path.Consecutive < 4) return null;

        if (direction is Direction.Right)
        {
            var newFacing = path.Facing switch
            {
                Facing.Up => Facing.Right,
                Facing.Down => Facing.Left,
                Facing.Left => Facing.Up,
                Facing.Right => Facing.Down,
                _ => throw new NotImplementedException(),
            };

            var (x, y) = _directions[newFacing];
            return new Path()
            {
                Row = path.Row + y,
                Col = path.Col + x,
                HeatLoss = path.HeatLoss,
                Consecutive = 1,
                Facing = newFacing,
                Steps = new(path.Steps)
            };
        }

        // Going left
        var leftFacing = path.Facing switch
        {
            Facing.Up => Facing.Left,
            Facing.Down => Facing.Right,
            Facing.Left => Facing.Down,
            Facing.Right => Facing.Up,
            _ => throw new NotImplementedException(),
        };

        var (dx, dy) = _directions[leftFacing];
        return new Path()
        {
            Row = path.Row + dy,
            Col = path.Col + dx,
            HeatLoss = path.HeatLoss,
            Consecutive = 1,
            Facing = leftFacing,
            Steps = new(path.Steps)
        };
    }


    private class Path : IComparable<Path>
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public long HeatLoss { get; set; }
        public Facing Facing { get; set; }
        public int Consecutive { get; set; }

        public HashSet<(int row, int col)> Steps = [];

        public int CompareTo(Path? other)
        {
            if (HeatLoss > other!.HeatLoss) return 1;
            else if (other.HeatLoss > HeatLoss) return -1;

            return Steps.Count > other.Steps.Count ? 1 : -1;
        }

        public string ToKey()
        {
            return $"({Row},{Col})|{Facing}|{Consecutive}";
        }
    }

    private enum Facing
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    private enum Direction
    {
        None,
        Left,
        Straight,
        Right
    }
}
