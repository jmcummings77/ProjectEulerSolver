using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The first value that can be written as the sum of primes in over five thousand different ways.</summary>
public sealed class Problem077 : Problem
{
    public override int Number => 77;

    public override string Title => "Prime Summations";

    public override object Solve()
    {
        var primes = Primes.UpTo(100);
        for (var n = 2; ; n++)
        {
            if (Combinatorics.CountPartitions(n, primes.Where(p => p <= n)) > 5000)
            {
                return n;
            }
        }
    }
}
