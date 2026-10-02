using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest odd composite that cannot be written as the sum of a prime and twice a square.</summary>
public sealed class Problem046 : Problem
{
    public override int Number => 46;

    public override string Title => "Goldbach's Other Conjecture";

    public override object Solve()
    {
        for (var n = 9; ; n += 2)
        {
            if (Primes.IsPrime(n))
            {
                continue;
            }

            var decomposable = false;
            for (var k = 1; 2 * k * k < n; k++)
            {
                if (Primes.IsPrime(n - 2 * k * k))
                {
                    decomposable = true;
                    break;
                }
            }

            if (!decomposable)
            {
                return n;
            }
        }
    }
}
