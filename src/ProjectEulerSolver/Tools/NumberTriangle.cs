namespace ProjectEulerSolver.Tools;

/// <summary>Triangles of numbers where each row has one more entry than the row above.</summary>
public static class NumberTriangle
{
    /// <summary>
    /// The largest total obtainable by starting at the apex and moving to one of the two adjacent numbers in the
    /// row below until the bottom is reached. Rows are given as space-separated integers.
    /// </summary>
    public static int MaximumPathSum(IEnumerable<string> rows)
    {
        var triangle = rows.Select(row => row.Split(' ').Select(int.Parse).ToArray()).ToArray();

        // Collapse from the bottom up: each cell becomes itself plus the better of its two children, so the
        // apex ends up holding the best path total.
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
