using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the digits in 100!.</summary>
public sealed class Problem020 : Problem
{
    public override int Number => 20;

    public override string Title => "Factorial Digit Sum";

    public override object Solve() => Digits.Sum(NumberTheory.Factorial(100));
}
