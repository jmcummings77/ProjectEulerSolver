namespace ProjectEulerSolver.Problems;

/// <summary>The product abc of the Pythagorean triplet with a + b + c = 1000.</summary>
public sealed class Problem009 : Problem
{
    // abc ≤ ((a + b + c) / 3)³ by the AM–GM inequality, which stays below (2^21)³ = 2^63 for perimeters under 3 × 2^21.
    private const int LargestPerimeter = 6_291_455;

    public override int Number => 9;

    public override string Title => "Special Pythagorean Triplet";

    public override object Solve() => Solve(perimeter: 1000);

    /// <summary>
    /// The product abc of the Pythagorean triplet a &lt; b &lt; c (a² + b² = c², all positive integers) with
    /// a + b + c = <paramref name="perimeter"/>. When several triplets share the perimeter, the largest product.
    /// </summary>
    /// <param name="perimeter">The sum a + b + c, from 1 to 6,291,455 (so the product fits in a long).</param>
    /// <exception cref="InvalidOperationException">No Pythagorean triplet has this perimeter.</exception>
    public static long Solve(int perimeter)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(perimeter, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(perimeter, LargestPerimeter);

        // Putting c = p − a − b into a² + b² = c² and solving for b gives b = p (p − 2a) / (2 (p − a)), so the
        // shortest side decides the triplet, and a < b < c means a < p / 3. The formula can also land on b < a,
        // which is an earlier triplet with its legs swapped and so the same product.
        long p = perimeter;
        long largest = 0;
        for (long a = 1; 3 * a < p; a++)
        {
            var numerator = p * (p - 2 * a);
            var denominator = 2 * (p - a);
            if (numerator % denominator == 0)
            {
                var b = numerator / denominator;
                largest = Math.Max(largest, a * b * (p - a - b));
            }
        }

        if (largest == 0)
        {
            throw new InvalidOperationException($"No Pythagorean triplet sums to {perimeter}.");
        }

        return largest;
    }
}
