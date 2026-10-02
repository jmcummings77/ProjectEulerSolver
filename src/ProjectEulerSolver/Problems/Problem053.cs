namespace ProjectEulerSolver.Problems;

/// <summary>How many values of C(n, r) for 1 ≤ n ≤ 100 exceed one million.</summary>
public sealed class Problem053 : Problem
{
    private const long Threshold = 1_000_000;

    public override int Number => 53;

    public override string Title => "Combinatoric Selections";

    public override object Solve()
    {
        // Build Pascal's triangle row by row, capping entries just above the threshold to avoid overflow.
        var count = 0;
        var row = new long[] { 1 };
        for (var n = 1; n <= 100; n++)
        {
            var next = new long[n + 1];
            next[0] = next[n] = 1;
            for (var r = 1; r < n; r++)
            {
                next[r] = Math.Min(row[r - 1] + row[r], Threshold + 1);
                if (next[r] > Threshold)
                {
                    count++;
                }
            }

            row = next;
        }

        return count;
    }
}
