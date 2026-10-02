using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The side length of the square spiral for which the ratio of primes along both diagonals first falls below 10%.</summary>
public sealed class Problem058 : Problem
{
    public override int Number => 58;

    public override string Title => "Spiral Primes";

    public override object Solve() => Solve(percent: 10);

    /// <summary>
    /// The side length of the first square spiral, growing from side 3 one layer at a time, in which fewer than
    /// <paramref name="percent"/> per cent of the numbers on the two diagonals are prime.
    /// </summary>
    /// <param name="percent">
    /// The ratio to fall below, as a whole percentage: from 6 to 100. The spiral needed grows ever faster as the
    /// percentage falls: side 26,241 for 10%, 1,213,001 for 7% and 10,273,343 (some fifteen million primality
    /// tests, about half a minute) for 6%. Smaller values are not accepted because 5% is not reached until side
    /// 204,066,341, twenty times the work again. From 61% up the answer is 3, whose diagonals are 60% prime.
    /// </param>
    public static int Solve(int percent)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(percent, 6);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(percent, 100);

        // A spiral that is below a given percentage is also below every larger one, so the answer never grows as
        // percent rises: it is at most the 10,273,343 found for 6%. The side therefore fits an int with room to
        // spare, and the largest corner, side², stays below 1.1 × 10^14, inside a long.
        long corner = 1;
        long primeCount = 0;
        for (var side = 3; ; side += 2)
        {
            // The bottom-right corner is side², which is never prime; test the other three.
            for (var i = 0; i < 4; i++)
            {
                corner += side - 1;
                if (i < 3 && Primes.IsPrime(corner))
                {
                    primeCount++;
                }
            }

            // The diagonals of a spiral of this side hold 2 × side - 1 numbers.
            if (primeCount * 100 < percent * (2L * side - 1))
            {
                return side;
            }
        }
    }
}
