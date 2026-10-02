using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>How many different ways one hundred can be written as a sum of at least two positive integers.</summary>
public sealed class Problem076 : Problem
{
    private const int MaxSupportedNumber = 100_000;

    public override int Number => 76;

    public override string Title => "Counting Summations";

    public override object Solve() => Solve(n: 100);

    /// <summary>How many different ways <paramref name="n"/> can be written as a sum of at least two positive integers.</summary>
    /// <param name="n">The number to split: from 1 to 100,000 (the work grows as n^1.5 additions of numbers with about √n digits).</param>
    public static BigInteger Solve(int n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, MaxSupportedNumber);

        // Every partition of n except "n" on its own has at least two parts.
        return PartitionNumber(n) - 1;
    }

    /// <summary>p(n), the number of ways to write n as an unordered sum of positive integers.</summary>
    private static BigInteger PartitionNumber(int n)
    {
        // Euler's pentagonal number theorem: p(m) = Σ (-1)^(k+1) [p(m - k(3k-1)/2) + p(m - k(3k+1)/2)] over k ≥ 1,
        // with p(0) = 1 and p of a negative number taken as 0.
        var partitions = new BigInteger[n + 1];
        partitions[0] = 1;
        for (var m = 1; m <= n; m++)
        {
            var total = BigInteger.Zero;
            for (var k = 1; k * (3 * k - 1) / 2 <= m; k++)
            {
                var pair = partitions[m - k * (3 * k - 1) / 2];
                var second = m - k * (3 * k + 1) / 2;
                if (second >= 0)
                {
                    pair += partitions[second];
                }

                total += k % 2 == 1 ? pair : -pair;
            }

            partitions[m] = total;
        }

        return partitions[n];
    }
}
