namespace AdventOfCode._2025.Solutions;

using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle01 : Puzzle2025<Puzzle01>
{
    private const int Start = 50;

    internal override void Part1()
    {
        var dial = Start;
        var zeroCounts = 0;
        foreach (var line in File)
        {
            var isLeft = line[0] == 'L';
            var turn = int.Parse(line[1..]);
            if (isLeft)
            {
                dial -= turn;
            }
            else
            {
                dial += turn;
            }

            if (dial % 100 == 0)
                zeroCounts++;
        }

        zeroCounts.Dump("Answer");
    }

    internal override void Part2()
    {
        var dial = Start;
        var zeroCounts = 0;
        foreach (var line in File)
        {
            var isLeft = line[0] == 'L';
            var turn = int.Parse(line[1..]);
            var rotations = 0;
            if (isLeft)
            {
                if (dial == 0)
                    dial = 100;

                var newDial = dial - turn;
                if (newDial < 0)
                {
                    rotations = 1 + Math.Abs(newDial / 100);
                    dial = 100 + newDial % 100;
                }
                else
                {
                    if (newDial == 0)
                        rotations = 1;
                    dial = newDial;
                }
            }
            else
            {
                if (dial == 100)
                    dial = 0;

                var newDial = dial + turn;
                rotations = newDial / 100;
                dial = newDial % 100;
            }

            zeroCounts += rotations;
        }

        zeroCounts.Dump("ans");
    }
}
