using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The greatest product of four adjacent numbers in any direction in a 20×20 grid.</summary>
public sealed class Problem011 : Problem
{
    private const int RunLength = 4;

    private static readonly (int Row, int Column)[] Directions = [(0, 1), (1, 0), (1, 1), (1, -1)];

    public override int Number => 11;

    public override string Title => "Largest Product in a Grid";

    public override object Solve()
    {
        var grid = Resources.ReadLines("0011_grid.txt")
            .Select(line => line.Split(' ').Select(int.Parse).ToArray())
            .ToArray();
        var size = grid.Length;

        long best = 0;
        for (var row = 0; row < size; row++)
        {
            for (var column = 0; column < size; column++)
            {
                foreach (var (dr, dc) in Directions)
                {
                    var endRow = row + dr * (RunLength - 1);
                    var endColumn = column + dc * (RunLength - 1);
                    if (endRow < 0 || endRow >= size || endColumn < 0 || endColumn >= size)
                    {
                        continue;
                    }

                    long product = 1;
                    for (var step = 0; step < RunLength; step++)
                    {
                        product *= grid[row + dr * step][column + dc * step];
                    }

                    best = Math.Max(best, product);
                }
            }
        }

        return best;
    }
}
