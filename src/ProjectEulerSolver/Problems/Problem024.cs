using System.Text;

namespace ProjectEulerSolver.Problems;

/// <summary>The millionth lexicographic permutation of the digits 0 through 9.</summary>
public sealed class Problem024 : Problem
{
    // 20! is the largest factorial that fits in a long.
    private const int LargestFactorialArgument = 20;

    public override int Number => 24;

    public override string Title => "Lexicographic Permutations";

    public override object Solve() => Solve(digits: "0123456789", position: 1_000_000);

    /// <summary>
    /// The permutation of the characters of <paramref name="digits"/> found at <paramref name="position"/> (counting
    /// from 1) when all of them are listed in lexicographic order. A character is one Unicode code point, and
    /// characters are ordered by code point, which is numerical order for digits and alphabetical order for letters
    /// of one case.
    /// </summary>
    /// <param name="digits">
    /// The characters to permute, in any order and all different: any well-formed text, of any length. A surrogate
    /// pair counts as the one character it encodes. An empty string has one permutation, itself.
    /// </param>
    /// <param name="position">
    /// Which permutation to return: from 1 to n!, where n is the number of characters. Beyond 20 characters n! is
    /// larger than any long, so every positive value is in range.
    /// </param>
    public static string Solve(string digits, long position)
    {
        ArgumentNullException.ThrowIfNull(digits);

        // Permuting UTF-16 code units would tear surrogate pairs apart and return ill-formed text, so work in code points.
        var codePoints = new List<int>(digits.Length);
        var offset = 0;
        while (offset < digits.Length)
        {
            if (!Rune.TryGetRuneAt(digits, offset, out var character))
            {
                throw new ArgumentException($"The text is not well formed: there is an unpaired surrogate at index {offset}.", nameof(digits));
            }

            codePoints.Add(character.Value);
            offset += character.Utf16SequenceLength;
        }

        var sorted = codePoints.ToArray();
        Array.Sort(sorted);
        for (var i = 1; i < sorted.Length; i++)
        {
            if (sorted[i] == sorted[i - 1])
            {
                throw new ArgumentException($"The character '{char.ConvertFromUtf32(sorted[i])}' appears more than once.", nameof(digits));
            }
        }

        var factorials = new long[LargestFactorialArgument + 1];
        factorials[0] = 1;
        for (var i = 1; i < factorials.Length; i++)
        {
            factorials[i] = factorials[i - 1] * i;
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(position, 1);
        if (sorted.Length <= LargestFactorialArgument)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(position, factorials[sorted.Length]);
        }

        // Factorial number system: with r characters left, each choice of the next character heads a block of
        // (r - 1)! permutations, so the 0-based index picks character index / (r - 1)! and carries the remainder on.
        // While r - 1 > 20 a block is bigger than any long index, so the smallest character is picked: everything
        // before the last 21 characters stays in sorted order.
        var result = (int[])sorted.Clone();
        var firstFree = Math.Max(0, sorted.Length - LargestFactorialArgument - 1);
        var remaining = new List<int>(sorted[firstFree..]);
        var index = position - 1;
        for (var slot = firstFree; slot < result.Length; slot++)
        {
            var blockSize = factorials[remaining.Count - 1];
            var pick = (int)(index / blockSize);
            index %= blockSize;
            result[slot] = remaining[pick];
            remaining.RemoveAt(pick);
        }

        var text = new StringBuilder(digits.Length);
        foreach (var codePoint in result)
        {
            text.Append(char.ConvertFromUtf32(codePoint));
        }

        return text.ToString();
    }
}
