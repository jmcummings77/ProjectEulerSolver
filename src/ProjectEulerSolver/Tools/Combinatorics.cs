using System.Numerics;

namespace ProjectEulerSolver.Tools;

/// <summary>Permutations, combinations and partition counting.</summary>
public static class Combinatorics
{
    /// <summary>
    /// Every permutation of <paramref name="items"/> in lexicographic order of the original positions.
    /// Each yielded array is a fresh copy, so callers may keep or mutate it.
    /// </summary>
    public static IEnumerable<T[]> Permutations<T>(IReadOnlyList<T> items)
    {
        var indices = Enumerable.Range(0, items.Count).ToArray();
        do
        {
            var permutation = new T[indices.Length];
            for (var i = 0; i < indices.Length; i++)
            {
                permutation[i] = items[indices[i]];
            }

            yield return permutation;
        }
        while (NextPermutation(indices));
    }

    /// <summary>
    /// Advances <paramref name="values"/> to the next lexicographic permutation in place.
    /// Returns false (and leaves the array unchanged) when it is already the last permutation.
    /// </summary>
    public static bool NextPermutation(int[] values)
    {
        var i = values.Length - 2;
        while (i >= 0 && values[i] >= values[i + 1])
        {
            i--;
        }

        if (i < 0)
        {
            return false;
        }

        var j = values.Length - 1;
        while (values[j] <= values[i])
        {
            j--;
        }

        (values[i], values[j]) = (values[j], values[i]);
        Array.Reverse(values, i + 1, values.Length - i - 1);
        return true;
    }

    /// <summary>All k-element subsets of <paramref name="items"/>, preserving the original order within each subset.</summary>
    public static IEnumerable<T[]> Combinations<T>(IReadOnlyList<T> items, int k)
    {
        if (k == 0)
        {
            yield return [];
            yield break;
        }

        for (var i = 0; i + k <= items.Count; i++)
        {
            var rest = new ArraySegment<T>([.. items], i + 1, items.Count - i - 1);
            foreach (var tail in Combinations(rest, k - 1))
            {
                yield return [items[i], .. tail];
            }
        }
    }

    /// <summary>
    /// Number of ways to write <paramref name="total"/> as a sum of the given parts, where order does not matter
    /// and each part may be used any number of times (the classic coin-change count).
    /// </summary>
    public static BigInteger CountPartitions(int total, IEnumerable<int> parts)
    {
        var ways = new BigInteger[total + 1];
        ways[0] = 1;
        foreach (var part in parts)
        {
            for (var amount = part; amount <= total; amount++)
            {
                ways[amount] += ways[amount - part];
            }
        }

        return ways[total];
    }
}
