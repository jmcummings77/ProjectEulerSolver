using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The 10001st prime number.</summary>
public sealed class Problem007 : Problem
{
    public override int Number => 7;

    public override string Title => "10001st Prime";

    public override object Solve()
    {
        // p_n < n (ln n + ln ln n) for n >= 6, which comfortably bounds the 10001st prime by 120,000.
        var primes = Primes.UpTo(120_000);
        return primes[10_000];
    }
}
