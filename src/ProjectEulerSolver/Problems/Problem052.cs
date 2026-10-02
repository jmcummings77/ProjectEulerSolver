using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest positive integer x such that 2x, 3x, 4x, 5x and 6x contain the same digits.</summary>
public sealed class Problem052 : Problem
{
    public override int Number => 52;

    public override string Title => "Permuted Multiples";

    public override object Solve() => Solve(maxMultiplier: 6);

    /// <summary>
    /// The smallest positive integer x such that 2x, 3x, ..., <paramref name="maxMultiplier"/> × x all contain
    /// exactly the same digits as x, in some order.
    /// </summary>
    /// <param name="maxMultiplier">The largest multiple that must be a rearrangement of x: from 2 to 6.</param>
    public static long Solve(int maxMultiplier)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxMultiplier, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxMultiplier, 6);

        // The largest multiple must have as many digits as x, so among the numbers from low = 10^k to 10^(k+1) - 1
        // only those up to (10^(k+1) - 1) / maxMultiplier can qualify. The digit count grows until an answer
        // appears (six digits suffice for every supported multiplier); the stop at 10^17 keeps 10 × low in a long.
        for (long low = 1; low <= 100_000_000_000_000_000; low *= 10)
        {
            var high = (low * 10 - 1) / maxMultiplier;
            for (var x = low; x <= high; x++)
            {
                if (AllMultiplesArePermutations(x, maxMultiplier))
                {
                    return x;
                }
            }
        }

        throw new InvalidOperationException($"No number below 10^18 has the same digits as its multiples up to {maxMultiplier}.");
    }

    private static bool AllMultiplesArePermutations(long x, int maxMultiplier)
    {
        for (var multiplier = 2; multiplier <= maxMultiplier; multiplier++)
        {
            if (!Digits.ArePermutations(x, multiplier * x))
            {
                return false;
            }
        }

        return true;
    }
}
