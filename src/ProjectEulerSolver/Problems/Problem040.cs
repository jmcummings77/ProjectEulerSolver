using System.Text;

namespace ProjectEulerSolver.Problems;

/// <summary>Product of the digits d1 × d10 × ... × d1000000 of Champernowne's constant.</summary>
public sealed class Problem040 : Problem
{
    public override int Number => 40;

    public override string Title => "Champernowne's Constant";

    public override object Solve()
    {
        const int longestIndex = 1_000_000;
        var fraction = new StringBuilder(longestIndex + 8);
        for (var n = 1; fraction.Length < longestIndex; n++)
        {
            fraction.Append(n);
        }

        var product = 1;
        for (var index = 1; index <= longestIndex; index *= 10)
        {
            product *= fraction[index - 1] - '0';
        }

        return product;
    }
}
