using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum digital sum of a^b for a, b &lt; 100.</summary>
public sealed class Problem056 : Problem
{
    public override int Number => 56;

    public override string Title => "Powerful Digit Sum";

    public override object Solve()
    {
        var best = 0;
        for (var a = 1; a < 100; a++)
        {
            for (var b = 1; b < 100; b++)
            {
                best = Math.Max(best, Digits.Sum(BigInteger.Pow(a, b)));
            }
        }

        return best;
    }
}
