using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum total from top to bottom of a 15-row triangle of numbers.</summary>
public sealed class Problem018 : Problem
{
    public override int Number => 18;

    public override string Title => "Maximum Path Sum I";

    public override object Solve() => NumberTriangle.MaximumPathSum(Resources.ReadLines("0018_triangle.txt"));
}
