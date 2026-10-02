namespace ProjectEulerSolver.Problems;

/// <summary>The first triangle number with over five hundred divisors.</summary>
public sealed class Problem012 : Problem
{
    private const int MaxDivisors = 10_000;

    public override int Number => 12;

    public override string Title => "Highly Divisible Triangular Number";

    public override object Solve() => Solve(divisors: 500);

    /// <summary>The first triangle number with more than <paramref name="divisors"/> divisors.</summary>
    /// <param name="divisors">The divisor count to exceed, from 0 to 10,000.</param>
    public static long Solve(int divisors)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(divisors);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(divisors, MaxDivisors);

        // T(n) = n(n+1)/2 and n, n+1 are coprime, so d(T(n)) is the product of the divisor
        // counts of the two halves (after taking the factor 2 out of the even one).
        //
        // How far the search runs is not known in advance, so the table of divisor counts doubles whenever n
        // reaches its end. The search is over by n = 14,753,024 at the latest, because
        // T(14,753,024) = 2^7 · 3^2 · 5^2 · 7 · 11 · 13^2 · 17 · 19 · 29 · 31 has
        // 8 · 3 · 3 · 2 · 2 · 3 · 2 · 2 · 2 · 2 = 13,824 divisors, more than any supported argument. Hence the
        // table stops growing once it reaches 2^24, the answer is at most 108,825,865,948,800, and the product of
        // two counts stays far inside an int (a number m has at most 2√m divisors, so at most 2^13 each here).
        long n = 1;
        for (var size = 1 << 14; ; size *= 2)
        {
            var counts = DivisorCounts(size);
            for (; n < size; n++)
            {
                var (a, b) = n % 2 == 0 ? (n / 2, n + 1) : (n, (n + 1) / 2);
                if (counts[a] * counts[b] > divisors)
                {
                    return a * b;
                }
            }
        }
    }

    /// <summary>The number of divisors of every m from 1 to <paramref name="limit"/>; index 0 is unused.</summary>
    private static int[] DivisorCounts(int limit)
    {
        var counts = new int[limit + 1];
        Array.Fill(counts, 1);
        for (var p = 2; p <= limit; p++)
        {
            if (counts[p] != 1)
            {
                continue; // Already scaled by a smaller prime, so p is composite.
            }

            // A prime dividing m exactly e times contributes a factor e + 1. Visiting the multiples of p, p², p³, …
            // in turn raises that factor one step at a time: it is k just before the multiples of p^k are visited.
            var k = 1;
            for (long power = p; power <= limit; power *= p, k++)
            {
                for (var multiple = power; multiple <= limit; multiple += power)
                {
                    counts[multiple] = counts[multiple] / k * (k + 1);
                }
            }
        }

        return counts;
    }
}
