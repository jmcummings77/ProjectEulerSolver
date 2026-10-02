using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all the primes below two million.</summary>
public sealed class Problem010 : Problem
{

    public override int Number => 10;

    public override string Title => "Summation of Primes";

    public override object Solve() => Solve(limit: 2_000_000);

    /// <summary>The sum of all the primes below <paramref name="limit"/>.</summary>
    /// <param name="limit">
    /// Exclusive upper bound, from 0 to <see cref="int.MaxValue"/>. The largest values sieve about two billion
    /// numbers and take several seconds.
    /// </param>
    public static long Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        // The sum is below 2^31 × 2^31 / 2, so it fits in a long.
        long sum = 0;
        foreach (var prime in Primes.Enumerate(limit - 1))
        {
            sum += prime;
        }

        return sum;
    }
}
