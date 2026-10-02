using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The greatest product of thirteen adjacent digits in a given 1000-digit number.</summary>
public sealed class Problem008 : Problem
{
    private const int WindowLength = 13;

    public override int Number => 8;

    public override string Title => "Largest Product in a Series";

    public override object Solve()
    {
        var digits = string.Concat(Resources.ReadLines("0008_number.txt")).Select(c => c - '0').ToArray();

        long best = 0;
        for (var start = 0; start + WindowLength <= digits.Length; start++)
        {
            long product = 1;
            for (var i = start; i < start + WindowLength; i++)
            {
                product *= digits[i];
            }

            best = Math.Max(best, product);
        }

        return best;
    }
}
