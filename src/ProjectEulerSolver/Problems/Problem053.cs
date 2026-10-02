namespace ProjectEulerSolver.Problems;

/// <summary>How many values of C(n, r) for 1 ≤ n ≤ 100 exceed one million.</summary>
public sealed class Problem053 : Problem
{
    public override int Number => 53;

    public override string Title => "Combinatoric Selections";

    public override object Solve() => Solve(maxN: 100, threshold: 1_000_000);

    /// <summary>
    /// How many binomial coefficients C(n, r) with 1 ≤ n ≤ <paramref name="maxN"/> and 0 ≤ r ≤ n are greater than
    /// <paramref name="threshold"/>. Equal values at different (n, r) are counted separately.
    /// </summary>
    /// <param name="maxN">The largest n: from 0 to 10,000 (the work grows with its square).</param>
    /// <param name="threshold">The value a coefficient must exceed: zero or more.</param>
    public static int Solve(int maxN, long threshold)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxN);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxN, 10_000);
        ArgumentOutOfRangeException.ThrowIfNegative(threshold);

        // Pascal's triangle, one row updated in place from right to left. The true values soon outgrow any integer
        // type, so every entry above the threshold is stored as threshold + 1 (which always fits in a ulong).
        var cap = (ulong)threshold + 1;
        var row = new ulong[maxN + 1];
        row[0] = 1;

        // At most (maxN + 1)(maxN + 2) / 2 entries are counted, about fifty million at the largest maxN.
        var count = 0;
        for (var n = 1; n <= maxN; n++)
        {
            row[n] = 1;
            for (var r = n - 1; r >= 1; r--)
            {
                // Both entries are at most cap, so comparing against cap - row[r] cannot overflow, unlike adding first.
                row[r] = row[r - 1] >= cap - row[r] ? cap : row[r] + row[r - 1];
            }

            for (var r = 0; r <= n; r++)
            {
                if (row[r] == cap)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
