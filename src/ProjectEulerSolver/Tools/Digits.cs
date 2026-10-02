using System.Numerics;

namespace ProjectEulerSolver.Tools;

/// <summary>
/// Helpers for treating integers as sequences of decimal digits. Every member works on the absolute value
/// of its argument: a minus sign is never a digit. <see cref="Reverse(long)"/> keeps the sign of its input.
/// </summary>
public static class Digits
{
    /// <summary>Decimal digits of |n|, most significant first.</summary>
    public static int[] Of(long n)
    {
        var magnitude = Magnitude(n);
        var digits = new int[Count(magnitude)];
        for (var i = digits.Length - 1; i >= 0; i--)
        {
            digits[i] = (int)(magnitude % 10);
            magnitude /= 10;
        }

        return digits;
    }

    /// <summary>Decimal digits of |n|, most significant first.</summary>
    public static int[] Of(BigInteger n)
    {
        var text = BigInteger.Abs(n).ToString();
        var digits = new int[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            digits[i] = text[i] - '0';
        }

        return digits;
    }

    /// <summary>Sum of the decimal digits of |n|.</summary>
    public static int Sum(BigInteger n) => Of(n).Sum();

    /// <summary>Sum of the decimal digits of |n|.</summary>
    public static int Sum(long n)
    {
        var sum = 0;
        for (var magnitude = Magnitude(n); magnitude > 0; magnitude /= 10)
        {
            sum += (int)(magnitude % 10);
        }

        return sum;
    }

    /// <summary>Number of decimal digits in |n| (1 for zero).</summary>
    public static int Count(long n) => Count(Magnitude(n));

    /// <summary>Number of decimal digits in |n| (1 for zero).</summary>
    public static int Count(BigInteger n) => BigInteger.Abs(n).ToString().Length;

    /// <summary>Builds an integer from digits given most significant first.</summary>
    public static long FromDigits(IEnumerable<int> digits) => digits.Aggregate(0L, (value, digit) => value * 10 + digit);

    /// <summary>n with the decimal digits of its magnitude reversed; the sign is preserved.</summary>
    public static long Reverse(long n)
    {
        long reversed = 0;
        for (var magnitude = Magnitude(n); magnitude > 0; magnitude /= 10)
        {
            reversed = reversed * 10 + (long)(magnitude % 10);
        }

        return n < 0 ? -reversed : reversed;
    }

    /// <summary>n with the decimal digits of its magnitude reversed; the sign is preserved.</summary>
    public static BigInteger Reverse(BigInteger n)
    {
        var chars = BigInteger.Abs(n).ToString().ToCharArray();
        Array.Reverse(chars);
        var reversed = BigInteger.Parse(chars);
        return n.Sign < 0 ? -reversed : reversed;
    }

    /// <summary>True when the string reads the same forwards and backwards.</summary>
    public static bool IsPalindrome(string s)
    {
        for (int i = 0, j = s.Length - 1; i < j; i++, j--)
        {
            if (s[i] != s[j])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>True when n is non-negative and its decimal representation is a palindrome.</summary>
    public static bool IsPalindrome(long n) => n >= 0 && n == Reverse(n);

    /// <summary>True when n is non-negative and its decimal representation is a palindrome.</summary>
    public static bool IsPalindrome(BigInteger n) => n.Sign >= 0 && IsPalindrome(n.ToString());

    /// <summary>True when n uses each digit 1..<paramref name="length"/> exactly once.</summary>
    public static bool IsPandigital(long n, int length)
    {
        var seen = 0;
        var count = 0;
        while (n > 0)
        {
            var digit = (int)(n % 10);
            if (digit == 0 || digit > length || (seen & (1 << digit)) != 0)
            {
                return false;
            }

            seen |= 1 << digit;
            count++;
            n /= 10;
        }

        return count == length;
    }

    /// <summary>A canonical key shared by all numbers with the same multiset of digits.</summary>
    public static string SortedKey(BigInteger n)
    {
        var chars = BigInteger.Abs(n).ToString().ToCharArray();
        Array.Sort(chars);
        return new string(chars);
    }

    /// <summary>True when |a| and |b| contain exactly the same digits in some order.</summary>
    public static bool ArePermutations(long a, long b) => SortedKey(a) == SortedKey(b);

    private static ulong Magnitude(long n) => n < 0 ? (ulong)(-(n + 1)) + 1 : (ulong)n;

    private static int Count(ulong magnitude)
    {
        var count = 1;
        while (magnitude >= 10)
        {
            magnitude /= 10;
            count++;
        }

        return count;
    }
}
