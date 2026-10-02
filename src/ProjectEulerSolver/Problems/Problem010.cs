using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all the primes below two million.</summary>
public sealed class Problem010 : Problem
{
    private const int BlockLength = 1 << 16;

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

        // A segmented sieve of Eratosthenes. Every composite below limit has a prime factor of at most
        // sqrt(limit), so those primes cross off one block at a time and memory does not grow with limit.
        // The sum is below 2^31 × 2^31 / 2, so it fits in a long.
        var basePrimes = Primes.UpTo((int)NumberTheory.ISqrt(limit));
        var composite = new bool[BlockLength];
        long sum = 0;
        for (long low = 2; low < limit; low += BlockLength)
        {
            var high = Math.Min(low + BlockLength, limit); // Exclusive.
            Array.Clear(composite);
            foreach (var prime in basePrimes)
            {
                var square = (long)prime * prime;
                if (square >= high)
                {
                    break;
                }

                for (var multiple = Math.Max(square, (low + prime - 1) / prime * prime); multiple < high; multiple += prime)
                {
                    composite[multiple - low] = true;
                }
            }

            for (var value = low; value < high; value++)
            {
                if (!composite[value - low])
                {
                    sum += value;
                }
            }
        }

        return sum;
    }
}
