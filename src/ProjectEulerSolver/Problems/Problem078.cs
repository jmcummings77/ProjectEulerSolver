namespace ProjectEulerSolver.Problems;

/// <summary>The least n for which the partition number p(n) is divisible by one million.</summary>
public sealed class Problem078 : Problem
{
    private const int MaxSupportedDivisor = 1_000_000;

    // The search is abandoned once n passes this multiple of the divisor: about three times the largest ratio
    // n / divisor seen among the divisors up to 100,000 (see Solve).
    private const int SearchLimitFactor = 32;

    public override int Number => 78;

    public override string Title => "Coin Partitions";

    public override object Solve() => Solve(divisor: 1_000_000);

    /// <summary>
    /// The least n ≥ 1 for which p(n), the number of ways n coins can be separated into piles, is divisible by
    /// <paramref name="divisor"/>.
    /// </summary>
    /// <param name="divisor">
    /// From 1 to 1,000,000. It is not known that every divisor divides some p(n), so the search is cut off at
    /// n = 32 × <paramref name="divisor"/>; reaching n costs about n^1.5 steps. In practice p(n) modulo the divisor
    /// behaves like a random residue and the least n is of the order of the divisor itself: every divisor up to
    /// 100,000 has been run, and none needs more than 11 × divisor or n = 978,187 (a second or two). Larger
    /// divisors have not all been run; most take seconds, and an unlucky one near 1,000,000 can take minutes.
    /// </param>
    /// <exception cref="InvalidOperationException">No n up to 32 × <paramref name="divisor"/> has p(n) divisible by <paramref name="divisor"/>.</exception>
    public static int Solve(int divisor)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(divisor, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(divisor, MaxSupportedDivisor);

        return LeastIndex(divisor, searchLimit: SearchLimitFactor * divisor);
    }

    /// <summary>The least n from 1 to <paramref name="searchLimit"/> with p(n) divisible by <paramref name="divisor"/>.</summary>
    internal static int LeastIndex(int divisor, int searchLimit)
    {
        // Euler's pentagonal number theorem: p(n) = Σ (-1)^(k+1) [p(n - k(3k-1)/2) + p(n - k(3k+1)/2)] over k ≥ 1.
        // Only residues modulo the divisor are kept. A sum has fewer than 2√n terms, each below the divisor, so
        // it stays inside a long for any int n and divisor.
        var offsets = GeneralizedPentagonals(searchLimit);
        var partitions = new int[Math.Min(searchLimit, 1023) + 1];
        partitions[0] = 1 % divisor;
        var terms = 0;
        for (var n = 1; n <= searchLimit; n++)
        {
            while (terms < offsets.Length && offsets[terms] <= n)
            {
                terms++;
            }

            long total = 0;
            var active = offsets.AsSpan(0, terms);
            for (var i = 0; i < active.Length; i++)
            {
                var term = partitions[n - active[i]];
                total += (i & 2) == 0 ? term : -term;
            }

            var residue = (int)(total % divisor);
            if (residue < 0)
            {
                residue += divisor;
            }

            if (residue == 0)
            {
                return n;
            }

            if (n == partitions.Length)
            {
                Array.Resize(ref partitions, (int)Math.Min(2L * n, searchLimit + 1L));
            }

            partitions[n] = residue;
        }

        throw new InvalidOperationException($"No n up to {searchLimit} has a partition number divisible by {divisor}.");
    }

    /// <summary>The generalized pentagonal numbers 1, 2, 5, 7, 12, 15, … (k(3k-1)/2 then k(3k+1)/2 for k = 1, 2, 3, …) up to <paramref name="max"/>.</summary>
    private static int[] GeneralizedPentagonals(int max)
    {
        var numbers = new List<int>();
        for (long k = 1; k * (3 * k - 1) / 2 <= max; k++)
        {
            numbers.Add((int)(k * (3 * k - 1) / 2));
            if (k * (3 * k + 1) / 2 <= max)
            {
                numbers.Add((int)(k * (3 * k + 1) / 2));
            }
        }

        return [.. numbers];
    }
}
