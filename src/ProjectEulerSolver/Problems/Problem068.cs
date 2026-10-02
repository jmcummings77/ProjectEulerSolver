using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum 16-digit string for a "magic" 5-gon ring.</summary>
public sealed class Problem068 : Problem
{
    public override int Number => 68;

    public override string Title => "Magic 5-gon Ring";

    public override object Solve()
    {
        // Layout: outer[i] -> inner[i] -> inner[(i + 1) % 5] for each of the five lines.
        // A 16-digit string needs 10 on the outer ring (an inner 10 would be written twice).
        var best = string.Empty;
        var numbers = Enumerable.Range(1, 10).ToArray();
        do
        {
            var outer = numbers[..5];
            var inner = numbers[5..];
            if (!outer.Contains(10) || outer.Min() != outer[0])
            {
                continue; // Start the description from the smallest outer node, as the problem requires.
            }

            var target = outer[0] + inner[0] + inner[1];
            var magic = true;
            for (var i = 1; i < 5 && magic; i++)
            {
                magic = outer[i] + inner[i] + inner[(i + 1) % 5] == target;
            }

            if (magic)
            {
                var description = string.Concat(Enumerable.Range(0, 5).Select(i => $"{outer[i]}{inner[i]}{inner[(i + 1) % 5]}"));
                if (string.CompareOrdinal(description, best) > 0)
                {
                    best = description;
                }
            }
        }
        while (Combinatorics.NextPermutation(numbers));

        return best;
    }
}
