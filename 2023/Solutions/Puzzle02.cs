using System.Drawing;
using System.Text.RegularExpressions;
using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal partial class Puzzle02 : Puzzle2023<Puzzle02>
{
    private const int MaxRed = 12;
    private const int MaxGreen = 13;
    private const int MaxBlue = 14;

    internal override void Part1()
    {
        var sum = 0;
        for (int i = 1; i <= File.Length; i++)
        {
            // Match the pattern against the input string
            Match match = GameAndSample().Match(File[i - 1]);
            var samples = match.Groups[2].Value.Split(' ');

            var isLegit = true;
            for (int j = 0; j < samples.Length; j += 2)
            {
                if (!IsLegit(int.Parse(samples[j]), samples[j + 1]))
                {
                    isLegit = false;
                    break;
                }
            }

            if (isLegit)
                sum += i;
        }

        sum.Dump("Answer Part 1");
    }

    private bool IsLegit(int amount, string colour)
    {
        if (colour.Contains("red")) return amount <= MaxRed;
        if (colour.Contains("green")) return amount <= MaxGreen;

        return amount <= MaxBlue;
    }

    internal override void Part2()
    {
        var power = 0;
        for (int i = 1; i <= File.Length; i++)
        {
            // Match the pattern against the input string
            Match match = GameAndSample().Match(File[i - 1]);
            var samples = match.Groups[2].Value.Split(' ');

            var maxSampleRed = 0;
            var maxSampleGreen = 0;
            var maxSampleBlue = 0;

            for (int j = 0; j < samples.Length; j += 2)
            {
                if (samples[j + 1].Contains("red"))
                {
                    maxSampleRed = Math.Max(maxSampleRed, int.Parse(samples[j]));
                }
                else if (samples[j + 1].Contains("green"))
                {
                    maxSampleGreen = Math.Max(maxSampleGreen, int.Parse(samples[j]));
                }
                else
                {
                    maxSampleBlue = Math.Max(maxSampleBlue, int.Parse(samples[j]));
                }
            }

            power += maxSampleBlue * maxSampleGreen * maxSampleRed;
        }

        power.Dump("Answer Part 2");
    }

    [GeneratedRegex(@"Game (\d+): (.+)$")]
    private static partial Regex GameAndSample();
}
