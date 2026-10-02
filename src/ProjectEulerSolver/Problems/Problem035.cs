using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many circular primes there are below one million.</summary>
public sealed class Problem035 : Problem
{
    private static readonly int[] RotatableDigits = [1, 3, 7, 9];

    public override int Number => 35;

    public override string Title => "Circular Primes";

    public override object Solve() => Solve(limit: 1_000_000);

    /// <summary>
    /// How many circular primes there are below <paramref name="limit"/>: primes for which every rotation
    /// of the digits is also prime. The rotations themselves may be at or above the limit.
    /// </summary>
    /// <param name="limit">Exclusive upper bound, from 0 to 2,147,483,647 (any non-negative int).</param>
    public static int Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);

        // Every digit of a number ends one of its rotations, and a prime with two or more digits ends in
        // 1, 3, 7 or 9. So apart from 2 and 5 only numbers written with those four digits can qualify: there
        // are 4^k of them with k digits, few enough to test each one directly instead of sieving up to the
        // limit. None contains a zero, so every rotation keeps its full length.
        var count = (limit > 2 ? 1 : 0) + (limit > 5 ? 1 : 0);
        return count + CountExtensions(prefix: 0, leadingPlace: 1, limit: limit);
    }

    /// <summary>
    /// Counts the circular primes below <paramref name="limit"/> that consist of <paramref name="prefix"/>
    /// followed by one or more of the digits 1, 3, 7, 9. <paramref name="leadingPlace"/> is the place value
    /// of the first digit once one digit has been appended.
    /// </summary>
    private static int CountExtensions(long prefix, long leadingPlace, int limit)
    {
        var count = 0;
        foreach (var digit in RotatableDigits)
        {
            var candidate = prefix * 10 + digit;
            if (candidate >= limit)
            {
                break; // The digits ascend, and anything longer is larger still.
            }

            if (IsCircularPrime(candidate, leadingPlace))
            {
                count++;
            }

            count += CountExtensions(candidate, leadingPlace * 10, limit);
        }

        return count;
    }

    private static bool IsCircularPrime(long n, long leadingPlace)
    {
        var rotation = n;
        do
        {
            if (!Primes.IsPrime(rotation))
            {
                return false;
            }

            // Move the leading digit to the end.
            rotation = rotation % leadingPlace * 10 + rotation / leadingPlace;
        }
        while (rotation != n);

        return true;
    }
}
