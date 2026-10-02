using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest 1-to-9 pandigital number formed as the concatenated product of an integer with (1, 2, ..., n), n &gt; 1.</summary>
public sealed class Problem038 : Problem
{
    public override int Number => 38;

    public override string Title => "Pandigital Multiples";

    public override object Solve()
    {
        // With n ≥ 2 the integer must have at most four digits for the concatenation to fit in nine.
        long best = 0;
        for (var x = 1; x < 10_000; x++)
        {
            var concatenated = string.Empty;
            for (var n = 1; concatenated.Length < 9; n++)
            {
                concatenated += x * n;
            }

            if (concatenated.Length == 9)
            {
                var value = long.Parse(concatenated);
                if (Digits.IsPandigital(value, 9))
                {
                    best = Math.Max(best, value);
                }
            }
        }

        return best;
    }
}
