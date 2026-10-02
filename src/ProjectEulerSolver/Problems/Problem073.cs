namespace ProjectEulerSolver.Problems;

/// <summary>How many fractions lie between 1/3 and 1/2 in the sorted set of reduced proper fractions for d ≤ 12,000.</summary>
public sealed class Problem073 : Problem
{
    private const int MaxSupportedDenominator = 100_000_000;

    public override int Number => 73;

    public override string Title => "Counting Fractions in a Range";

    public override object Solve() => Solve(maxDenominator: 12_000);

    /// <summary>
    /// How many reduced proper fractions with a denominator of at most <paramref name="maxDenominator"/> lie
    /// strictly between 1/3 and 1/2.
    /// </summary>
    /// <param name="maxDenominator">The largest denominator counted: from 1 to 100,000,000 (the sieve takes four bytes per denominator).</param>
    public static long Solve(int maxDenominator)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxDenominator, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxDenominator, MaxSupportedDenominator);

        // counts[d] starts as the number of all fractions n/d with 1/3 < n/d < 1/2, reduced or not. Every
        // unreduced one is a reduced fraction with a denominator that properly divides d, so subtracting each
        // denominator's reduced count from its multiples (in ascending order) leaves only the reduced fractions.
        // An entry starts at no more than d/6 and only ever decreases towards its final, non-negative value,
        // so an int holds it; the total is below maxDenominator² / 12 and needs a long.
        var counts = new int[maxDenominator + 1];
        for (var d = 1; d <= maxDenominator; d++)
        {
            counts[d] = (d - 1) / 2 - d / 3;
        }

        long total = 0;
        for (var d = 1; d <= maxDenominator; d++)
        {
            var reduced = counts[d];
            if (reduced == 0)
            {
                continue;
            }

            total += reduced;
            for (var multiple = 2L * d; multiple <= maxDenominator; multiple += d)
            {
                counts[multiple] -= reduced;
            }
        }

        return total;
    }
}
