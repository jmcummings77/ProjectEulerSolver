using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The D ≤ 1000 whose minimal solution in x to x² − Dy² = 1 has the largest x.</summary>
public sealed class Problem066 : Problem
{
    public override int Number => 66;

    public override string Title => "Diophantine Equation";

    public override object Solve()
    {
        var bestD = 0;
        BigInteger bestX = 0;
        for (var d = 2; d <= 1000; d++)
        {
            if (NumberTheory.IsSquare(d))
            {
                continue;
            }

            // The fundamental solution of Pell's equation is the first convergent h/k of √D with h² − Dk² = 1.
            var (x, _) = ContinuedFractions.Convergents(ContinuedFractions.SqrtTerms(d))
                .First(c => c.Numerator * c.Numerator - d * c.Denominator * c.Denominator == 1);
            if (x > bestX)
            {
                bestX = x;
                bestD = d;
            }
        }

        return bestD;
    }
}
