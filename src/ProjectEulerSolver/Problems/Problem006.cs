using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>Difference between the square of the sum and the sum of the squares of the first 100 natural numbers.</summary>
public sealed class Problem006 : Problem
{
    public override int Number => 6;

    public override string Title => "Sum Square Difference";

    public override object Solve() => Solve(n: 100);

    /// <summary>
    /// Difference between the square of the sum and the sum of the squares of the first <paramref name="n"/> natural
    /// numbers: (1 + ... + n)² − (1² + ... + n²).
    /// </summary>
    /// <param name="n">How many natural numbers to take, from 0 to <see cref="int.MaxValue"/>.</param>
    public static BigInteger Solve(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);

        // Closed forms for both sums; the difference grows like n^4 / 4 and passes a long near n = 78,000.
        BigInteger count = n;
        var sum = count * (count + 1) / 2;
        var sumOfSquares = count * (count + 1) * (2 * count + 1) / 6;
        return sum * sum - sumOfSquares;
    }
}
