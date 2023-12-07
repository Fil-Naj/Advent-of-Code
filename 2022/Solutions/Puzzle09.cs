using AdventOfCode.Extensions;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle09 : Puzzle2022<Puzzle09>
{
    private readonly HashSet<string> _visited = [];

    internal override void Part1()
    {
        var head = new Head();

        int numTails = 9;
        var tails = new Tail[numTails];
        for (int i = 0; i < numTails; i++)
        {
            tails[i] = new Tail();
        }

        _visited.Add(tails[numTails - 1].GetCoordinates());

        foreach (var line in File)
        {
            var args = line.Split(" ");

            var direction = args[0];
            var amount = int.Parse(args[1]);

            while (amount > 0)
            {
                head.Move(direction);
                tails[0].Follow2(head);

                for (int i = 1; i < numTails; i++)
                {
                    tails[i].Follow2(tails[i - 1]);
                }

                // Get tail 9's coordinates
                _visited.Add(tails[numTails - 1].GetCoordinates());
                amount--;
            }
        }

        _visited.Count.Dump();
    }

    internal override void Part2()
    {
        // TODO: do not know what bit is what
    }

    private class Head : End
    {
        public int Move(string direction) => direction switch
        {
            "U" => Y++,
            "D" => Y--,
            "L" => X--,
            "R" => X++,
            _ => default
        };
    }

    private class Tail : End
    {
        private readonly int[,] _guide = new int[5, 5]
                                            // Columns represent the difference in the opposite coordinates
                                            {//	-2		-1		0		1		2		// Rows represent difference in coordinates we want to change
											{-1,    -1,     -1,     -1,     -1},	//-2
											{-1,    0,      0,      0,      -1},	// -1
											{0,     0,      0,      0,      0},		// 0
											{1,     0,      0,      0,      1},		// 1
											{1,     1,      1,      1,      1,}};   // 2


        public void Follow2(End head)
        {
            // Add +2 to normalise difference to 0 indexing
            // In this case, we don't care if they are touching
            var xDiff = head.X - X + 2;
            var yDiff = head.Y - Y + 2;

            X += _guide[xDiff, yDiff];
            Y += _guide[yDiff, xDiff];
        }

        public bool IsTouchingHead(int xDiff, int yDiff)
            => (xDiff >= -1 && xDiff <= 1) && (yDiff >= -1 && yDiff <= 1);
    }

    private abstract class End
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string GetCoordinates()
        {
            return $"{X}-{Y}";
        }
    }
}
