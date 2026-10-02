using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the digits in 100!.</summary>
public sealed class Problem020 : Problem
{
    private const int MaxN = 50_000;

    public override int Number => 20;

    public override string Title => "Factorial Digit Sum";

    public override object Solve() => Solve(n: 100);

    /// <summary>The sum of the decimal digits of <paramref name="n"/>! = 1 × 2 × … × <paramref name="n"/>.</summary>
    /// <param name="n">
    /// The number whose factorial is taken, from 0 (0! = 1) to 50,000. Building the factorial and writing it out
    /// in decimal both take time that grows roughly with the square of <paramref name="n"/>, which is what sets the upper end.
    /// </param>
    public static int Solve(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, MaxN);

        // 50,000! has 213,237 digits, so the digit sum is below 9 × 213,237 and fits an int with room to spare.
        return Digits.Sum(NumberTheory.Factorial(n));
    }
}
