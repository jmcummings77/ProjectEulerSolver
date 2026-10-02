using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>How many distinct terms are in the sequence a^b for 2 ≤ a ≤ 100 and 2 ≤ b ≤ 100.</summary>
public sealed class Problem029 : Problem
{
    public override int Number => 29;

    public override string Title => "Distinct Powers";

    public override object Solve()
    {
        var powers = new HashSet<BigInteger>();
        for (var a = 2; a <= 100; a++)
        {
            for (var b = 2; b <= 100; b++)
            {
                powers.Add(BigInteger.Pow(a, b));
            }
        }

        return powers.Count;
    }
}
