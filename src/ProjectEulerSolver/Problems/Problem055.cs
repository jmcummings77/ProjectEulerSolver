namespace ProjectEulerSolver.Problems;

/// <summary>How many Lychrel numbers there are below ten thousand.</summary>
public sealed class Problem055 : Problem
{
    // The widest start value, int.MaxValue, has ten digits.
    private const int MaxStartDigits = 10;

    public override int Number => 55;

    public override string Title => "Lychrel Numbers";

    public override object Solve() => Solve(limit: 10_000, maxIterations: 50);

    /// <summary>
    /// How many positive integers below <paramref name="limit"/> do not produce a palindrome within
    /// <paramref name="maxIterations"/> steps of adding the number to its own digit reversal. A start value that
    /// is itself a palindrome still has to produce one by adding.
    /// </summary>
    /// <param name="limit">Exclusive upper bound on the start values: from 1 to 1,000,000.</param>
    /// <param name="maxIterations">
    /// How many reverse-and-add steps a number is given: from 0 (nothing has been added yet, so every number is
    /// counted) to 500. The work grows with limit × maxIterations².
    /// </param>
    public static int Solve(int limit, int maxIterations)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 1_000_000);
        ArgumentOutOfRangeException.ThrowIfNegative(maxIterations);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxIterations, 500);

        // A number and its reversal have the same d digits, so their sum is below 2 × 10^d: each step adds at
        // most one digit, and no value ever needs more than MaxStartDigits + maxIterations of them.
        var digits = new byte[MaxStartDigits + maxIterations];
        var scratch = new byte[MaxStartDigits + maxIterations];

        var count = 0;
        for (var start = 1; start < limit; start++)
        {
            if (IsLychrel(start, maxIterations, digits, scratch))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>Works on decimal digits stored least significant first; the two buffers swap roles after every step.</summary>
    private static bool IsLychrel(int start, int maxIterations, byte[] digits, byte[] scratch)
    {
        var length = 0;
        for (var remaining = start; remaining > 0; remaining /= 10)
        {
            digits[length++] = (byte)(remaining % 10);
        }

        for (var iteration = 0; iteration < maxIterations; iteration++)
        {
            var carry = 0;
            for (var i = 0; i < length; i++)
            {
                var sum = digits[i] + digits[length - 1 - i] + carry;
                scratch[i] = (byte)(sum % 10);
                carry = sum / 10;
            }

            if (carry > 0)
            {
                scratch[length++] = (byte)carry;
            }

            (digits, scratch) = (scratch, digits);
            if (IsPalindrome(digits, length))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsPalindrome(byte[] digits, int length)
    {
        for (int i = 0, j = length - 1; i < j; i++, j--)
        {
            if (digits[i] != digits[j])
            {
                return false;
            }
        }

        return true;
    }
}
