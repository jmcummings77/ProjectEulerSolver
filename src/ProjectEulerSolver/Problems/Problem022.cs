using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The total of all the name scores in a file of over five thousand first names.</summary>
public sealed class Problem022 : Problem
{
    public override int Number => 22;

    public override string Title => "Names Scores";

    public override object Solve() => Solve(names: Resources.ReadQuotedList("0022_names.txt"));

    /// <summary>
    /// Total of the name scores of <paramref name="names"/>: after sorting into alphabetical order, a name's score is
    /// the sum of its letters' positions in the alphabet (A = 1, ..., Z = 26) multiplied by its 1-based position in the list.
    /// </summary>
    /// <param name="names">
    /// Any number of names in any order, each made of one or more upper-case letters A to Z. Repeated names are kept
    /// and take consecutive positions. The total must fit in a long, which it does for any list of practical size.
    /// </param>
    public static long Solve(IReadOnlyList<string> names)
    {
        ArgumentNullException.ThrowIfNull(names);
        var sorted = new string[names.Count];
        for (var i = 0; i < sorted.Length; i++)
        {
            var name = names[i];
            if (string.IsNullOrEmpty(name) || name.Any(letter => letter is < 'A' or > 'Z'))
            {
                throw new ArgumentException($"Expected names made of the upper-case letters A to Z, but item {i + 1} is '{name}'.", nameof(names));
            }

            sorted[i] = name;
        }

        // For upper-case A to Z, ordinal order is alphabetical order (and a prefix sorts before its extensions).
        Array.Sort(sorted, StringComparer.Ordinal);

        long total = 0;
        for (var i = 0; i < sorted.Length; i++)
        {
            long value = 0;
            foreach (var letter in sorted[i])
            {
                value += letter - 'A' + 1;
            }

            total = checked(total + (i + 1) * value);
        }

        return total;
    }
}
