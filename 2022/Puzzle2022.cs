namespace AdventOfCode._2022;

internal abstract class Puzzle2022<T> : Puzzle<T> where T : class, new()
{
    internal override string[] File => System.IO.File.ReadAllLines($"../../../2022/Files/{PuzzleName.ToLower()}.txt");
}
