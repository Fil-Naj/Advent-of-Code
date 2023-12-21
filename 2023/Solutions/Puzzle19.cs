using AdventOfCode.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Solutions;

internal partial class Puzzle19 : Puzzle2023<Puzzle19>
{
    private readonly Dictionary<string, List<Condition>> _map = [];
    internal override void Part1()
    {
        var firstSection = true;
        List<Xmas> values = [];
        foreach (var line in File)
        {
            if (line.Trim() == string.Empty)
            {
                firstSection = false;
                continue;
            }

            if (firstSection)
            {
                Match match = SourceAndConditions().Match(line);
                List<Condition> conditions = [];

                var possible = match.Groups[2].Value.Split(',');
                for (int i = 0; i < possible.Length - 1; i++)
                {
                    var indexOfColon = possible[i].IndexOf(':');
                    conditions.Add(new()
                    {
                        Item = possible[i][0],
                        Operator = possible[i][1],
                        Number = int.Parse(possible[i][2..indexOfColon]),
                        Next = possible[i][(indexOfColon + 1)..]
                    });
                }
                conditions.Add(new()
                {
                    Next = possible[^1]
                });

                _map[match.Groups[1].Value] = conditions;
            }
            else
            {
                var x = line.TrimStart('{').TrimEnd('}').Split(',');
                values.Add(new()
                {
                    X = int.Parse(x[0][2..]),
                    M = int.Parse(x[1][2..]),
                    A = int.Parse(x[2][2..]),
                    S = int.Parse(x[3][2..]),
                });
            }
        }

        var sum = 0L;
        foreach (var val in values)
        {
            var source = "in";
            while (source != "A" && source != "R")
            {
                // Updates the source variable
                _map[source].First(c => c.TryGiveNext(val, out source));
            }

            if (source == "R") continue;

            sum += val.Sum;
        }

        sum.Dump("Part 1 Answer");
    }

    internal override void Part2()
    {
        // This populates the _map field
        Part1();

        var intialXmasRange = new XmasRange()
        {
            Xmas = new()
            {
                { 'x', new(1, 4000) },
                { 'm', new(1, 4000) },
                { 'a', new(1, 4000) },
                { 's', new(1, 4000) },
            }
        };

        var sumCombinations = 0L;
        void Dfs(XmasRange range, string source)
        {
            // If Rejected, give up (just like irl)
            if (source == "R") return;

            // If accepted, count it UP! (just like irl)
            if (source == "A")
            {
                var combinations = range.IsLegit ? range.Combinations : 0;
                sumCombinations += combinations;

                // Then give up, cos we made it baby yeahhhhhh
                return;
            }

            var map = _map[source];

            foreach (var condition in map)
            {
                // In case of last condition
                if (condition.Item is default(char))
                {
                    // Whatever the default is, go to that
                    // Range will have been updated by here to fail all previous cases in group
                    Dfs(range, condition.Next);
                    continue;
                }

                if (condition.Operator == '<')
                {
                    var endOfNewRange = condition.Number - 1;

                    var newRange = range.Copy();

                    newRange.Xmas[condition.Item].End = Math.Min(endOfNewRange, newRange.Xmas[condition.Item].End);

                    // Update the current range to say fail the condition
                    range.Xmas[condition.Item].Start = condition.Number;

                    Dfs(newRange, condition.Next);
                }
                else
                {
                    var startOfNewRange = condition.Number + 1;

                    var newRange = range.Copy();

                    newRange.Xmas[condition.Item].Start = Math.Max(startOfNewRange, newRange.Xmas[condition.Item].Start);

                    // Update the current range to say fail the condition
                    range.Xmas[condition.Item].End = condition.Number;

                    Dfs(newRange, condition.Next);
                }
            }
        }

        Dfs(intialXmasRange, "in");

        sumCombinations.Dump("Part 2 Answer");
    }

    public class Condition
    {
        public char Item { get; set; }
        public char Operator { get; set; }
        public int Number { get; set; }
        public string Next { get; set; }

        public bool TryGiveNext(Xmas path, out string next)
        {
            next = string.Empty;

            if (Item is default(char))
            {
                next = Next;
                return true;
            }

            var value = Item switch
            {
                'x' => path.X,
                'm' => path.M,
                'a' => path.A,
                's' => path.S,
                _ => throw new Exception("Alright Grinch"),
            };

            var meetsCondition = Operator == '<'
                ? value < Number
                : value > Number;

            if (meetsCondition)
            {
                next = Next;
                return true;
            }

            return false;
        }
    }

    public class Xmas
    {
        public long X { get; set; }
        public long M { get; set; }
        public long A { get; set; }
        public long S { get; set; }

        public long Sum => X + M + A + S;
    }

    public class XmasRange
    {
        public Dictionary<char, Range> Xmas { get; set; } = [];

        // If we have a start of 5 but an end of 3, it is not acceptable
        public bool IsLegit => Xmas.All(r => r.Value.Length > 0);

        // Get number of combinations possible with current ranges. Calculated by multiplying all lengths
        public long Combinations => Xmas.Aggregate(1L, (x, y) => x * y.Value.Length);

        // Just reference type tings
        public XmasRange Copy()
        {
            var newRange = new XmasRange();
            foreach (var c in Xmas)
            {
                newRange.Xmas[c.Key] = new(c.Value.Start, c.Value.End);
            }

            return newRange;
        }
    }

    public class Range(int start, int end)
    {
        public int Start { get; set; } = start;
        public int End { get; set; } = end;
        public int Length => End - Start + 1;
    }

    [GeneratedRegex(@"^([^{}]+)\{([^{}]+)\}")]
    private static partial Regex SourceAndConditions();
}
