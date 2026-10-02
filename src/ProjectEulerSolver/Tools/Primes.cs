namespace ProjectEulerSolver.Tools;

/// <summary>Prime sieves, primality tests and factorisation.</summary>
public static class Primes
{
    // Deterministic Miller-Rabin witness sets (Jaeschke; Sorenson and Webster) and the bounds they cover.
    private static readonly ulong[] SmallBases = [2, 7, 61]; // n < 4,759,123,141
    private static readonly ulong[] MediumBases = [2, 3, 5, 7, 11, 13, 17]; // n < 341,550,071,728,321
    private static readonly ulong[] LargeBases = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37]; // all 64-bit n

    // Cheap trial divisors tried before Miller-Rabin; they reject most composites for a fraction of the cost.
    private static readonly long[] SmallOddPrimes = [5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97];

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

    /// <summary>The primes marked in a sieve table, in ascending order.</summary>
    public static int[] FromSieve(bool[] isPrime)
    {
        var primes = new List<int>();
        for (var i = 2; i < isPrime.Length; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return [.. primes];
    }

    /// <summary>All primes up to and including <paramref name="limit"/>, in ascending order.</summary>
    public static int[] UpTo(int limit) => FromSieve(Sieve(limit));

    /// <summary>Number of distinct primes dividing n, for every n ≤ limit, computed with a sieve.</summary>
    public static int[] DistinctPrimeFactorCounts(int limit)
    {
        var counts = new int[limit + 1];
        for (var p = 2; p <= limit; p++)
        {
            if (counts[p] != 0)
            {
                continue; // Already counted a smaller prime factor, so p is composite.
            }

            for (var multiple = p; multiple <= limit; multiple += p)
            {
                counts[multiple]++;
            }
        }

        return counts;
    }

    /// <summary>Deterministic primality test for any 64-bit integer (negatives, 0 and 1 are not prime).</summary>
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

        foreach (var p in SmallOddPrimes)
        {
            if (n % p == 0)
            {
                return false;
            }
        }

        return MillerRabin((ulong)n);
    }

    /// <summary>Prime factorisation as (prime, exponent) pairs in ascending prime order. Empty for n &lt; 2.</summary>
    public static IEnumerable<(long Prime, int Exponent)> Factor(long n)
    {
        if (n < 2)
        {
            yield break;
        }

        // Trial division by 2 and then odd numbers. Whenever the remaining cofactor is itself prime we stop
        // early instead of dividing all the way up to its square root; that check is only worth doing when
        // the cofactor has just changed.
        if (IsPrime(n))
        {
            yield return (n, 1);
            yield break;
        }

        for (long p = 2; p <= n / p; p += p == 2 ? 1 : 2)
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
            if (n > 1 && IsPrime(n))
            {
                yield return (n, 1);
                yield break;
            }
        }

        if (n > 1)
        {
            yield return (n, 1);
        }
    }

    /// <summary>Number of positive divisors of n (n ≥ 1), from its factorisation.</summary>
    public static int DivisorCount(long n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
        return Factor(n).Aggregate(1, (count, factor) => count * (factor.Exponent + 1));
    }

    private static bool MillerRabin(ulong n)
    {
        var bases = n < 4_759_123_141 ? SmallBases : n < 341_550_071_728_321 ? MediumBases : LargeBases;

        var d = n - 1;
        var r = 0;
        while ((d & 1) == 0)
        {
            d >>= 1;
            r++;
        }

        foreach (var a in bases)
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
