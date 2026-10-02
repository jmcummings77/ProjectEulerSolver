using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The n &lt; 10^7 for which φ(n) is a permutation of n and n/φ(n) is minimal.</summary>
public sealed class Problem070 : Problem
{
    private const int Limit = 10_000_000;

    public override int Number => 70;

    public override string Title => "Totient Permutation";

    public override object Solve()
    {
        // Sieve every totient below the limit and test each n exhaustively. Two cheap filters remove almost
        // all the candidates before the digit comparison: permutations share a digit sum, so n ≡ φ(n) (mod 9),
        // and only a ratio better than the best so far is worth checking at all.
        var phi = NumberTheory.Totients(Limit - 1);
        var bestN = 0;
        var bestPhi = 1;
        for (var n = 2; n < Limit; n++)
        {
            var p = phi[n];
            if ((n - p) % 9 != 0)
            {
                continue;
            }

            // n/p < bestN/bestPhi, cross-multiplied to stay in integers.
            if (bestN != 0 && (long)n * bestPhi >= (long)bestN * p)
            {
                continue;
            }

            if (Digits.ArePermutations(n, p))
            {
                bestN = n;
                bestPhi = p;
            }
        }

        return bestN;
    }
}
