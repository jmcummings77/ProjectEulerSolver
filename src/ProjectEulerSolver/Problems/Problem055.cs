using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many Lychrel numbers there are below ten thousand.</summary>
public sealed class Problem055 : Problem
{
    public override int Number => 55;

    public override string Title => "Lychrel Numbers";

    public override object Solve() => Enumerable.Range(1, 9999).Count(IsLychrel);

    private static bool IsLychrel(int start)
    {
        BigInteger value = start;
        for (var iteration = 0; iteration < 50; iteration++)
        {
            value += Digits.Reverse(value);
            if (Digits.IsPalindrome(value))
            {
                return false;
            }
        }

        return true;
    }
}
