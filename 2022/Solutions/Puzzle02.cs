namespace AdventOfCode._2022.Solutions;

internal class Puzzle02 : Puzzle2022<Puzzle02>
{
    // Turns
    const int Rock = 0;
    const int Paper = 1;
    const int Scissors = 2;


    //                                                 Rock	 Paper	 Scissors (Us)
    private readonly int[,] _outComes = new int[,] { { 3,    6,      0 }, 	// Rock
								                     { 0,    3,      6 }, 	// Paper
								                     { 6,    0,      3 }}; 	// Scissors (elf)

    private readonly int[,] _toPlay = new int[,] {
                                { Scissors, Rock,       Paper }, 	// Rock
								{ Rock,     Paper,      Scissors }, // Paper
								{ Paper,    Scissors,   Rock}}; 	// Scissors

    internal override void Part1()
    {
        // TODO: 
    }

    internal override void Part2()
    {
        var score = 0;
        foreach (var line in File)
        {
            var hands = line.Split(" ");
            var handScore = _toPlay[ToHand(hands[0]), IndexOfPlay(hands[1])];
            score += OutcomeScore(hands[1]) + handScore + 1;
        }

        Console.WriteLine(score);
    }

    private int ToHand(string hand) => hand switch
    {
        "A" or "X" => Rock,
        "B" or "Y" => Paper,
        "C" or "Z" => Scissors,
        _ => Rock,
    };

    private int OutcomeScore(string hand) => hand switch
    {
        "X" => 0,
        "Y" => 3,
        "Z" => 6,
        _ => 0,
    };

    private int IndexOfPlay(string hand) => hand switch
    {
        "X" => 0,
        "Y" => 1,
        "Z" => 2,
        _ => 0,
    };
}
