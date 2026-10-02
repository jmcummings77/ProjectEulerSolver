using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The side length of the square spiral for which the ratio of primes along both diagonals first falls below 10%.</summary>
public sealed class Problem058 : Problem
{
    public override int Number => 58;

    public override string Title => "Spiral Primes";

    public override object Solve()
    {
        long corner = 1;
        var diagonalCount = 1;
        var primeCount = 0;
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

            diagonalCount += 4;
            if (primeCount * 10 < diagonalCount)
            {
                return side;
            }
        }
    }
}
