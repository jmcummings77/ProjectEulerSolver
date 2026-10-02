using System.Globalization;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum total from top to bottom of a 15-row triangle of numbers.</summary>
public sealed class Problem018 : Problem
{
    public override int Number => 18;

    public override string Title => "Maximum Path Sum I";

    public override object Solve() => Solve(rows: Resources.ReadLines("0018_triangle.txt"));

    /// <summary>
    /// The largest total obtainable by starting at the top of the triangle given by <paramref name="rows"/> and moving
    /// to one of the two adjacent numbers in the row below until the bottom row is reached.
    /// </summary>
    /// <param name="rows">
    /// The triangle from the top down, one row per item, each a whitespace-separated list of whole numbers in the
    /// range of an int (negative numbers are allowed). There must be at least one row, and row k must hold exactly
    /// k numbers.
    /// </param>
    public static long Solve(IReadOnlyList<string> rows)
    {
        if (rows.Count == 0)
        {
            throw new ArgumentException("Expected at least one row.", nameof(rows));
        }

        var triangle = new int[rows.Count][];
        for (var row = 0; row < rows.Count; row++)
        {
            triangle[row] = ParseRow(rows, row);
        }

        // Collapse from the bottom up: best[i] becomes the largest total of any path that starts at entry i of the
        // current row and runs to the bottom, so it ends up holding the answer at the apex. A total adds one int
        // per row and there are fewer than 2^31 rows, so it stays within ±2^62.
        var best = Array.ConvertAll(triangle[^1], value => (long)value);
        for (var row = rows.Count - 2; row >= 0; row--)
        {
            for (var i = 0; i <= row; i++)
            {
                best[i] = triangle[row][i] + Math.Max(best[i], best[i + 1]);
            }
        }

        return best[0];
    }

    private static int[] ParseRow(IReadOnlyList<string> rows, int row)
    {
        var entries = (rows[row] ?? string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (entries.Length != row + 1)
        {
            throw new ArgumentException($"Row {row + 1} of a triangle needs {row + 1} numbers but has {entries.Length}.", nameof(rows));
        }

        var values = new int[entries.Length];
        for (var i = 0; i < entries.Length; i++)
        {
            if (!int.TryParse(entries[i], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out values[i]))
            {
                throw new ArgumentException($"'{entries[i]}' in row {row + 1} is not a whole number in the range of an int.", nameof(rows));
            }
        }

        return values;
    }
}
