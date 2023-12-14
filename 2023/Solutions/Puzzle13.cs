using AdventOfCode.Extensions;
using System.Text;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle13 : Puzzle2023<Puzzle13>
{
    internal override void Part1()
    {
        List<string[]> patterns = GetPatterns(File);

        var leftCount = 0L;
        var upCount = 0L;
        foreach (var pattern in patterns)
        {
            var (toTheLeft, upAndAbove) = CountReflections(pattern);
            leftCount += toTheLeft;
            upCount += upAndAbove;
        }

        (leftCount + 100 * upCount).Dump("Part 1 Answer");
    }

    private List<string[]> GetPatterns(string[] lines)
    {
        List<string[]> patterns = [];

        List<string> currentPattern = [];
        foreach (var line in File)
        {
            if (line.Length == 0)
            {
                patterns.Add([.. currentPattern]);
                currentPattern.Clear();
            }
            else
            {
                currentPattern.Add(line);
            }
        }

        // Take care if last one
        if (currentPattern.Count > 0) 
            patterns.Add(currentPattern.ToArray());

        return patterns;
    }

    private string[] GetColumns(string[] pattern)
    {
        var rows = pattern.Length;
        var columns = new string[pattern[0].Length];
        for (int c = 0; c < columns.Length; c++)
        {
            StringBuilder sb = new();
            for (int r = 0; r < rows; r++)
            {
                sb.Append(pattern[r][c]);
            }

            columns[c] = sb.ToString();
        }

        return columns;
    }

    private (long toTheLeft, long upAndAbove) CountReflections(string[] pattern)
    {
        var rows = pattern;
        var columns = GetColumns(pattern);

        var toTheLeft = 0L;
        for (var c = 1; c < columns.Length; c++)
            toTheLeft = IsReflectionPoint(columns, c - 1, c) ? c : toTheLeft;

        var rowsAbove = 0L;
        for (var r = 1; r < rows.Length; r++)
            rowsAbove = IsReflectionPoint(rows, r - 1, r) ? r : rowsAbove;

        toTheLeft.Dump("Everything you own in a box to the left");
        rowsAbove.Dump("Up and above");

        if (toTheLeft > 0 && rowsAbove > 0)
            throw new Exception("TWO REFLECTION POINTS");

        return (toTheLeft, rowsAbove);
    }

    private bool IsReflectionPoint(string[] pattern, int s1, int s2)
    {
        // If we are too far left or too far above, stop counting
        if (s1 < 0 || s2 >= pattern.Length) return true;

        // If both within bounds, check reflections normally
        // If they do not match, then no more reflections to count, chain is broken
        if (pattern[s1] != pattern[s2]) return false;

        // If there is a match, keep expanding
        return IsReflectionPoint(pattern, s1 - 1, s2 + 1);
    }

    internal override void Part2()
    {
        List<string[]> patterns = GetPatterns(File);

        var leftCount = 0L;
        var upCount = 0L;
        foreach (var pattern in patterns)
        {
            var (toTheLeft, upAndAbove) = CountReflectionsWhereSmudgeWasFixed(pattern);
            leftCount += toTheLeft;
            upCount += upAndAbove;
        }

        (leftCount + 100 * upCount).Dump("Part 1 Answer");
    }

    private (long toTheLeft, long upAndAbove) CountReflectionsWhereSmudgeWasFixed(string[] pattern)
    {
        var rows = pattern;
        var columns = GetColumns(pattern);

        var toTheLeft = 0L;
        for (var c = 1; c < columns.Length; c++)
            toTheLeft = IsReflectionPointAndWasTheSmudgeFixed(columns, c - 1, c, false) ? c : toTheLeft;

        var rowsAbove = 0L;
        for (var r = 1; r < rows.Length; r++)
            rowsAbove = IsReflectionPointAndWasTheSmudgeFixed(rows, r - 1, r, false) ? r : rowsAbove;

        toTheLeft.Dump("Everything you own in a box to the left");
        rowsAbove.Dump("Up and above");

        if (toTheLeft > 0 && rowsAbove > 0)
            throw new Exception("TWO REFLECTION POINTS");

        return (toTheLeft, rowsAbove);
    }

    private bool IsReflectionPointAndWasTheSmudgeFixed(string[] pattern, int s1, int s2, bool wasSmudgeFixed)
    {
        // If we are too far left or too far above, stop counting
        if (s1 < 0 || s2 >= pattern.Length) return true && wasSmudgeFixed;

        // If both within bounds, check reflections normally
        // If they do not match, then check if there was a smudge to be changed
        if (pattern[s1] != pattern[s2])
        {
            // If different by one, could be a potential smudge
            if (DiffersByOne(pattern[s1], pattern[s2]))
            {
                // If we have a difference of 1, but we already had a difference of 1, then there can't be TWO smudges
                // But if this is our first possible smudge, then kepe goign!
                return !wasSmudgeFixed && IsReflectionPointAndWasTheSmudgeFixed(pattern, s1 - 1, s2 + 1, true);
            }

            // If different by more than one, then that's not a smudge, it is just wrong
            return false;
        };

        // If there is a match, keep expanding
        return IsReflectionPointAndWasTheSmudgeFixed(pattern, s1 - 1, s2 + 1, wasSmudgeFixed);
    }

    private bool DiffersByOne(string s1, string s2)
    {
        var delta = 0;
        for (var i = 0; i < s1.Length; i++)
            delta += s1[i] == s2[i] ? 0 : 1;

        return delta == 1;
    }
}
