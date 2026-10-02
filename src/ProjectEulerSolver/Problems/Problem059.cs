using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the ASCII values of the plain text, after breaking a three-letter XOR cipher.</summary>
public sealed class Problem059 : Problem
{
    public override int Number => 59;

    public override string Title => "XOR Decryption";

    public override object Solve() =>
        Solve(cipher: Resources.ReadQuotedList("0059_cipher_original.txt").Select(int.Parse).ToArray(), keyLength: 3);

    /// <summary>
    /// The sum of the ASCII values of the text obtained by decrypting <paramref name="cipher"/> with the most plausible
    /// repeating XOR key of <paramref name="keyLength"/> lowercase letters. Each key letter is chosen on its own:
    /// it must turn every code it applies to into a printable character, tab or line break, and among such
    /// letters the one that produces the most letters and spaces wins (the earliest in the alphabet if several
    /// are equally good).
    /// </summary>
    /// <param name="cipher">The encrypted ASCII codes, each from 0 to 127; at least one.</param>
    /// <param name="keyLength">The number of letters in the key: from 1 to the length of <paramref name="cipher"/>.</param>
    /// <exception cref="InvalidOperationException">No lowercase key of that length decrypts the cipher to printable text.</exception>
    public static long Solve(IReadOnlyList<int> cipher, int keyLength)
    {
        ArgumentNullException.ThrowIfNull(cipher);
        if (cipher.Count == 0 || cipher.Any(code => code is < 0 or > 127))
        {
            throw new ArgumentException("Expected a non-empty list of ASCII codes from 0 to 127.", nameof(cipher));
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(keyLength, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(keyLength, cipher.Count);

        // Key letter number p encrypts the characters at positions p, p + keyLength, p + 2 × keyLength, ... and no
        // others, so each letter can be recovered independently of the rest.
        long sum = 0;
        for (var position = 0; position < keyLength; position++)
        {
            var best = (Score: -1, Sum: 0L);
            for (var letter = 'a'; letter <= 'z'; letter++)
            {
                var attempt = Decrypt(cipher, position, keyLength, letter);
                if (attempt.Score > best.Score)
                {
                    best = attempt;
                }
            }

            if (best.Score < 0)
            {
                throw new InvalidOperationException(
                    $"No lowercase letter in key position {position + 1} decrypts the cipher to printable text.");
            }

            sum += best.Sum;
        }

        return sum;
    }

    /// <summary>
    /// Decrypts the characters that one key letter applies to. The score is the number of letters and spaces
    /// produced, or -1 when some character is not printable; the sum is that of the decrypted ASCII values.
    /// </summary>
    private static (int Score, long Sum) Decrypt(IReadOnlyList<int> cipher, int position, int keyLength, char letter)
    {
        var score = 0;
        var sum = 0L;

        // Counting in a long keeps position + k × keyLength from overflowing near the end of a very long list.
        for (long i = position; i < cipher.Count; i += keyLength)
        {
            var plain = (char)(cipher[(int)i] ^ letter);
            if (!IsPrintable(plain))
            {
                return (-1, 0);
            }

            sum += plain;
            if (IsLetterOrSpace(plain))
            {
                score++;
            }
        }

        return (score, sum);
    }

    private static bool IsLetterOrSpace(char c) => c == ' ' || char.IsAsciiLetter(c);

    private static bool IsPrintable(char c) => c is (>= ' ' and <= '~') or '\t' or '\n' or '\r';
}
