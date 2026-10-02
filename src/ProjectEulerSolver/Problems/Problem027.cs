using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>Product of the coefficients a, b (|a| &lt; 1000, |b| ≤ 1000) whose quadratic n² + an + b yields the most consecutive primes from n = 0.</summary>
public sealed class Problem027 : Problem
{
    public override int Number => 27;

    public override string Title => "Quadratic Primes";

    public override object Solve()
    {
        var bestProduct = 0;
        var bestRun = 0;

        // n = 0 gives b itself, so b must be prime (and positive).
        foreach (var b in Primes.UpTo(1000))
        {
            for (var a = -999; a < 1000; a++)
            {
                var n = 0;
                while (Primes.IsPrime((long)n * n + (long)a * n + b))
                {
                    n++;
                }

                if (n > bestRun)
                {
                    bestRun = n;
                    bestProduct = a * b;
                }
            }
        }

        return bestProduct;
    }
}
