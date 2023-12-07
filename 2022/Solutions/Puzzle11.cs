using AdventOfCode.Extensions;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle11 : Puzzle2022<Puzzle11>
{
    internal override void Part1()
    {
        // TODO: I do not know what answers Part 1
    }

    internal override void Part2()
    {
        Monkey[] monkeys;
        List<Monkey> monkeyList = [];

        // read in monkey values
        Monkey? currentMonkey = null;
        int monkeyNum = 0;
        foreach (var line in File)
        {
            if (line.StartsWith("Monkey"))
            {
                currentMonkey = new Monkey();
                monkeyList.Add(currentMonkey);
                monkeyNum++;
                continue;
            }

            var op = line.Trim();
            if (op.StartsWith("Starting items:"))
            {
                var items = line[18..].Split(", ").Select(m => int.Parse(m)).ToArray();
                currentMonkey!.Items = new Queue<int>(items);
            }
            else if (op.StartsWith("Operation:"))
            {
                var operation = $"old => {line[19..]}";
                currentMonkey!.Operation = CSharpScript.EvaluateAsync<Func<long, long>>(operation).GetAwaiter().GetResult();
            }
            else if (op.StartsWith("Test:"))
            {
                currentMonkey!.TestDivisibleBy = int.Parse(op.Split(" ")[3]);
            }
            else if (op.StartsWith("If true:"))
            {
                currentMonkey!.TrueMonkey = int.Parse(op.Split(" ")[5]);
            }
            else if (op.StartsWith("If false:"))
            {
                currentMonkey!.FalseMonkey = int.Parse(op.Split(" ")[5]);
            }
        }

        monkeys = [.. monkeyList];

        // Do the rounds
        var roundNumber = 1;
        long modulo = monkeys.Aggregate(1, (x, y) => x * y.TestDivisibleBy);
        HashSet<int> rounds = [1, 20, 1000, 2000, 3000, 10000];
        while (roundNumber <= 10000)
        {
            for (int i = 0; i < monkeys.Length; i++)
            {
                var monkey = monkeys[i];
                while (monkey.Items!.Count > 0)
                {
                    // Inspect
                    var item = monkey.Items.Dequeue();
                    // Divide by 3 for part 1
                    //var worryLevel = (int)Math.Floor((double)monkey.Operation(item) / 3);
                    var worryLevel = monkey!.Operation!(item) % modulo;

                    // Test
                    if (worryLevel % monkey.TestDivisibleBy == 0)
                        monkeys[monkey.TrueMonkey].Items!.Enqueue((int)worryLevel);
                    else
                        monkeys[monkey.FalseMonkey].Items!.Enqueue((int)worryLevel);

                    // Increment times inspected
                    monkey.NumTimesInspected++;
                }
            }
            if (rounds.Contains(roundNumber))
            {
                $"After Round {roundNumber}".Dump();
                for (int i = 0; i < monkeys.Length; i++)
                {
                    $"Monkey {i} inspected: {monkeys[i].NumTimesInspected}".Dump();
                }
            }

            roundNumber++;
        }
        for (int i = 0; i < monkeys.Length; i++)
        {
            $"Monkey {i}: {string.Join(", ", monkeys[i].Items!)}".Dump();
        }
        monkeys.OrderByDescending(m => m.NumTimesInspected).Take(2).Aggregate(1L, (x, y) => x * y.NumTimesInspected).Dump("Monkey business after 10000 rounds");
    }

    private class Monkey
    {
        public Queue<int>? Items { get; set; }

        public Func<long, long>? Operation { get; set; }

        // Testing stuff
        public int TestDivisibleBy { get; set; }
        public int TrueMonkey { get; set; }
        public int FalseMonkey { get; set; }

        // Inspected
        public long NumTimesInspected { get; set; }
    }
}
