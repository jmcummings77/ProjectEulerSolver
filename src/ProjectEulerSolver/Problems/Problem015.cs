using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>Number of monotone lattice paths through a 20×20 grid.</summary>
public sealed class Problem015 : Problem
{
    private const int MaxGridSize = 100_000;

    public override int Number => 15;

    public override string Title => "Lattice Paths";

    public override object Solve() => Solve(gridSize: 20);

    /// <summary>
    /// Number of routes from the top-left to the bottom-right corner of a <paramref name="gridSize"/> ×
    /// <paramref name="gridSize"/> grid, moving only right and down along its lines.
    /// </summary>
    /// <param name="gridSize">
    /// The number of cells along each side, from 0 (a single point, with one empty route) to 100,000. The count has
    /// about 0.6 × <paramref name="gridSize"/> digits and the time to compute it grows with the square of
    /// <paramref name="gridSize"/>, which is what sets the upper end.
    /// </param>
    public static BigInteger Solve(int gridSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(gridSize);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(gridSize, MaxGridSize);

        // Every path is a sequence of gridSize rights and gridSize downs, so choose which half of the
        // 2 × gridSize moves are rights.
        return NumberTheory.Binomial(2 * gridSize, gridSize);
    }
}
