using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum total from top to bottom of a 15-row triangle of numbers.</summary>
public sealed class Problem018 : Problem
{
    public override int Number => 18;

    public override string Title => "Maximum Path Sum I";

    public override object Solve() => MaximumPathSum(Resources.ReadLines("0018_triangle.txt"));

    /// <summary>
    /// Collapses the triangle from the bottom up: each cell becomes itself plus the larger of its two children,
    /// so the apex ends up holding the best path total. Shared with Problem 67.
    /// </summary>
    internal static int MaximumPathSum(IEnumerable<string> rows)
    {
        var triangle = rows.Select(row => row.Split(' ').Select(int.Parse).ToArray()).ToArray();
        for (var row = triangle.Length - 2; row >= 0; row--)
        {
            for (var i = 0; i < triangle[row].Length; i++)
            {
                triangle[row][i] += Math.Max(triangle[row + 1][i], triangle[row + 1][i + 1]);
            }
        }

        return triangle[0][0];
    }
}
