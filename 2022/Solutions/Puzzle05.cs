using System.Text;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle05 : Puzzle2022<Puzzle05>
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
        foreach (var line in File)
        {
            var x = line.Split(" ");

            var amount = int.Parse(x[1]);
            var from = int.Parse(x[3]) - 1;
            var to = int.Parse(x[5]) - 1;

            // Part 1 requires this to be a Queue. Part 2, a Stack
            Queue<char> toMove = new();

            // pick up
            for (int i = 0; i < amount; i++)
            {
                toMove.Enqueue(_stacks[from].Pop());
            }

            // pick up
            for (int i = 0; i < amount; i++)
            {
                _stacks[to].Push(toMove.Dequeue());
            }
        }

        Console.WriteLine(GetTops(_stacks));
    }

    internal override void Part2()
    {
        foreach (var line in File)
        {
            var x = line.Split(" ");

            var amount = int.Parse(x[1]);
            var from = int.Parse(x[3]) - 1;
            var to = int.Parse(x[5]) - 1;

            // Part 1 requires this to be a Queue. Part 2, a Stack
            Stack<char> toMove = new();

            // pick up
            for (int i = 0; i < amount; i++)
            {
                toMove.Push(_stacks[from].Pop());
            }

            // pick up
            for (int i = 0; i < amount; i++)
            {
                _stacks[to].Push(toMove.Pop());
            }
        }

        Console.WriteLine(GetTops(_stacks));
    }

    private string GetTops(Stack<char>[] arr)
    {
        StringBuilder sb = new();

        foreach (var element in arr)
        {
            sb.Append(element.Peek());
        }
        return sb.ToString();
    }
}
