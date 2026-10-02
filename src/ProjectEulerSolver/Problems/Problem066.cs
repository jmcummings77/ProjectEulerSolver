using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The D ≤ 1000 whose minimal solution in x to x² − Dy² = 1 has the largest x.</summary>
public sealed class Problem066 : Problem
{
    public override int Number => 66;

    public override string Title => "Diophantine Equation";

    public override object Solve() => Solve(maxD: 1000);

    /// <summary>
    /// The non-square D from 2 to <paramref name="maxD"/> whose minimal solution in positive integers of
    /// x² − Dy² = 1 has the largest x. If several D share that largest x, the smallest of them is returned.
    /// </summary>
    /// <param name="maxD">
    /// Inclusive upper bound on D, from 2 (the first non-square) to 1,000,000. The arithmetic is exact at any
    /// size; the cap only bounds the running time, which grows roughly as maxD² because the period of √D and the
    /// length of its minimal solution both grow a little faster than √D (2,475 digits at D = 952,429).
    /// </param>
    public static int Solve(int maxD)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxD, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxD, 1_000_000);

        var bestD = 0;
        BigInteger bestX = 0;
        for (var d = 2; d <= maxD; d++)
        {
            var x = MinimalX(d);
            if (x > bestX)
            {
                bestX = x;
                bestD = d;
            }
        }

        return bestD;
    }

    /// <summary>The smallest positive x with x² − dy² = 1 for some positive y, or 0 when d is a perfect square.</summary>
    internal static BigInteger MinimalX(int d)
    {
        var (leading, period) = ContinuedFractions.SqrtExpansion(d);
        if (period.Length == 0)
        {
            return 0;
        }

        // With h(i)/k(i) the convergents of √d = [a0; a1, ..., ar repeating], h² − dk² = ±1 holds exactly at
        // i = jr − 1, with sign (−1)^(jr). So the smallest solution of the +1 equation is convergent r − 1 when
        // the period r is even and convergent 2r − 1 when it is odd.
        var lastIndex = (period.Length % 2 == 0 ? period.Length : 2 * period.Length) - 1;
        BigInteger previous = 1;
        BigInteger numerator = leading;
        for (var i = 1; i <= lastIndex; i++)
        {
            (previous, numerator) = (numerator, period[(i - 1) % period.Length] * numerator + previous);
        }

        return numerator;
    }
}
