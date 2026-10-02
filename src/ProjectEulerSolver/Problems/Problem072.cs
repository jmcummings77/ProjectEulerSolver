using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many reduced proper fractions exist with d ≤ 1,000,000.</summary>
public sealed class Problem072 : Problem
{
    public override int Number => 72;

    public override string Title => "Counting Fractions";

    // Each denominator d contributes φ(d) reduced proper fractions, so the answer is Σ φ(d) for 2 ≤ d ≤ limit.
    public override object Solve() => NumberTheory.Totients(1_000_000).Skip(2).Sum(phi => (long)phi);
}
