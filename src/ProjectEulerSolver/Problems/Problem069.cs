using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The n ≤ 1,000,000 for which n/φ(n) is a maximum.</summary>
public sealed class Problem069 : Problem
{
    public override int Number => 69;

    public override string Title => "Totient Maximum";

    public override object Solve()
    {
        // n/φ(n) = Π p/(p − 1) over the distinct primes of n, so it is maximised by multiplying
        // the smallest primes together for as long as the product stays within the limit.
        long product = 1;
        foreach (var p in Primes.UpTo(100))
        {
            if (product * p > 1_000_000)
            {
                break;
            }

            product *= p;
        }

        return product;
    }
}
