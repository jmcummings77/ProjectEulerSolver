using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of all numbers that can be written as the sum of fifth powers of their digits.</summary>
public sealed class Problem030 : Problem
{
    public override int Number => 30;

    public override string Title => "Digit Fifth Powers";

    public override object Solve() => Solve(power: 5);

    /// <summary>
    /// The sum of all numbers with at least two digits that equal the sum of the <paramref name="power"/>-th powers
    /// of their own decimal digits. Zero when there are none.
    /// </summary>
    /// <param name="power">The exponent applied to each digit, from 1 to 18 (so that every digit-power sum fits in a long).</param>
    public static BigInteger Solve(int power)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(power, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(power, 18);

        var digitPowers = new long[10];
        for (var digit = 1; digit <= 9; digit++)
        {
            digitPowers[digit] = (long)BigInteger.Pow(digit, power);
        }

        // A number with d digits is at least 10^(d - 1), while its digit powers add up to at most d·9^power, so d
        // digits are only possible while 10^(d - 1) ≤ d·9^power. Once that fails it fails for every longer number
        // too: 10^(d - 1) > d·9^power gives 10^d > 10d·9^power ≥ (d + 1)·9^power. For power ≤ 18 the largest
        // possible sum, maxDigits·9^power, is at most 19·9^18 < 2.9 × 10^18 and fits in a long.
        var maxDigits = 1;
        while (BigInteger.Pow(10, maxDigits) <= (maxDigits + 1) * (BigInteger)digitPowers[9])
        {
            maxDigits++;
        }

        // Zeros add nothing, so a number's digit-power sum is fixed by how many of each non-zero digit it has.
        // Try every such multiset of up to maxDigits digits: it yields a solution exactly when its sum has those
        // same non-zero digits. Each solution is found once, from the multiset of its own digits.
        var counts = new int[10];
        var sumCounts = new int[10];
        BigInteger total = 0;
        Search(digit: 9, slots: maxDigits, sum: 0);
        return total;

        void Search(int digit, int slots, long sum)
        {
            if (digit == 0)
            {
                if (sum >= 10 && HasChosenDigits(sum))
                {
                    total += sum;
                }

                return;
            }

            for (var count = 0; count <= slots; count++)
            {
                counts[digit] = count;
                Search(digit - 1, slots - count, sum + count * digitPowers[digit]);
            }
        }

        bool HasChosenDigits(long sum)
        {
            Array.Clear(sumCounts);
            for (var rest = sum; rest > 0; rest /= 10)
            {
                sumCounts[rest % 10]++;
            }

            for (var digit = 1; digit <= 9; digit++)
            {
                if (sumCounts[digit] != counts[digit])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
