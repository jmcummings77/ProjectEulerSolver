using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the ASCII values of the plain text, after breaking a three-letter XOR cipher.</summary>
public sealed class Problem059 : Problem
{
    private const int KeyLength = 3;

    public override int Number => 59;

    public override string Title => "XOR Decryption";

    public override object Solve()
    {
        var cipher = Resources.ReadText("0059_cipher_original.txt").Split(',').Select(byte.Parse).ToArray();

        // Each key byte encrypts every third character independently, so pick, per position, the lowercase
        // letter that turns the most bytes into ordinary English text (letters and spaces).
        var key = new byte[KeyLength];
        for (var position = 0; position < KeyLength; position++)
        {
            var bytes = cipher.Where((_, i) => i % KeyLength == position).ToArray();
            key[position] = (byte)Enumerable.Range('a', 26)
                .MaxBy(candidate => bytes.Count(b => IsPlainTextLike((char)(b ^ candidate))));
        }

        return cipher.Select((b, i) => b ^ key[i % KeyLength]).Sum();
    }

    private static bool IsPlainTextLike(char c) => c == ' ' || char.IsAsciiLetter(c);
}
