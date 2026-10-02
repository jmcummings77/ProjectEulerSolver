using System.Globalization;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum total from top to bottom of a 100-row triangle of numbers.</summary>
public sealed class Problem067 : Problem
{
    public override int Number => 67;

    public override string Title => "Maximum Path Sum II";

    public override object Solve() => Solve(rows: Resources.ReadLines("0067_triangle.txt"));

    /// <summary>
    /// The largest total obtainable by starting at the top of the triangle in <paramref name="rows"/> and moving to
    /// one of the two adjacent numbers in the row below until the bottom row is reached.
    /// </summary>
    /// <param name="rows">
    /// The triangle from the top down: at least one row, where row k (counting from 1) holds exactly k whole
    /// numbers separated by spaces. Each number must fit in an int and may be negative.
    /// </param>
    public static long Solve(IReadOnlyList<string> rows)
    {
        var triangle = Parse(rows);

        // Collapse from the bottom up: each cell becomes itself plus the better of its two children, so the apex
        // ends up holding the best path total. A path adds one int per row, so the totals cannot overflow a long.
        var best = Array.ConvertAll(triangle[^1], value => (long)value);
        for (var row = triangle.Length - 2; row >= 0; row--)
        {
            for (var i = 0; i <= row; i++)
            {
                best[i] = triangle[row][i] + Math.Max(best[i], best[i + 1]);
            }
        }

        return best[0];
    }

    private static int[][] Parse(IReadOnlyList<string> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        if (rows.Count == 0)
        {
            throw new ArgumentException("Expected at least one row.", nameof(rows));
        }

        var triangle = new int[rows.Count][];
        for (var row = 0; row < rows.Count; row++)
        {
            if (rows[row] is not { } line)
            {
                throw new ArgumentException($"Row {row + 1} is missing.", nameof(rows));
            }

            var items = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != row + 1)
            {
                throw new ArgumentException($"Row {row + 1} must hold {row + 1} numbers but holds {items.Length}.", nameof(rows));
            }

            triangle[row] = new int[items.Length];
            for (var i = 0; i < items.Length; i++)
            {
                if (!int.TryParse(items[i], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out triangle[row][i]))
                {
                    throw new ArgumentException($"'{items[i]}' in row {row + 1} is not a whole number that fits in an int.", nameof(rows));
                }
            }
        }

        return triangle;
    }
}
