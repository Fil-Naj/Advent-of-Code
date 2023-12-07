using AdventOfCode.Extensions;

namespace AdventOfCode._2022.Solutions;

internal class Puzzle07 : Puzzle2022<Puzzle07>
{
    private readonly Dictionary<string, Directory> Dirs = [];

    internal override void Part1()
    {
        var currentDir = new Directory("/", null);
        Dirs.Add(currentDir.Name, currentDir);

        foreach (var line in base.File)
        {
            var args = line.Split(" ");


            // Process command
            if (args[0] == "$")
            {
                // CD
                var command = args[1];
                if (command == "cd")
                {
                    var dir = args[2];
                    if (dir == "..")
                    {
                        currentDir = currentDir!.Parent;
                    }
                    else
                    {
                        var goingIntoDir = currentDir!.FullPath(dir);
                        if (Dirs.TryGetValue(goingIntoDir, out Directory? value))
                        {
                            currentDir = value;
                        }
                        else
                        {
                            var newDir = new Directory(dir, currentDir);
                            Dirs.Add(newDir.Name, newDir);
                            currentDir.Directories.Add(newDir);
                            currentDir = newDir;
                        }
                    }
                }
                else if (command == "ls")
                {
                    // we just read the lines
                    continue;
                }
            }
            else if (args[0] == "dir")
            {
                var dirName = args[1];
                var fullPathDir = currentDir!.FullPath(dirName);
                if (!Dirs.ContainsKey(fullPathDir))
                {
                    var newEmptyDir = new Directory(fullPathDir, currentDir);
                    Dirs.Add(newEmptyDir.Name, newEmptyDir);
                    currentDir.Directories.Add(newEmptyDir);
                }
            }
            else
            {
                var newFile = new File(int.Parse(args[0]), args[1]);
                currentDir!.Files.Add(newFile);
            }
        }

        // 1908462
    }

    internal override void Part2()
    {
        Part1();
        var unused = 70000000 - 43956976;
        var requiredDeleted = 30000000 - unused;
        Dirs.Values.Where(d => d.Size >= requiredDeleted).Min(d => d.Size).Dump();
    }

    private class Directory
    {
        public string Name { get; set; }
        public List<File> Files { get; set; }
        public List<Directory> Directories { get; set; }

        public Directory? Parent { get; set; }

        public int Size => Files.Sum(f => f.Size) + Directories.Sum(d => d.Size);

        public string FullPath(string newFile)
        {
            if (newFile == "/") return "/";
            return $"{Name}/{newFile}";
        }

        public Directory(string name, Directory? parent)
        {
            if (parent == null)
                Name = name;
            else
                Name = $"{parent?.Name}/{name}";
            Files = [];
            Directories = [];
            Parent = parent;
        }
    }

    private class File(int size, string name)
    {
        public int Size { get; set; } = size;
        public string Name { get; set; } = name;
    }
}
