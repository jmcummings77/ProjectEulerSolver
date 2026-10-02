using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many distinct terms are in the sequence a^b for 2 ≤ a ≤ 100 and 2 ≤ b ≤ 100.</summary>
public sealed class Problem029 : Problem
{
    public override int Number => 29;

    public override string Title => "Distinct Powers";

    public override object Solve() => Solve(maxBase: 100, maxExponent: 100);

    /// <summary>
    /// How many distinct values a^b takes for 2 ≤ a ≤ <paramref name="maxBase"/> and 2 ≤ b ≤ <paramref name="maxExponent"/>.
    /// </summary>
    /// <param name="maxBase">The largest base, 2 or more.</param>
    /// <param name="maxExponent">The largest exponent, from 2 to 1,000,000.</param>
    public static long Solve(int maxBase, int maxExponent)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxBase, 2);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxExponent, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxExponent, 1_000_000); // Keeps the table of products below to 30 MB.

        // Every base a ≥ 2 is r^k for exactly one root r that is not itself a perfect power, and powers of different
        // roots never coincide. So a^b = r^(kb), and a root whose powers r, r², ..., r^K are the bases up to maxBase
        // contributes one value per distinct product k·b with 1 ≤ k ≤ K. That count depends only on K, and K is
        // largest for the root 2.
        var largestK = 0;
        for (long power = 2; power <= maxBase; power *= 2)
        {
            largestK++;
        }

        // largestK is at most 30, since 2^31 exceeds every int.
        var isProduct = new bool[largestK * maxExponent + 1];
        var distinctProducts = new long[largestK + 1];
        for (var k = 1; k <= largestK; k++)
        {
            distinctProducts[k] = distinctProducts[k - 1];
            for (var b = 2; b <= maxExponent; b++)
            {
                if (!isProduct[k * b])
                {
                    isProduct[k * b] = true;
                    distinctProducts[k]++;
                }
            }
        }

        // Only roots up to √maxBase have a second power in range, so only they need to be visited one by one.
        var rootLimit = (int)NumberTheory.ISqrt(maxBase);
        var isPerfectPower = new bool[rootLimit + 1];
        long total = 0;
        long basesCovered = 0;
        for (var root = 2; root <= rootLimit; root++)
        {
            if (isPerfectPower[root])
            {
                continue;
            }

            var powersInRange = 0;
            for (long power = root; power <= maxBase; power *= root)
            {
                powersInRange++;
                if (power <= rootLimit)
                {
                    isPerfectPower[power] = true;
                }
            }

            total += distinctProducts[powersInRange];
            basesCovered += powersInRange;
        }

        // Every base not covered above is a root above √maxBase standing alone (K = 1).
        return total + (maxBase - 1 - basesCovered) * distinctProducts[1];
    }
}
