using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle08 : Puzzle2023<Puzzle08>
{
    private readonly Dictionary<string, (string left, string right)> _map = [];

    private const string Start = "AAA";
    private const string End = "ZZZ";

    private const char Left = 'L';
    private const char Right = 'R';

    internal override void Part1()
    {
        var instructions = File[0];
        var instructionsLength = instructions.Length;

        foreach (var line in File[2..])
        {
            var directions = line[(line.IndexOf('(') + 1)..line.IndexOf(')')].Split(", ");
            _map[line[..3]] = (directions[0], directions[1]);
        }

        var currentLocation = "AAA";
        var instructionIndex = 0;
        var stepsCount = 0L;
        while (currentLocation != End)
        {
            currentLocation = instructions[instructionIndex] == Left
                ? _map[currentLocation].left
                : _map[currentLocation].right;

            stepsCount++;
            instructionIndex = (instructionIndex + 1) % instructionsLength;
        }

        stepsCount.Dump("Part 1 Answer");
    }

    private const char StartSuffix = 'A';
    private const char EndSuffix = 'Z';

    internal override void Part2()
    {
        var instructions = File[0];
        var instructionsLength = instructions.Length;

        List<string> currentLocations = [];
        foreach (var line in File[2..])
        {
            var directions = line[(line.IndexOf('(') + 1)..line.IndexOf(')')].Split(", ");
            var start = line[..3];
            _map[start] = (directions[0], directions[1]);

            // If ends with 'A', use as starting location
            if (start.EndsWith(StartSuffix))
                currentLocations.Add(start);
        }

        long StepsToZ(string current)
        {
            var steps = 0;
            while (!current.EndsWith(EndSuffix))
            {
                current = instructions[steps % instructions.Length] == Left
                    ? _map[current].left
                    : _map[current].right;
                steps++;
            }

            return steps;
        }

        long lcm = 1;
        foreach (var start in currentLocations)
        {
            lcm = LowestCommonMultiple(lcm, StepsToZ(start));
        }

        lcm.Dump("Part 2 Answer");
    }
    private long LowestCommonMultiple(long a, long b) => a * b / Gcd(a, b);

    private long Gcd(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}
