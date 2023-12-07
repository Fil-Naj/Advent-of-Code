using AdventOfCode.Extensions;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle08 : Puzzle2022<Puzzle08>
{
    internal override void Part1()
    {
        var rowSize = File.Length;
        var colSize = File[0].Length;

        var visible = new bool[rowSize, colSize];

        int[] upMax = new int[colSize];
        int[] leftMax = new int[rowSize];

        // Left to down
        for (int row = 0; row < rowSize; row++)
        {
            for (int col = 0; col < colSize; col++)
            {
                var tree = int.Parse(File[row].ElementAt(col).ToString());
                if (col == 0 || col == colSize - 1)
                {
                    leftMax[row] = tree;
                    visible[row, col] = true;
                }
                else
                {
                    if (tree > leftMax[row]) visible[row, col] = true;
                    leftMax[row] = Math.Max(tree, leftMax[row]);
                }

                if (row == 0 || row == rowSize - 1)
                {
                    upMax[col] = tree;
                    visible[row, col] = true;
                }
                else
                {
                    if (tree > upMax[col]) visible[row, col] = true;
                    upMax[col] = Math.Max(tree, upMax[col]);
                }
            }
        }

        // Right and up
        int[] downMax = new int[colSize];
        int[] RightMax = new int[rowSize];
        for (int row = rowSize - 1; row >= 0; row--)
        {
            for (int col = colSize - 1; col >= 0; col--)
            {
                var tree = int.Parse(File[row].ElementAt(col).ToString());

                // going left
                if (tree > RightMax[row]) visible[row, col] = true;
                RightMax[row] = Math.Max(tree, RightMax[row]);

                // going up
                if (tree > downMax[col]) visible[row, col] = true;
                downMax[col] = Math.Max(tree, downMax[col]);
            }
        }
        // visible.Dump();

        var count = 0;
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                count += visible[i, j] ? 1 : 0;
            }
        }
        count.Dump();
    }

    internal override void Part2()
    {
        var rowSize = File.Length;
        var colSize = File[0].Length;

        int[,] grid = new int[rowSize, colSize];

        // Populate grid
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                grid[i, j] = int.Parse(File[i].ElementAt(j).ToString());
            }
        }
        // grid.Dump();

        var maxScene = 0;
        for (int row = 0; row < rowSize; row++)
        {
            for (int col = 0; col < colSize; col++)
            {
                var leftD = 0; var rightD = 0; var upD = 0; var downD = 0;

                // left
                var lp = col;
                while (lp > 0)
                {
                    leftD++;
                    lp--;
                    if (lp >= 0 && grid[row, lp] >= grid[row, col]) break;
                }

                // right
                var rp = col;
                while (rp < colSize - 1)
                {
                    rightD++;
                    rp++;
                    if (rp < colSize && grid[row, rp] >= grid[row, col]) break;
                }

                // up
                var up = row;
                while (up > 0)
                {
                    upD++;
                    up--;
                    if (up >= 0 && grid[up, col] >= grid[row, col]) break;
                }

                // down
                var dp = row;
                while (dp < rowSize - 1)
                {
                    downD++;
                    dp++;
                    if (dp < rowSize && grid[dp, col] >= grid[row, col]) break;

                }

                var space = rightD * leftD * upD * downD;
                maxScene = Math.Max(maxScene, space);
            }
        }
        maxScene.Dump();
    }
}
