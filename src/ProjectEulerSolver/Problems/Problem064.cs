using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many continued fractions of √N for N ≤ 10000 have an odd period.</summary>
public sealed class Problem064 : Problem
{
    public override int Number => 64;

    public override string Title => "Odd Period Square Roots";

    public override object Solve() => Solve(limit: 10_000);

    /// <summary>
    /// How many N from 1 to <paramref name="limit"/> have a continued fraction of √N whose period is odd.
    /// Perfect squares have no periodic part and are not counted.
    /// </summary>
    /// <param name="limit">
    /// Inclusive upper bound on N, from 0 to 5,000,000. The arithmetic is exact for any int; the cap only bounds
    /// the running time, which grows as limit^1.5 because the period of √N is about √N terms long.
    /// </param>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 5_000_000);

        var count = 0;
        for (var n = 2; n <= limit; n++)
        {
            if (ContinuedFractions.SqrtPeriodLength(n) % 2 == 1)
            {
                count++;
            }
        }

        return count;
    }
}
