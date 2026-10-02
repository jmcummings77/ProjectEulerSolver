using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The n &lt; 10^7 for which φ(n) is a permutation of n and n/φ(n) is minimal.</summary>
public sealed class Problem070 : Problem
{
    public override int Number => 70;

    public override string Title => "Totient Permutation";

    public override object Solve() => Solve(limit: 10_000_000);

    /// <summary>
    /// The n with 1 &lt; n &lt; <paramref name="limit"/> for which φ(n) is a permutation of the digits of n and
    /// n/φ(n) is a minimum. If several n share the minimal ratio, the smallest is returned.
    /// </summary>
    /// <param name="limit">
    /// Exclusive upper bound on n, from 0 to 100,000,000. A table of every totient below the limit is built, which
    /// takes four bytes per number.
    /// </param>
    /// <exception cref="InvalidOperationException">No n below the limit has a totient that is a permutation of its digits.</exception>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 100_000_000);

        // Sieve every totient below the limit and test each n exhaustively. Two cheap filters remove almost
        // all the candidates before the digit comparison: permutations share a digit sum, so n ≡ φ(n) (mod 9),
        // and only a ratio better than the best so far is worth checking at all.
        var phi = NumberTheory.Totients(Math.Max(limit - 1, 0));
        var bestN = 0;
        var bestPhi = 1;
        for (var n = 2; n < limit; n++)
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

        return bestN != 0
            ? bestN
            : throw new InvalidOperationException($"No n with 1 < n < {limit} has a totient that is a permutation of its digits.");
    }
}
