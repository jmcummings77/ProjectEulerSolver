using System.Diagnostics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest palindrome made from the product of two 3-digit numbers.</summary>
public sealed class Problem004 : Problem
{
    public override int Number => 4;

    public override string Title => "Largest Palindrome Product";

    public override object Solve() => Solve(digits: 3);

    /// <summary>
    /// The largest palindrome that is a product of two numbers with exactly <paramref name="digits"/> decimal digits each.
    /// </summary>
    /// <param name="digits">Number of digits in each factor, from 1 to 9 (so the product fits in a long).</param>
    public static long Solve(int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, 9); // Two 9-digit factors stay below 10^18; two 10-digit ones need not.

        var smallest = PowerOfTen(digits - 1);
        var largest = smallest * 10 - 1;

        // A product of two such factors has 2·digits or 2·digits − 1 digits. Palindromes of either length are
        // fixed by their leading `digits` digits, so counting those down visits the candidates from the largest
        // to the smallest, and the first one that splits into two factors of the right size is the answer.
        for (var length = 2 * digits; length >= 2 * digits - 1; length--)
        {
            var evenLength = length % 2 == 0;
            for (var leading = largest; leading >= smallest; leading--)
            {
                var palindrome = evenLength
                    ? leading * (smallest * 10) + Digits.Reverse(leading)
                    : leading * smallest + Digits.Reverse(leading / 10);

                // 10 ≡ −1 (mod 11), so the mirrored digits of an even-length palindrome cancel in pairs and it is
                // divisible by 11. One of its two factors therefore is too, and only those need to be tried.
                if (HasFactorPair(palindrome, smallest, largest, step: evenLength ? 11 : 1))
                {
                    return palindrome;
                }
            }
        }

        // (10^(d−1) + 1)² = 10…020…01 is a palindrome for d ≥ 2, and 9 = 3 × 3 covers d = 1.
        throw new UnreachableException("Every digit count has a palindromic product.");
    }

    /// <summary>
    /// True when <paramref name="product"/> = x·y with both factors in [smallest, largest] and x a multiple of <paramref name="step"/>.
    /// </summary>
    private static bool HasFactorPair(long product, long smallest, long largest, long step)
    {
        // The cofactor product / x lies in [smallest, largest] exactly when x lies in [product / largest, product / smallest].
        var from = Math.Max(smallest, (product + largest - 1) / largest);
        var to = Math.Min(largest, product / smallest);
        for (var factor = to - to % step; factor >= from; factor -= step)
        {
            if (product % factor == 0)
            {
                return true;
            }
        }

        return false;
    }

    private static long PowerOfTen(int exponent)
    {
        long power = 1;
        for (var i = 0; i < exponent; i++)
        {
            power *= 10;
        }

        return power;
    }
}
