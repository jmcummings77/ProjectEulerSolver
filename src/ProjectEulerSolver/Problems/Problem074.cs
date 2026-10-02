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
        var factorials = Enumerable.Range(0, 10).Select(d => (int)NumberTheory.Factorial(d)).ToArray();

        // Chain lengths are cached for every value seen; 9! * 7 = 2,540,160 bounds every term after the first.
        var lengths = new int[7 * factorials[9] + 1];
        var count = 0;
        for (var start = 1; start < Limit; start++)
        {
            if (ChainLength(start) == 60)
            {
                count++;
            }
        }

        return count;

        int ChainLength(int start)
        {
            var path = new List<int>();
            var current = start;
            while (true)
            {
                if (lengths[current] != 0)
                {
                    break; // Reached a value whose chain length is already known.
                }

                var loopIndex = path.IndexOf(current);
                if (loopIndex >= 0)
                {
                    // Everything in the loop has the loop's length; earlier terms count their distance to it.
                    var loopLength = path.Count - loopIndex;
                    for (var i = loopIndex; i < path.Count; i++)
                    {
                        lengths[path[i]] = loopLength;
                    }

                    break;
                }

                path.Add(current);
                current = Digits.Of(current).Sum(d => factorials[d]);
            }

            var tail = lengths[current];
            for (var i = path.Count - 1; i >= 0 && lengths[path[i]] == 0; i--)
            {
                lengths[path[i]] = ++tail;
            }

            return lengths[start];
        }
    }
}
