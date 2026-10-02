namespace ProjectEulerSolver.Problems;

/// <summary>How many fractions lie between 1/3 and 1/2 in the sorted set of reduced proper fractions for d ≤ 12,000.</summary>
public sealed class Problem073 : Problem
{
    private const int Limit = 12_000;

    public override int Number => 73;

    public override string Title => "Counting Fractions in a Range";

    public override object Solve()
    {
        // counts[d] starts as the number of all fractions n/d with 1/3 < n/d < 1/2, reduced or not. Every
        // unreduced one is a reduced fraction with a denominator that properly divides d, so subtracting each
        // denominator's reduced count from its multiples (in ascending order) leaves only the reduced fractions.
        var counts = new long[Limit + 1];
        for (var d = 1; d <= Limit; d++)
        {
            counts[d] = (d - 1) / 2 - d / 3;
        }

        for (var d = 1; d <= Limit; d++)
        {
            for (var multiple = 2 * d; multiple <= Limit; multiple += d)
            {
                counts[multiple] -= counts[d];
            }
        }

        return counts.Sum();
    }
}
