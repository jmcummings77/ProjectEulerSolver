using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest positive number evenly divisible by all of the numbers from 1 to 20.</summary>
public sealed class Problem005 : Problem
{
    public override int Number => 5;

    public override string Title => "Smallest Multiple";

    public override object Solve() => Solve(n: 20);

    /// <summary>The smallest positive number evenly divisible by every integer from 1 to <paramref name="n"/>.</summary>
    /// <param name="n">
    /// The largest divisor, from 0 (nothing to divide by, so the answer is 1) to 100,000. The answer grows like e^n,
    /// about 0.43 × n decimal digits, and the cap keeps computing and printing it to a fraction of a second.
    /// </param>
    public static BigInteger Solve(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, 100_000);

        // The least common multiple needs each prime exactly as often as the number in 1..n that uses it most,
        // which is the largest power of that prime not exceeding n.
        BigInteger multiple = 1;
        foreach (var prime in Primes.UpTo(n))
        {
            long power = prime;
            while (power <= n / prime)
            {
                power *= prime;
            }

            multiple *= power;
        }

        return multiple;
    }
}
