using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle03 : Puzzle2023<Puzzle03>
{
    private const char Blank = '.';
    private const char Gear = '*';

    private readonly int[][] Directions =
    [
        [-1, -1],   // Top left
        [-1, 0],    // Top middle
        [-1, 1],    // Top right
        [0, -1],    // Middle left
        [0, 1],     // Middle right
        [1, -1],    // Bottom left
        [1, 0],     // Bottom middle
        [1, 1],     // Bottom right
    ];

    private readonly List<Number> Numbers = [];
    private readonly List<(int row, int col)> Symbols = [];

    internal override void Part1()
    {
        var n = File.Length;
        var m = File[0].Length;

        // Fill in the table so we know where all the numbers apply
        for (int row = 0; row < n; row++)
        {
            for (int col = 0; col < m; col++)
            {
                if (File[row][col] == Blank) continue;
                if (!char.IsDigit(File[row][col]))
                {
                    Symbols.Add((row, col));
                    continue;
                };

                var number = File[row][col].ToString();

                var startIndex = col;
                while (col + 1 < n && char.IsDigit(File[row][col + 1]))
                {
                    col++;
                    number += File[row][col];
                }

                Numbers.Add(new()
                {
                    Val = int.Parse(number),
                    Row = row,
                    ColStart = startIndex,
                    ColEnd = col
                });
            }
        }

        int sum = 0;
        foreach (var (row, col) in Symbols)
        {
            List<Number> adjacent = [];
            foreach (var direction in Directions)
            {
                var x = col + direction[1];
                var y = row + direction[0];

                adjacent.AddRange(Numbers.Where(n => n.Row == y && n.ColStart <= x && n.ColEnd >= x));
            }

            sum += adjacent.Distinct().Sum(x => x.Val);
        }
        sum.Dump("Part 1 Answer");
    }

    private bool IsSymbol(char c) => !(char.IsDigit(c) || c == Blank);

    internal override void Part2()
    {
        Part1();
        var potentialGears = Symbols.Where(s => File[s.row][s.col] == Gear);

        var ratio = 0;
        foreach (var (row, col) in potentialGears)
        {
            List<Number> adjacent = [];
            foreach (var direction in Directions)
            {
                var x = col + direction[1];
                var y = row + direction[0];

                adjacent.AddRange(Numbers.Where(n => n.Row == y && n.ColStart <= x && n.ColEnd >= x));
            }

            var adjacentNumbers = adjacent.Distinct();

            if (adjacentNumbers.Count() == 2)
            {
                ratio += adjacentNumbers.First().Val * adjacentNumbers.Last().Val;
            }
        }

        ratio.Dump("Part 2 Answer");
    }

    private class Number
    {
        public int Val { get; set; }
        public int Row { get; set; }
        public int ColStart { get; set; }
        public int ColEnd { get; set; }
    }
}
