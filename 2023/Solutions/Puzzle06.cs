using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle06 : Puzzle2023<Puzzle06>
{
    internal override void Part1()
    {
        var times = File[0][6..].Split(' ').Where(n => n.Trim().Length > 0).Select(long.Parse).ToArray();
        var distances = File[1][9..].Split(' ').Where(n => n.Trim().Length > 0).Select(long.Parse).ToArray();

        times.Dump();
        distances.Dump();

        var totalPoints = 1;
        for (int i = 0; i < times.Length; i++)
        {
            var raceWins = 0;
            for (int j = 1; j < times[i]; j++)
            {
                if (j * (times[i] - j) > distances[i]) raceWins++;
            }

            totalPoints *= raceWins;
        }

        totalPoints.Dump("Part 1 Answer");
    }

    internal override void Part2()
    {
        var time = long.Parse(string.Join(string.Empty, File[0][6..].Split(' ').Where(n => n.Trim().Length > 0)));
        var distance = long.Parse(string.Join(string.Empty, File[1][9..].Split(' ').Where(n => n.Trim().Length > 0)));

        time.Dump();
        distance.Dump();

        var l = 0L;
        var r = time;

        // Binary search to find small side
        while (l < r)
        {
            var midPoint = l + (r - l) / 2;
            if (midPoint * (time - midPoint) > distance)
            {
                r = midPoint;
            }
            else
            {
                l = midPoint + 1;
            }
        }

        var start = l;

        // Binary search to find small side
        l = 0L;
        r = time;
        while (l < r)
        {
            var midPoint = l + (r - l) / 2;
            if (midPoint * (time - midPoint) > distance)
            {
                l = midPoint;
            }
            else
            {
                r = midPoint - 1;
            }
        }

        var end = l;
        (end - start + 1).Dump("Part 2 Answer");
    }
}
