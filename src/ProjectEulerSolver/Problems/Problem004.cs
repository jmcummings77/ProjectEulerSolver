using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The largest palindrome made from the product of two 3-digit numbers.</summary>
public sealed class Problem004 : Problem
{
    public override int Number => 4;

    public override string Title => "Largest Palindrome Product";

    public override object Solve()
    {
        long largest = 0;
        for (var a = 999; a >= 100; a--)
        {
            if ((long)a * 999 <= largest)
            {
                break; // No product in this row can beat the best so far.
            }

            for (var b = a; b >= 100; b--)
            {
                var product = (long)a * b;
                if (product <= largest)
                {
                    break;
                }

                if (Digits.IsPalindrome(product))
                {
                    largest = product;
                }
            }
        }

        return largest;
    }
}
