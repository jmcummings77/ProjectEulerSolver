using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest n-digit pandigital prime.</summary>
public sealed class Problem041 : Problem
{
    public override int Number => 41;

    public override string Title => "Pandigital Prime";

    public override object Solve()
    {
        // 1..8 and 1..9 pandigitals have digit sums 36 and 45, so they are divisible by 3; start from 7 digits.
        for (var length = 7; length >= 1; length--)
        {
            var digits = Enumerable.Range(1, length).Reverse().ToArray();
            var candidates = Combinatorics.Permutations(digits)
                .Select(Digits.FromDigits)
                .OrderDescending();
            foreach (var candidate in candidates)
            {
                if (Primes.IsPrime(candidate))
                {
                    return candidate;
                }
            }
        }

        throw new InvalidOperationException("No pandigital prime exists.");
    }
}
