namespace ProjectEulerSolver.Problems;

/// <summary>Sum of all the multiples of 3 or 5 below 1000.</summary>
public sealed class Problem001 : Problem
{
    public override int Number => 1;

    public override string Title => "Multiples of 3 or 5";

    public override object Solve() => Solve(limit: 1000);

    /// <summary>Sum of the positive multiples of 3 or 5 below <paramref name="limit"/>.</summary>
    /// <param name="limit">Exclusive upper bound, from 0 to 3,000,000,000.</param>
    public static long Solve(long limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 3_000_000_000); // Keeps every intermediate within a long.

        // Inclusion–exclusion on arithmetic series: multiples of 3, plus multiples of 5, minus those counted twice.
        return SumOfMultiples(3) + SumOfMultiples(5) - SumOfMultiples(15);

        long SumOfMultiples(long k)
        {
            var count = limit == 0 ? 0 : (limit - 1) / k;
            return k * count * (count + 1) / 2;
        }
    }
}
