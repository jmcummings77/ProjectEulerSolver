using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The index of the first Fibonacci term to contain 1000 digits.</summary>
public sealed class Problem025 : Problem
{
    public override int Number => 25;

    public override string Title => "1000-digit Fibonacci Number";

    public override object Solve() => Solve(digits: 1000);

    /// <summary>
    /// The index of the first term of the Fibonacci sequence (F1 = F2 = 1) with at least <paramref name="digits"/> decimal digits.
    /// </summary>
    /// <param name="digits">The number of digits to reach, from 1 to 1,000,000.</param>
    public static int Solve(int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 1);

        // The cap keeps the run to a few seconds: the terms near the answer are as long as the digit count asked for,
        // and they are multiplied a few dozen times. It also keeps the index, about 4.8 times the digit count, in an int.
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, 1_000_000);

        var threshold = BigInteger.Pow(10, digits - 1); // The smallest number with that many digits.

        // Fn is the nearest integer to φ^n / √5, which puts the answer near (digits - 1 + log10 √5) / log10 φ.
        // Floating point only chooses where to start: the exact terms below decide, stepping either way if needed.
        var goldenRatio = (1 + Math.Sqrt(5)) / 2;
        var index = Math.Max(1, (int)Math.Ceiling((digits - 1 + Math.Log10(Math.Sqrt(5))) / Math.Log10(goldenRatio)));
        var (previous, current) = ConsecutiveTerms(index - 1);

        // The terms never decrease from F0 = 0 on, so the answer is the one index with F(index - 1) < threshold ≤ F(index).
        while (previous >= threshold)
        {
            (previous, current) = (current - previous, previous);
            index--;
        }

        while (current < threshold)
        {
            (previous, current) = (current, previous + current);
            index++;
        }

        return index;
    }

    /// <summary>
    /// F(n) and F(n + 1), with F0 = 0, by the doubling identities F(2k) = F(k)·(2F(k + 1) - F(k)) and
    /// F(2k + 1) = F(k)² + F(k + 1)².
    /// </summary>
    private static (BigInteger Term, BigInteger Next) ConsecutiveTerms(int n)
    {
        if (n == 0)
        {
            return (BigInteger.Zero, BigInteger.One);
        }

        var (half, halfNext) = ConsecutiveTerms(n / 2);
        var even = half * (2 * halfNext - half);
        var odd = half * half + halfNext * halfNext;
        return n % 2 == 0 ? (even, odd) : (odd, even + odd);
    }
}
