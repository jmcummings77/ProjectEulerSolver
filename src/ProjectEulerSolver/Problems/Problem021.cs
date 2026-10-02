using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all the amicable numbers under 10000.</summary>
public sealed class Problem021 : Problem
{
    public override int Number => 21;

    public override string Title => "Amicable Numbers";

    public override object Solve() => Solve(limit: 10_000);

    /// <summary>
    /// Sum of the amicable numbers below <paramref name="limit"/>: every a with d(a) = b, d(b) = a and a ≠ b, where d(n)
    /// is the sum of the proper divisors of n. Only a has to be below the limit; its partner b may lie above it.
    /// </summary>
    /// <param name="limit">Exclusive upper bound on the amicable numbers that are summed, from 0 to 10,000,000.</param>
    public static long Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        // The cap keeps the divisor-sum sieve to 40 MB and a few seconds. It also rules out overflow: the proper
        // divisors of n are among n/2, n/3, ..., n/n, so they add up to less than n·ln(n), under 1.7 × 10^8 here.
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 10_000_000);

        var divisorSums = NumberTheory.ProperDivisorSums(Math.Max(limit - 1, 0));
        long total = 0;
        for (var a = 2; a < limit; a++)
        {
            var b = divisorSums[a];
            if (b == a)
            {
                continue; // Perfect numbers are not amicable.
            }

            // The sieve only covers numbers below the limit, so a partner beyond it is checked directly.
            var back = b < limit ? divisorSums[b] : NumberTheory.ProperDivisorSum(b);
            if (back == a)
            {
                total += a;
            }
        }

        return total;
    }
}
