using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all 0-to-9 pandigital numbers with the sub-string divisibility property.</summary>
public sealed class Problem043 : Problem
{
    private static readonly int[] Divisors = [2, 3, 5, 7, 11, 13, 17];

    public override int Number => 43;

    public override string Title => "Sub-string Divisibility";

    public override object Solve() => Solve(maxDigit: 9);

    /// <summary>
    /// The sum of all 0-to-<paramref name="maxDigit"/> pandigital digit strings d1 d2 d3 ... (each digit used once;
    /// d1 may be zero, which only shortens the number) with the sub-string divisibility property: d2d3d4 is
    /// divisible by 2, d3d4d5 by 3, d4d5d6 by 5, and so on through 7, 11, 13 and 17 for as many three-digit
    /// windows as the string has.
    /// </summary>
    /// <param name="maxDigit">The largest digit used, from 3 (four digits, one window) to 9 (ten digits, seven windows).</param>
    public static long Solve(int maxDigit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxDigit, 3);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxDigit, 9);

        // At most 10! strings, each worth less than 10^10, so the total stays below 4 × 10^16 and fits in a long.
        long total = 0;
        var digits = Enumerable.Range(0, maxDigit + 1).ToArray();
        do
        {
            if (HasProperty(digits))
            {
                total += Digits.FromDigits(digits);
            }
        }
        while (Combinatorics.NextPermutation(digits));

        return total;
    }

    /// <summary>
    /// True when every three-digit window of <paramref name="d"/> after the first digit is divisible by its prime.
    /// A string of n digits has n − 3 such windows, so at most ten digits can be checked.
    /// </summary>
    internal static bool HasProperty(int[] d)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(d.Length, Divisors.Length + 3, nameof(d));
        for (var i = 0; i + 3 < d.Length; i++)
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
