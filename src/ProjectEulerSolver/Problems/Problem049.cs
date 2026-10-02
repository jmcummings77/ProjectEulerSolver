using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The 12-digit number formed by the other arithmetic sequence of three 4-digit prime permutations.</summary>
public sealed class Problem049 : Problem
{
    public override int Number => 49;

    public override string Title => "Prime Permutations";

    // 1487, 4817, 8147 is the sequence given in the problem statement; the answer is the only other one.
    public override object Solve() => Solve(digits: 4).Single(sequence => sequence != "148748178147");

    /// <summary>
    /// Every increasing arithmetic sequence of three <paramref name="digits"/>-digit primes that are permutations
    /// of one another, each written as its three terms run together, in ascending order.
    /// </summary>
    /// <param name="digits">
    /// The number of digits in each prime, from 1 to 7, which keeps the sieve to ten million flags and the pairwise
    /// search within each group of permutations to a fraction of a second. Primes of fewer than four digits form
    /// no such sequence, so the list is then empty.
    /// </param>
    public static IReadOnlyList<string> Solve(int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, 7);

        var upper = 1;
        for (var i = 0; i < digits; i++)
        {
            upper *= 10;
        }

        // The terms of a sequence share their digits, so each sequence lies inside one group of primes with the
        // same signature. Primes are added in ascending order, which keeps every group sorted.
        var isPrime = Primes.Sieve(upper - 1);
        var groups = new Dictionary<long, List<int>>();
        for (var prime = upper / 10; prime < upper; prime++)
        {
            if (!isPrime[prime])
            {
                continue;
            }

            var signature = Digits.MultisetKey(prime);
            if (!groups.TryGetValue(signature, out var members))
            {
                groups[signature] = members = [];
            }

            members.Add(prime);
        }

        var sequences = new List<string>();
        foreach (var (signature, members) in groups)
        {
            for (var i = 0; i < members.Count; i++)
            {
                for (var j = i + 1; j < members.Count; j++)
                {
                    // The first two terms fix the third. It is below 2 × 10^7, so it cannot overflow.
                    var third = 2 * members[j] - members[i];
                    if (third >= upper)
                    {
                        break;
                    }

                    if (isPrime[third] && Digits.MultisetKey(third) == signature)
                    {
                        sequences.Add($"{members[i]}{members[j]}{third}");
                    }
                }
            }
        }

        // Every entry has the same length, so ordinal order is numeric order.
        sequences.Sort(StringComparer.Ordinal);
        return sequences;
    }

    // Counts each digit value in its own four bits (a count is at most 7), so two numbers share a signature
    // exactly when one is a permutation of the other.
}
