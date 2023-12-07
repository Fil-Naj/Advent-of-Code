//using AdventOfCode.Extensions;

//namespace AdventOfCode._2022.Solutions;

//internal class Puzzle14 : Puzzle2022<Puzzle14>
//{
//    private const int ERROR_MARGIN = 20;

//    private const char Rock = '#';
//    private const char Air = '.';
//    private const char Source = '+';
//    private const char Sand = 'o';

//    private int MinX = int.MaxValue;
//    private int MaxX = int.MinValue;
//    private int MinY = int.MaxValue;
//    private int MaxY = int.MinValue;

//    private Dictionary<(int x, int y), char> Grid { get; set; }

//    private int SandSourceX { get; set; }
//    private int SandSourceY { get; set; }

//    public Puzzle14()
//    {
//        Grid = [];

//        foreach (var line in File)
//        {
//            var args = line.Split(" -> ");
//            var currentCoords = args[0].Split(",");
//            var currentX = int.Parse(currentCoords[0]);
//            var currentY = int.Parse(currentCoords[1]);
//            for (int arg = 1; arg < args.Length; arg++)
//            {
//                var coords = args[arg].Split(",");

//                // Normalise
//                var x = int.Parse(coords[0]);
//                var y = int.Parse(coords[1]);

//                // Vertical direciton
//                for (int row = Math.Min(y, currentY); row <= Math.Max(y, currentY); row++)
//                {
//                    AddToGrid(row, x, Rock);
//                }

//                // Horizontal direciton
//                for (int col = Math.Min(x, currentX); col <= Math.Max(x, currentX); col++)
//                {
//                    AddToGrid(y, col, Rock);
//                }

//                currentX = x;
//                currentY = y;
//            }
//        }

//        MaxY = Grid.Keys.Max(k => k.y);
//        MaxX = Grid.Keys.Max(k => k.x);
//        MinY = Grid.Keys.Min(k => k.y);
//        MinX = Grid.Keys.Min(k => k.x);


//        SandSourceX = 500;
//        SandSourceY = 0;
//    }

//    internal override void Part1()
//    {
//        var count = 0;
//        while (true)
//        {
//            var sandX = SandSourceX;
//            var sandY = SandSourceY;
//            count++;

//            // DROP
//            while (true)
//            {
//                // Moving Down
//                if (CanMoveToGrid(sandY + 1, sandX))
//                {
//                    sandY += 1;
//                    continue;
//                }

//                // Left and Down
//                if (CanMoveToGrid(sandY + 1, sandX - 1))
//                {
//                    sandY += 1;
//                    sandX -= 1;
//                    continue;
//                }

//                // Right and down
//                if (CanMoveToGrid(sandY + 1, sandX + 1))
//                {
//                    sandY += 1;
//                    sandX += 1;
//                    continue;
//                }

//                // if can no longer move and not into void, place sand
//                AddToGrid(sandY, sandX, Sand);
//                break;
//            }
//        }
//    }

//    internal override void Part2()
//    {
//        var count = 0;
//        while (true)
//        {
//            var sandX = SandSourceX;
//            var sandY = SandSourceY;
//            count++;

//            if (Grid.ContainsKey((SandSourceX, SandSourceY)))
//            {
//                (count - 1).Dump("ANSWER:");
//                PrintGrid();
//                return;
//            }

//            // DROP
//            while (true)
//            {
//                // Moving Down
//                if (CanMoveToGrid(sandY + 1, sandX))
//                {
//                    sandY += 1;
//                    continue;
//                }

//                // Left and Down
//                if (CanMoveToGrid(sandY + 1, sandX - 1))
//                {
//                    sandY += 1;
//                    sandX -= 1;
//                    continue;
//                }

//                // Right and dwon
//                if (CanMoveToGrid(sandY + 1, sandX + 1))
//                {
//                    sandY += 1;
//                    sandX += 1;
//                    continue;
//                }

//                // if can no longer move and not into void, place sand
//                AddToGrid(sandY, sandX, Sand);
//                break;
//            }
//        }
//    }

//    // Normalised
//    void AddToGrid(int row, int col, char type)
//    {
//        Grid[row - minY + ERROR_MARGIN, col - minX] = type;
//    }


//    // Not normalised
//    bool CanMoveToGrid(int row, int col)
//    {
//        // Out of Bounds
//        if (IsOutIntoVoid(row, col)) return false;

//        char[] blockers = [Rock, Sand];

//        HashSet<char> Blockers = new(blockers);
//        if (Blockers.Contains(grid[row, col])) return false;

//        return true;
//    }

//    bool IsOutIntoVoid(int row, int col)
//    {
//        // Left and Right
//        if (col < 0 || col >= (MaxX - MinX + 1)) return true;

//        // Reached bottom
//        if (row >= (MaxY - MinY + 1 + ERROR_MARGIN)) return true;

//        return false;
//    }

//    private void PrintGrid()
//    {
//        int xMax = Grid.Keys.Max(x => x.x);
//        int yMax = Grid.Keys.Max(k => k.y);

//        int xMin = Grid.Keys.Min(x => x.x);
//        int yMin = 0;

//        char[,] grid = new char[yMax - yMin + 1, xMax - xMin + 1];

//        foreach (var i in Grid)
//        {
//            try
//            {
//                grid[i.Key.y - yMin, i.Key.x - xMin] = i.Value;
//            }
//            catch
//            {
//                // i.Dump();
//            }
//        }
//        grid.Dump();
//    }
//}
