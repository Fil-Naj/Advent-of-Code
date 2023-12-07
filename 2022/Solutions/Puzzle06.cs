using AdventOfCode.Extensions;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle06 : Puzzle2022<Puzzle06>
{
    private readonly Stack<char>[] _stacks =
    [
        new("WRF"),
        new("THMCDVWP"),
        new("PMZNL"),
        new("JCHR"),
        new("CPGHQTB"),
        new("GCWLFZ"),
        new("WVLQZJGC"),
        new("PNRFWTVC"),
        new("JWHGRSV")
    ];

    internal override void Part1()
    {
        char[] window = new char[4];
        var count = 0;
        foreach (char letter in File[0])
        {
            Slide(letter, window);
            count++;
            if (AreFourDifferent(window))
            {
                // window.Dump();
                count.Dump();
                return;
            };
        }
    }

    internal override void Part2()
    {
        char[] window = new char[14];
        var count = 0;
        foreach (char letter in File[0])
        {
            Slide(letter, window);
            count++;
            if (AreNDifferent(window, 14))
            {
                // window.Dump();
                count.Dump();
                return;
            };
        }
    }

    private bool AreFourDifferent(char[] window)
    {
        return window.Distinct().Count(l => l != default(char)) == 4;
    }

    private bool AreNDifferent(char[] window, int n)
    {
        return window.Distinct().Count(l => l != default(char)) == n;
    }

    private void Slide(char newLetter, char[] window)
    {
        for (int i = 0; i < window.Length - 1; i++)
        {
            window[i] = window[i + 1];
        }
        window[^1] = newLetter;
    }
}
