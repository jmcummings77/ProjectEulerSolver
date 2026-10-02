using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>The general solvers of problems 61 to 70, checked against their statements and against brute force.</summary>
public class Problems061To070Tests
{
    private static readonly Dictionary<int, int[]> FourDigitPolygonals = Enumerable.Range(3, 6).ToDictionary(
        sides => sides,
        sides => Enumerable.Range(1000, 9000).Where(value => Figurate.IsPolygonal(sides, value)).ToArray());

    [Fact]
    public void Problem061_finds_the_three_type_set_from_the_statement()
    {
        // The statement's example for triangle, square and pentagonal numbers: 8128, 2882, 8281.
        Assert.Equal(8128 + 2882 + 8281, Problem061.Solve([3, 4, 5]));
        Assert.Equal(8128 + 2882 + 8281, Problem061.Solve([5, 3, 4]));
    }

    [Fact]
    public void Problem061_matches_brute_force_for_every_set_of_types()
    {
        for (var mask = 1; mask < 1 << 6; mask++)
        {
            var sides = Enumerable.Range(3, 6).Where(type => (mask & (1 << (type - 3))) != 0).ToArray();
            if (SmallestCyclicSumBySearch(sides) is { } expected)
            {
                Assert.Equal(expected, Problem061.Solve(sides));
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => Problem061.Solve(sides));
            }
        }
    }

    [Fact]
    public void Problem061_handles_single_types_and_rejects_bad_ones()
    {
        Assert.Equal(5050, Problem061.Solve([3])); // The 100th triangle number repeats its first two digits.

        // A square of the form "abab" is a multiple of 101 and so of 101², which has five digits.
        Assert.Throws<InvalidOperationException>(() => Problem061.Solve([4]));

        Assert.Throws<ArgumentException>(() => Problem061.Solve([]));
        Assert.Throws<ArgumentException>(() => Problem061.Solve([3, 4, 3]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem061.Solve([2, 3]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem061.Solve([3, 9]));
    }

    [Fact]
    public void Problem062_matches_brute_force_over_every_supported_count()
    {
        var expected = SmallestCubesByPermutationCount(maxDigits: 18);

        // The statement's example: 41063625 = 345³ is the smallest cube with exactly three cube permutations.
        Assert.Equal(41_063_625, expected[3]);

        // The supported range is exactly the counts that occur among cubes below 10^18.
        Assert.All(Enumerable.Range(1, 38), count => Assert.Contains(count, expected.Keys));
        Assert.DoesNotContain(39, expected.Keys);

        // Counts up to 12 are settled by 15-digit cubes; 38 is the top of the range and needs every 18-digit cube.
        foreach (var permutations in Enumerable.Range(1, 12).Append(38))
        {
            Assert.Equal(expected[permutations], Problem062.Solve(permutations));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(39)]
    public void Problem062_rejects_counts_outside_its_range(int permutations) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem062.Solve(permutations));

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 2)]
    [InlineData(9, 2)]
    [InlineData(10, 3)]
    [InlineData(12, 3)]
    [InlineData(13, 4)] // The statement's count for N ≤ 13: the odd periods belong to √2, √5, √10 and √13.
    public void Problem064_matches_the_periods_listed_in_the_statement(int limit, int expected) =>
        Assert.Equal(expected, Problem064.Solve(limit));

    [Fact]
    public void Problem064_matches_brute_force()
    {
        var count = 0;
        for (var n = 1; n <= 100; n++)
        {
            if (!NumberTheory.IsSquare(n) && HasOddPeriodByPell(n))
            {
                count++;
            }

            Assert.Equal(count, Problem064.Solve(n));
        }
    }

    [Fact]
    public void Problem064_handles_limits_beyond_the_statement()
    {
        Assert.Equal(Enumerable.Range(1, 20_000).Count(n => PeriodLengthLonghand(n) % 2 == 1), Problem064.Solve(20_000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem064.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem064.Solve(5_000_001));
    }

    [Theory] // The statement's first ten convergents: 2, 3, 8/3, 11/4, 19/7, 87/32, 106/39, 193/71, 1264/465, 1457/536.
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 8)]
    [InlineData(4, 1 + 1)]
    [InlineData(5, 1 + 9)]
    [InlineData(6, 8 + 7)]
    [InlineData(7, 1 + 0 + 6)]
    [InlineData(8, 1 + 9 + 3)]
    [InlineData(9, 1 + 2 + 6 + 4)]
    [InlineData(10, 1 + 4 + 5 + 7)]
    public void Problem065_matches_the_convergents_listed_in_the_statement(int n, int expected) =>
        Assert.Equal(expected, Problem065.Solve(n));

    [Fact]
    public void Problem065_matches_brute_force()
    {
        foreach (var n in Enumerable.Range(1, 80).Concat([999, 1000, 1001, 3000]))
        {
            var numerator = NumeratorOfEByNestedFractions(n);
            Assert.Equal(numerator.ToString().Sum(digit => digit - '0'), Problem065.Solve(n));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    [InlineData(100_001)]
    public void Problem065_rejects_convergents_outside_its_range(int n) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem065.Solve(n));

    [Fact]
    public void Problem066_matches_the_statement_examples()
    {
        // The statement lists the minimal solutions for D = 2, 3, 5, 6, 7 and concludes that D = 5 has the largest x.
        Assert.Equal(new BigInteger[] { 3, 2, 9, 5, 8 }, new[] { 2, 3, 5, 6, 7 }.Select(Problem066.MinimalX));
        Assert.Equal(5, Problem066.Solve(7));

        Assert.Equal(649, Problem066.MinimalX(13)); // 649² − 13 × 180² = 1
        Assert.Equal(0, Problem066.MinimalX(49)); // A square D has no solution in positive integers.
    }

    [Fact]
    public void Problem066_matches_brute_force()
    {
        // 61 is the first D whose minimal solution is out of reach of a plain search.
        var bestD = 0;
        long bestX = 0;
        for (var d = 2; d <= 60; d++)
        {
            var x = NumberTheory.IsSquare(d) ? 0 : MinimalXBySearch(d);
            Assert.Equal(x, Problem066.MinimalX(d));
            if (x > bestX)
            {
                bestX = x;
                bestD = d;
            }

            Assert.Equal(bestD, Problem066.Solve(d));
        }
    }

    [Fact]
    public void Problem066_handles_bounds_beyond_the_statement()
    {
        const int MaxD = 2500;
        var minimal = Enumerable.Range(0, MaxD + 1)
            .Select(d => d < 2 || NumberTheory.IsSquare(d) ? BigInteger.Zero : MinimalXByTestingConvergents(d))
            .ToArray();
        Assert.All(Enumerable.Range(2, MaxD - 1), d => Assert.Equal(minimal[d], Problem066.MinimalX(d)));
        Assert.Equal(Array.IndexOf(minimal, minimal.Max()), Problem066.Solve(MaxD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(1_000_001)]
    public void Problem066_rejects_bounds_outside_its_range(int maxD) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem066.Solve(maxD));

    [Fact]
    public void Problem067_matches_the_statement_example()
    {
        // The four-row triangle shared with Problem 18: 3 + 7 + 4 + 9 = 23.
        Assert.Equal(23, Problem067.Solve(["3", "7 4", "2 4 6", "8 5 9 3"]));
        Assert.Equal(23, Problem067.Solve(["03", "07  04", " 02 04 06 ", "08 05 09 03"]));
    }

    [Fact]
    public void Problem067_matches_brute_force_over_every_path()
    {
        var random = new Random(67);
        for (var rowCount = 1; rowCount <= 12; rowCount++)
        {
            var triangle = Enumerable.Range(1, rowCount)
                .Select(length => Enumerable.Range(0, length).Select(_ => random.Next(-50, 100)).ToArray())
                .ToArray();
            var rows = triangle.Select(row => string.Join(' ', row)).ToArray();
            Assert.Equal(BestPathTotal(triangle, 0, 0), Problem067.Solve(rows));
        }
    }

    [Fact]
    public void Problem067_handles_triangles_larger_than_the_statement()
    {
        // The only large entries lie on the left edge, so the best path follows it.
        var rows = Enumerable.Range(1, 400).Select(length => string.Join(' ', Enumerable.Repeat(1, length - 1).Prepend(1000)));
        Assert.Equal(400_000, Problem067.Solve(rows.ToArray()));

        // Totals beyond an int.
        Assert.Equal(3L * int.MaxValue, Problem067.Solve(["2147483647", "1 2147483647", "0 2147483647 -5"]));
        Assert.Equal(-3, Problem067.Solve(["-1", "-2 -1", "-9 -1 -7"]));
    }

    [Fact]
    public void Problem067_rejects_malformed_triangles()
    {
        Assert.Throws<ArgumentException>(() => Problem067.Solve([]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", "2"]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", "2 3 4"]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1 2"]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", "2 x"]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", "2 3.5"]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", "2 2147483648"]));
        Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", string.Empty]));
    }

    [Fact]
    public void Problem068_finds_the_eight_three_gon_rings_from_the_statement()
    {
        // The statement's table of solutions, two for each of the totals 9, 10, 11 and 12.
        var rings = "423531612 432621513 235451613 253631415 146362524 164542326 156264345 165354246".Split(' ');
        Assert.Equal(rings.Order(), Problem068.Descriptions(3).Order());

        // The statement's maximum 9-digit string for a 3-gon ring.
        Assert.Equal("432621513", Problem068.Solve(3, 9));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Problem068_matches_brute_force_over_every_arrangement(int gonSize)
    {
        var expected = LargestRingDescriptionsByLength(gonSize);
        Assert.NotEmpty(expected);
        foreach (var stringLength in Enumerable.Range(1, 6 * gonSize))
        {
            if (expected.TryGetValue(stringLength, out var best))
            {
                Assert.Equal(best, Problem068.Solve(gonSize, stringLength));
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => Problem068.Solve(gonSize, stringLength));
            }
        }
    }

    [Fact]
    public void Problem068_handles_the_longer_strings_and_rejects_bad_arguments()
    {
        // With 10 on the inner ring it is written twice, giving 17 digits instead of the statement's 16.
        var longest = Problem068.Solve(5, 17);
        Assert.Equal(17, longest.Length);
        Assert.Equal(2, longest.Split("10").Length - 1);

        Assert.Throws<InvalidOperationException>(() => Problem068.Solve(3, 10));
        Assert.Throws<InvalidOperationException>(() => Problem068.Solve(5, 15));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem068.Solve(2, 6));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem068.Solve(6, 18));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem068.Solve(3, 0));
    }

    [Fact]
    public void Problem069_matches_the_statement_and_brute_force()
    {
        Assert.Equal(6, Problem069.Solve(10)); // The statement's example: n = 6 gives the maximum for n ≤ 10.

        var bestN = 1;
        var bestPhi = 1;
        for (var n = 1; n <= 700; n++)
        {
            // n/φ(n) > bestN/bestPhi, cross-multiplied; a tie keeps the smaller n.
            var phi = TotientByCounting(n);
            if (n * bestPhi > bestN * phi)
            {
                bestN = n;
                bestPhi = phi;
            }

            Assert.Equal(bestN, Problem069.Solve(n));
        }
    }

    [Fact]
    public void Problem069_handles_limits_beyond_the_statement()
    {
        Assert.Equal(200_560_490_130, Problem069.Solve(1_000_000_000_000)); // 2 × 3 × ... × 31; the next prime, 37, overshoots.
        Assert.Equal(200_560_490_130, Problem069.Solve(200_560_490_130));
        Assert.Equal(6_469_693_230, Problem069.Solve(200_560_490_129)); // 2 × 3 × ... × 29

        // The product up to 47 is the last that fits in a long.
        long[] primes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47];
        var largest = Problem069.Solve(long.MaxValue);
        Assert.Equal(primes.Aggregate(BigInteger.One, (product, p) => product * p), largest);
        Assert.True(largest * new BigInteger(53) > long.MaxValue);

        Assert.Throws<ArgumentOutOfRangeException>(() => Problem069.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem069.Solve(-10));
    }

    [Theory]
    [InlineData(22)]
    [InlineData(291)]
    [InlineData(292)]
    [InlineData(3000)]
    [InlineData(4436)]
    [InlineData(20_618)]
    [InlineData(75_842)]
    [InlineData(87_110)]
    [InlineData(100_000)]
    [InlineData(11_000_000)] // Beyond the statement's limit.
    public void Problem070_matches_brute_force(int limit) =>
        Assert.Equal(BestTotientPermutation(TotientsBySmallestPrimeFactor(limit - 1), limit), Problem070.Solve(limit));

    [Fact]
    public void Problem070_is_consistent_with_the_statement_example()
    {
        // The statement's example: φ(87109) = 79180, a permutation of 87109.
        var phi = TotientsBySmallestPrimeFactor(87_109);
        Assert.Equal(79_180, phi[87_109]);

        // So just above 87109 an answer exists, and its ratio is no worse than this one's.
        var best = Problem070.Solve(87_110);
        Assert.True(Digits.ArePermutations(best, phi[best]));
        Assert.True((long)best * 79_180 <= 87_109L * phi[best]);
    }

    [Fact]
    public void Problem070_reports_limits_with_no_answer_and_rejects_bad_ones()
    {
        // φ(21) = 12 is the first totient that is a permutation of its argument.
        Assert.All(new[] { 0, 1, 2, 3, 10, 21 }, limit => Assert.Throws<InvalidOperationException>(() => Problem070.Solve(limit)));
        Assert.Equal(21, Problem070.Solve(22));

        Assert.Throws<ArgumentOutOfRangeException>(() => Problem070.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem070.Solve(100_000_001));
    }

    [Fact]
    public void Totient_helpers_used_by_these_tests_agree()
    {
        var phi = TotientsBySmallestPrimeFactor(600);
        Assert.All(Enumerable.Range(1, 600), n => Assert.Equal(TotientByCounting(n), phi[n]));
    }

    // The expected values in the tests below were computed separately from these solvers, by exhaustive search
    // written straight from each problem statement.

    [Theory]
    [InlineData(new[] { 6 }, 5151)]
    [InlineData(new[] { 3, 6 }, 12_120)] // Not 5050 + 5151: the two numbers must chain into each other.
    [InlineData(new[] { 4, 5, 7, 8 }, 10_504)]
    [InlineData(new[] { 3, 4, 5, 6, 8 }, 9393)]
    [InlineData(new[] { 8, 7, 6, 5, 4, 3 }, 28_684)]
    [InlineData(new[] { 5, 3, 8, 4, 7, 6 }, 28_684)]
    public void Problem061_matches_separately_computed_sums_in_any_order(int[] sides, int expected) =>
        Assert.Equal(expected, Problem061.Solve(sides));

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 125)]
    [InlineData(6, 1_000_600_120_008)]
    [InlineData(13, 1_027_182_645_350_792)]
    [InlineData(15, 106_874_325_794_816)] // Shorter than the answers for 13 and 14: the answers do not grow steadily.
    [InlineData(26, 101_048_186_462_359_375)]
    [InlineData(37, 117_286_342_075_495_936)]
    public void Problem062_matches_separately_computed_cubes(int permutations, long expected) =>
        Assert.Equal(expected, Problem062.Solve(permutations));

    [Theory]
    [InlineData(99, 20)]
    [InlineData(101, 21)]
    [InlineData(1000, 152)]
    [InlineData(123_456, 13_996)]
    public void Problem064_matches_separately_computed_counts(int limit, int expected) =>
        Assert.Equal(expected, Problem064.Solve(limit));

    [Theory]
    [InlineData(99, 298)]
    [InlineData(101, 255)]
    [InlineData(4999, 25_652)]
    [InlineData(5000, 25_561)]
    public void Problem065_matches_separately_computed_digit_sums(int n, int expected) =>
        Assert.Equal(expected, Problem065.Solve(n));

    [Theory]
    [InlineData(2, 2)]
    [InlineData(4, 2)] // D = 3 has the smaller x = 2, and 4 is a square.
    [InlineData(61, 61)]
    [InlineData(999, 661)]
    [InlineData(1001, 661)]
    [InlineData(10_000, 9949)]
    [InlineData(30_000, 29_269)]
    public void Problem066_matches_separately_computed_bounds(int maxD, int expected) =>
        Assert.Equal(expected, Problem066.Solve(maxD));

    [Fact]
    public void Problem066_minimal_solutions_hold_at_the_top_of_the_range()
    {
        // The largest minimal solution for D ≤ 1,000,000 belongs to D = 952,429 and has 2,475 digits.
        Assert.Equal(2475, Problem066.MinimalX(952_429).ToString().Length);

        foreach (var d in Enumerable.Range(999_960, 41).Append(952_429))
        {
            var x = Problem066.MinimalX(d);
            if (NumberTheory.IsSquare(d))
            {
                Assert.Equal(0, x);
                continue;
            }

            // x² − 1 must be d times a perfect square, and no earlier convergent may already solve the equation.
            var (ySquared, remainder) = BigInteger.DivRem(x * x - 1, d);
            Assert.Equal(0, remainder);
            var y = SquareRoot(ySquared);
            Assert.True(y > 0 && y * y == ySquared);
            Assert.Equal(MinimalXByTestingConvergents(d), x);
        }
    }

    [Fact]
    public void Problem067_rejects_a_missing_row_as_malformed()
    {
        Assert.Equal("rows", Assert.Throws<ArgumentException>(() => Problem067.Solve(["1", null!])).ParamName);
        Assert.Equal("rows", Assert.Throws<ArgumentException>(() => Problem067.Solve([null!])).ParamName);
        Assert.Throws<ArgumentNullException>(() => Problem067.Solve(null!));
    }

    [Fact]
    public void Problem067_accepts_signs_and_any_whitespace()
    {
        Assert.Equal(10, Problem067.Solve(["+1", "\t2   +3 ", "4\t5  6\r"]));
        Assert.Equal(-3, Problem067.Solve(["-3"]));
    }

    [Fact]
    public void Problem068_lists_every_ring_exactly_once()
    {
        var fourGon = "158284743635 168483537276 185653734248 186267573438 248581617374 256364841715 "
            + "265751814346 284347671518 328481715652 382625751418 426561813732 462723831516";
        var fiveGon = "11069627285843410 11085864693972710 16103104548782926 18102107379496568 21049436378715110 "
            + "24105101817673934 2594936378711015 27858434106101917 28797161103104548 2951051817673439 "
            + "6357528249411013 6531031914842725";
        Assert.Equal(fourGon.Split(' ').Order(StringComparer.Ordinal), Problem068.Descriptions(4).Order(StringComparer.Ordinal));
        Assert.Equal(fiveGon.Split(' ').Order(StringComparer.Ordinal), Problem068.Descriptions(5).Order(StringComparer.Ordinal));

        Assert.Equal("462723831516", Problem068.Solve(4, 12));
        Assert.Equal("28797161103104548", Problem068.Solve(5, 17));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 2)]
    [InlineData(9_699_689, 510_510)]
    [InlineData(9_699_690, 9_699_690)]
    [InlineData(100_000_000, 9_699_690)]
    [InlineData(223_092_869, 9_699_690)]
    [InlineData(223_092_870, 223_092_870)]
    public void Problem069_matches_separately_sieved_maxima(long limit, long expected) =>
        Assert.Equal(expected, Problem069.Solve(limit));

    [Theory]
    [InlineData(2817, 291)]
    [InlineData(2818, 2817)]
    [InlineData(1_000_000, 783_169)]
    [InlineData(1_956_103, 1_924_891)]
    [InlineData(1_956_104, 1_956_103)]
    public void Problem070_matches_separately_sieved_minima(int limit, int expected) =>
        Assert.Equal(expected, Problem070.Solve(limit));

    /// <summary>Problem 61 by trying every order of the types and every number of each type in turn.</summary>
    private static int? SmallestCyclicSumBySearch(int[] sides)
    {
        int? smallest = null;
        foreach (var order in Combinatorics.Permutations(sides))
        {
            Search(order, []);
        }

        return smallest;

        void Search(int[] order, List<int> chain)
        {
            if (chain.Count == order.Length)
            {
                if (chain[^1] % 100 == chain[0] / 100 && (smallest is null || chain.Sum() < smallest))
                {
                    smallest = chain.Sum();
                }

                return;
            }

            foreach (var number in FourDigitPolygonals[order[chain.Count]])
            {
                if (chain.Contains(number) || (chain.Count > 0 && chain[^1] % 100 != number / 100))
                {
                    continue;
                }

                chain.Add(number);
                Search(order, chain);
                chain.RemoveAt(chain.Count - 1);
            }
        }
    }

    /// <summary>
    /// Problem 62 for every count at once: groups all the cubes of each length by their digits and records, for
    /// each group size, the smallest cube of the first length where that size occurs.
    /// </summary>
    private static Dictionary<int, long> SmallestCubesByPermutationCount(int maxDigits)
    {
        var smallest = new Dictionary<int, long>();
        long root = 1;
        long nextLength = 10;
        for (var digits = 1; digits <= maxDigits; digits++, nextLength *= 10)
        {
            // Cubes arrive in ascending order, so the first one seen with a given key is the smallest of its group.
            var groups = new Dictionary<long, (long First, int Count)>();
            for (; root * root * root < nextLength; root++)
            {
                var key = LargestDigitPermutation(root * root * root);
                groups[key] = groups.TryGetValue(key, out var group) ? (group.First, group.Count + 1) : (root * root * root, 1);
            }

            // Only the first length in which a group size occurs counts, and within it the smallest such cube.
            var settled = smallest.Keys.ToHashSet();
            foreach (var (first, count) in groups.Values)
            {
                if (!settled.Contains(count) && (!smallest.TryGetValue(count, out var current) || first < current))
                {
                    smallest[count] = first;
                }
            }
        }

        return smallest;
    }

    /// <summary>The digits of n in descending order, read as a number: equal exactly for digit permutations.</summary>
    private static long LargestDigitPermutation(long n)
    {
        Span<int> counts = stackalloc int[10];
        for (; n > 0; n /= 10)
        {
            counts[(int)(n % 10)]++;
        }

        long largest = 0;
        for (var digit = 9; digit >= 0; digit--)
        {
            for (var i = 0; i < counts[digit]; i++)
            {
                largest = largest * 10 + digit;
            }
        }

        return largest;
    }

    /// <summary>
    /// The period of √n is odd exactly when x² − ny² = −1 has a solution, and the smallest such solution then
    /// has a smaller y than the smallest solution of x² − ny² = 1, which always exists for non-square n.
    /// </summary>
    private static bool HasOddPeriodByPell(long n)
    {
        for (long y = 1; ; y++)
        {
            if (NumberTheory.IsSquare(n * y * y - 1))
            {
                return true;
            }

            if (NumberTheory.IsSquare(n * y * y + 1))
            {
                return false;
            }
        }
    }

    /// <summary>The period of √n from the textbook recurrence on (√n + m)/d, stopping when the first state returns.</summary>
    private static int PeriodLengthLonghand(long n)
    {
        var root = (long)Math.Floor(Math.Sqrt(n));
        if (root * root == n)
        {
            return 0;
        }

        var seen = new List<(long M, long D)>();
        long m = 0;
        long d = 1;
        while (true)
        {
            var a = (root + m) / d;
            m = a * d - m;
            d = (n - m * m) / d;
            if (seen.Count > 0 && seen[0] == (m, d))
            {
                return seen.Count;
            }

            seen.Add((m, d));
        }
    }

    /// <summary>The n-th convergent of e, evaluated as a nested fraction from the innermost term outwards.</summary>
    private static BigInteger NumeratorOfEByNestedFractions(int n)
    {
        // 2, then 1, 2k, 1 for k = 1, 2, 3, ...
        var terms = new List<int> { 2 };
        for (var k = 1; terms.Count < n; k++)
        {
            terms.AddRange([1, 2 * k, 1]);
        }

        // a + 1/(numerator/denominator) = (a × numerator + denominator)/numerator, which stays in lowest terms.
        BigInteger numerator = terms[n - 1];
        BigInteger denominator = 1;
        for (var i = n - 2; i >= 0; i--)
        {
            (numerator, denominator) = (terms[i] * numerator + denominator, numerator);
        }

        return numerator;
    }

    private static long MinimalXBySearch(long d)
    {
        for (long y = 1; ; y++)
        {
            var square = d * y * y + 1;
            var x = NumberTheory.ISqrt(square);
            if (x * x == square)
            {
                return x;
            }
        }
    }

    /// <summary>The first convergent h/k of √d with h² − dk² = 1, testing every convergent in turn.</summary>
    private static BigInteger MinimalXByTestingConvergents(int d) =>
        ContinuedFractions.Convergents(ContinuedFractions.SqrtTerms(d))
            .First(c => c.Numerator * c.Numerator - d * c.Denominator * c.Denominator == 1)
            .Numerator;

    /// <summary>The integer square root of a non-negative n, by Newton's iteration from above.</summary>
    private static BigInteger SquareRoot(BigInteger n)
    {
        if (n < 2)
        {
            return n;
        }

        var x = BigInteger.One << (int)((n.GetBitLength() + 1) / 2);
        while (true)
        {
            var next = (x + n / x) >> 1;
            if (next >= x)
            {
                return x;
            }

            x = next;
        }
    }

    private static long BestPathTotal(int[][] triangle, int row, int index) =>
        row == triangle.Length - 1
            ? triangle[row][index]
            : triangle[row][index] + Math.Max(BestPathTotal(triangle, row + 1, index), BestPathTotal(triangle, row + 1, index + 1));

    /// <summary>
    /// Problem 68 over every arrangement of 1..2g: positions [0, g) are the outer numbers, [g, 2g) the inner ring,
    /// and line i reads outer[i], inner[i], inner[i + 1]. Returns the largest description of each length.
    /// </summary>
    private static Dictionary<int, string> LargestRingDescriptionsByLength(int gonSize)
    {
        var best = new Dictionary<int, string>();
        var numbers = Enumerable.Range(1, 2 * gonSize).ToArray();
        do
        {
            var startsAtLowestOuter = true;
            var magic = true;
            for (var i = 1; i < gonSize; i++)
            {
                startsAtLowestOuter &= numbers[i] > numbers[0];
                magic &= LineTotal(i) == LineTotal(0);
            }

            if (!startsAtLowestOuter || !magic)
            {
                continue;
            }

            var description = string.Concat(Enumerable.Range(0, gonSize)
                .Select(i => $"{numbers[i]}{numbers[gonSize + i]}{numbers[gonSize + (i + 1) % gonSize]}"));
            if (!best.TryGetValue(description.Length, out var current) || string.CompareOrdinal(description, current) > 0)
            {
                best[description.Length] = description;
            }
        }
        while (Combinatorics.NextPermutation(numbers));

        return best;

        int LineTotal(int i) => numbers[i] + numbers[gonSize + i] + numbers[gonSize + (i + 1) % gonSize];
    }

    private static int TotientByCounting(int n) => Enumerable.Range(1, n).Count(k => NumberTheory.Gcd(n, k) == 1);

    /// <summary>φ(n) for every n ≤ limit, built multiplicatively: φ(pn) is pφ(n) when p divides n and (p − 1)φ(n) otherwise.</summary>
    private static int[] TotientsBySmallestPrimeFactor(int limit)
    {
        var phi = new int[limit + 1];
        var primes = new List<int>();
        phi[1] = 1;
        for (var n = 2; n <= limit; n++)
        {
            if (phi[n] == 0)
            {
                phi[n] = n - 1;
                primes.Add(n);
            }

            foreach (var p in primes)
            {
                if ((long)p * n > limit)
                {
                    break;
                }

                if (n % p == 0)
                {
                    phi[p * n] = p * phi[n];
                    break;
                }

                phi[p * n] = (p - 1) * phi[n];
            }
        }

        return phi;
    }

    /// <summary>Problem 70 by scanning every n below the limit; a tie keeps the smaller n.</summary>
    private static int BestTotientPermutation(int[] phi, int limit)
    {
        var bestN = 0;
        var bestPhi = 1;
        for (var n = 2; n < limit; n++)
        {
            var improves = bestN == 0 || (long)n * bestPhi < (long)bestN * phi[n];
            if (improves && HaveTheSameDigits(n, phi[n]))
            {
                bestN = n;
                bestPhi = phi[n];
            }
        }

        return bestN;
    }

    private static bool HaveTheSameDigits(int a, int b)
    {
        Span<int> balance = stackalloc int[10];
        for (; a > 0; a /= 10)
        {
            balance[a % 10]++;
        }

        for (; b > 0; b /= 10)
        {
            balance[b % 10]--;
        }

        return balance.IndexOfAnyExcept(0) < 0;
    }
}
