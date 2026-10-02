using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the digits of 2^1000.</summary>
public sealed class Problem016 : Problem
{
    private const int MaxExponent = 1_000_000;

    public override int Number => 16;

    public override string Title => "Power Digit Sum";

    public override object Solve() => Solve(exponent: 1000);

    /// <summary>The sum of the decimal digits of 2 raised to the power <paramref name="exponent"/>.</summary>
    /// <param name="exponent">
    /// The power of two, from 0 to 1,000,000. Writing the power out in decimal takes time that grows with the square
    /// of <paramref name="exponent"/>, which is what sets the upper end.
    /// </param>
    public static int Solve(int exponent)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(exponent);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(exponent, MaxExponent);

        // 2^1,000,000 has 301,030 digits, so the digit sum is below 9 × 301,030 and fits an int with room to spare.
        return Digits.Sum(BigInteger.Pow(2, exponent));
    }
}
