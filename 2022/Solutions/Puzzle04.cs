namespace AdventOfCode._2022.Solutions;

internal class Puzzle04 : Puzzle2022<Puzzle04>
{
    internal override void Part1()
    {
        var sum = 0;

        foreach (var line in File)
        {
            var sections = line.Split(',');

            var elf1 = sections[0].Split('-');
            var elf2 = sections[1].Split('-');

            // Elf 1 contains Elf 2
            if (int.Parse(elf1[0]) <= int.Parse(elf2[0]) && int.Parse(elf1[1]) >= int.Parse(elf2[1]))
            {
                sum++;
                continue;
            }
            else if (int.Parse(elf2[0]) <= int.Parse(elf1[0]) && int.Parse(elf2[1]) >= int.Parse(elf1[1]))
            {
                sum++;
                continue;
            }
        }

        Console.WriteLine(sum);
    }

    internal override void Part2()
    {
        var sum = 0;

        foreach (var line in File)
        {
            var sections = line.Split(',');

            var elf1 = sections[0].Split('-');
            var elf2 = sections[1].Split('-');

            // Elf 1 start first
            if (int.Parse(elf1[0]) <= int.Parse(elf2[0]) && int.Parse(elf1[1]) >= int.Parse(elf2[0]))
                sum++;
            // Elf 2 start first
            else if (int.Parse(elf2[0]) <= int.Parse(elf1[0]) && int.Parse(elf2[1]) >= int.Parse(elf1[0]))
                sum++;
        }

        Console.WriteLine(sum);
    }
}
