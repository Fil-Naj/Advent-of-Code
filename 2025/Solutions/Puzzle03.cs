namespace AdventOfCode._2025.Solutions;

using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle03 : Puzzle2025<Puzzle03>
{
    internal override void Part1()
    {
        var sum = 0L;
        foreach (var line in File)
        {
            var max = line[^1];
            var numMax = 0;
            for (var i = line.Length - 2; i >= 0; i--)
            {
                if (line[i] > max)
                {
                    numMax = Math.Max(int.Parse($"{line[i]}{max}"), numMax);
                    max = line[i];
                }
                else
                {
                    numMax = Math.Max(int.Parse($"{line[i]}{max}"), numMax);
                }
            }

            numMax.Dump($"Max for line");
            sum += numMax;
        }

        sum.Dump("ans");
    }

    internal override void Part2()
    {
        const int Size = 12;
        var sum = 0L;
        foreach (var line in File)
        {
            var n = line.Length;
            var max = line[0] + "00000000000";
            for (var i = 1; i < line.Length; i++)
            {
                var minIndex = Math.Max(0, Size - (n - i));
                var number = max[minIndex];
                while (minIndex < Size)
                {
                    if (line[i] > max[minIndex])
                    {
                        break;
                    }
                    minIndex++;
                }

                if (minIndex < Size)
                {
                    var left = Size - minIndex - 1;
                    max = max[..minIndex] + line[i] + new string('0', left);
                }

            }

            sum += long.Parse(max);
        }

        sum.Dump("ans");
    }
}
