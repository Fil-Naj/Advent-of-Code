namespace AdventOfCode;

internal abstract class Puzzle<TPuzzle> where TPuzzle : class, new()
{
    internal static string PuzzleName => typeof(TPuzzle).Name;

    internal static TPuzzle Instance = new();

    internal abstract string[] File { get; }

    internal abstract void Part1();

    internal abstract void Part2();
}
