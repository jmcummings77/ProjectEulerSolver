using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The smallest cube for which exactly five permutations of its digits are also cube.</summary>
public sealed class Problem062 : Problem
{
    // Searching every cube below 10^18 finds a cube with exactly k cube permutations for each k from 1 to 38,
    // and none for k = 39. Staying within that range keeps every cube, and the search itself, inside a long.
    private const int MaxPermutations = 38;

    public override int Number => 62;

    public override string Title => "Cubic Permutations";

    public override object Solve() => Solve(permutations: 5);

    /// <summary>
    /// The smallest positive cube for which exactly <paramref name="permutations"/> permutations of its digits are
    /// cubes. The cube itself counts as one of them, and a permutation may not start with a zero.
    /// </summary>
    /// <param name="permutations">
    /// How many of the digit permutations are cubes, from 1 to 38. Each of those counts has its answer below 10^18,
    /// so every cube examined fits in a long; 39 is the first count that needs longer cubes.
    /// </param>
    public static long Solve(int permutations)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(permutations, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(permutations, MaxPermutations);

        // Cubes with the same digits share a key. Permutations keep the digit count, so once every cube of one
        // length has been grouped the group sizes are final and "exactly" can be decided before moving on.
        // Shorter cubes are smaller, so the first length with a group of the right size holds the answer.
        var groups = new Dictionary<long, (long Smallest, int Count)>();
        long nextLength = 10;
        for (long root = 1; ; root++)
        {
            var cube = checked(root * root * root);
            if (cube >= nextLength)
            {
                long? smallest = null;
                foreach (var (first, count) in groups.Values)
                {
                    if (count == permutations && (smallest is null || first < smallest))
                    {
                        smallest = first;
                    }
                }

                if (smallest is not null)
                {
                    return smallest.Value;
                }

                // (root + 1)³ < 10 root³, so consecutive cubes never skip a digit count.
                groups.Clear();
                nextLength = checked(nextLength * 10);
            }

            var key = Digits.MultisetKey(cube);
            groups[key] = groups.TryGetValue(key, out var group) ? (group.Smallest, group.Count + 1) : (cube, 1);
        }
    }
}
