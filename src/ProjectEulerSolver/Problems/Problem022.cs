using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The total of all the name scores in a file of over five thousand first names.</summary>
public sealed class Problem022 : Problem
{
    public override int Number => 22;

    public override string Title => "Names Scores";

    public override object Solve()
    {
        var names = Resources.ReadQuotedList("0022_names.txt");
        Array.Sort(names, StringComparer.Ordinal);

        long total = 0;
        for (var i = 0; i < names.Length; i++)
        {
            total += (i + 1) * AlphabetValue(names[i]);
        }

        return total;
    }

    /// <summary>Sum of letter positions (A = 1, B = 2, ...). Shared with Problem 42.</summary>
    internal static int AlphabetValue(string word) => word.Sum(c => c - 'A' + 1);
}
