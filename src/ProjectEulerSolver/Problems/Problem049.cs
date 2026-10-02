using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The 12-digit number formed by the other arithmetic sequence of three 4-digit prime permutations.</summary>
public sealed class Problem049 : Problem
{
    public override int Number => 49;

    public override string Title => "Prime Permutations";

    public override object Solve()
    {
        var isPrime = Primes.Sieve(9999);
        for (var first = 1001; first < 10_000; first += 2)
        {
            if (!isPrime[first] || first == 1487)
            {
                continue; // 1487, 4817, 8147 is the sequence given in the problem statement.
            }

            for (var step = 2; first + 2 * step < 10_000; step += 2)
            {
                var second = first + step;
                var third = second + step;
                if (isPrime[second] && isPrime[third]
                    && Digits.ArePermutations(first, second) && Digits.ArePermutations(first, third))
                {
                    return $"{first}{second}{third}";
                }
            }
        }

        throw new InvalidOperationException("No other sequence found.");
    }
}
