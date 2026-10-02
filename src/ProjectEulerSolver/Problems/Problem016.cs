using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the digits of 2^1000.</summary>
public sealed class Problem016 : Problem
{
    public override int Number => 16;

    public override string Title => "Power Digit Sum";

    public override object Solve() => Digits.Sum(BigInteger.Pow(2, 1000));
}
