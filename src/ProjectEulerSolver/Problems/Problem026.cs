namespace ProjectEulerSolver.Problems;

/// <summary>The d &lt; 1000 for which 1/d has the longest recurring cycle in its decimal expansion.</summary>
public sealed class Problem026 : Problem
{
    public override int Number => 26;

    public override string Title => "Reciprocal Cycles";

    public override object Solve() => Solve(limit: 1000);

    /// <summary>
    /// The positive d below <paramref name="limit"/> for which 1/d has the longest recurring cycle in its decimal
    /// expansion. When several share the longest cycle (including when none recurs at all) the smallest d is returned.
    /// </summary>
    /// <param name="limit">Exclusive upper bound on d, from 2 to 10,000,000.</param>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 2);

        // The cap keeps the run to a few seconds: one long division takes up to d steps, and up to a few hundred
        // values of d are tried before one with a cycle of nearly its own length ends the search.
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 10_000_000);

        // The cycle of 1/d is shorter than d (see CycleLength). So, working downwards, once d is no bigger than the
        // longest cycle found, neither it nor anything smaller can match that cycle and the search is over.
        var bestD = 1;
        var bestCycle = 0;
        for (var d = limit - 1; d > bestCycle; d--)
        {
            var cycle = CycleLength(d);
            if (cycle >= bestCycle)
            {
                // Taking equal cycles too makes the smaller d win a tie.
                bestCycle = cycle;
                bestD = d;
            }
        }

        return bestD;
    }

    /// <summary>
    /// Length of the repeating block of 1/d. Factors of 2 and 5 only delay the cycle, so strip them;
    /// the cycle length is then the smallest k with 10^k ≡ 1 (mod d), found by long division.
    /// The remainders met are distinct and non-zero, so k is at most d - 1.
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

        long remainder = 10 % d;
        var length = 1;
        while (remainder != 1)
        {
            remainder = remainder * 10 % d;
            length++;
        }

        return length;
    }
}
