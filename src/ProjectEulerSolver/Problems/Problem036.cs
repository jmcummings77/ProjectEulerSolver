using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all numbers below one million that are palindromic in base 10 and base 2.</summary>
public sealed class Problem036 : Problem
{
    public override int Number => 36;

    public override string Title => "Double-base Palindromes";

    public override object Solve() => Solve(limit: 1_000_000, radix: 2);

    /// <summary>
    /// The sum of the positive integers below <paramref name="limit"/> that read the same in both directions
    /// when written in base 10 and when written in base <paramref name="radix"/>, without leading zeros.
    /// </summary>
    /// <param name="limit">Exclusive upper bound, from 0 to 2,147,483,647 (any non-negative int).</param>
    /// <param name="radix">The second base, from 2 to 9.</param>
    public static long Solve(int limit, int radix)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfLessThan(radix, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(radix, 9);

        // Build the decimal palindromes from their leading halves, in ascending order, rather than testing
        // every number: there are only about 2 * sqrt(limit) of them. The loop ends at the first palindrome
        // that reaches the limit; that one is below 10^10, and fewer than 200,000 ints are summed.
        long total = 0;
        for (var length = 1; ; length++)
        {
            long firstHalf = 1;
            for (var i = 0; i < (length - 1) / 2; i++)
            {
                firstHalf *= 10;
            }

            for (var half = firstHalf; half < firstHalf * 10; half++)
            {
                var palindrome = Mirror(half, shareMiddleDigit: length % 2 == 1);
                if (palindrome >= limit)
                {
                    return total;
                }

                if (Digits.IsPalindrome(palindrome, radix))
                {
                    total += palindrome;
                }
            }
        }
    }

    /// <summary>
    /// The decimal palindrome that starts with the digits of <paramref name="half"/>: of odd length when the
    /// last of those digits is the shared middle one, of even length otherwise.
    /// </summary>
    private static long Mirror(long half, bool shareMiddleDigit)
    {
        var palindrome = half;
        for (var rest = shareMiddleDigit ? half / 10 : half; rest > 0; rest /= 10)
        {
            palindrome = palindrome * 10 + rest % 10;
        }

        return palindrome;
    }
}
