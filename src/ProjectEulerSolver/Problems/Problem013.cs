using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first ten digits of the sum of one hundred 50-digit numbers.</summary>
public sealed class Problem013 : Problem
{
    public override int Number => 13;

    public override string Title => "Large Sum";

    public override object Solve()
    {
        var total = Resources.ReadLines("0013_numbers.txt")
            .Aggregate(BigInteger.Zero, (sum, line) => sum + BigInteger.Parse(line));
        return total.ToString()[..10];
    }
}
