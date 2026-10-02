using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all the primes below two million.</summary>
public sealed class Problem010 : Problem
{
    public override int Number => 10;

    public override string Title => "Summation of Primes";

    public override object Solve() => Primes.UpTo(1_999_999).Sum(p => (long)p);
}
