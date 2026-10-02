using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The denominator of the product of the four non-trivial digit-cancelling fractions, in lowest terms.</summary>
public sealed class Problem033 : Problem
{
    public override int Number => 33;

    public override string Title => "Digit Cancelling Fractions";

    public override object Solve()
    {
        long numeratorProduct = 1;
        long denominatorProduct = 1;

        // Numerator = 10a + b, denominator = 10b + c; "cancelling" the shared b leaves a/c. Any other
        // placement of the shared digit only yields trivial fractions (or none below one).
        for (var a = 1; a <= 9; a++)
        {
            for (var b = 1; b <= 9; b++)
            {
                for (var c = 1; c <= 9; c++)
                {
                    var numerator = 10 * a + b;
                    var denominator = 10 * b + c;
                    if (numerator < denominator && numerator * c == a * denominator)
                    {
                        numeratorProduct *= numerator;
                        denominatorProduct *= denominator;
                    }
                }
            }
        }

        return denominatorProduct / NumberTheory.Gcd(numeratorProduct, denominatorProduct);
    }
}
