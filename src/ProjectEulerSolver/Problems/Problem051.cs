using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest prime which, by replacing some of its digits with the same digit, is part of an eight-prime family.</summary>
public sealed class Problem051 : Problem
{
    private const int Limit = 1_000_000;

    public override int Number => 51;

    public override string Title => "Prime Digit Replacements";

    public override object Solve()
    {
        var isPrime = Primes.Sieve(Limit);
        for (var prime = 11; prime < Limit; prime += 2)
        {
            if (!isPrime[prime])
            {
                continue;
            }

            var digits = Digits.Of(prime);

            // An eight-member family needs eight of the ten replacement digits to work, so the digit
            // being replaced must be 0, 1 or 2 (otherwise there are fewer than eight larger choices).
            for (var replaced = 0; replaced <= 2; replaced++)
            {
                var positions = Enumerable.Range(0, digits.Length).Where(i => digits[i] == replaced).ToArray();
                for (var mask = 1; mask < 1 << positions.Length; mask++)
                {
                    var family = FamilyPrimes(digits, positions, mask, isPrime);
                    if (family.Count == 8)
                    {
                        return family.Min();
                    }
                }
            }
        }

        throw new InvalidOperationException($"No eight-prime family below {Limit}.");
    }

    private static List<int> FamilyPrimes(int[] digits, int[] positions, int mask, bool[] isPrime)
    {
        var family = new List<int>();
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

            var value = (int)Digits.FromDigits(candidate);
            if (isPrime[value])
            {
                family.Add(value);
            }
        }

        return family;
    }
}
