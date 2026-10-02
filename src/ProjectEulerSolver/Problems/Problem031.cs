using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many different ways £2 can be made using any number of the eight British coins.</summary>
public sealed class Problem031 : Problem
{
    private static readonly int[] Coins = [1, 2, 5, 10, 20, 50, 100, 200];

    public override int Number => 31;

    public override string Title => "Coin Sums";

    public override object Solve() => Combinatorics.CountPartitions(200, Coins);
}
