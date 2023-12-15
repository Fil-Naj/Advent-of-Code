using System.Text.RegularExpressions;
using AdventOfCode.Extensions;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle15 : Puzzle2023<Puzzle15>
{
    internal override void Part1()
    {
        var instructions = File[0].Split(',');
        var sum = 0L;
        foreach (var instruction in instructions)
        {
            var hash = 0L;
            foreach (char c in instruction)
            {
                hash += c;
                hash *= 17;
                hash %= 256;
            }
            sum += hash;
        }

        sum.Dump("Part 1 Answer");
    }

    private readonly Dictionary<string, int> _hashCache = [];

    internal override void Part2()
    {
        var instructions = File[0].Split(',');

        // Setup the boxes
        var boxes = new Box[256];
        for (int i = 0; i < boxes.Length; i++)
            boxes[i] = new Box();

        foreach (var instruction in instructions)
        {
            string pattern = @"([a-zA-Z]+)([=-])(\d)?";
            Match match = Regex.Match(instruction, pattern);

            var label = match.Groups[1].Value;
            var hash = ComputeLabelHash(label);

            // If operation is '='
            if (match.Groups[2].Value == "=")
            {
                var focalLength = int.Parse(match.Groups[3].Value);
                var lensToAdd = new Lens()
                {
                    Label = label,
                    FocalLength = focalLength,
                };

                boxes[hash].Add(lensToAdd);

            }
            // If operation is '-'
            else
            {
                boxes[hash].Remove(label);
            }
        }

        var focusingPower = 0L;
        for (int b = 0; b < boxes.Length; b++)
        {
            for (int i = 0; i < boxes[b].LensOrder.Count; i++)
            {
                var lens = boxes[b].LensOrder[i];
                focusingPower += (1 + b) * (i + 1) * lens.FocalLength;
            }
        }

        focusingPower.Dump("Part 2 Answer");
    }

    private int ComputeLabelHash(string label)
    {
        if (_hashCache.TryGetValue(label, out var hashValue))
            return hashValue;

        var hash = 0;
        foreach (char c in label)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }

        _hashCache[label] = hash;
        return hash;
    }

    private class Box
    {
        public Dictionary<string, Lens> LensInBox = [];
        public List<Lens> LensOrder = [];

        public void Add(Lens lens)
        {
            if (LensInBox.TryGetValue(lens.Label!, out var lensToReplace))
            {
                lensToReplace.FocalLength = lens.FocalLength;
                return;
            }

            LensOrder.Add(lens);
            LensInBox[lens.Label!] = lens;
        }

        public void Remove(string label)
        {
            if (!LensInBox.TryGetValue(label, out var lensToRemove)) return;

            LensInBox.Remove(label);
            LensOrder.Remove(lensToRemove);
        }
    }

    private class Lens
    {
        public string? Label { get; set; }
        public int FocalLength { get; set; }
    }
}
