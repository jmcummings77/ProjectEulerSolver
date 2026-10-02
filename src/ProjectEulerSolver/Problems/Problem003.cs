using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest prime factor of 600851475143.</summary>
public sealed class Problem003 : Problem
{
    public override int Number => 3;

    public override string Title => "Largest Prime Factor";

    public override object Solve() => Primes.Factor(600_851_475_143).Last().Prime;
}
