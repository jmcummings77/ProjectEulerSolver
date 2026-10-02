using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest prime which, by replacing some of its digits with the same digit, is part of an eight-prime family.</summary>
public sealed class Problem051 : Problem
{
    public override int Number => 51;

    public override string Title => "Prime Digit Replacements";

    public override object Solve() => Solve(familySize: 8);

    /// <summary>
    /// The smallest prime that belongs to a family of exactly <paramref name="familySize"/> primes. A family is made
    /// by choosing some, but not all, of a number's digit positions and writing each digit 0 to 9 in turn into all of
    /// them at once; a leading zero is not allowed, and the family's members are the primes among the results.
    /// </summary>
    /// <param name="familySize">How many of the generated numbers are prime: from 2 to 8.</param>
    public static int Solve(int familySize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(familySize, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(familySize, 8);

        // The answer is the smallest member of its own family (a smaller member would be a smaller answer), so the
        // other familySize - 1 members come from larger digits: the digit it has in the replaced positions is at
        // most 10 - familySize. Scanning primes in ascending order with only those digits therefore finds it first.
        var maxReplaced = 10 - familySize;

        // Replacement keeps the number of digits, so a sieve of every number with the current digit count covers
        // every family member. The count grows until a family turns up; all seven supported sizes are settled by
        // six digits, and the nine-digit stop only keeps the sieve within an int.
        for (var low = 10; low <= 100_000_000; low *= 10)
        {
            var isPrime = Primes.Sieve(low * 10 - 1);
            for (var prime = low + 1; prime < low * 10; prime += 2)
            {
                if (isPrime[prime] && HasFamily(prime, familySize, maxReplaced, isPrime))
                {
                    return prime;
                }
            }
        }

        throw new InvalidOperationException($"No family of exactly {familySize} primes among the primes below 1,000,000,000.");
    }

    private static bool HasFamily(int prime, int familySize, int maxReplaced, bool[] isPrime)
    {
        var digits = Digits.Of(prime);
        for (var replaced = 0; replaced <= maxReplaced; replaced++)
        {
            var positions = Enumerable.Range(0, digits.Length).Where(i => digits[i] == replaced).ToArray();

            // Every non-empty choice of those positions, except all the digits of the number at once.
            var choices = (1 << positions.Length) - (positions.Length == digits.Length ? 1 : 0);
            for (var mask = 1; mask < choices; mask++)
            {
                if (CountFamilyPrimes(digits, positions, mask, isPrime) == familySize)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static int CountFamilyPrimes(int[] digits, int[] positions, int mask, bool[] isPrime)
    {
        var count = 0;
        var candidate = (int[])digits.Clone();
        for (var digit = 0; digit <= 9; digit++)
        {
            for (var i = 0; i < positions.Length; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    candidate[positions[i]] = digit;
                }
            }

            if (candidate[0] == 0)
            {
                continue; // Leading zero would shorten the number.
            }

            if (isPrime[(int)Digits.FromDigits(candidate)])
            {
                count++;
            }
        }

        return count;
    }
}
