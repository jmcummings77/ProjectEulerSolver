using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The perimeter p ≤ 1000 with the most integer-sided right triangle solutions.</summary>
public sealed class Problem039 : Problem
{
    public override int Number => 39;

    public override string Title => "Integer Right Triangles";

    public override object Solve() => Solve(maxPerimeter: 1000);

    /// <summary>
    /// The perimeter p ≤ <paramref name="maxPerimeter"/> shared by the greatest number of right triangles with
    /// integer sides; the smallest such p when several perimeters tie.
    /// </summary>
    /// <param name="maxPerimeter">
    /// The largest perimeter considered, from 12 (the 3-4-5 triangle, the smallest there is) to 10,000,000.
    /// </param>
    public static int Solve(int maxPerimeter)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxPerimeter, 12);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxPerimeter, 10_000_000); // The tally holds an int per perimeter.

        var counts = TriangleCounts(maxPerimeter);
        var best = 12;
        for (var p = 13; p <= maxPerimeter; p++)
        {
            if (counts[p] > counts[best])
            {
                best = p;
            }
        }

        return best;
    }

    /// <summary>
    /// For every perimeter p ≤ <paramref name="maxPerimeter"/> (at most 10,000,000), the number of right
    /// triangles with integer sides and that perimeter.
    /// </summary>
    internal static int[] TriangleCounts(int maxPerimeter)
    {
        // Euclid's formula: each primitive triple is (m² - n², 2mn, m² + n²) for exactly one pair m > n ≥ 1
        // that is coprime and of opposite parity, and its perimeter is 2m(m + n). Every right triangle is
        // exactly one multiple of exactly one primitive triple, so tallying the multiples counts each once.
        // The smallest perimeter for a given m is 2m(m + 1), which bounds m; every perimeter formed is below
        // 4m² < 2 * maxPerimeter, so nothing overflows.
        var counts = new int[maxPerimeter + 1];
        for (var m = 2; 2 * m * (m + 1) <= maxPerimeter; m++)
        {
            for (var n = m % 2 == 0 ? 1 : 2; n < m; n += 2)
            {
                var primitive = 2 * m * (m + n);
                if (primitive > maxPerimeter)
                {
                    break;
                }

                if (NumberTheory.Gcd(m, n) != 1)
                {
                    continue;
                }

                for (var p = primitive; p <= maxPerimeter; p += primitive)
                {
                    counts[p]++;
                }
            }
        }

        return counts;
    }
}
