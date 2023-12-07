using System.Diagnostics;
using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle04 : Puzzle2023<Puzzle04>
{
    private int[]? MatchingNumbers { get; set; }

    internal override void Part1()
    {
        long points = 0;
        MatchingNumbers = new int[File.Length];
        for (int i = 0; i < File.Length; i++)
        {
            var numbers = File[i][(File[i].IndexOf(':') + 2)..].Split('|');
            var matches = numbers[0].Split(' ').Where(n => n.Trim() != string.Empty)
                .Intersect(numbers[1].Split(' ').Where(n => n.Trim() != string.Empty))
                .Count();

            MatchingNumbers[i] = matches;

            if (matches > 0)
                points += (long)Math.Pow(2, matches - 1);
        }

        points.Dump("Part 1 Answer");
    }

    internal override void Part2()
    {
        Part1();

        // Memoisation baby let's goooooo
        long[] copies = new long[File.Length];
        long cards = 0L;
        long Dfs(int game)
        {
            if (copies[game] > 0) return copies[game];

            var childCopies = 1L;
            for (int i = 1; i <= MatchingNumbers![game]; i++)
            {
                childCopies += Dfs(game + i);
            }

            copies[game] = childCopies;

            return copies[game];
        }

        for (int i = 0; i < File.Length; i++)
        {
            cards += Dfs(i);
        }

        cards.Dump("Part 2 Answer");
    }
}
