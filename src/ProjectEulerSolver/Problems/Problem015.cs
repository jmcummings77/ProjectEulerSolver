using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>Number of monotone lattice paths through a 20×20 grid.</summary>
public sealed class Problem015 : Problem
{
    public override int Number => 15;

    public override string Title => "Lattice Paths";

    // Every path is a sequence of 20 rights and 20 downs, so choose which 20 of the 40 moves are rights.
    public override object Solve() => NumberTheory.Binomial(40, 20);
}
