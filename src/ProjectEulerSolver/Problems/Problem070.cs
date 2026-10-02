using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The n &lt; 10^7 for which φ(n) is a permutation of n and n/φ(n) is minimal.</summary>
public sealed class Problem070 : Problem
{
    private const long Limit = 10_000_000;

    public override int Number => 70;

    public override string Title => "Totient Permutation";

    public override object Solve()
    {
        // To make n/φ(n) small, n wants few, large prime factors. A prime itself never works
        // (φ(p) = p − 1 is not a permutation), so the next best shape is a product of two primes
        // near √10^7 ≈ 3162, for which φ(pq) = (p − 1)(q − 1) comes for free.
        var primes = Primes.UpTo(5000).Where(p => p > 2000).ToArray();

        long bestN = 0;
        var bestRatio = double.MaxValue;
        for (var i = 0; i < primes.Length; i++)
        {
            for (var j = i + 1; j < primes.Length; j++)
            {
                var n = (long)primes[i] * primes[j];
                if (n >= Limit)
                {
                    break;
                }

                var phi = (long)(primes[i] - 1) * (primes[j] - 1);
                var ratio = (double)n / phi;
                if (ratio < bestRatio && Digits.ArePermutations(n, phi))
                {
                    bestRatio = ratio;
                    bestN = n;
                }
            }
        }

        return bestN;
    }
}
