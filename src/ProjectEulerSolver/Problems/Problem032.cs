using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all products whose multiplicand/multiplier/product identity is 1 through 9 pandigital.</summary>
public sealed class Problem032 : Problem
{
    public override int Number => 32;

    public override string Title => "Pandigital Products";

    public override object Solve() => Solve(digits: 9);

    /// <summary>
    /// The sum of all products c for which some identity a × b = c, written out as the digits of a, b and c,
    /// uses each of the digits 1 to <paramref name="digits"/> exactly once. A product that can be reached
    /// in more than one way is counted once.
    /// </summary>
    /// <param name="digits">How many digits the identity has in total, from 4 to 9.</param>
    public static int Solve(int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 4);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, 9);

        // Distinct products below 10^4, so the sum stays far inside an int.
        return PandigitalProducts(digits).Sum();
    }

    /// <summary>The distinct products of the identities that are 1 to <paramref name="digits"/> pandigital (4 to 9).</summary>
    internal static IReadOnlySet<int> PandigitalProducts(int digits)
    {
        // An x-digit number times a y-digit number has x + y - 1 or x + y digits, so the identity has
        // 2(x + y) - 1 or 2(x + y) digits. Whichever parity `digits` has, that forces x + y = ceil(digits / 2)
        // and leaves floor(digits / 2) digits for the product. Taking a < b (equal factors would repeat a
        // digit), a is the shorter factor and has at most (x + y) / 2 digits.
        var factorDigits = (digits + 1) / 2;
        var multiplicandLimit = (int)NumberTheory.PowerOfTen(factorDigits / 2);
        var productLimit = (int)NumberTheory.PowerOfTen(digits / 2);

        var products = new HashSet<int>();
        for (var a = 1; a < multiplicandLimit; a++)
        {
            for (var b = a + 1; a * b < productLimit; b++)
            {
                var product = a * b;

                // The pandigital test also rejects identities of the wrong total length.
                if (Digits.IsPandigital(Digits.Concatenate(Digits.Concatenate(a, b), product), digits))
                {
                    products.Add(product);
                }
            }
        }

        return products;
    }
}
