using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all 0-to-9 pandigital numbers with the sub-string divisibility property.</summary>
public sealed class Problem043 : Problem
{
    private static readonly int[] Divisors = [2, 3, 5, 7, 11, 13, 17];

    public override int Number => 43;

    public override string Title => "Sub-string Divisibility";

    public override object Solve()
    {
        long total = 0;
        var digits = Enumerable.Range(0, 10).ToArray();
        do
        {
            if (digits[0] != 0 && HasProperty(digits))
            {
                total += Digits.FromDigits(digits);
            }
        }
        while (Combinatorics.NextPermutation(digits));

        return total;
    }

    private static bool HasProperty(int[] d)
    {
        for (var i = 0; i < Divisors.Length; i++)
        {
            var window = 100 * d[i + 1] + 10 * d[i + 2] + d[i + 3];
            if (window % Divisors[i] != 0)
            {
                return false;
            }
        }

        return true;
    }
}
