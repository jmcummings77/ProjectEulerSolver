using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest cube for which exactly five permutations of its digits are also cube.</summary>
public sealed class Problem062 : Problem
{
    public override int Number => 62;

    public override string Title => "Cubic Permutations";

    public override object Solve()
    {
        // Cubes with the same digit multiset share a sorted-digit key. Process cubes in digit-length
        // batches so "exactly five" can be verified before moving on.
        var groups = new Dictionary<string, (long Smallest, int Count)>();
        var currentLength = 1;
        for (long n = 1; ; n++)
        {
            var cube = n * n * n;
            var length = Digits.Count(cube);
            if (length != currentLength)
            {
                var hit = groups.Values.Where(g => g.Count == 5).Select(g => g.Smallest).DefaultIfEmpty(0).Min();
                if (hit != 0)
                {
                    return hit;
                }

                groups.Clear();
                currentLength = length;
            }

            var key = Digits.SortedKey(cube);
            groups[key] = groups.TryGetValue(key, out var group) ? (group.Smallest, group.Count + 1) : (cube, 1);
        }
    }
}
