namespace ProjectEulerSolver.Problems;

/// <summary>The maximum 16-digit string for a "magic" 5-gon ring.</summary>
public sealed class Problem068 : Problem
{
    public override int Number => 68;

    public override string Title => "Magic 5-gon Ring";

    public override object Solve() => Solve(gonSize: 5, stringLength: 16);

    /// <summary>
    /// The numerically largest string of exactly <paramref name="stringLength"/> digits that describes a "magic"
    /// <paramref name="gonSize"/>-gon ring filled with the numbers 1 to 2 × <paramref name="gonSize"/>: every line of
    /// three numbers has the same total, and the lines are written out clockwise starting from the one with the
    /// lowest outer number.
    /// </summary>
    /// <param name="gonSize">The number of lines in the ring, from 3 to 5.</param>
    /// <param name="stringLength">
    /// The required length of the description, 1 or more. A 3-gon is always described by 9 digits and a 4-gon by 12;
    /// a 5-gon takes 16 digits when 10 is an outer number and 17 when it is an inner number, which is written twice.
    /// </param>
    /// <exception cref="InvalidOperationException">No magic ring of that size has a description of that length.</exception>
    public static string Solve(int gonSize, int stringLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(gonSize, 3);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(gonSize, 5);
        ArgumentOutOfRangeException.ThrowIfLessThan(stringLength, 1);

        // Among strings of equal length, ordinal order is numeric order.
        string? best = null;
        foreach (var description in Descriptions(gonSize))
        {
            if (description.Length == stringLength && (best is null || string.CompareOrdinal(description, best) > 0))
            {
                best = description;
            }
        }

        return best
            ?? throw new InvalidOperationException($"No magic {gonSize}-gon ring is described by a {stringLength}-digit string.");
    }

    /// <summary>The description of every magic <paramref name="gonSize"/>-gon ring, each ring exactly once.</summary>
    internal static List<string> Descriptions(int gonSize)
    {
        // Line i reads outer[i], inner[i], inner[(i + 1) % gonSize]. Making outer[0] the lowest outer number fixes
        // the rotation, so each ring is produced once, already in the order its description is written.
        var highest = 2 * gonSize;
        var outer = new int[gonSize];
        var inner = new int[gonSize];
        var used = new bool[highest + 1];
        var descriptions = new List<string>();

        for (outer[0] = 1; outer[0] <= highest; outer[0]++)
        {
            used[outer[0]] = true;
            for (inner[0] = 1; inner[0] <= highest; inner[0]++)
            {
                if (used[inner[0]])
                {
                    continue;
                }

                used[inner[0]] = true;
                for (inner[1] = 1; inner[1] <= highest; inner[1]++)
                {
                    if (used[inner[1]])
                    {
                        continue;
                    }

                    used[inner[1]] = true;
                    CompleteLines(1, outer[0] + inner[0] + inner[1]);
                    used[inner[1]] = false;
                }

                used[inner[0]] = false;
            }

            used[outer[0]] = false;
        }

        return descriptions;

        // Lines before 'line' are placed, and so is inner[line]. Each further line has one free choice, its outer
        // number; the line total then forces its other inner number.
        void CompleteLines(int line, int total)
        {
            if (line == gonSize - 1)
            {
                // The last line closes the ring on inner[0], so its outer number is forced instead.
                var last = total - inner[line] - inner[0];
                if (IsFree(last) && last > outer[0])
                {
                    outer[line] = last;
                    descriptions.Add(string.Concat(
                        Enumerable.Range(0, gonSize).Select(i => $"{outer[i]}{inner[i]}{inner[(i + 1) % gonSize]}")));
                }

                return;
            }

            for (var candidate = outer[0] + 1; candidate <= highest; candidate++)
            {
                var forced = total - candidate - inner[line];
                if (used[candidate] || forced == candidate || !IsFree(forced))
                {
                    continue;
                }

                outer[line] = candidate;
                inner[line + 1] = forced;
                used[candidate] = used[forced] = true;
                CompleteLines(line + 1, total);
                used[candidate] = used[forced] = false;
            }
        }

        bool IsFree(int value) => value >= 1 && value <= highest && !used[value];
    }
}
