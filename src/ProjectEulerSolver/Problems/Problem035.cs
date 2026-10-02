using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many circular primes there are below one million.</summary>
public sealed class Problem035 : Problem
{
    private const int Limit = 1_000_000;

    public override int Number => 35;

    public override string Title => "Circular Primes";

    public override object Solve()
    {
        var isPrime = Primes.Sieve(Limit);
        var count = 0;
        for (var n = 2; n < Limit; n++)
        {
            if (isPrime[n] && Rotations(n).All(r => isPrime[r]))
            {
                count++;
            }
        }

        return count;
    }

    private static IEnumerable<int> Rotations(int n)
    {
        var digits = Digits.Of(n);
        for (var shift = 1; shift < digits.Length; shift++)
        {
            yield return (int)Digits.FromDigits(digits.Skip(shift).Concat(digits.Take(shift)));
        }
    }
}
