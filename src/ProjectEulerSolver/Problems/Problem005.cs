using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest positive number evenly divisible by all of the numbers from 1 to 20.</summary>
public sealed class Problem005 : Problem
{
    public override int Number => 5;

    public override string Title => "Smallest Multiple";

    public override object Solve() =>
        Enumerable.Range(1, 20).Aggregate(1L, (lcm, n) => NumberTheory.Lcm(lcm, n));
}
