namespace AdventOfCode._2023;

internal abstract class Puzzle2023<T> : Puzzle<T> where T : class, new()
{
    internal override string[] File => System.IO.File.ReadAllLines($"../../../2023/Files/{PuzzleName.ToLower()}.txt");
}
