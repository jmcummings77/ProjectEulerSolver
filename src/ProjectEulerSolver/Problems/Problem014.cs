namespace ProjectEulerSolver.Problems;

/// <summary>The starting number under one million that produces the longest Collatz chain.</summary>
public sealed class Problem014 : Problem
{
    private const int MaxLimit = 100_000_000;

    public override int Number => 14;

    public override string Title => "Longest Collatz Sequence";

    public override object Solve() => Solve(limit: 1_000_000);

    /// <summary>
    /// The starting number below <paramref name="limit"/> that produces the longest Collatz chain
    /// (n → n/2 when n is even, n → 3n + 1 when n is odd, until 1 is reached). When several starting numbers
    /// share the longest chain, the smallest of them is returned.
    /// </summary>
    /// <param name="limit">
    /// Exclusive upper bound on the starting number, from 2 to 100,000,000. The solver keeps two bytes per
    /// starting number, so the largest limit needs about 200 MB.
    /// </param>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, MaxLimit);

        // Known Collatz records for starting numbers below 10^8 (the supported range): every chain reaches 1;
        // the longest has 950 terms (from 63,728,127); and the largest term anywhere is 2,185,143,829,170,100
        // (from 80,049,391). So the walk below always falls under its start, a long holds every term and a
        // ushort every length.
        var lengths = new ushort[limit];
        lengths[1] = 1;

        var bestStart = 1;
        for (var start = 2; start < limit; start++)
        {
            // Follow the chain only until it drops below its start: from there on it is the chain of a smaller
            // starting number, whose length is already in the table.
            long term = start;
            var steps = 0;
            while (term >= start)
            {
                term = term % 2 == 0 ? term / 2 : 3 * term + 1;
                steps++;
            }

            var length = (ushort)(steps + lengths[term]);
            lengths[start] = length;

            // Strictly greater, so the smallest start wins a tie.
            if (length > lengths[bestStart])
            {
                bestStart = start;
            }
        }

        return bestStart;
    }
}
