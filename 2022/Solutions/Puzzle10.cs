using AdventOfCode.Extensions;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle10 : Puzzle2022<Puzzle10>
{
    private int cycle = 1;
    private int X = 1;
    private int sum = 0;
    private readonly char[,] crt = new char[6, 40];

    private static readonly int[] Collection = [20, 60, 100, 140, 180, 220];
    private HashSet<int> cycleNumbersToCheck = new(Collection);

    internal override void Part1()
    {
        foreach (var line in File)
        {
            var args = line.Split(" ");
            var op = args[0];

            if (op == "noop")
            {
                IncrementCycle();
                continue;
            }
            else
            {
                // Load instruction
                IncrementCycle();

                // Execute instruction
                IncrementCycle();

                var toAdd = int.Parse(args[1]);
                X += toAdd;
            }
        }

        sum.Dump("Sum");
    }

    internal override void Part2()
    {
        Part1();

        crt.Dump("CRT");
    }

    private int SignalStrength() => cycle * X;

    private void IncrementCycle()
    {
        if (cycleNumbersToCheck.Contains(cycle))
        {
            sum += SignalStrength();
            $"Cycle number: {cycle}. X: {X}".Dump();
        }
        DrawOnCrt();
        cycle++;
        // $"Cycle number: {cycle}. X: {X}".Dump();
    }

    private void DrawOnCrt()
    {
        int row = (cycle - 1) / 40;
        int col = (cycle - 1) % 40;

        var diff = Math.Abs(col - X);

        $"Row: {row}. Col: {col}. CycleNum: {cycle}. X pos: {X}".Dump();
        crt[row, col] = (diff == 0 || diff == 1) ? '#' : '.';
    }
}
