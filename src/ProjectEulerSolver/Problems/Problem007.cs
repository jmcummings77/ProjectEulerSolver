using System.Diagnostics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The 10001st prime number.</summary>
public sealed class Problem007 : Problem
{
    // 2^31 − 1 is the 105,097,565th prime, so this is the last index whose prime fits in an int.
    private const int LargestIndex = 105_097_565;

    private const int BlockLength = 1 << 16;

    public override int Number => 7;

    public override string Title => "10001st Prime";

    public override object Solve() => Solve(n: 10_001);

    /// <summary>The <paramref name="n"/>-th prime number, counting 2 as the first.</summary>
    /// <param name="n">
    /// Position in the sequence of primes, from 1 to 105,097,565 (the position of 2^31 − 1, the largest prime an int
    /// can hold). The largest values sieve about two billion numbers and take several seconds.
    /// </param>
    public static int Solve(int n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, LargestIndex);

        // Rosser and Schoenfeld (1962): p_n < n (ln n + ln ln n) for n ≥ 6, and the first five primes end at 11.
        // Rounding up absorbs the floating-point error, and the cap on n means int.MaxValue is a bound as well.
        var estimate = n < 6 ? 11 : Math.Ceiling(n * (Math.Log(n) + Math.Log(Math.Log(n))));
        var bound = (long)Math.Min(estimate, int.MaxValue);

        // A segmented sieve of Eratosthenes over 2..bound. Every composite in that range has a prime factor of at
        // most sqrt(bound), so those primes cross off one block at a time and memory does not grow with n.
        var basePrimes = Primes.UpTo((int)NumberTheory.ISqrt(bound));
        var composite = new bool[BlockLength];
        var remaining = n;
        for (long low = 2; low <= bound; low += BlockLength)
        {
            var high = Math.Min(low + BlockLength, bound + 1); // Exclusive.
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
                if (!composite[value - low] && --remaining == 0)
                {
                    return (int)value;
                }
            }
        }

        throw new UnreachableException($"The bound {bound} holds fewer than {n} primes.");
    }
}
