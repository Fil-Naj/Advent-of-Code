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
        foreach (var line in File)
            sum += FindNumberOfPermutations(line);

        sum.Dump("Part 2 Answer");
    }

    private long FindNumberOfPermutations(string line)
    {
        var items = line.Split(' ');
        var str = string.Join('?', Enumerable.Repeat(items[0], 5));
        var nums = string.Join(',', Enumerable.Repeat(items[1], 5)).Split(',').Select(int.Parse).ToList();

        Dictionary<string, long> dp = [];
        long Dfs(string str, List<int> nums)
        {
            var key = CreateKey(str, nums);

            if (dp.TryGetValue(key, out var val))
                return val;

            // If we do not have any more groups to create, finish search
            // If have any broken gears, then not valid solution
            // If we have ? still left, then we don't care because they all have to be dotss
            if (nums.Count == 0)
                return str.Contains('#') ? 0 : 1;

            // If we still have groups to go, but no more strings, then leave brother damn give up
            if (str.Length == 0) return 0;

            // If next character is a dot, get rid of them allllll up until something ain't a dot
            if (str.StartsWith('.'))
            {
                var newStr = str.TrimStart('.');
                var newKey = CreateKey(newStr, nums);
                dp[newKey] = Dfs(newStr, nums);
                return dp[newKey];
            }

            // If we mysterious in this bch, then go find your path by replacing it for a '.' or a '#'
            if (str.StartsWith('?'))
            {
                // Replace with '.'
                var newNotBrokenString = '.' + str[1..];
                var newNotBrokenKey = CreateKey(newNotBrokenString, nums);
                dp[newNotBrokenKey] = Dfs(newNotBrokenString, nums);

                // Replace with '#'
                var newBrokenString = '#' + str[1..];
                var newBrokenKey = CreateKey(newBrokenString, nums);
                dp[newBrokenKey] = Dfs(newBrokenString, nums);


                return dp[newNotBrokenKey] + dp[newBrokenKey];
            }

            // It's broken records time
            // At this point, it is guaranteed we have a broken '#'

            // If we have a broken still, but no more groups to make, then it is NOT VALID
            if (nums.Count == 0) return 0;

            // If we still have groups to make, but not enough brokens to make the group with, then it is INVALID AGAIN
            if (str.Length < nums[0]) return 0;

            // If we have a record, then everything in this group must either be a '#' or a '?'
            // Otherwise, this shit is INVALID
            if (str[..nums[0]].Contains('.')) return 0;

            // If we have two or more groups to go (i.e., not the last one)
            if (nums.Count > 1)
            {
                // If our suspected group is then followed by another '#', you guessed it, IT IS INVALID
                if (str.Length < nums[0] + 1 || str[nums[0]] == '#') return 0;

                // Now that we are all happy with our pruning, we can be certain that the next nums[0] characters, plus the following one, are all good
                // Thus, skip to them and remove the current group
                str = str[(nums[0] + 1)..];
                nums = nums[1..];

                var newkey = CreateKey(str, nums);
                dp[newkey] = Dfs(str, nums);
                return dp[newkey];
            }


            // If we get to here, this is the last group to go
            str = str[nums[0]..];
            nums = nums[1..];
            var newNextStepKey = CreateKey(str, nums);
            dp[newNextStepKey] = Dfs(str, nums);

            return dp[newNextStepKey];
        }

        var toAdd = Dfs(str, nums);
        toAdd.Dump("Count");
        return toAdd;
    }

    private string CreateKey(string str, List<int> nums) => $"{str}|{string.Join(',', nums)}";
}
