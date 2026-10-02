using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The sum of the only ordered set of six cyclic 4-digit numbers covering each polygonal type from triangle to octagonal.</summary>
public sealed class Problem061 : Problem
{
    public override int Number => 61;

    public override string Title => "Cyclical Figurate Numbers";

    public override object Solve() => Solve(sides: [3, 4, 5, 6, 7, 8]);

    /// <summary>
    /// The sum of an ordered set of distinct 4-digit numbers, one for each polygonal type in <paramref name="sides"/>,
    /// that is cyclic: the last two digits of each number are the first two digits of the next, and the last number
    /// wraps round to the first. When several such sets exist the smallest sum is returned.
    /// </summary>
    /// <param name="sides">
    /// The polygonal types to cover, as numbers of sides (3 for triangle up to 8 for octagonal): between one and six
    /// distinct values, each from 3 to 8, in any order. A single type asks for one number whose last two digits
    /// repeat its first two.
    /// </param>
    /// <exception cref="InvalidOperationException">No cyclic set covers the given types.</exception>
    public static int Solve(IReadOnlyList<int> sides)
    {
        ArgumentNullException.ThrowIfNull(sides);
        if (sides.Count == 0)
        {
            throw new ArgumentException("Expected at least one polygonal type.", nameof(sides));
        }

        foreach (var type in sides)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(type, 3, nameof(sides));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(type, 8, nameof(sides));
        }

        if (sides.Distinct().Count() != sides.Count)
        {
            throw new ArgumentException("Polygonal types must be distinct.", nameof(sides));
        }

        // Four-digit polygonal numbers of each type, grouped by their leading two digits so the chain can be
        // extended by looking up the previous number's trailing two digits.
        var byType = sides.ToDictionary(type => type, type => FourDigitPolygonals(type).ToLookup(value => value / 100));

        // Every cyclic set holds exactly one number of each type and can be read starting from any of them, so
        // starting every chain at one fixed type still reaches every set. The most-sided type has the fewest candidates.
        var firstType = sides.Max();
        var remainingTypes = sides.Where(type => type != firstType).ToList();
        var chain = new List<int>();
        int? smallestSum = null;
        foreach (var start in byType[firstType].SelectMany(group => group))
        {
            chain.Add(start);
            Extend(start);
            chain.Clear();
        }

        return smallestSum
            ?? throw new InvalidOperationException($"No cyclic set of 4-digit numbers covers the polygonal types {string.Join(", ", sides)}.");

        void Extend(int sum)
        {
            if (remainingTypes.Count == 0)
            {
                if (chain[^1] % 100 == chain[0] / 100 && (smallestSum is null || sum < smallestSum))
                {
                    smallestSum = sum;
                }

                return;
            }

            for (var i = 0; i < remainingTypes.Count; i++)
            {
                var type = remainingTypes[i];
                remainingTypes.RemoveAt(i);
                foreach (var next in byType[type][chain[^1] % 100])
                {
                    // A number that is polygonal in two ways may stand for either type, but only once.
                    if (chain.Contains(next))
                    {
                        continue;
                    }

                    chain.Add(next);
                    Extend(sum + next);
                    chain.RemoveAt(chain.Count - 1);
                }

                remainingTypes.Insert(i, type);
            }
        }
    }

    private static List<int> FourDigitPolygonals(int sides)
    {
        // Polygonal numbers increase with n, so stopping at the first five-digit value misses nothing.
        var numbers = new List<int>();
        for (var n = 1; ; n++)
        {
            var value = Figurate.Polygonal(sides, n);
            if (value >= 10_000)
            {
                return numbers;
            }

            // Trailing digits "0x" cannot start the next 4-digit number, so such a value fits in no cycle.
            if (value >= 1000 && value % 100 >= 10)
            {
                numbers.Add((int)value);
            }
        }
    }
}
