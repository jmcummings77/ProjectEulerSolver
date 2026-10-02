namespace ProjectEulerSolver.Tools;

/// <summary>Prime sieves, primality tests and factorisation.</summary>
public static class Primes
{
    private static readonly ulong[] MillerRabinBases = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37];

    /// <summary>Sieve of Eratosthenes. Returns a table where <c>table[n]</c> is true when n is prime, for 0 ≤ n ≤ limit.</summary>
    public static bool[] Sieve(int limit)
    {
        var isPrime = new bool[limit + 1];
        if (limit >= 2)
        {
            Array.Fill(isPrime, true, 2, limit - 1);
        }

        for (var i = 2; (long)i * i <= limit; i++)
        {
            if (!isPrime[i])
            {
                continue;
            }

            for (var j = i * i; j <= limit; j += i)
            {
                isPrime[j] = false;
            }
        }

        return isPrime;
    }

    /// <summary>All primes up to and including <paramref name="limit"/>, in ascending order.</summary>
    public static int[] UpTo(int limit)
    {
        var sieve = Sieve(limit);
        var primes = new List<int>();
        for (var i = 2; i <= limit; i++)
        {
            if (sieve[i])
            {
                primes.Add(i);
            }
        }

        return [.. primes];
    }

    /// <summary>Deterministic primality test for any non-negative 64-bit integer.</summary>
    public static bool IsPrime(long n)
    {
        if (n < 2)
        {
            return false;
        }

        if (n < 4)
        {
            return true;
        }

        if (n % 2 == 0 || n % 3 == 0)
        {
            return false;
        }

        if (n < 1_000_000)
        {
            for (long i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        return MillerRabin((ulong)n);
    }

    /// <summary>Prime factorisation as (prime, exponent) pairs in ascending prime order.</summary>
    public static IEnumerable<(long Prime, int Exponent)> Factor(long n)
    {
        if (n < 2)
        {
            yield break;
        }

        for (long p = 2; p * p <= n; p += p == 2 ? 1 : 2)
        {
            if (n % p != 0)
            {
                continue;
            }

            var exponent = 0;
            while (n % p == 0)
            {
                n /= p;
                exponent++;
            }

            yield return (p, exponent);
        }

        if (n > 1)
        {
            yield return (n, 1);
        }
    }

    /// <summary>Number of distinct primes dividing n.</summary>
    public static int DistinctPrimeFactorCount(long n) => Factor(n).Count();

    /// <summary>Number of positive divisors of n, from its factorisation.</summary>
    public static int DivisorCount(long n) =>
        n == 1 ? 1 : Factor(n).Aggregate(1, (count, factor) => count * (factor.Exponent + 1));

    private static bool MillerRabin(ulong n)
    {
        var d = n - 1;
        var r = 0;
        while ((d & 1) == 0)
        {
            d >>= 1;
            r++;
        }

        foreach (var a in MillerRabinBases)
        {
            if (a % n == 0)
            {
                continue;
            }

            var x = PowMod(a, d, n);
            if (x == 1 || x == n - 1)
            {
                continue;
            }

            var composite = true;
            for (var i = 1; i < r; i++)
            {
                x = MulMod(x, x, n);
                if (x == n - 1)
                {
                    composite = false;
                    break;
                }
            }

            if (composite)
            {
                return false;
            }
        }

        return true;
    }

    private static ulong MulMod(ulong a, ulong b, ulong m) => (ulong)((UInt128)a * b % m);

    private static ulong PowMod(ulong value, ulong exponent, ulong modulus)
    {
        ulong result = 1;
        value %= modulus;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                result = MulMod(result, value, modulus);
            }

            value = MulMod(value, value, modulus);
            exponent >>= 1;
        }

        return result;
    }
}
