using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all the amicable numbers under 10000.</summary>
public sealed class Problem021 : Problem
{
    private const int Limit = 10_000;

    public override int Number => 21;

    public override string Title => "Amicable Numbers";

    public override object Solve()
    {
        var divisorSums = NumberTheory.ProperDivisorSums(Limit);
        var total = 0;
        for (var a = 2; a < Limit; a++)
        {
            var b = divisorSums[a];
            if (b != a && NumberTheory.ProperDivisorSum(b) == a)
            {
                total += a;
            }
        }

        return total;
    }
}
