namespace ProjectEulerSolver.Problems;

/// <summary>Sum of the even-valued Fibonacci terms that do not exceed four million.</summary>
public sealed class Problem002 : Problem
{
    public override int Number => 2;

    public override string Title => "Even Fibonacci Numbers";

    public override object Solve() => Solve(maxValue: 4_000_000);

    /// <summary>
    /// Sum of the even-valued terms of the Fibonacci sequence 1, 2, 3, 5, 8, ... that do not exceed <paramref name="maxValue"/>.
    /// </summary>
    /// <param name="maxValue">Inclusive upper bound on the terms, from 0 to <see cref="long.MaxValue"/>.</param>
    public static long Solve(long maxValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxValue);

        // The sum cannot overflow. Even terms are three apart and so more than quadruple each time, which keeps
        // their total below 4/3 of the largest one, and the largest even term a long can hold is about 2.9 × 10^18.
        long sum = 0;
        long previous = 1;
        long current = 2;
        while (current <= maxValue)
        {
            if (current % 2 == 0)
            {
                sum += current;
            }

            if (previous > maxValue - current)
            {
                break; // The next term would exceed maxValue; forming it could overflow.
            }

            (previous, current) = (current, previous + current);
        }

        return sum;
    }
}
