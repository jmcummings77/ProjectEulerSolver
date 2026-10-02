using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all positive integers that cannot be written as the sum of two abundant numbers.</summary>
public sealed class Problem023 : Problem
{
    public override int Number => 23;

    public override string Title => "Non-Abundant Sums";

    // The statement notes that every integer above 28123 is the sum of two abundant numbers, so this limit gives the sum of them all.
    public override object Solve() => Solve(limit: 28_123);

    /// <summary>
    /// Sum of the positive integers up to and including <paramref name="limit"/> that cannot be written as the sum of
    /// two abundant numbers (numbers whose proper divisors add up to more than the number itself).
    /// </summary>
    /// <param name="limit">Inclusive upper bound, from 0 to 10,000,000.</param>
    public static long Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        // The cap keeps the divisor-sum sieve to 40 MB and about a second. It also rules out overflow: the proper
        // divisors of n are among n/2, n/3, ..., n/n, so they add up to less than n·ln(n), under 1.7 × 10^8 here.
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 10_000_000);

        var divisorSums = NumberTheory.ProperDivisorSums(limit);
        var isAbundant = new bool[limit + 1];
        var abundant = new List<int>();
        var oddAbundant = new List<int>();
        for (var n = 1; n <= limit; n++)
        {
            if (divisorSums[n] > n)
            {
                isAbundant[n] = true;
                abundant.Add(n);
                if (n % 2 == 1)
                {
                    oddAbundant.Add(n);
                }
            }
        }

        long total = 0;
        for (var n = 1; n <= limit; n++)
        {
            if (!IsSumOfTwoAbundant(n))
            {
                total += n;
            }
        }

        return total;

        bool IsSumOfTwoAbundant(int n)
        {
            // An odd n needs exactly one odd part, and odd abundant numbers are rare, so trying each of those below n
            // as that part settles it in a few steps. For an even n the smaller part can be any abundant number up to n/2.
            var isOdd = n % 2 == 1;
            var candidates = isOdd ? oddAbundant : abundant;
            var largestCandidate = isOdd ? n : n / 2;
            foreach (var part in candidates)
            {
                if (part > largestCandidate)
                {
                    break;
                }

                if (isAbundant[n - part])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
