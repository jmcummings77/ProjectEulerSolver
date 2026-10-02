using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>Total letters used when the numbers 1 to 1000 are written out in words.</summary>
public sealed class Problem017 : Problem
{
    public override int Number => 17;

    public override string Title => "Number Letter Counts";

    public override object Solve() =>
        Enumerable.Range(1, 1000)
            .Select(NumberWords.ToWords)
            .Sum(words => words.Count(char.IsLetter));
}
