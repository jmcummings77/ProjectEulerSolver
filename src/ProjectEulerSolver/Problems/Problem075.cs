using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>For how many wire lengths L ≤ 1,500,000 can exactly one integer-sided right triangle be formed.</summary>
public sealed class Problem075 : Problem
{
    private const int MaxSupportedLength = 100_000_000;

    public override int Number => 75;

    public override string Title => "Singular Integer Right Triangles";

    public override object Solve() => Solve(maxLength: 1_500_000);

    /// <summary>
    /// How many wire lengths of at most <paramref name="maxLength"/> can be bent into an integer-sided right
    /// triangle in exactly one way.
    /// </summary>
    /// <param name="maxLength">The longest wire considered: from 1 to 100,000,000 (the tally takes one byte per length).</param>
    public static int Solve(int maxLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLength, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxLength, MaxSupportedLength);

        // Euclid's formula: each primitive triple comes from exactly one pair of coprime m > n of opposite parity
        // and has perimeter 2m(m + n); every multiple of that perimeter is another (non-primitive) triangle.
        // The perimeter grows with n and is at least 2m(m + 1), which bounds both loops by maxLength.
        // Only "none", "one" and "more than one" matter, so each tally saturates at 2 and fits in a byte.
        var triangles = new byte[maxLength + 1];
        for (long m = 2; 2 * m * (m + 1) <= maxLength; m++)
        {
            for (var n = 1 + m % 2; n < m; n += 2)
            {
                var perimeter = 2 * m * (m + n);
                if (perimeter > maxLength)
                {
                    break;
                }

                if (NumberTheory.Gcd(m, n) != 1)
                {
                    continue;
                }

                for (var length = perimeter; length <= maxLength; length += perimeter)
                {
                    if (triangles[length] < 2)
                    {
                        triangles[length]++;
                    }
                }
            }
        }

        return triangles.Count(tally => tally == 1);
    }
}
