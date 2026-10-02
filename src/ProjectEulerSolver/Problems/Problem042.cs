using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many of the words in a 16K text file are triangle words.</summary>
public sealed class Problem042 : Problem
{
    public override int Number => 42;

    public override string Title => "Coded Triangle Numbers";

    public override object Solve() =>
        Resources.ReadQuotedList("0042_words.txt")
            .Count(word => Figurate.IsTriangle(Words.AlphabetValue(word)));
}
