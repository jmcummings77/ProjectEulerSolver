using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>Product of the coefficients a, b (|a| &lt; 1000, |b| ≤ 1000) whose quadratic n² + an + b yields the most consecutive primes from n = 0.</summary>
public sealed class Problem027 : Problem
{
    public override int Number => 27;

    public override string Title => "Quadratic Primes";

    public override object Solve() => Solve(aLimit: 1000, bLimit: 1000);

    /// <summary>
    /// The product a·b of the coefficients with |a| &lt; <paramref name="aLimit"/> and |b| ≤ <paramref name="bLimit"/>
    /// whose quadratic n² + an + b yields the longest run of primes for consecutive n starting at n = 0.
    /// If several pairs share the longest run, the one with the smallest b, and then the smallest a, is used.
    /// </summary>
    /// <param name="aLimit">Exclusive bound on |a|, from 1 to 100,000.</param>
    /// <param name="bLimit">
    /// Inclusive bound on |b|, from 0 to 100,000. Below 2 no b is prime, so no quadratic yields a prime at n = 0
    /// and there is no answer.
    /// </param>
    /// <exception cref="InvalidOperationException">No quadratic in range yields a prime at n = 0.</exception>
    public static long Solve(int aLimit, int bLimit)
    {
        // The caps keep the run to about a second: the search below examines every pair of primes (b, 1 + a + b)
        // in range, some 1.3 × 10^8 pairs when both limits are at their largest.
        ArgumentOutOfRangeException.ThrowIfLessThan(aLimit, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(aLimit, 100_000);
        ArgumentOutOfRangeException.ThrowIfNegative(bLimit);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bLimit, 100_000);
        if (bLimit < 2)
        {
            throw new InvalidOperationException($"No quadratic with |b| ≤ {bLimit} yields a prime at n = 0, so none has a run of primes.");
        }

        // The values for n ≤ 4 are below 16 + 4·aLimit + bLimit. A sieve of that size settles nearly every candidate
        // by lookup, and the general primality test takes over for the larger values further along a run.
        var isSmallPrime = Primes.Sieve(16 + 4 * aLimit + bLimit);
        var smallPrimes = Primes.FromSieve(isSmallPrime);

        // n = 0 gives b and n = 1 gives 1 + a + b. The longest run has at least two primes, because n² + 2 is always
        // in range and starts 2, 3; so only pairs for which both of those values are prime can be the answer.
        // Choosing the two primes, b and p = 1 + a + b, visits exactly those pairs, and in tie-break order
        // (b ascending, then a ascending), so the first longest run seen is the answer.
        long bestProduct = 0;
        var bestRun = 0;
        foreach (var b in smallPrimes)
        {
            if (b > bLimit)
            {
                break;
            }

            foreach (var p in smallPrimes)
            {
                var a = p - 1 - b;
                if (a <= -aLimit)
                {
                    continue;
                }

                if (a >= aLimit)
                {
                    break;
                }

                // Every run ends by n = 2b: n = b gives b·(b + a + 1), prime only when a = -b, and then n = 2b
                // gives b·(2b + 1). So n and |a| stay within 200,000 and the value fits easily in a long.
                var n = 2;
                while (IsPrime((long)n * n + (long)a * n + b))
                {
                    n++;
                }

                if (n > bestRun)
                {
                    bestRun = n;
                    bestProduct = (long)a * b;
                }
            }
        }

        return bestProduct;

        bool IsPrime(long value) => value < isSmallPrime.Length ? value >= 0 && isSmallPrime[value] : Primes.IsPrime(value);
    }
}
