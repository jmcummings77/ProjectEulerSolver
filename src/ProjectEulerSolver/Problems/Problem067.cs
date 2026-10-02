using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum total from top to bottom of a 100-row triangle of numbers.</summary>
public sealed class Problem067 : Problem
{
    public override int Number => 67;

    public override string Title => "Maximum Path Sum II";

    public override object Solve() => Problem018.MaximumPathSum(Resources.ReadLines("0067_triangle.txt"));
}
