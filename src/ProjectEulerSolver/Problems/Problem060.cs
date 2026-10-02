using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The lowest sum for a set of five primes for which any two concatenate to produce another prime.</summary>
public sealed class Problem060 : Problem
{
    // No prime handed to the pair graph reaches this, so two of them concatenate to less than 10^16, well inside a long.
    private const int MaxMember = 100_000_000;

    // The first search stops widening here. Its candidate is then a sum of at most five primes below 10^7, which
    // keeps the second search, whose members are all smaller than the candidate, below MaxMember as well.
    private const int MaxFirstSearch = MaxMember / 10;

    // 2 and 5 can never be members (a concatenation ending in them is even or a multiple of 5), so these are the
    // four smallest primes that could accompany the largest member of a set of up to five.
    private static readonly int[] SmallestMembers = [3, 7, 11, 13];

    public override int Number => 60;

    public override string Title => "Prime Pair Sets";

    public override object Solve() => Solve(setSize: 5);

    /// <summary>
    /// The lowest sum of a set of <paramref name="setSize"/> different primes in which every two members, written
    /// one after the other in either order, form a prime.
    /// </summary>
    /// <param name="setSize">How many primes the set holds: from 2 to 5.</param>
    public static int Solve(int setSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(setSize, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(setSize, 5);

        // First pass: any qualifying set gives an upper bound on the answer, so widen the universe of primes
        // tenfold until one turns up (below 10,000 for every supported size; MaxFirstSearch only guards the
        // arithmetic). Pair results are shared between all the searches.
        var pairs = new PrimePairGraph(setSize);
        var candidate = PrimePairGraph.None;
        for (var limit = 100; candidate == PrimePairGraph.None; limit *= 10)
        {
            if (limit > MaxFirstSearch)
            {
                throw new InvalidOperationException($"No set of {setSize} such primes below {MaxFirstSearch:N0}.");
            }

            candidate = pairs.LowestSum(Primes.UpTo(limit), PrimePairGraph.None);
        }

        // Second pass: prove nothing smaller exists. In a set with a smaller sum the other setSize - 1 members add
        // up to at least the sum of the smallest possible ones, so every member is at most candidate - 1 - that sum.
        // A search of that whole universe, pruned by the candidate, is therefore exhaustive. The candidate is a
        // sum of at most five members below MaxFirstSearch, so it fits an int and is itself below MaxMember.
        var smallestOthers = SmallestMembers.Take(setSize - 1).Sum();
        return (int)pairs.LowestSum(Primes.UpTo((int)candidate - 1 - smallestOthers), candidate);
    }

    /// <summary>
    /// The "concatenate both ways to primes" relation, computed lazily and memoised, with a pruned depth-first
    /// search for the clique of the wanted size with the smallest sum.
    /// </summary>
    private sealed class PrimePairGraph(int setSize)
    {
        /// <summary>The bound to search under when no qualifying set is known yet.</summary>
        public const long None = long.MaxValue;

        private readonly Dictionary<long, bool> compatibility = [];

        /// <summary>The smallest sum of a qualifying set drawn from <paramref name="allPrimes"/>, or <paramref name="bound"/> when none beats it.</summary>
        public long LowestSum(int[] allPrimes, long bound)
        {
            // 2 and 5 can never be members: a concatenation ending in them is even or a multiple of 5.
            var primes = allPrimes.Where(p => p != 2 && p != 5).ToArray();
            var members = new int[setSize];
            var best = bound;

            Extend(0, 0);
            return best;

            void Extend(int depth, long sum)
            {
                if (depth == setSize)
                {
                    best = sum;
                    return;
                }

                // Members are chosen in ascending order, so the remaining ones are each at least primes[i].
                var remaining = setSize - depth;
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

        private bool ArePair(int a, int b)
        {
            var key = (long)a * MaxMember + b; // Unique, and below 10^16, because both primes are below MaxMember.
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

            return Primes.IsPrime(Digits.Concatenate(a, b)) && Primes.IsPrime(Digits.Concatenate(b, a));
        }
    }
}
