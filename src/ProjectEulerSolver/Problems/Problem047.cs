using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first of four consecutive integers that each have four distinct prime factors.</summary>
public sealed class Problem047 : Problem
{
    private const int Limit = 200_000;

    public override int Number => 47;

    public override string Title => "Distinct Primes Factors";

    public override object Solve()
    {
        var factorCounts = Primes.DistinctPrimeFactorCounts(Limit);
        var run = 0;
        for (var n = 2; n <= Limit; n++)
        {
            run = factorCounts[n] == 4 ? run + 1 : 0;
            if (run == 4)
            {
                return n - 3;
            }
        }

        throw new InvalidOperationException($"No run of four found below {Limit}.");
    }
}
