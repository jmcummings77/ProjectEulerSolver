using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest 1-to-9 pandigital number formed as the concatenated product of an integer with (1, 2, ..., n), n &gt; 1.</summary>
public sealed class Problem038 : Problem
{
    public override int Number => 38;

    public override string Title => "Pandigital Multiples";

    public override object Solve() => Solve(digits: 9);

    /// <summary>
    /// The largest number that uses each of the digits 1 to <paramref name="digits"/> exactly once and is the
    /// concatenation of x × 1, x × 2, ..., x × n for some positive integer x and some n &gt; 1.
    /// </summary>
    /// <param name="digits">How many digits the number has, from 2 to 9.</param>
    public static long Solve(int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, 9);

        // With n ≥ 2 both x and 2x appear, and 2x is at least as long as x, so x has at most digits / 2
        // digits. There is always an answer: x = 1 with n = digits gives 12...digits.
        var limit = 1;
        for (var i = 0; i < digits / 2; i++)
        {
            limit *= 10;
        }

        long best = 0;
        for (var x = 1; x < limit; x++)
        {
            var candidate = ConcatenatedProduct(x, digits);

            // The pandigital test also rejects concatenations that overshoot the length.
            if (Digits.IsPandigital(candidate, digits))
            {
                best = Math.Max(best, candidate);
            }
        }

        return best;
    }

    /// <summary>
    /// The concatenation of x × 1, x × 2, ... stopped as soon as it has at least <paramref name="digits"/>
    /// digits (1 to 9), for 1 ≤ x &lt; 10^<paramref name="digits"/>.
    /// </summary>
    internal static long ConcatenatedProduct(int x, int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(x, 1); // Zero would never grow to the required length.

        long smallest = 1;
        for (var i = 1; i < digits; i++)
        {
            smallest *= 10;
        }

        // Every term adds a digit, so at most nine are used: before each append the value is below 10^8 and
        // the multiple below 9 * 10^9, which leaves room in a long.
        long concatenated = x;
        for (var n = 2; concatenated < smallest; n++)
        {
            long multiple = (long)x * n;
            for (var rest = multiple; rest > 0; rest /= 10)
            {
                concatenated *= 10;
            }

            concatenated += multiple;
        }

        return concatenated;
    }
}
