using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many digit factorial chains with a starting number below one million contain exactly sixty non-repeating terms.</summary>
public sealed class Problem074 : Problem
{
    private const int Limit = 1_000_000;

    public override int Number => 74;

    public override string Title => "Digit Factorial Chains";

    public override object Solve()
    {
        var chains = new DigitFactorialChains();
        var count = 0;
        for (var start = 1; start < Limit; start++)
        {
            if (chains.Length(start) == 60)
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

        // 7 × 9! = 2,540,160 bounds every term after the first for starts below ten million, so a flat array works.
        private readonly int[] lengths = new int[7 * Factorials[9] + 1];

        /// <summary>Number of non-repeating terms in the chain that starts at <paramref name="start"/>.</summary>
        public int Length(int start)
        {
            var path = new List<int>();
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
