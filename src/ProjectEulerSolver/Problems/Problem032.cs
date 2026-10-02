using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all products whose multiplicand/multiplier/product identity is 1 through 9 pandigital.</summary>
public sealed class Problem032 : Problem
{
    public override int Number => 32;

    public override string Title => "Pandigital Products";

    public override object Solve()
    {
        // Nine digits in total means either 1×4 or 2×3 digit factors, so the multiplicand is below 100
        // and the product below 10,000.
        var products = new HashSet<int>();
        for (var a = 1; a < 100; a++)
        {
            for (var b = a + 1; a * b < 10_000; b++)
            {
                var product = a * b;
                var identity = $"{a}{b}{product}";
                if (identity.Length == 9 && Digits.IsPandigital(long.Parse(identity), 9))
                {
                    products.Add(product);
                }
            }
        }

        return products.Sum();
    }
}
