using System.Globalization;
using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The greatest product of four adjacent numbers in any direction in a 20×20 grid.</summary>
public sealed class Problem011 : Problem
{
    private static readonly (int Row, int Column)[] Directions = [(0, 1), (1, 0), (1, 1), (1, -1)];

    public override int Number => 11;

    public override string Title => "Largest Product in a Grid";

    public override object Solve() => Solve(rows: Resources.ReadLines("0011_grid.txt"), runLength: 4);

    /// <summary>
    /// The greatest product of <paramref name="runLength"/> adjacent numbers lying in a straight line (across, down or
    /// along either diagonal) in the grid given by <paramref name="rows"/>.
    /// </summary>
    /// <param name="rows">
    /// The grid, one row per item, each a whitespace-separated list of whole numbers. The numbers may be of any size
    /// and may be negative; there must be at least one row and every row must hold the same number of entries (at least one).
    /// </param>
    /// <param name="runLength">
    /// How many adjacent numbers to multiply: from 1 to the larger of the grid's height and width, so that at least
    /// one line fits. Directions in which no line of that length fits are simply not considered.
    /// </param>
    public static BigInteger Solve(IReadOnlyList<string> rows, int runLength)
    {
        var grid = ParseGrid(rows);
        var height = grid.Length;
        var width = grid[0].Length;
        ArgumentOutOfRangeException.ThrowIfLessThan(runLength, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(runLength, Math.Max(height, width));

        // The range check above guarantees a horizontal or a vertical line fits, so there is always a first product.
        BigInteger? best = null;
        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                foreach (var (dr, dc) in Directions)
                {
                    // long, because the far end of a line that does not fit can lie beyond the range of an int.
                    var endRow = row + (long)dr * (runLength - 1);
                    var endColumn = column + (long)dc * (runLength - 1);
                    if (endRow >= height || endColumn < 0 || endColumn >= width)
                    {
                        continue;
                    }

                    var product = BigInteger.One;
                    for (var step = 0; step < runLength; step++)
                    {
                        product *= grid[row + dr * step][column + dc * step];
                    }

                    if (best is null || product > best)
                    {
                        best = product;
                    }
                }
            }
        }

        return best!.Value;
    }

    private static BigInteger[][] ParseGrid(IReadOnlyList<string> rows)
    {
        if (rows.Count == 0)
        {
            throw new ArgumentException("Expected at least one row.", nameof(rows));
        }

        var grid = new BigInteger[rows.Count][];
        for (var row = 0; row < rows.Count; row++)
        {
            var entries = (rows[row] ?? string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (entries.Length == 0 || (row > 0 && entries.Length != grid[0].Length))
            {
                throw new ArgumentException(
                    $"Row {row + 1} has {entries.Length} entries; every row needs the same number, and at least one.",
                    nameof(rows));
            }

            grid[row] = new BigInteger[entries.Length];
            for (var column = 0; column < entries.Length; column++)
            {
                if (!BigInteger.TryParse(entries[column], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out grid[row][column]))
                {
                    throw new ArgumentException($"'{entries[column]}' in row {row + 1} is not a whole number.", nameof(rows));
                }
            }
        }

        return grid;
    }
}
