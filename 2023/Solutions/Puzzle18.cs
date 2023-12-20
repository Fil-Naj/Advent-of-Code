using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle18 : Puzzle2023<Puzzle18>
{
    // Shoutout Green's theorem
    // Much better than Blue's theorem
    // Essententially, the area of any flat (i.e., only has straight lines), 2D shape can be calculated using Green's Theorem
    // Don't even worry about curves, too much math
    // Green's Theorem states that an on object's area is equal to the sum of one of its coordinates multiplied by the delta of the opposing coordinate at each vertex
    // In simple terms, A = sum(y * dx) = sum(x * dy)
    // Pick whether we will track y or x. For this explanation, we will choose to track y as we have done so in the code
    // Then, for each vertex, if the instruction is a change in y (i.e. up or down move), keep track of y so we know the y of each vertex
    // When we encounter a change in x (i.e., left or right), we multiply the y coordinate by the change in x (in this case dx is numSteps)
    // In our axis, left is a negative change in x, while right is positive dx
    // Sum up these y * dx values for each vertex
    // And BOOM! You have the area of some random straight lined shape
    internal override void Part1()
    {
        GetAreaGreensTheoremStyle(1).Dump("Part 1 Answer");
    }

    internal override void Part2()
    {
        GetAreaGreensTheoremStyle(2).Dump("Part 1 Answer");
    }

    private long GetAreaGreensTheoremStyle(int partNumber)
    {
        var area = 0L;
        var y = 0L;
        var perimeter = 0L;
        foreach (var line in File)
        {
            var instructions = line.Split(' ');
            string direction;
            long numSteps;
            if (partNumber == 1)
            {
                direction = instructions[0];
                numSteps = int.Parse(instructions[1]);
            }
            else
            {
                var hexNum = instructions[2][2..7];
                numSteps = Convert.ToInt64(instructions[2][2..7], 16);
                direction = instructions[2][7] switch
                {
                    '0' => "R",
                    '1' => "D",
                    '2' => "L",
                    '3' => "U",
                    _ => string.Empty
                };
            }

            perimeter += numSteps;

            if (direction is "L")
                area -= y * numSteps;
            else if (direction is "R")
                area += y * numSteps;
            else if (direction == "U")
                y += numSteps;
            else
                y -= numSteps;
        }

        return area + (perimeter / 2) + 1;
    }
}
