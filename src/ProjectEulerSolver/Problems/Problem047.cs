using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first of four consecutive integers that each have four distinct prime factors.</summary>
public sealed class Problem047 : Problem
{
    public override int Number => 47;

    public override string Title => "Distinct Primes Factors";

    public override object Solve() => Solve(count: 4);

    /// <summary>
    /// The first of the earliest <paramref name="count"/> consecutive integers that each have exactly
    /// <paramref name="count"/> distinct prime factors.
    /// </summary>
    /// <param name="count">
    /// Both the length of the run and the number of distinct prime factors, from 1 to 4. (The run for 5 starts
    /// beyond 129 million, where this sieve of factor counts would need half a gigabyte.)
    /// </param>
    public static int Solve(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, 4);

        // Nothing bounds the answer in advance, so sieve up to a limit and keep doubling it until a run appears.
        // Every pass scans from 2, so the first run it meets is the earliest one. The supported counts all have
        // runs (starting at 2, 14, 644 and 134043), so the loop ends; the doubling is checked so that it could
        // only ever fail loudly rather than wrap around.
        for (var limit = 1 << 10; ; limit = checked(limit * 2))
        {
            var factorCounts = Primes.DistinctPrimeFactorCounts(limit);
            var run = 0;
            for (var n = 2; n <= limit; n++)
            {
                run = factorCounts[n] == count ? run + 1 : 0;
                if (run == count)
                {
                    return n - count + 1;
                }
            }
        }
    }
}
