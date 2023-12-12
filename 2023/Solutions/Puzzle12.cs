using AdventOfCode.Extensions;
using System.Text;

namespace AdventOfCode._2023.Solutions;

internal class Puzzle12 : Puzzle2023<Puzzle12>
{
    internal override void Part1()
    {
        var sum = 0L;
        foreach (var line in File)
        {
            var items = line.Split(' ');
            var str = items[0];
            var nums = items[1].Split(',').Select(int.Parse).ToArray();

            StringBuilder sb = new();
            Dictionary<string, long> dp = [];
            // DFS every solution. Done it once before? Don't care do it again. Is already broken? Keep going!
            // Call me Brute how I come with this force
            long Dfs(int index)
            {
                var currentString = sb.ToString();
                if (dp.TryGetValue(currentString, out var value))
                    return value;

                if (index == str.Length)
                {
                    var areEqual = AreEqual(CalculateNums(sb.ToString()), nums);

                    return areEqual ? 1 : 0;
                }

                if (str[index] == '?')
                {
                    // Count if adding not broken
                    sb.Append('.');
                    var countFromNotBroken = Dfs(index + 1);
                    sb.Length--;

                    // Count if adding broken
                    sb.Append('#');
                    var countFromBroken = Dfs(index + 1);
                    sb.Length--;

                    dp[currentString] = countFromBroken + countFromNotBroken;

                    return dp[currentString];
                }
                
                // If not mystery, then just continue
                sb.Append(str[index]);
                var countFromContinuing = Dfs(index + 1);
                sb.Length--;

                dp[currentString] = countFromContinuing;
                return dp[currentString];
            }

            sum += Dfs(0);
        }

        sum.Dump("Part 1 Answer");
    }

    private int[] CalculateNums(string path)
    {
        return path.Split('.')
            .Where(s => s.Trim().Length > 0)
            .Select(s => s.Length)
            .ToArray();
    }

    private bool AreEqual(int[] arr1, int[] arr2)
    {
        if (arr1.Length != arr2.Length) return false;

        for (int i = 0; i < arr1.Length; i++)
            if (arr1[i] != arr2[i]) return false;

        return true;
    }

    internal override void Part2()
    {
        // To be continued...
        var sum = 0L;
        Parallel.ForEach(
            File,
            () => 0L, // Local sum for each thread
            (line, loopState, localSum) =>
            {
                // Perform the operation on the current line
                long result = FindNumberOfPermutations(line);

                // Add the result to the local sum
                localSum += result;

                return localSum;
            },
            localSum =>
            {
                // Add the local sum to the total sum in a thread-safe manner
                Interlocked.Add(ref sum, localSum);
            }
        );

        sum.Dump("Part 2 Answer");
    }

    private long FindNumberOfPermutations(string line)
    {
        var items = line.Split(' ');
        var str = string.Join('?', Enumerable.Repeat(items[0], 5));
        var nums = string.Join(',', Enumerable.Repeat(items[1], 5)).Split(',').Select(int.Parse).ToArray();

        StringBuilder sb = new();
        Dictionary<string, long> dp = [];
        // FFS we have to actually do some optimisation on this one
        // Time for Dynamiccc Programminggg:
        // 1. Keep track of all visited strings and how many different ways they can be done (0 if none)
        // 2. Prune! If shit is fucked already, don't bother with continuing. Give up like a real man!
        // Uh actually I think that is it?
        long Dfs(int index)
        {
            var currentString = sb.ToString();

            // If we have already visited the string, then just return the value from last time
            if (dp.TryGetValue(currentString, out var value))
                return value;

            // Prune, baby, prune!
            var current = CalculateNums(currentString);
            if (!AreEqualSoFar(current, nums)) return 0;

            if (index == str.Length)
            {
                var areEqual = AreEqual(CalculateNums(currentString), nums);
                var count = areEqual ? 1 : 0;
                dp[currentString] = count;

                return dp[currentString];
            }

            if (str[index] == '?')
            {
                // Count if adding not broken
                sb.Append('.');
                var countFromNotBroken = Dfs(index + 1);
                sb.Length--;

                // Count if adding broken
                sb.Append('#');
                var countFromBroken = Dfs(index + 1);
                sb.Length--;

                dp[currentString] = countFromBroken + countFromNotBroken;

                return dp[currentString];
            }

            // If not mystery, then just continue
            sb.Append(str[index]);
            var countFromContinuing = Dfs(index + 1);
            sb.Length--;

            dp[currentString] = countFromContinuing;
            return dp[currentString];
        }

        var toAdd = Dfs(0);
        return toAdd;
    }

    private bool AreEqualSoFar(int[] actual, int[] expected)
    {
        if (actual.Length > expected.Length) return false;

        // Check that all numbers up to the last one are equal
        for (int i = 0; i < actual.Length - 1; i++)
            if (actual[i] != expected[i]) return false;

        // With the last one of the actual array, ensure it is not greater than its respective value in the expected array
        if (actual.Length > 0 && actual[^1] > expected[actual.Length - 1]) return false;

        return true;
    }
}
