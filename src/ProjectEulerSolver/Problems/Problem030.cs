using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all numbers that can be written as the sum of fifth powers of their digits.</summary>
public sealed class Problem030 : Problem
{
    public override int Number => 30;

    public override string Title => "Digit Fifth Powers";

    public override object Solve()
    {
        // A 7-digit number is at most 7 * 9^5 = 413,343, which has only 6 digits, so 6 digits is the ceiling.
        var limit = 6 * (int)Math.Pow(9, 5);
        var fifthPowers = Enumerable.Range(0, 10).Select(d => (int)Math.Pow(d, 5)).ToArray();

        var total = 0;
        for (var n = 10; n <= limit; n++)
        {
            if (Digits.Of(n).Sum(d => fifthPowers[d]) == n)
            {
                total += n;
            }
        }

        return total;
    }
}
