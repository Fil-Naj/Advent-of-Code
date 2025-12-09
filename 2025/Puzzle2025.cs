namespace AdventOfCode._2025;

internal abstract class Puzzle2025<T> : Puzzle<T> where T : class, new()
{
    private string[] _file;
    internal override string[] File
    {
        get
        {
            return _file ??= System.IO.File.ReadAllLines($"../../../2025/Files/{PuzzleName.ToLower()}.txt");
        }
    }
}
