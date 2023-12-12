namespace AdventOfCode._2023;

internal abstract class Puzzle2023<T> : Puzzle<T> where T : class, new()
{
    private string[] _file;
    internal override string[] File
    {
        get
        {
            return _file ??= System.IO.File.ReadAllLines($"../../../2023/Files/{PuzzleName.ToLower()}.txt");
        }
    }
}
