using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the only eleven primes that are truncatable from both left and right.</summary>
public sealed class Problem037 : Problem
{
    private static readonly int[] AppendableDigits = [1, 3, 7, 9];

    public override int Number => 37;

    public override string Title => "Truncatable Primes";

    public override object Solve() => Solve(limit: 1_000_000);

    /// <summary>
    /// The sum of the primes below <paramref name="limit"/> that stay prime as digits are removed one at a
    /// time from the left, and also as they are removed one at a time from the right. The single-digit primes
    /// 2, 3, 5 and 7 are not counted.
    /// </summary>
    /// <param name="limit">
    /// Exclusive upper bound, from 0 to 2,147,483,647 (any non-negative int). Only eleven such primes exist,
    /// the largest being 739,397, so every limit above that gives the same sum.
    /// </param>
    public static long Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        // A right-truncatable prime is a shorter right-truncatable prime with one digit appended, and a prime
        // of two or more digits ends in 1, 3, 7 or 9. Growing them from the single-digit primes therefore
        // reaches every one below the limit, with no sieve to size; each is then tested from the left.
        // None contains a zero, so dropping leading digits is the same as taking remainders.
        long total = 0;
        var pending = new Stack<long>([2, 3, 5, 7]);
        while (pending.Count > 0)
        {
            var prefix = pending.Pop();
            foreach (var digit in AppendableDigits)
            {
                var candidate = prefix * 10 + digit;
                if (candidate >= limit || !Primes.IsPrime(candidate))
                {
                    continue;
                }

                pending.Push(candidate);
                if (IsLeftTruncatable(candidate))
                {
                    total += candidate;
                }
            }
        }

        return total;
    }

    private static bool IsLeftTruncatable(long n)
    {
        for (long place = 10; place < n; place *= 10)
        {
            if (!Primes.IsPrime(n % place))
            {
                return false;
            }
        }

        return true;
    }
}
