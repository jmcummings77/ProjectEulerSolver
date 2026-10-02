using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>In the first one thousand expansions of √2, how many have a numerator with more digits than the denominator.</summary>
public sealed class Problem057 : Problem
{
    public override int Number => 57;

    public override string Title => "Square Root Convergents";

    public override object Solve()
    {
        // √2 = [1; 2, 2, 2, ...]. The first expansion 3/2 is the convergent after the leading 1.
        var terms = Enumerable.Repeat(2, 1000).Prepend(1);
        return ContinuedFractions.Convergents(terms)
            .Skip(1)
            .Count(c => Digits.Count(c.Numerator) > Digits.Count(c.Denominator));
    }
}
