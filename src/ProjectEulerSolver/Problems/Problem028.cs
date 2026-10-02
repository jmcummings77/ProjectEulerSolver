namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the numbers on the diagonals of a 1001×1001 number spiral.</summary>
public sealed class Problem028 : Problem
{
    public override int Number => 28;

    public override string Title => "Number Spiral Diagonals";

    public override object Solve()
    {
        // Each ring of side length s (odd, s ≥ 3) has corners spaced s - 1 apart, ending at s².
        long total = 1;
        long corner = 1;
        for (var side = 3; side <= 1001; side += 2)
        {
            for (var i = 0; i < 4; i++)
            {
                corner += side - 1;
                total += corner;
            }
        }

        return total;
    }
}
