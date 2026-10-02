using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The next triangle number after 40755 that is also pentagonal and hexagonal.</summary>
public sealed class Problem045 : Problem
{
    public override int Number => 45;

    public override string Title => "Triangular, Pentagonal, and Hexagonal";

    public override object Solve() => Solve(after: 40755);

    /// <summary>The smallest number greater than <paramref name="after"/> that is triangular, pentagonal and hexagonal at once.</summary>
    /// <param name="after">Exclusive lower bound: zero or more, with no upper limit.</param>
    public static BigInteger Solve(BigInteger after)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(after);

        // Every hexagonal number is triangular (H(n) = T(2n − 1)), so the task is H(n) = n(2n − 1) = P(m) = m(3m − 1)/2.
        // Since 8·H(n) + 1 = (4n − 1)² and 24·P(m) + 1 = (6m − 1)², writing x = 6m − 1 and y = 4n − 1 turns it into
        // x² − 3y² = −2, and the common value is (y² − 1)/8.
        //
        // The positive solutions of that equation are exactly (1, 1), (5, 3), (19, 11), ... under the step
        // (x, y) → (2x + 3y, x + 2y). Its inverse (2x − 3y, 2y − x) sends any positive solution with y > 1 to a
        // positive solution with a smaller y (using x² = 3y² − 2: 4x² > 9y² and y < x < 2y), so descending from any
        // solution must end at y = 1, x = 1.
        //
        // A solution gives whole m and n when x ≡ 5 (mod 6) and y ≡ 3 (mod 4). The step is invertible modulo 12, so
        // the residues cycle and (5, 3)'s class comes round again for ever: the loop always ends. y grows at every
        // step, so the first value past the bound is the smallest.
        BigInteger x = 5;
        BigInteger y = 3;
        while (true)
        {
            if (x % 6 == 5 && y % 4 == 3)
            {
                var number = (y * y - 1) / 8;
                if (number > after)
                {
                    return number;
                }
            }

            (x, y) = (2 * x + 3 * y, x + 2 * y);
        }
    }
}
