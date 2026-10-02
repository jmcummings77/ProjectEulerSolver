namespace ProjectEulerSolver.Problems;

/// <summary>The starting number under one million that produces the longest Collatz chain.</summary>
public sealed class Problem014 : Problem
{
    private const int Limit = 1_000_000;

    public override int Number => 14;

    public override string Title => "Longest Collatz Sequence";

    public override object Solve()
    {
        // Chain lengths for starting values below the limit, filled in lazily.
        var lengths = new int[Limit];
        lengths[1] = 1;

        var bestStart = 1;
        var bestLength = 1;
        for (var start = 2; start < Limit; start++)
        {
            var length = ChainLength(start, lengths);
            if (length > bestLength)
            {
                bestLength = length;
                bestStart = start;
            }
        }

        return bestStart;
    }

    private static int ChainLength(long n, int[] lengths)
    {
        if (n < lengths.Length && lengths[n] != 0)
        {
            return lengths[n];
        }

        var next = n % 2 == 0 ? n / 2 : 3 * n + 1;
        var length = 1 + ChainLength(next, lengths);
        if (n < lengths.Length)
        {
            lengths[n] = length;
        }

        return length;
    }
}
