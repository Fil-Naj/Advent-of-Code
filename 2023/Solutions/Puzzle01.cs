using System.Text.RegularExpressions;
using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal partial class Puzzle01 : Puzzle2023<Puzzle01>
{
    private readonly Dictionary<string, int> _numbers = new()
    {
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };

    internal override void Part1()
    {
        var sum = 0;
        foreach (var line in File)
        {
            var numAsString = GetOnlyNumbersFromAString().Replace(line, "");
            sum += int.Parse($"{numAsString[0]}{numAsString[^1]}");
        }

        sum.Dump();
    }

    internal override void Part2()
    {
        var sum = 0;
        foreach (var line in File)
        {
            List<(int index, int value)> numbers = [];

            // Get all numbers from words
            foreach (var num in _numbers)
            {
                var indexes = AllIndexesOf(line, num.Key);

                if (indexes.Count == 0) continue;

                foreach (var value in indexes)
                {
                    numbers.Add((value, num.Value));
                }
            }

            var order = numbers.OrderBy(n => n.index);

            sum += int.Parse($"{order.First().value}{order.Last().value}");
        }

        sum.Dump();
    }

    private static List<int> AllIndexesOf(string str, string value)
    {
        List<int> indexes = [];
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }

    [GeneratedRegex("[^0-9.]")]
    private static partial Regex GetOnlyNumbersFromAString();
}
