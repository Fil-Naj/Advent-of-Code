namespace AdventOfCode._2022.Solutions;

internal class Puzzle03 : Puzzle2022<Puzzle03>
{
    internal override void Part1()
    {
        var sum = 0;

        foreach (var line in File)
        {
            // First we split
            var half = line.Length / 2;
            HashSet<char> firstHalf = new(line.Take(half));
            HashSet<char> secondHalf = new(line.TakeLast(half));

            sum += GetPriority(firstHalf.Intersect(secondHalf).Single());
        }

        Console.WriteLine(sum);
    }

    internal override void Part2()
    {
        var sum = 0;

        var elfGroupCount = 0;
        var group = new HashSet<char>[3];
        foreach (var line in File)
        {
            if (elfGroupCount == 0) Array.Clear(group, 0, 3);

            group[elfGroupCount] = new HashSet<char>(line);
            elfGroupCount++;

            // Do at end so no lines are skipped
            if (elfGroupCount == 3)
            {
                sum += GetPriority(group[0].Intersect(group[1].Intersect(group[2])).Single());
                elfGroupCount = 0;
            }
        }

        Console.WriteLine(sum);
    }

    private int GetPriority(char item) => char.IsLower(item) ? item - 'a' + 1 : item - 'A' + 27;
}
