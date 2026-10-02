using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The last ten digits of 1^1 + 2^2 + ... + 1000^1000.</summary>
public sealed class Problem048 : Problem
{
    public override int Number => 48;

    public override string Title => "Self Powers";

    public override object Solve()
    {
        var modulus = BigInteger.Pow(10, 10);
        BigInteger total = 0;
        for (var n = 1; n <= 1000; n++)
        {
            total = (total + BigInteger.ModPow(n, n, modulus)) % modulus;
        }

        return total.ToString().PadLeft(10, '0');
    }
}
