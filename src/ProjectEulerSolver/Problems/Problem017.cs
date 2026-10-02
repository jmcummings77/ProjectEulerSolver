using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>Total letters used when the numbers 1 to 1000 are written out in words.</summary>
public sealed class Problem017 : Problem
{
    // The word NumberWords.ToWords puts before a remainder below 100, and the group words it uses, largest first.
    private const string Conjunction = "and";

    private static readonly (int Value, string Name)[] Scales =
    [
        (1_000_000_000, "billion"),
        (1_000_000, "million"),
        (1_000, "thousand"),
        (100, "hundred"),
    ];

    public override int Number => 17;

    public override string Title => "Number Letter Counts";

    public override object Solve() => Solve(limit: 1000);

    /// <summary>
    /// Total letters used when the numbers 1 to <paramref name="limit"/> inclusive are written out in British English
    /// words ("three hundred and forty-two"); spaces and hyphens are not counted.
    /// </summary>
    /// <param name="limit">
    /// The last number written, from 0 (nothing is written) to 2,147,483,647: every non-negative int, which is exactly
    /// the range <see cref="NumberWords.ToWords"/> can spell.
    /// </param>
    public static long Solve(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        return LettersUpTo(limit);
    }

    /// <summary>Letters in the words for 1 to <paramref name="limit"/>; at most about 2^31 × 100, well inside a long.</summary>
    private static long LettersUpTo(int limit)
    {
        if (limit < 100)
        {
            long letters = 0;
            for (var n = 1; n <= limit; n++)
            {
                letters += Letters(n);
            }

            return letters;
        }

        // Take the largest scale not above limit. Every n from scale to limit is written
        // "<q> <scale name>", followed when r > 0 by "and" (only if r < 100) and the words for r,
        // where q = n / scale and r = n % scale. Summing by q instead of by n needs only a few smaller sums.
        var (scale, name) = Scales.First(s => s.Value <= limit);
        var lastQuotient = limit / scale;
        var lastRemainder = limit % scale;
        var belowScale = LettersUpTo(scale - 1);

        // q = 1 .. lastQuotient - 1: each pairs with every remainder 0 .. scale - 1, of which 99 take "and".
        var completeBlocks = lastQuotient - 1;
        var complete = (long)scale * (LettersUpTo(completeBlocks) + (long)completeBlocks * name.Length)
            + completeBlocks * (belowScale + 99 * Conjunction.Length);

        // q = lastQuotient: remainders 0 .. lastRemainder only.
        var partial = (lastRemainder + 1L) * (Letters(lastQuotient) + name.Length)
            + LettersUpTo(lastRemainder)
            + Math.Min(lastRemainder, 99) * Conjunction.Length;

        return belowScale + complete + partial;
    }

    private static int Letters(int n) => NumberWords.ToWords(n).Count(char.IsLetter);
}
