using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The greatest product of thirteen adjacent digits in a given 1000-digit number.</summary>
public sealed class Problem008 : Problem
{
    public override int Number => 8;

    public override string Title => "Largest Product in a Series";

    public override object Solve() =>
        Solve(digits: string.Concat(Resources.ReadLines("0008_number.txt")), windowLength: 13);

    /// <summary>The greatest product of <paramref name="windowLength"/> adjacent digits in <paramref name="digits"/>.</summary>
    /// <param name="digits">A string of decimal digits.</param>
    /// <param name="windowLength">How many adjacent digits to multiply: from 1 to 19 (so the product fits in a long) and no longer than <paramref name="digits"/>.</param>
    public static long Solve(string digits, int windowLength)
    {
        if (digits.Length == 0 || digits.Any(c => c is < '0' or > '9'))
        {
            throw new ArgumentException("Expected a non-empty string of decimal digits.", nameof(digits));
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(windowLength, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(windowLength, Math.Min(19, digits.Length)); // 9^19 < long.MaxValue < 9^20

        long best = 0;
        for (var start = 0; start + windowLength <= digits.Length; start++)
        {
            long product = 1;
            for (var i = start; i < start + windowLength; i++)
            {
                product *= digits[i] - '0';
            }

            best = Math.Max(best, product);
        }

        return best;
    }
}
