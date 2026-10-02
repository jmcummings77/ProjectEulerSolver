using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The lowest sum for a set of five primes for which any two concatenate to produce another prime.</summary>
public sealed class Problem060 : Problem
{
    private const int SetSize = 5;

    public override int Number => 60;

    public override string Title => "Prime Pair Sets";

    public override object Solve()
    {
        // Find a candidate among primes below 10,000, then prove nothing smaller exists: every member of a set
        // with a smaller sum is below candidate − (3 + 7 + 11 + 13), so a second search over that universe,
        // pruned by the candidate, is exhaustive. Pair results are shared between the two passes.
        var pairs = new PrimePairGraph();
        var candidate = pairs.LowestSum(Primes.UpTo(10_000), long.MaxValue);
        return pairs.LowestSum(Primes.UpTo((int)candidate - 34), candidate);
    }

    /// <summary>
    /// The "concatenate both ways to primes" relation, computed lazily and memoised, with a pruned depth-first
    /// search for the five-clique of smallest sum.
    /// </summary>
    private sealed class PrimePairGraph
    {
        private readonly Dictionary<long, bool> compatibility = [];

        /// <summary>The smallest sum of a qualifying five-set drawn from <paramref name="allPrimes"/>, or <paramref name="bound"/> when none beats it.</summary>
        public long LowestSum(int[] allPrimes, long bound)
        {
            // 2 and 5 can never be members: a concatenation ending in them is even or a multiple of 5.
            var primes = allPrimes.Where(p => p != 2 && p != 5).ToArray();
            var members = new int[SetSize];
            var best = bound;

            Extend(0, 0);
            return best;

            void Extend(int depth, long sum)
            {
                if (depth == SetSize)
                {
                    best = sum;
                    return;
                }

                // Members are chosen in ascending order, so the remaining ones are each at least primes[i].
                var remaining = SetSize - depth;
                var first = depth == 0 ? 0 : members[depth - 1] + 1;
                for (var i = first; i < primes.Length && sum + remaining * (long)primes[i] < best; i++)
                {
                    var compatible = true;
                    for (var m = 0; m < depth && compatible; m++)
                    {
                        compatible = ArePair(primes[members[m]], primes[i]);
                    }

                    if (compatible)
                    {
                        members[depth] = i;
                        Extend(depth + 1, sum + primes[i]);
                    }
                }
            }
        }

        private bool ArePair(long a, long b)
        {
            var key = a * 1_000_000 + b; // Both primes are far below a million.
            if (!compatibility.TryGetValue(key, out var result))
            {
                result = ConcatenateToPrimes(a, b);
                compatibility[key] = result;
            }

            return result;
        }

        private static bool ConcatenateToPrimes(long a, long b)
        {
            // Apart from 3 itself, two members must share a residue mod 3, or one concatenation is divisible by 3.
            if (a != 3 && b != 3 && a % 3 != b % 3)
            {
                return false;
            }

            return Primes.IsPrime(Concatenate(a, b)) && Primes.IsPrime(Concatenate(b, a));
        }

        private static long Concatenate(long a, long b)
        {
            var shifted = a;
            for (var m = b; m > 0; m /= 10)
            {
                shifted *= 10;
            }

            return shifted + b;
        }
    }
}
