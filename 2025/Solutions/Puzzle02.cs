namespace AdventOfCode._2025.Solutions;

using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle02 : Puzzle2025<Puzzle02>
{
    internal override void Part1()
    {
        List<(long start, long end)> ranges = [.. File[0].Split(',').Select(r =>
        {
            var x = r.Split('-');
            return (long.Parse(x[0]), long.Parse(x[1]));
        }).OrderBy(r => r.Item1)];

        var maximum = ranges.Last().end;
        long start = 1;
        long repeated = 11;

        var rangeIndex = 0;
        long sum = 0;
        while (repeated < maximum)
        {
            while (ranges[rangeIndex].end < repeated)
            {
                rangeIndex++;
            }

            if (ranges[rangeIndex].start <= repeated && ranges[rangeIndex].end >= repeated)
            {
                sum += repeated;
            }

            start++;
            repeated = long.Parse($"{start}{start}");
        }

        sum.Dump("ans");
    }

    internal override void Part2()
    {
        List<(long start, long end)> ranges = [.. File[0].Split(',').Select(r =>
        {
            var x = r.Split('-');
            return (long.Parse(x[0]), long.Parse(x[1]));
        }).OrderBy(r => r.Item1)];

        var maximum = ranges.Last().end;
        var temp = maximum;
        var pivot = 0;
        while (temp > 0)
        {
            pivot++;
            temp /= 10;
        }

        var stop = maximum % Math.Pow(10, pivot / 2);

        long sum = 0;
        HashSet<long> visisted = [];
        void Expand(long num)
        {
            var repeated = long.Parse($"{num}{num}");
            var rangeIndex = 0;
            while (repeated < maximum && visisted.Add(repeated))
            {
                while (ranges[rangeIndex].end < repeated)
                {
                    rangeIndex++;
                }

                if (ranges[rangeIndex].start <= repeated && ranges[rangeIndex].end >= repeated)
                {
                    sum += repeated;
                }

                if (!long.TryParse($"{repeated}{num}", out repeated))
                    return;
            }
        }

        long start = 1;
        while (start < stop)
        {
            Expand(start);
            start++;
        }

        sum.Dump("ans");
    }
}
