using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The maximum 16-digit string for a "magic" 5-gon ring.</summary>
public sealed class Problem068 : Problem
{
    public override int Number => 68;

    public override string Title => "Magic 5-gon Ring";

    public override object Solve()
    {
        // Layout: numbers[0..5) are the outer nodes and numbers[5..10) the inner ring; line i is
        // outer[i] -> inner[i] -> inner[(i + 1) % 5]. A 16-digit string needs 10 on the outer ring
        // (an inner 10 would be written twice), and the string starts from the smallest outer node.
        var best = string.Empty;
        var numbers = Enumerable.Range(1, 10).ToArray();
        do
        {
            if (!IsCandidate(numbers))
            {
                continue;
            }

            var target = numbers[0] + numbers[5] + numbers[6];
            var magic = true;
            for (var i = 1; i < 5 && magic; i++)
            {
                magic = numbers[i] + numbers[5 + i] + numbers[5 + (i + 1) % 5] == target;
            }

            if (magic)
            {
                var description = string.Concat(Enumerable.Range(0, 5).Select(i => $"{numbers[i]}{numbers[5 + i]}{numbers[5 + (i + 1) % 5]}"));
                if (string.CompareOrdinal(description, best) > 0)
                {
                    best = description;
                }
            }
        }
        while (Combinatorics.NextPermutation(numbers));

        return best;
    }

    private static bool IsCandidate(int[] numbers)
    {
        var outerHasTen = false;
        for (var i = 0; i < 5; i++)
        {
            if (numbers[i] < numbers[0])
            {
                return false; // Not starting from the smallest outer node.
            }

            outerHasTen |= numbers[i] == 10;
        }

        return outerHasTen;
    }
}
