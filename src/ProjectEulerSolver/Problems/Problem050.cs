using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The prime below one million that can be written as the sum of the most consecutive primes.</summary>
public sealed class Problem050 : Problem
{
    public override int Number => 50;

    public override string Title => "Consecutive Prime Sum";

    public override object Solve() => Solve(limit: 1_000_000);

    /// <summary>
    /// The prime below <paramref name="limit"/> that can be written as the sum of the most consecutive primes,
    /// where a prime on its own counts as a sum of one term. When several primes tie for the longest sum, the
    /// smallest of them is returned.
    /// </summary>
    /// <param name="limit">
    /// Exclusive upper bound, from 0 to 100,000,000 (the sieve keeps one flag for every number below it).
    /// There is no prime below 0, 1 or 2, so those have no answer.
    /// </param>
    /// <exception cref="InvalidOperationException">There is no prime below <paramref name="limit"/>.</exception>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 100_000_000);
        if (limit <= 2)
        {
            throw new InvalidOperationException($"There is no prime below {limit}.");
        }

        // Every term of a sum that is below the limit is itself below the limit, so one sieve over the numbers
        // below the limit supplies all the terms and answers every primality question about a sum.
        var isPrime = Primes.Sieve(limit - 1);
        var primes = Primes.FromSieve(isPrime);

        // prefix[i] = sum of the first i primes, so any run sum is a difference of two prefixes.
        var prefix = new long[primes.Length + 1];
        for (var i = 0; i < primes.Length; i++)
        {
            prefix[i + 1] = prefix[i] + primes[i];
        }

        // Only runs strictly longer than the best so far are tried. Run sums of one length grow with the start,
        // so the first run found at each length is the one with the smallest sum.
        var bestLength = 0;
        var bestPrime = 0;
        for (var start = 0; start < primes.Length; start++)
        {
            for (var end = start + bestLength + 1; end <= primes.Length; end++)
            {
                var sum = prefix[end] - prefix[start];
                if (sum >= limit)
                {
                    break;
                }

                if (isPrime[sum])
                {
                    bestLength = end - start;
                    bestPrime = (int)sum;
                }
            }
        }

        return bestPrime;
    }
}
