using System.Globalization;
using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first ten digits of the sum of one hundred 50-digit numbers.</summary>
public sealed class Problem013 : Problem
{
    public override int Number => 13;

    public override string Title => "Large Sum";

    public override object Solve() => Solve(numbers: Resources.ReadLines("0013_numbers.txt"), digits: 10);

    /// <summary>The first <paramref name="digits"/> digits of the sum of <paramref name="numbers"/>.</summary>
    /// <param name="numbers">
    /// At least one non-negative whole number, each written as a string of decimal digits of any length
    /// (leading zeros are allowed).
    /// </param>
    /// <param name="digits">How many leading digits to return: from 1 to the number of digits in the sum.</param>
    public static string Solve(IReadOnlyList<string> numbers, int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 1);
        if (numbers.Count == 0)
        {
            throw new ArgumentException("Expected at least one number.", nameof(numbers));
        }

        var total = BigInteger.Zero;
        foreach (var number in numbers)
        {
            if (string.IsNullOrEmpty(number) || number.Any(c => c is < '0' or > '9'))
            {
                throw new ArgumentException($"'{number}' is not a string of decimal digits.", nameof(numbers));
            }

            total += BigInteger.Parse(number, NumberStyles.None, CultureInfo.InvariantCulture);
        }

        var sum = total.ToString(CultureInfo.InvariantCulture);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, sum.Length);
        return sum[..digits];
    }
}
