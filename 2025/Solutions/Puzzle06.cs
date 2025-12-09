namespace AdventOfCode._2025.Solutions;

using System.Text;
using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle06 : Puzzle2025<Puzzle06>
{
    internal override void Part1()
    {
        var ops = File[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
        long[] res = [.. ops.Select(o => o == "*" ? 1L : 0L)];
        for (var i = 0; i < File.Length - 1; i++)
        {
            var numbers = File[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            for (var j = 0; j < ops.Length; j++)
            {
                switch (ops[j])
                {
                    case "*":
                        res[j] *= numbers[j];
                        break;
                    case "+":
                        res[j] += numbers[j];
                        break;
                }
            }
        }

        res.Sum().Dump("Answer");
    }

    internal override void Part2()
    {
        var ops = File[^1];
        var c = 0;
        var op = '/';
        long curr = 0;
        long ans = 0;
        while (c < ops.Length)
        {
            if (ops[c] == '*')
            {
                op = '*';
                curr = 1;
            }
            else if (ops[c] == '+')
            {
                op = '+';
                curr = 0;
            }
            else if (c + 1 < ops.Length && ops[c + 1] != ' ')
            {
                ans += curr;
                curr.Dump();
                c++;
                continue;
            }

            StringBuilder sb = new();
            for (var r = 0; r < File.Length - 1; r++)
            {
                if (File[r][c] != ' ')
                    sb.Append(File[r][c]);
            }

            long num = long.Parse(sb.ToString());
            num.Dump("Num");
            curr = op == '*'
                ? curr * num
                : curr + num;

            c++;
        }

        ans += curr;
        ans.Dump();
    }
}
