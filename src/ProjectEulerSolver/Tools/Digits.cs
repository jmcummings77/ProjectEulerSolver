using System.Numerics;

namespace ProjectEulerSolver.Tools;

/// <summary>Helpers for treating integers as sequences of decimal digits.</summary>
public static class Digits
{
    /// <summary>Decimal digits of n, most significant first.</summary>
    public static int[] Of(long n) => Of(n.ToString());

    /// <summary>Decimal digits of n, most significant first.</summary>
    public static int[] Of(BigInteger n) => Of(BigInteger.Abs(n).ToString());

    /// <summary>Sum of the decimal digits of n.</summary>
    public static int Sum(BigInteger n) => Of(n).Sum();

    /// <summary>Sum of the decimal digits of n.</summary>
    public static int Sum(long n) => Of(n).Sum();

    /// <summary>Number of decimal digits in n.</summary>
    public static int Count(long n) => Math.Abs(n).ToString().Length;

    /// <summary>Number of decimal digits in n.</summary>
    public static int Count(BigInteger n) => BigInteger.Abs(n).ToString().Length;

    /// <summary>Builds an integer from digits given most significant first.</summary>
    public static long FromDigits(IEnumerable<int> digits) => digits.Aggregate(0L, (value, digit) => value * 10 + digit);

    /// <summary>n with its decimal digits reversed.</summary>
    public static long Reverse(long n)
    {
        long reversed = 0;
        while (n > 0)
        {
            reversed = reversed * 10 + n % 10;
            n /= 10;
        }

        return reversed;
    }

    /// <summary>n with its decimal digits reversed.</summary>
    public static BigInteger Reverse(BigInteger n)
    {
        var chars = n.ToString().ToCharArray();
        Array.Reverse(chars);
        return BigInteger.Parse(chars);
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

    /// <summary>True when n's decimal representation is a palindrome.</summary>
    public static bool IsPalindrome(long n) => n >= 0 && n == Reverse(n);

    /// <summary>True when n's decimal representation is a palindrome.</summary>
    public static bool IsPalindrome(BigInteger n) => IsPalindrome(n.ToString());

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
        var chars = n.ToString().ToCharArray();
        Array.Sort(chars);
        return new string(chars);
    }

    /// <summary>True when a and b contain exactly the same digits in some order.</summary>
    public static bool ArePermutations(long a, long b) => SortedKey(a) == SortedKey(b);

    private static int[] Of(string text)
    {
        var digits = new int[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            digits[i] = text[i] - '0';
        }

        return digits;
    }
}
