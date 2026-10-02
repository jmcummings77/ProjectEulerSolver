using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first triangle number with over five hundred divisors.</summary>
public sealed class Problem012 : Problem
{
    public override int Number => 12;

    public override string Title => "Highly Divisible Triangular Number";

    public override object Solve()
    {
        // T(n) = n(n+1)/2 and n, n+1 are coprime, so d(T(n)) is the product of the divisor
        // counts of the two halves (after taking the factor 2 out of the even one).
        for (long n = 1; ; n++)
        {
            var (a, b) = n % 2 == 0 ? (n / 2, n + 1) : (n, (n + 1) / 2);
            if (Primes.DivisorCount(a) * Primes.DivisorCount(b) > 500)
            {
                return a * b;
            }
        }
    }
}
