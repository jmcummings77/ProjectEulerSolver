using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many fractions lie between 1/3 and 1/2 in the sorted set of reduced proper fractions for d ≤ 12,000.</summary>
public sealed class Problem073 : Problem
{
    public override int Number => 73;

    public override string Title => "Counting Fractions in a Range";

    public override object Solve()
    {
        var count = 0;
        for (var d = 2; d <= 12_000; d++)
        {
            for (var n = d / 3 + 1; 2 * n < d; n++)
            {
                if (NumberTheory.Gcd(n, d) == 1)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
