using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle09 : Puzzle2023<Puzzle09>
{
    internal override void Part1()
    {
        var sum = 0L;
        foreach (var line in File)
        {
            var numbers = line.Split(' ').Select(long.Parse).ToArray();
            sum += FindNextDiff(numbers);
        }

        sum.Dump("Part 1 Answer");
    }

    private long FindNextDiff(long[] numbers)
    {
        var diffs = new long[numbers.Length - 1];
        for (int i = 1; i <= diffs.Length; i++)
        {
            diffs[i - 1] = numbers[i] - numbers[i - 1];
        }

        return diffs.All(a => a == diffs[0])
            ? numbers[^1] + diffs[0]
            : numbers[^1] + FindNextDiff(diffs);
    }

    internal override void Part2()
    {
        var sum = 0L;
        foreach (var line in File)
        {
            var numbers = line.Split(' ').Select(long.Parse).ToArray();
            sum += FindPrevDiff(numbers);
        }

        sum.Dump("Part 1 Answer");
    }

    private long FindPrevDiff(long[] numbers)
    {
        var diffs = new long[numbers.Length - 1];
        for (int i = 1; i <= diffs.Length; i++)
        {
            diffs[i - 1] = numbers[i] - numbers[i - 1];
        }

        return diffs.All(a => a == diffs[0])
            ? numbers[0] - diffs[0]
            : numbers[0] - FindPrevDiff(diffs);
    }
}
