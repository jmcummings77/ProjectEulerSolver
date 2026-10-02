using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many of the words in a 16K text file are triangle words.</summary>
public sealed class Problem042 : Problem
{
    public override int Number => 42;

    public override string Title => "Coded Triangle Numbers";

    public override object Solve() => Solve(words: Resources.ReadQuotedList("0042_words.txt"));

    /// <summary>
    /// How many of <paramref name="words"/> are triangle words: the alphabet positions of the letters
    /// (A = 1, B = 2, ..., Z = 26) add up to a triangle number n(n + 1)/2 with n ≥ 1.
    /// </summary>
    /// <param name="words">Any number of words, each one or more upper-case letters A to Z; repeats are counted each time.</param>
    public static int Solve(IReadOnlyList<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);
        if (words.Any(word => string.IsNullOrEmpty(word) || word.Any(letter => letter is < 'A' or > 'Z')))
        {
            throw new ArgumentException("Every word must be one or more upper-case letters A to Z.", nameof(words));
        }

        // Summed as a long: a string can be long enough for its word value to overflow an int.
        return words.Count(word => Figurate.IsTriangle(word.Sum(letter => (long)(letter - 'A' + 1))));
    }
}
