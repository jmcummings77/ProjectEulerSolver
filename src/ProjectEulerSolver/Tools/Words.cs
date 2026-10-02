namespace ProjectEulerSolver.Tools;

/// <summary>Scores for upper-case words, as used by the names and words puzzles.</summary>
public static class Words
{
    /// <summary>Sum of letter positions in the alphabet (A = 1, B = 2, ..., Z = 26) for an upper-case word.</summary>
    public static int AlphabetValue(string word)
    {
        var value = 0;
        foreach (var c in word)
        {
            if (c is < 'A' or > 'Z')
            {
                throw new ArgumentException($"'{word}' contains the non-upper-case letter '{c}'.", nameof(word));
            }

            value += c - 'A' + 1;
        }

        return value;
    }
}
