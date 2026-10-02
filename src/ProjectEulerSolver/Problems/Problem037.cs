using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the only eleven primes that are truncatable from both left and right.</summary>
public sealed class Problem037 : Problem
{
    public override int Number => 37;

    public override string Title => "Truncatable Primes";

    public override object Solve()
    {
        var isPrime = Primes.Sieve(1_000_000);
        long total = 0;
        var found = 0;
        for (var n = 11; found < 11; n += 2)
        {
            if (isPrime[n] && IsTruncatable(n, isPrime))
            {
                total += n;
                found++;
            }
        }

        return total;
    }

    private static bool IsTruncatable(int n, bool[] isPrime)
    {
        // Right to left: drop the last digit repeatedly.
        for (var m = n / 10; m > 0; m /= 10)
        {
            if (!isPrime[m])
            {
                return false;
            }
        }

        // Left to right: drop the leading digit repeatedly.
        for (var modulus = (int)Math.Pow(10, Digits.Count(n) - 1); modulus > 1; modulus /= 10)
        {
            if (!isPrime[n % modulus])
            {
                return false;
            }
        }

        return true;
    }
}
