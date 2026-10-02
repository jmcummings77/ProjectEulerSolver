using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum total from top to bottom of a 15-row triangle of numbers.</summary>
public sealed class Problem018 : Problem
{
    public override int Number => 18;

    public override string Title => "Maximum Path Sum I";

    public override object Solve() => Solve(rows: Resources.ReadLines("0018_triangle.txt"));

    /// <summary>
    /// The largest total obtainable by starting at the top of the triangle in <paramref name="rows"/> and moving to
    /// one of the two adjacent numbers in the row below until the bottom row is reached.
    /// </summary>
    /// <param name="rows">
    /// The triangle from the top down: at least one row, where row k (counting from 1) holds exactly k whole
    /// numbers separated by whitespace. Each number must fit in an int and may be negative.
    /// </param>
    public static long Solve(IReadOnlyList<string> rows) => NumberTriangle.MaximumPathSum(rows);
}
