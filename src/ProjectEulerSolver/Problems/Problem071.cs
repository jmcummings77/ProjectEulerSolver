namespace ProjectEulerSolver.Problems;

/// <summary>The numerator of the reduced proper fraction immediately to the left of 3/7 for d ≤ 1,000,000.</summary>
public sealed class Problem071 : Problem
{
    public override int Number => 71;

    public override string Title => "Ordered Fractions";

    public override object Solve()
    {
        // For each denominator the best candidate is the largest n with n/d < 3/7, i.e. n = floor((3d - 1) / 7).
        long bestN = 0;
        long bestD = 1;
        for (long d = 2; d <= 1_000_000; d++)
        {
            var n = (3 * d - 1) / 7;
            if (n * bestD > bestN * d)
            {
                bestN = n;
                bestD = d;
            }
        }

        return bestN;
    }
}
