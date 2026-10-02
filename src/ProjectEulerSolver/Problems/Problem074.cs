using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many digit factorial chains with a starting number below one million contain exactly sixty non-repeating terms.</summary>
public sealed class Problem074 : Problem
{
    public override int Number => 74;

    public override string Title => "Digit Factorial Chains";

    public override object Solve() => Solve(limit: 1_000_000, chainLength: 60);

    /// <summary>
    /// How many positive starting numbers below <paramref name="limit"/> have a digit factorial chain with exactly
    /// <paramref name="chainLength"/> non-repeating terms.
    /// </summary>
    /// <param name="limit">Exclusive upper bound on the starting number: from 1 to <see cref="int.MaxValue"/>. The running time is proportional to it.</param>
    /// <param name="chainLength">The number of non-repeating terms wanted: 1 or more. A length that no chain has simply gives zero.</param>
    public static int Solve(int limit, int chainLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(chainLength, 1);

        var chains = new DigitFactorialChains();
        var count = 0;
        for (var start = 1; start < limit; start++)
        {
            if (chains.Length(start) == chainLength)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>Lengths of "sum of the factorials of the digits" chains, cached across calls.</summary>
    internal sealed class DigitFactorialChains
    {
        private static readonly int[] Factorials = Enumerable.Range(0, 10).Select(d => (int)NumberTheory.Factorial(d)).ToArray();

        // An int has at most ten digits, so every term after the first is at most 10 × 9! = 3,628,800 and a flat
        // array indexed by term covers them all.
        private readonly int[] lengths = new int[10 * Factorials[9] + 1];

        // The terms walked so far in the current call; kept between calls so that most of them allocate nothing.
        private readonly List<int> path = [];

        /// <summary>Number of non-repeating terms in the chain that starts at <paramref name="start"/> (1 or more).</summary>
        public int Length(int start)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(start, 1);
            if (start >= lengths.Length)
            {
                // Too large to be the image of any int, so it is not part of a loop and never comes back:
                // its chain is itself followed by the chain of its image.
                return 1 + Length(Next(start));
            }

            path.Clear();
            var current = start;
            while (lengths[current] == 0)
            {
                var loopIndex = path.IndexOf(current);
                if (loopIndex >= 0)
                {
                    // A new loop: every term in it has the loop's length. Drop it from the path so the terms
                    // that led into it are measured as their distance to the loop plus the loop length.
                    var loopLength = path.Count - loopIndex;
                    for (var i = loopIndex; i < path.Count; i++)
                    {
                        lengths[path[i]] = loopLength;
                    }

                    path.RemoveRange(loopIndex, loopLength);
                    break;
                }

                path.Add(current);
                current = Next(current);
            }

            var length = lengths[current];
            for (var i = path.Count - 1; i >= 0; i--)
            {
                lengths[path[i]] = ++length;
            }

            return lengths[start];
        }

        /// <summary>The next term: the sum of the factorials of the digits.</summary>
        public static int Next(int n)
        {
            var sum = 0;
            for (var m = n; m > 0; m /= 10)
            {
                sum += Factorials[m % 10];
            }

            return sum;
        }
    }
}
