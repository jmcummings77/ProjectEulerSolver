using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The prime below one million that can be written as the sum of the most consecutive primes.</summary>
public sealed class Problem050 : Problem
{
    private const int Limit = 1_000_000;

    public override int Number => 50;

    public override string Title => "Consecutive Prime Sum";

    public override object Solve()
    {
        var isPrime = Primes.Sieve(Limit);
        var primes = Primes.UpTo(Limit);

        // prefix[i] = sum of the first i primes, so any run sum is a difference of two prefixes.
        var prefix = new long[primes.Length + 1];
        for (var i = 0; i < primes.Length; i++)
        {
            prefix[i + 1] = prefix[i] + primes[i];
        }

        var bestLength = 0;
        long bestPrime = 0;
        for (var start = 0; start < primes.Length; start++)
        {
            for (var end = start + bestLength + 1; end <= primes.Length; end++)
            {
                var sum = prefix[end] - prefix[start];
                if (sum >= Limit)
                {
                    break;
                }

                if (isPrime[sum])
                {
                    bestLength = end - start;
                    bestPrime = sum;
                }
            }
        }

        return bestPrime;
    }
}
