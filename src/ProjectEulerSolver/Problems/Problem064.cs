using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many continued fractions of √N for N ≤ 10000 have an odd period.</summary>
public sealed class Problem064 : Problem
{
    public override int Number => 64;

    public override string Title => "Odd Period Square Roots";

    public override object Solve() =>
        Enumerable.Range(2, 9999).Count(n => ContinuedFractions.SqrtPeriodLength(n) % 2 == 1);
}
