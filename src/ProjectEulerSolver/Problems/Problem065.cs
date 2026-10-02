using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the digits in the numerator of the 100th convergent of the continued fraction for e.</summary>
public sealed class Problem065 : Problem
{
    public override int Number => 65;

    public override string Title => "Convergents of e";

    public override object Solve() => Solve(n: 100);

    /// <summary>The sum of the digits in the numerator of the <paramref name="n"/>-th convergent of the continued fraction for e.</summary>
    /// <param name="n">
    /// Which convergent, counting 2 as the first: from 1 to 100,000. The arithmetic is exact at any size; the cap
    /// only bounds the running time, which grows a little faster than n² because the numerator's length grows a
    /// little faster than n.
    /// </param>
    public static int Solve(int n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, 100_000);

        // e = [2; 1, 2, 1, 1, 4, 1, 1, 6, 1, ...]: term i is 2(i + 1)/3 when i ≡ 2 (mod 3) and 1 otherwise.
        // Numerators follow h(i) = a(i)·h(i − 1) + h(i − 2), starting from h(−1) = 1 and h(0) = 2.
        BigInteger previous = 1;
        BigInteger numerator = 2;
        for (var i = 1; i < n; i++)
        {
            var term = i % 3 == 2 ? 2 * ((i + 1) / 3) : 1;
            (previous, numerator) = (numerator, term * numerator + previous);
        }

        return Digits.Sum(numerator);
    }
}
