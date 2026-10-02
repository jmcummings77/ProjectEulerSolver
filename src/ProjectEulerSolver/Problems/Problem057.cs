using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>In the first one thousand expansions of √2, how many have a numerator with more digits than the denominator.</summary>
public sealed class Problem057 : Problem
{
    public override int Number => 57;

    public override string Title => "Square Root Convergents";

    public override object Solve() => Solve(expansions: 1000);

    /// <summary>
    /// Among the first <paramref name="expansions"/> expansions of the continued fraction of √2 (3/2, 7/5, 17/12, ...),
    /// how many have a numerator with more digits than the denominator.
    /// </summary>
    /// <param name="expansions">How many expansions to examine: from 0 to 100,000 (the work grows with its square).</param>
    public static int Solve(int expansions)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(expansions);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(expansions, 100_000);

        // Each expansion n/d is followed by (n + 2d)/(d + n), starting from 1/1. The numerator is the larger of the
        // two, so it has more digits exactly when it reaches the smallest power of ten above the denominator. That
        // power is carried along, which avoids converting numbers of thousands of digits to text at every step.
        BigInteger numerator = 1;
        BigInteger denominator = 1;
        BigInteger power = 10;
        var count = 0;
        for (var i = 0; i < expansions; i++)
        {
            (numerator, denominator) = (numerator + 2 * denominator, numerator + denominator);
            while (denominator >= power)
            {
                power *= 10;
            }

            if (numerator >= power)
            {
                count++;
            }
        }

        return count;
    }
}
