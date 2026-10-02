using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the digits in the numerator of the 100th convergent of the continued fraction for e.</summary>
public sealed class Problem065 : Problem
{
    public override int Number => 65;

    public override string Title => "Convergents of e";

    public override object Solve()
    {
        // e = [2; 1, 2, 1, 1, 4, 1, 1, 6, 1, ...]: every third term after the leading 2 is 2k.
        var terms = Enumerable.Range(1, 99).Select(i => i % 3 == 2 ? 2 * (i + 1) / 3 : 1).Prepend(2);
        var hundredth = ContinuedFractions.Convergents(terms).Last();
        return Digits.Sum(hundredth.Numerator);
    }
}
