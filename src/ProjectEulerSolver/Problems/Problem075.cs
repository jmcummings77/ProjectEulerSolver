using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>For how many wire lengths L ≤ 1,500,000 can exactly one integer-sided right triangle be formed.</summary>
public sealed class Problem075 : Problem
{
    private const int Limit = 1_500_000;

    public override int Number => 75;

    public override string Title => "Singular Integer Right Triangles";

    public override object Solve()
    {
        // Euclid's formula: for coprime m > n of opposite parity, the primitive triple has perimeter 2m(m + n),
        // and every multiple of that perimeter is another (non-primitive) triangle.
        var triangles = new int[Limit + 1];
        for (long m = 2; 2 * m * m < Limit; m++)
        {
            for (var n = 1 + m % 2; n < m; n += 2)
            {
                if (NumberTheory.Gcd(m, n) != 1)
                {
                    continue;
                }

                var perimeter = 2 * m * (m + n);
                for (var length = perimeter; length <= Limit; length += perimeter)
                {
                    triangles[length]++;
                }
            }
        }

        return triangles.Count(count => count == 1);
    }
}
