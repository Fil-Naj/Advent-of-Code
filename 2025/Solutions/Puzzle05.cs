namespace AdventOfCode._2025.Solutions;

using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle05 : Puzzle2025<Puzzle05>
{
    internal override void Part1()
    {
        List<(long start, long end)> fresh = [];
        List<long> ids = [];
        var lineNum = 0;
        while (!string.IsNullOrWhiteSpace(File[lineNum]))
        {
            var line = File[lineNum++].Split('-');
            fresh.Add((long.Parse(line[0]), long.Parse(line[1])));
        }

        while (++lineNum < File.Length)
        {
            ids.Add(long.Parse(File[lineNum]));
        }

        List<(long start, long end)> combined = [];
        fresh = [.. fresh.OrderBy(x => x.start).ThenBy(x => x.end)];
        var curr = fresh[0];
        for (var i = 1; i < fresh.Count; i++)
        {
            if (curr.end >= fresh[i].start)
            {
                curr.end = Math.Max(fresh[i].end, curr.end);
            }
            else
            {
                combined.Add(curr);
                curr = fresh[i];
            }
        }
        combined.Add(curr);

        bool IsInRange(long id)
        {
            foreach (var (start, end) in combined)
            {
                if (start <= id && end >= id)
                    return true;

                if (start > id)
                    return false;
            }

            return false;
        }

        var count = 0L;
        foreach (var id in ids)
        {
            if (IsInRange(id))
                count++;
        }

        count.Dump("ans");
    }

    internal override void Part2()
    {
        List<(long start, long end)> fresh = [];
        var lineNum = 0;
        while (!string.IsNullOrWhiteSpace(File[lineNum]))
        {
            var line = File[lineNum++].Split('-');
            fresh.Add((long.Parse(line[0]), long.Parse(line[1])));
        }

        List<(long start, long end)> combined = [];
        fresh = [.. fresh.OrderBy(x => x.start).ThenBy(x => x.end)];
        var curr = fresh[0];
        for (var i = 1; i < fresh.Count; i++)
        {
            if (curr.end >= fresh[i].start)
            {
                curr.end = Math.Max(fresh[i].end, curr.end);
            }
            else
            {
                combined.Add(curr);
                curr = fresh[i];
            }
        }
        combined.Add(curr);

        var count = 0L;
        foreach (var r in combined)
        {
            count += r.end - r.start + 1;
        }

        count.Dump("ans");
    }
}
