using AdventOfCode.Extensions;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle12 : Puzzle2022<Puzzle12>
{
    private int Rows { get; set; }
    private int Cols { get; set; }

    private char[,]? Grid { get; set; }
    private bool[,]? Visited { get; set; }
    private Queue<Node>? Queue { get; set; }

    internal override void Part1()
    {
        // TODO: I do not know what answers Part 1
    }

    internal override void Part2()
    {
        // Make a grid
        Rows = File.Length;
        Cols = File[0].Length;

        Grid = new char[Rows, Cols];

        const char STARTLETTER = 'S';
        const char TARGET = 'E';


        Node? start = null;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                // $"rows: {i}, cols: {j}".Dump();

                Grid[i, j] = File[i].ElementAt(j);

                if (Grid[i, j] == STARTLETTER) start = new Node(j, i, 0);
            }
        }

        Visited = new bool[Rows, Cols];

        Queue = new();

        // Starting node add to list
        Queue.Enqueue(start!);
        Visited[start!.Y, start.X] = true;


        while (Queue.Count > 0)
        {
            var node = Queue.Dequeue();
            var nodeVal = Grid[node.Y, node.X];


            if (nodeVal == TARGET)
            {
                $"Number of steps taken: {node.NumSteps}".Dump("Answer");
                return;
            }

            // Left
            if (node.X > 0)
            {
                if (CanMove(Grid[node.Y, node.X - 1], Grid[node.Y, node.X]))
                {
                    Enqueue(node.X - 1, node.Y, node.NumSteps + 1);
                }
            }

            // Right
            if (node.X < Cols - 1 && CanMove(Grid[node.Y, node.X + 1], Grid[node.Y, node.X]))
            {
                Enqueue(node.X + 1, node.Y, node.NumSteps + 1);
            }

            // Up
            if (node.Y > 0 && CanMove(Grid[node.Y - 1, node.X], Grid[node.Y, node.X]))
            {
                Enqueue(node.X, node.Y - 1, node.NumSteps + 1);
            }

            // down
            if (node.Y < Rows - 1 && CanMove(Grid[node.Y + 1, node.X], Grid[node.Y, node.X]))
            {
                Enqueue(node.X, node.Y + 1, node.NumSteps + 1);
            }
            //$"Visited x: {node.X}, y: {node.Y}, value: {nodeVal}".Dump();
        }
    }

    private void Enqueue(int x, int y, int steps)
    {
        if (Visited![y, x] == true) return;
        Visited[y, x] = true;
        Queue!.Enqueue(new Node(x, y, steps));
    }

    private bool CanMoveInverse(char to, char from)
    {
        if (from == 'E' && (to == 'z' || to == 'y')) return true;
        if (to == 'S' && (from == 'a' || from == 'b')) return true;

        // Going down a letter
        if (to + 1 == from) return true;

        // Same letter or less
        if (from != 'E' && from <= to) return true;

        return false;
    }

    private bool CanMove(char to, char from)
    {
        if (from == 'S' && (to == 'a' || to == 'z')) return true;
        if (to == 'E' && (from == 'z' || from == 'x')) return true;

        // Going up a letter
        if (from + 1 == to) return true;

        // Same letter or less
        if (from >= to) return true;

        return false;
    }


    private class Node(int x, int y, int steps)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int NumSteps { get; set; } = steps;
    }
}
