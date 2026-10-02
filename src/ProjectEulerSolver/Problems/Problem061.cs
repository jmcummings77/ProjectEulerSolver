using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the only ordered set of six cyclic 4-digit numbers covering each polygonal type from triangle to octagonal.</summary>
public sealed class Problem061 : Problem
{
    public override int Number => 61;

    public override string Title => "Cyclical Figurate Numbers";

    public override object Solve()
    {
        // Four-digit polygonal numbers for each type 3..8, grouped by their leading two digits so the
        // chain can be extended by looking up the previous number's trailing two digits.
        var byType = new Dictionary<int, ILookup<int, int>>();
        for (var sides = 3; sides <= 8; sides++)
        {
            var numbers = new List<int>();
            for (var n = 1; ; n++)
            {
                var value = Figurate.Polygonal(sides, n);
                if (value >= 10_000)
                {
                    break;
                }

                if (value >= 1000 && value % 100 >= 10) // Trailing "0x" could not start another 4-digit number.
                {
                    numbers.Add((int)value);
                }
            }

            byType[sides] = numbers.ToLookup(v => v / 100);
        }

        // Fix the octagonal number first (fewest candidates) and search the remaining types in any order.
        foreach (var start in byType[8].SelectMany(group => group))
        {
            var chain = new List<int> { start };
            if (Extend(chain, [3, 4, 5, 6, 7]))
            {
                return chain.Sum();
            }
        }

        throw new InvalidOperationException("No cyclic set found.");

        bool Extend(List<int> chain, HashSet<int> remainingTypes)
        {
            if (remainingTypes.Count == 0)
            {
                return chain[^1] % 100 == chain[0] / 100;
            }

            foreach (var sides in remainingTypes.ToArray())
            {
                foreach (var next in byType[sides][chain[^1] % 100])
                {
                    if (chain.Contains(next))
                    {
                        continue;
                    }

                    chain.Add(next);
                    remainingTypes.Remove(sides);
                    if (Extend(chain, remainingTypes))
                    {
                        return true;
                    }

                    remainingTypes.Add(sides);
                    chain.RemoveAt(chain.Count - 1);
                }
            }

            return false;
        }
    }
}
