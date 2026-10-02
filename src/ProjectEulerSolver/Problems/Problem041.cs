using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest n-digit pandigital prime.</summary>
public sealed class Problem041 : Problem
{
    public override int Number => 41;

    public override string Title => "Pandigital Prime";

    public override object Solve() => Solve(limit: 987_654_321);

    /// <summary>
    /// The largest prime not exceeding <paramref name="limit"/> that is n-digit pandigital for some n from 1 to 9:
    /// it uses each of the digits 1 to n exactly once.
    /// </summary>
    /// <param name="limit">
    /// Inclusive upper bound, from 0 to <see cref="int.MaxValue"/>. The smallest pandigital prime is 1423, so
    /// anything below that has no answer.
    /// </param>
    /// <exception cref="InvalidOperationException">No pandigital prime is at most <paramref name="limit"/>.</exception>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        // An n-digit pandigital has n ≤ 9 digits, and one with more digits than the limit is larger than the limit.
        for (var length = Math.Min(9, Digits.Count(limit)); length >= 1; length--)
        {
            // The digits 1..n sum to n(n + 1)/2. When that is a multiple of 3, every arrangement of them is a
            // multiple of 3 as well, and none of them is 3 itself, so that whole length holds no primes.
            if (length * (length + 1) / 2 % 3 == 0)
            {
                continue;
            }

            // Seeding the digits in descending order makes the permutations come out largest first.
            var digits = Enumerable.Range(1, length).Reverse().ToArray();
            foreach (var candidate in Combinatorics.Permutations(digits).Select(Digits.FromDigits))
            {
                if (candidate <= limit && Primes.IsPrime(candidate))
                {
                    return (int)candidate;
                }
            }
        }

        throw new InvalidOperationException($"No pandigital prime is at most {limit}.");
    }
}
