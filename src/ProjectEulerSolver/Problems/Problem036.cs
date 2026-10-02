using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all numbers below one million that are palindromic in base 10 and base 2.</summary>
public sealed class Problem036 : Problem
{
    public override int Number => 36;

    public override string Title => "Double-base Palindromes";

    public override object Solve()
    {
        var total = 0;
        for (var n = 1; n < 1_000_000; n += 2) // Even numbers end in 0 in binary, so they cannot be palindromes.
        {
            if (Digits.IsPalindrome(n) && Digits.IsPalindrome(Convert.ToString(n, 2)))
            {
                total += n;
            }
        }

        return total;
    }
}
