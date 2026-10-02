using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many n-digit positive integers exist that are also an nth power.</summary>
public sealed class Problem063 : Problem
{
    public override int Number => 63;

    public override string Title => "Powerful Digit Counts";

    public override object Solve()
    {
        // 10^n always has n + 1 digits, so the base must be 1..9; and once b^n drops below n digits it stays below.
        var count = 0;
        for (var b = 1; b <= 9; b++)
        {
            for (var n = 1; Digits.Count(BigInteger.Pow(b, n)) == n; n++)
            {
                count++;
            }
        }

        return count;
    }
}
