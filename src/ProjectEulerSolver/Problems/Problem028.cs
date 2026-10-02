using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the numbers on the diagonals of a 1001×1001 number spiral.</summary>
public sealed class Problem028 : Problem
{
    public override int Number => 28;

    public override string Title => "Number Spiral Diagonals";

    public override object Solve() => Solve(size: 1001);

    /// <summary>
    /// The sum of the numbers on both diagonals of a <paramref name="size"/> × <paramref name="size"/> grid filled by
    /// starting with 1 in the centre and spiralling outwards clockwise.
    /// </summary>
    /// <param name="size">The side length of the grid: any positive odd number.</param>
    public static BigInteger Solve(int size)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size, 1);
        if (size % 2 == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "The spiral needs a centre cell, so its side length must be odd.");
        }

        // The ring of side s = 2k + 1 ends at s² and its corners are s - 1 apart, so they add up to
        // 4s² - 6(s - 1) = 16k² + 4k + 4. Summing that over k = 1..m and adding the centre gives the closed form.
        BigInteger m = size / 2;
        return 1 + 8 * m * (m + 1) * (2 * m + 1) / 3 + 2 * m * (m + 1) + 4 * m;
    }
}
