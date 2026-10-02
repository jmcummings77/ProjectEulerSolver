using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many different ways one hundred can be written as a sum of at least two positive integers.</summary>
public sealed class Problem076 : Problem
{
    public override int Number => 76;

    public override string Title => "Counting Summations";

    // Partitions of 100 using parts 1..99 (excluding the single part 100 itself).
    public override object Solve() => Combinatorics.CountPartitions(100, Enumerable.Range(1, 99));
}
