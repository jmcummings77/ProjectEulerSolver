using System.Numerics;

namespace ProjectEulerSolver.Tools;

/// <summary>Greatest common divisors, divisor sums, totients, factorials and friends.</summary>
public static class NumberTheory
{
    /// <summary>Greatest common divisor by Euclid's algorithm. Always non-negative.</summary>
    public static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return Math.Abs(a);
    }

    /// <summary>Least common multiple. Always non-negative, and zero when either argument is zero.</summary>
    public static long Lcm(long a, long b) => a == 0 || b == 0 ? 0 : Math.Abs(a / Gcd(a, b) * b);

    /// <summary>10 raised to <paramref name="exponent"/>, for exponents 0 to 18 (the largest power of ten in a long).</summary>
    public static long PowerOfTen(int exponent)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(exponent);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(exponent, 18);

        long power = 1;
        for (var i = 0; i < exponent; i++)
        {
            power *= 10;
        }

        return power;
    }

    /// <summary>Integer square root: the largest s with s*s ≤ n. Valid for every non-negative long.</summary>
    public static long ISqrt(long n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);

        // Math.Sqrt is within one of the answer for 64-bit input; correct it without ever forming a square
        // that could overflow (s*s ≤ n is the same as s ≤ n/s for positive s).
        var s = (long)Math.Sqrt(n);
        while (s > 0 && s > n / s)
        {
            s--;
        }

        while (s + 1 <= n / (s + 1))
        {
            s++;
        }

        return s;
    }

    /// <summary>True when n is a perfect square.</summary>
    public static bool IsSquare(long n)
    {
        if (n < 0)
        {
            return false;
        }

        var s = ISqrt(n);
        return s * s == n;
    }

    /// <summary>
    /// Sum of the proper divisors of n (divisors excluding n itself). The sum itself can exceed
    /// <see cref="long.MaxValue"/> for highly composite n above roughly 10^18.
    /// </summary>
    public static long ProperDivisorSum(long n)
    {
        if (n < 2)
        {
            return 0;
        }

        long sum = 1;
        for (long d = 2; d <= n / d; d++)
        {
            if (n % d != 0)
            {
                continue;
            }

            sum += d;
            var paired = n / d;
            if (paired != d)
            {
                sum += paired;
            }
        }

        return sum;
    }

    /// <summary>Proper divisor sums for every n ≤ limit, computed with a sieve.</summary>
    public static int[] ProperDivisorSums(int limit)
    {
        var sums = new int[limit + 1];
        for (var d = 1; d <= limit / 2; d++)
        {
            for (var multiple = 2 * d; multiple <= limit; multiple += d)
            {
                sums[multiple] += d;
            }
        }

        return sums;
    }

    /// <summary>Euler's totient φ(n) for every n ≤ limit, computed with a sieve.</summary>
    public static int[] Totients(int limit)
    {
        var phi = new int[limit + 1];
        for (var i = 0; i <= limit; i++)
        {
            phi[i] = i;
        }

        for (var p = 2; p <= limit; p++)
        {
            if (phi[p] != p)
            {
                continue; // p has already been reduced, so it is composite.
            }

            for (var multiple = p; multiple <= limit; multiple += p)
            {
                phi[multiple] -= phi[multiple] / p;
            }
        }

        return phi;
    }

    /// <summary>n! as an arbitrary-precision integer.</summary>
    public static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (var i = 2; i <= n; i++)
        {
            result *= i;
        }

        return result;
    }

    /// <summary>The binomial coefficient C(n, k).</summary>
    public static BigInteger Binomial(int n, int k)
    {
        if (k < 0 || k > n)
        {
            return 0;
        }

        k = Math.Min(k, n - k);
        BigInteger result = 1;
        for (var i = 1; i <= k; i++)
        {
            result = result * (n - k + i) / i;
        }

        return result;
    }
}
