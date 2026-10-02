using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first value that can be written as the sum of primes in over five thousand different ways.</summary>
public sealed class Problem077 : Problem
{
    public override int Number => 77;

    public override string Title => "Prime Summations";

    public override object Solve() => Solve(ways: 5000);

    /// <summary>
    /// The first value that can be written as a sum of primes in more than <paramref name="ways"/> different ways.
    /// A prime on its own counts as one way of writing itself.
    /// </summary>
    /// <param name="ways">The number of ways to exceed: from 0 to <see cref="long.MaxValue"/>.</param>
    public static int Solve(long ways)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ways);

        // A sum for n only uses primes up to n, so a table built from the primes up to `limit` is exact for every
        // value up to `limit`; double `limit` until the table contains the answer. The search always ends because
        // the number of ways is unbounded (2s and 3s alone give more than n/6 ways for even n). The answer can
        // only grow with `ways` and is 1,287 for long.MaxValue, so `limit` never passes 2,048.
        for (var limit = 16; ; limit *= 2)
        {
            var counts = new BigInteger[limit + 1];
            counts[0] = 1;
            foreach (var prime in Primes.UpTo(limit))
            {
                for (var amount = prime; amount <= limit; amount++)
                {
                    counts[amount] += counts[amount - prime];
                }
            }

            for (var n = 2; n <= limit; n++)
            {
                if (counts[n] > ways)
                {
                    return n;
                }
            }
        }
    }
}
