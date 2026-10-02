namespace ProjectEulerSolver.Problems;

/// <summary>The d &lt; 1000 for which 1/d has the longest recurring cycle in its decimal expansion.</summary>
public sealed class Problem026 : Problem
{
    public override int Number => 26;

    public override string Title => "Reciprocal Cycles";

    public override object Solve()
    {
        var bestD = 0;
        var bestCycle = 0;
        for (var d = 2; d < 1000; d++)
        {
            var cycle = CycleLength(d);
            if (cycle > bestCycle)
            {
                bestCycle = cycle;
                bestD = d;
            }
        }

        return bestD;
    }

    /// <summary>
    /// Length of the repeating block of 1/d. Factors of 2 and 5 only delay the cycle, so strip them;
    /// the cycle length is then the smallest k with 10^k ≡ 1 (mod d), found by long division.
    /// </summary>
    private static int CycleLength(int d)
    {
        while (d % 2 == 0)
        {
            d /= 2;
        }

        while (d % 5 == 0)
        {
            d /= 5;
        }

        if (d == 1)
        {
            return 0;
        }

        var remainder = 10 % d;
        var length = 1;
        while (remainder != 1)
        {
            remainder = remainder * 10 % d;
            length++;
        }

        return length;
    }
}
