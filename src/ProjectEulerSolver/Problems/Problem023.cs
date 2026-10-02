using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all positive integers that cannot be written as the sum of two abundant numbers.</summary>
public sealed class Problem023 : Problem
{
    // Every integer above this bound is the sum of two abundant numbers.
    private const int Limit = 28_123;

    public override int Number => 23;

    public override string Title => "Non-Abundant Sums";

    public override object Solve()
    {
        var divisorSums = NumberTheory.ProperDivisorSums(Limit);
        var abundant = Enumerable.Range(12, Limit - 11).Where(n => divisorSums[n] > n).ToArray();

        var expressible = new bool[Limit + 1];
        for (var i = 0; i < abundant.Length; i++)
        {
            for (var j = i; j < abundant.Length; j++)
            {
                var sum = abundant[i] + abundant[j];
                if (sum > Limit)
                {
                    break;
                }

                expressible[sum] = true;
            }
        }

        long total = 0;
        for (var n = 1; n <= Limit; n++)
        {
            if (!expressible[n])
            {
                total += n;
            }
        }

        return total;
    }
}
