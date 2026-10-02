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

    /// <summary>
    /// All k-element subsets of <paramref name="items"/> in lexicographic order of positions, preserving the
    /// original order within each subset. Only the yielded arrays are allocated.
    /// </summary>
    public static IEnumerable<T[]> Combinations<T>(IReadOnlyList<T> items, int k)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(k);
        if (k > items.Count)
        {
            yield break;
        }

        var indices = Enumerable.Range(0, k).ToArray();
        while (true)
        {
            var combination = new T[k];
            for (var i = 0; i < k; i++)
            {
                combination[i] = items[indices[i]];
            }

            yield return combination;

            // Advance the rightmost index that has room to move, then reset everything after it.
            var pivot = k - 1;
            while (pivot >= 0 && indices[pivot] == items.Count - k + pivot)
            {
                pivot--;
            }

            if (pivot < 0)
            {
                yield break;
            }

            indices[pivot]++;
            for (var i = pivot + 1; i < k; i++)
            {
                indices[i] = indices[i - 1] + 1;
            }
        }
    }

    /// <summary>
    /// Number of ways to write <paramref name="total"/> as a sum of the given positive parts, where order does
    /// not matter and each part may be used any number of times (the classic coin-change count).
    /// </summary>
    public static BigInteger CountPartitions(int total, IEnumerable<int> parts)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(total);
        var ways = new BigInteger[total + 1];
        ways[0] = 1;
        foreach (var part in parts)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(part, 1, nameof(parts));
            for (var amount = part; amount <= total; amount++)
            {
                ways[amount] += ways[amount - part];
            }
        }

        return ways[total];
    }
}
