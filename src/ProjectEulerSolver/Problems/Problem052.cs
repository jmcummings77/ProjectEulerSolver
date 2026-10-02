using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest positive integer x such that 2x, 3x, 4x, 5x and 6x contain the same digits.</summary>
public sealed class Problem052 : Problem
{
    public override int Number => 52;

    public override string Title => "Permuted Multiples";

    public override object Solve()
    {
        for (long x = 1; ; x++)
        {
            if (Digits.Count(6 * x) != Digits.Count(x))
            {
                // 6x has grown an extra digit: jump x to the next power of ten.
                x = (long)Math.Pow(10, Digits.Count(x)) - 1;
                continue;
            }

            if (Enumerable.Range(2, 5).All(m => Digits.ArePermutations(x, m * x)))
            {
                return x;
            }
        }
    }
}
