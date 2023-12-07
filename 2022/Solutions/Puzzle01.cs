namespace AdventOfCode._2022.Solutions;

internal class Puzzle01 : Puzzle2022<Puzzle01>
{
    private readonly List<long> _elvesCarryAmount = [];

    internal override void Part1()
    {
        var currentCalorieSum = 0;
        foreach (var calories in File)
        {
            if (calories.Trim().Length > 0)
            {
                currentCalorieSum += int.Parse(calories);
            }
            else
            {
                _elvesCarryAmount.Add(currentCalorieSum);
                currentCalorieSum = 0;
            }
        }

        // Not the most efficient way, but the most readable
        Console.WriteLine(_elvesCarryAmount.Max());
    }

    internal override void Part2()
    {
        Part1();

        // Again, not the most efficient, but the most readable
        Console.WriteLine(_elvesCarryAmount.OrderByDescending(x => x).Take(3).Sum());
    }
}
