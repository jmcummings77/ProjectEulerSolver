using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The lowest sum for a set of five primes for which any two concatenate to produce another prime.</summary>
public sealed class Problem060 : Problem
{
    private const int Limit = 10_000;
    private const int SetSize = 5;

    public override int Number => 60;

    public override string Title => "Prime Pair Sets";

    public override object Solve()
    {
        // 2 and 5 can never be a member: a concatenation ending in them is even or a multiple of 5.
        var primes = Primes.UpTo(Limit).Where(p => p != 2 && p != 5).ToArray();

        // Build the compatibility graph, then search for a 5-clique with the smallest total.
        var neighbours = new List<int>[primes.Length];
        for (var i = 0; i < primes.Length; i++)
        {
            neighbours[i] = [];
        }

        for (var i = 0; i < primes.Length; i++)
        {
            for (var j = i + 1; j < primes.Length; j++)
            {
                if (ArePair(primes[i], primes[j]))
                {
                    neighbours[i].Add(j);
                }
            }
        }

        var best = long.MaxValue;
        var clique = new int[SetSize];
        for (var i = 0; i < primes.Length; i++)
        {
            clique[0] = i;
            Extend(1, neighbours[i], primes[i]);
        }

        return best;

        void Extend(int depth, List<int> candidates, long sum)
        {
            if (sum >= best)
            {
                return;
            }

            if (depth == SetSize)
            {
                best = sum;
                return;
            }

            foreach (var candidate in candidates)
            {
                // Only keep candidates adjacent to every member chosen so far.
                var next = candidates.Where(c => c > candidate && neighbours[candidate].BinarySearch(c) >= 0).ToList();
                clique[depth] = candidate;
                Extend(depth + 1, next, sum + primes[candidate]);
            }
        }
    }

    private static bool ArePair(long a, long b)
    {
        // Apart from 3 itself, two members must share a residue mod 3 or one concatenation is divisible by 3.
        if (a != 3 && b != 3 && a % 3 != b % 3)
        {
            return false;
        }

        return Primes.IsPrime(Concatenate(a, b)) && Primes.IsPrime(Concatenate(b, a));
    }

    private static long Concatenate(long a, long b) => a * (long)Math.Pow(10, Digits.Count(b)) + b;
}
