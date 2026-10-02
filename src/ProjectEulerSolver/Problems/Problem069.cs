namespace ProjectEulerSolver.Problems;

/// <summary>The n ≤ 1,000,000 for which n/φ(n) is a maximum.</summary>
public sealed class Problem069 : Problem
{
    // 2 × 3 × ... × 47 fits in a long and the same product continued to 53 does not, so no limit needs a prime past 53.
    private static readonly int[] SmallPrimes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53];

    public override int Number => 69;

    public override string Title => "Totient Maximum";

    public override object Solve() => Solve(limit: 1_000_000);

    /// <summary>
    /// The n from 1 to <paramref name="limit"/> for which n/φ(n) is a maximum. Every multiple of the answer that
    /// has no other prime factor shares the same ratio; the smallest such n is returned.
    /// </summary>
    /// <param name="limit">Inclusive upper bound on n: any long from 1 upwards.</param>
    public static long Solve(long limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);

        // n/φ(n) = Π p/(p − 1) over the distinct primes p of n, and p/(p − 1) falls as p grows. A number with k
        // distinct primes is at least the product of the first k primes and has at most that product's ratio, with
        // equality only when its primes are the first k. So the maximum belongs to the longest such product within
        // the limit, and every other n attaining it is a larger multiple of that product.
        long product = 1;
        foreach (var p in SmallPrimes)
        {
            // product × p > limit, written so the multiplication cannot overflow.
            if (product > limit / p)
            {
                break;
            }

            product *= p;
        }

        return product;
    }
}
