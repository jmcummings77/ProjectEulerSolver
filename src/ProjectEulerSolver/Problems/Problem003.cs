using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest prime factor of 600851475143.</summary>
public sealed class Problem003 : Problem
{
    public override int Number => 3;

    public override string Title => "Largest Prime Factor";

    public override object Solve() => Solve(n: 600_851_475_143);

    /// <summary>The largest prime factor of <paramref name="n"/>.</summary>
    /// <param name="n">
    /// The number to factor, from 2 to <see cref="long.MaxValue"/>. Factoring is by trial division, so the slowest
    /// inputs are products of two primes near 3 × 10^9, which take a few seconds.
    /// </param>
    public static long Solve(long n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 2);

        return Primes.Factor(n).Last().Prime;
    }
}
