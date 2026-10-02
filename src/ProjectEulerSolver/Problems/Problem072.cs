using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many reduced proper fractions exist with d ≤ 1,000,000.</summary>
public sealed class Problem072 : Problem
{
    private const int MaxSupportedDenominator = 100_000_000;

    public override int Number => 72;

    public override string Title => "Counting Fractions";

    public override object Solve() => Solve(maxDenominator: 1_000_000);

    /// <summary>How many reduced proper fractions have a denominator of at most <paramref name="maxDenominator"/>.</summary>
    /// <param name="maxDenominator">The largest denominator counted: from 1 to 100,000,000 (the totient sieve takes four bytes per denominator).</param>
    public static long Solve(int maxDenominator)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxDenominator, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxDenominator, MaxSupportedDenominator);

        // Each denominator d contributes φ(d) reduced proper fractions, so the answer is Σ φ(d) for
        // 2 ≤ d ≤ maxDenominator. Every φ(d) is below d, so the sum stays below 10^16 and fits in a long.
        return NumberTheory.Totients(maxDenominator).Skip(2).Sum(phi => (long)phi);
    }
}
