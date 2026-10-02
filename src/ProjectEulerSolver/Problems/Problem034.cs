using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all numbers equal to the sum of the factorials of their digits.</summary>
public sealed class Problem034 : Problem
{
    public override int Number => 34;

    public override string Title => "Digit Factorials";

    public override object Solve()
    {
        // 7 * 9! = 2,540,160 has seven digits, but 8 * 9! has only seven too, so no 8-digit number qualifies.
        var factorials = Enumerable.Range(0, 10).Select(d => (int)NumberTheory.Factorial(d)).ToArray();
        var limit = 7 * factorials[9];

        var total = 0;
        for (var n = 10; n <= limit; n++)
        {
            if (Digits.Of(n).Sum(d => factorials[d]) == n)
            {
                total += n;
            }
        }

        return total;
    }
}
