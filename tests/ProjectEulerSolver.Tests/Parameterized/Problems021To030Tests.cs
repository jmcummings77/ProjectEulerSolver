using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>
/// The general solvers of problems 21 to 30: the examples from the problem statements, cross-checks against
/// brute force, argument validation, and arguments beyond the statement's values.
/// </summary>
public class Problems021To030Tests
{
    private static Dictionary<string, string> Arguments(params (string Name, string Value)[] pairs) =>
        pairs.ToDictionary(pair => pair.Name, pair => pair.Value);

    private static int ProperDivisorSum(int n) => Enumerable.Range(1, n / 2).Where(d => n % d == 0).Sum();

    private static bool IsPrime(long n)
    {
        if (n < 2)
        {
            return false;
        }

        for (long d = 2; d * d <= n; d++)
        {
            if (n % d == 0)
            {
                return false;
            }
        }

        return true;
    }

    [Fact]
    public void The_runner_can_bind_every_solver_from_text()
    {
        Assert.Equal(504L, ParameterizedSolver.Solve(new Problem021(), Arguments(("limit", "285"))));
        Assert.Equal(53L, ParameterizedSolver.Solve(new Problem022(), Arguments(("names", "\"COLIN\""))));
        Assert.Equal(301L, ParameterizedSolver.Solve(new Problem023(), Arguments(("limit", "25"))));
        Assert.Equal("201", ParameterizedSolver.Solve(new Problem024(), Arguments(("digits", "012"), ("position", "5"))));
        Assert.Equal(12, ParameterizedSolver.Solve(new Problem025(), Arguments(("digits", "3"))));
        Assert.Equal(7, ParameterizedSolver.Solve(new Problem026(), Arguments(("limit", "11"))));
        Assert.Equal(-41L, ParameterizedSolver.Solve(new Problem027(), Arguments(("aLimit", "2"), ("bLimit", "41"))));
        Assert.Equal(new BigInteger(101), ParameterizedSolver.Solve(new Problem028(), Arguments(("size", "5"))));
        Assert.Equal(15L, ParameterizedSolver.Solve(new Problem029(), Arguments(("maxBase", "5"), ("maxExponent", "5"))));
        Assert.Equal(new BigInteger(19_316), ParameterizedSolver.Solve(new Problem030(), Arguments(("power", "4"))));
    }

    // Problem 21: the statement's pair is 220 and 284.
    [Theory]
    [InlineData(0, 0)]
    [InlineData(220, 0)]
    [InlineData(221, 220)] // 220 counts although its partner 284 is not below the limit.
    [InlineData(284, 220)]
    [InlineData(285, 504)]
    public void Problem021_counts_each_member_of_the_statement_pair_once_it_is_below_the_limit(int limit, long expected) =>
        Assert.Equal(expected, Problem021.Solve(limit));

    [Fact]
    public void Problem021_matches_brute_force_for_every_small_limit()
    {
        const int Largest = 3000;
        long expected = 0;
        for (var limit = 0; limit <= Largest; limit++)
        {
            Assert.Equal(expected, Problem021.Solve(limit));

            var partner = ProperDivisorSum(limit);
            if (partner != limit && ProperDivisorSum(partner) == limit)
            {
                expected += limit;
            }
        }

        Assert.Equal(220 + 284 + 1184 + 1210 + 2620 + 2924, expected);
    }

    [Fact]
    public void Problem021_handles_a_limit_beyond_the_statement()
    {
        const int Limit = 40_000;
        long expected = 0;
        for (var a = 2; a < Limit; a++)
        {
            var b = NumberTheory.ProperDivisorSum(a);
            if (b != a && NumberTheory.ProperDivisorSum(b) == a)
            {
                expected += a;
            }
        }

        Assert.Equal(expected, Problem021.Solve(Limit));
    }

    // Expected values from a separate sieve over every number up to 60 million, with no trial division.
    // 12285 pairs with 14595, so it is counted from limit 12286 on while its partner stays out until 14596.
    [Theory]
    [InlineData(12_285, 53_226)]
    [InlineData(12_286, 65_511)]
    [InlineData(14_595, 65_511)]
    [InlineData(14_596, 80_106)]
    [InlineData(100_000, 852_810)]
    [InlineData(1_000_000, 27_220_963)]
    public void Problem021_matches_independently_computed_sums(int limit, long expected) =>
        Assert.Equal(expected, Problem021.Solve(limit));

    [Theory]
    [InlineData(-1)]
    [InlineData(10_000_001)]
    public void Problem021_rejects_limits_out_of_range(int limit) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem021.Solve(limit));

    // Problem 22: "COLIN, which is worth 3 + 15 + 12 + 9 + 14 = 53, is the 938th name in the list.
    // So, COLIN would obtain a score of 938 × 53 = 49714."
    [Fact]
    public void Problem022_scores_the_statement_example()
    {
        Assert.Equal(53, Problem022.Solve(["COLIN"]));

        var names = Resources.ReadQuotedList("0022_names.txt");
        var before = names.Where(name => string.CompareOrdinal(name, "COLIN") < 0).ToArray();
        Assert.Equal(937, before.Length);
        Assert.Equal(49_714, Problem022.Solve([.. before, "COLIN"]) - Problem022.Solve(before));
    }

    [Fact]
    public void Problem022_sorts_a_copy_and_handles_prefixes_duplicates_and_no_names()
    {
        string[] names = ["AB", "B", "A"];
        Assert.Equal(1 * 1 + 2 * 3 + 3 * 2, Problem022.Solve(names)); // A, AB, B
        Assert.Equal(new[] { "AB", "B", "A" }, names); // The caller's list is left as it was.

        Assert.Equal(1 * 26 + 2 * 26, Problem022.Solve(["Z", "Z"]));
        Assert.Equal(0, Problem022.Solve([]));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 5)]
    [InlineData(3, 40)]
    [InlineData(4, 1000)]
    [InlineData(5, 20_000)] // Several times the size of the statement's list.
    public void Problem022_matches_brute_force_on_generated_names(int seed, int count)
    {
        var random = new Random(seed);
        var names = Enumerable.Range(0, count)
            .Select(_ => string.Concat(Enumerable.Range(0, random.Next(1, 12)).Select(_ => (char)('A' + random.Next(26)))))
            .ToArray();

        var expected = names
            .OrderBy(name => name, StringComparer.Ordinal)
            .Select((name, index) => (index + 1L) * name.Sum(letter => letter - 'A' + 1))
            .Sum();
        Assert.Equal(expected, Problem022.Solve(names));
    }

    [Theory]
    [InlineData("colin")]
    [InlineData("MARY ANN")]
    [InlineData("O'NEIL")]
    [InlineData("")]
    public void Problem022_rejects_names_that_are_not_upper_case_letters(string name)
    {
        var exception = Assert.Throws<ArgumentException>(() => Problem022.Solve(["MARY", name]));
        Assert.Equal("names", exception.ParamName);
    }

    [Fact]
    public void Problem022_rejects_a_missing_list_or_name()
    {
        Assert.Throws<ArgumentNullException>(() => Problem022.Solve(null!));
        Assert.Throws<ArgumentException>(() => Problem022.Solve(["MARY", null!]));
    }

    // Problem 23: "12 is the smallest abundant number ... the smallest number that can be written as the sum of
    // two abundant numbers is 24", and "all integers greater than 28123 can be written as the sum of two abundant numbers".
    [Fact]
    public void Problem023_matches_the_statement()
    {
        Assert.Equal(Enumerable.Range(1, 23).Sum(), Problem023.Solve(23));
        Assert.Equal(Problem023.Solve(23), Problem023.Solve(24));
        Assert.Equal(Problem023.Solve(23) + 25, Problem023.Solve(25));

        // 20161 is in fact the largest number that is not such a sum, so nothing is added beyond it.
        Assert.Equal(Problem023.Solve(20_160) + 20_161, Problem023.Solve(20_161));
        Assert.Equal(Problem023.Solve(20_161), Problem023.Solve(28_123));
        Assert.Equal(Problem023.Solve(28_123), Problem023.Solve(1_000_000));
    }

    [Fact]
    public void Problem023_matches_brute_force_for_small_limits()
    {
        const int Largest = 3000;
        var abundant = Enumerable.Range(1, Largest).Where(n => ProperDivisorSum(n) > n).ToArray();
        var isSum = new bool[2 * Largest + 1];
        foreach (var first in abundant)
        {
            foreach (var second in abundant)
            {
                isSum[first + second] = true;
            }
        }

        long expected = 0;
        for (var limit = 0; limit <= Largest; limit++)
        {
            if (limit > 0 && !isSum[limit])
            {
                expected += limit;
            }

            if (limit <= 100 || limit % 97 == 0 || limit == Largest)
            {
                Assert.Equal(expected, Problem023.Solve(limit));
            }
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10_000_001)]
    public void Problem023_rejects_limits_out_of_range(int limit) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem023.Solve(limit));

    // Problem 24: "The lexicographic permutations of 0, 1 and 2 are: 012 021 102 120 201 210".
    [Fact]
    public void Problem024_lists_the_statement_example_in_order()
    {
        string[] expected = ["012", "021", "102", "120", "201", "210"];
        for (var position = 1; position <= expected.Length; position++)
        {
            Assert.Equal(expected[position - 1], Problem024.Solve("012", position));
            Assert.Equal(expected[position - 1], Problem024.Solve("201", position)); // The input order is irrelevant.
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("x")]
    [InlineData("ba")]
    [InlineData("3120")]
    [InlineData("ecadb")]
    [InlineData("Zz09aA")]
    [InlineData("6543210")]
    public void Problem024_matches_the_sorted_list_of_all_permutations(string characters)
    {
        var all = Combinatorics.Permutations(characters.ToCharArray())
            .Select(permutation => new string(permutation))
            .Order(StringComparer.Ordinal)
            .ToArray();

        for (var position = 1; position <= all.Length; position++)
        {
            Assert.Equal(all[position - 1], Problem024.Solve(characters, position));
        }

        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve(characters, all.Length + 1));
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrst", 1)] // 20 characters: 20! is the last factorial that fits in a long.
    [InlineData("abcdefghijklmnopqrst", 2_432_902_008_176_640_000)] // 20!, the final permutation.
    [InlineData("abcdefghijklmnopqrstu", 2_432_902_008_176_640_000)]
    [InlineData("abcdefghijklmnopqrstu", 2_432_902_008_176_640_001)]
    [InlineData("abcdefghijklmnopqrstu", long.MaxValue)]
    [InlineData("abcdefghijklmnopqrstuv", long.MaxValue)]
    [InlineData("zyxwvutsrqponmlkjihgfedcba9876543210", 1)]
    [InlineData("zyxwvutsrqponmlkjihgfedcba9876543210", 6_227_020_801)] // 13! + 1
    [InlineData("zyxwvutsrqponmlkjihgfedcba9876543210", long.MaxValue)]
    public void Problem024_matches_unbounded_arithmetic_for_long_inputs(string characters, long position)
    {
        // The same walk through the factorial number system, but with factorials of any size and no shortcuts.
        var remaining = characters.Order().ToList();
        var index = new BigInteger(position) - 1;
        var expected = new List<char>();
        while (remaining.Count > 0)
        {
            var blockSize = NumberTheory.Factorial(remaining.Count - 1);
            var pick = (int)(index / blockSize);
            index %= blockSize;
            expected.Add(remaining[pick]);
            remaining.RemoveAt(pick);
        }

        Assert.Equal(new string([.. expected]), Problem024.Solve(characters, position));
    }

    /// <summary>The 1-based position of a permutation, by counting the permutations of its characters that sort before it.</summary>
    private static BigInteger LexicographicPosition(string permutation)
    {
        var codePoints = permutation.EnumerateRunes().Select(rune => rune.Value).ToArray();
        BigInteger position = 1;
        for (var i = 0; i < codePoints.Length; i++)
        {
            // Each smaller character still to come could stand here instead, followed by the rest in any order.
            var smallerLater = codePoints.Skip(i + 1).Count(later => later < codePoints[i]);
            position += smallerLater * NumberTheory.Factorial(codePoints.Length - 1 - i);
        }

        return position;
    }

    [Theory]
    [InlineData("0123456789", 1_000_000)]
    [InlineData("tsrqponmlkjihgfedcba", 2_432_902_008_176_639_999)] // 20! - 1
    [InlineData("abcdefghijklmnopqrstu", 2_432_902_008_176_640_001)] // 20! + 1, with 21 characters
    [InlineData("abcdefghijklmnopqrstu", 7_298_706_024_529_932_345)]
    [InlineData("utsrqponmlkjihgfedcba", long.MaxValue)]
    [InlineData("abcdefghijklmnopqrstuv", long.MaxValue)]
    [InlineData("zyxwvutsrqponmlkjihgfedcba9876543210", long.MaxValue)]
    [InlineData("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 9_000_000_000_000_000_000)]
    public void Problem024_returns_a_permutation_that_stands_at_the_requested_position(string characters, long position)
    {
        var permutation = Problem024.Solve(characters, position);

        Assert.Equal(characters.Order(), permutation.Order());
        Assert.Equal(position, LexicographicPosition(permutation));
    }

    // Expected values from a separate implementation working with unbounded integers.
    [Theory]
    [InlineData("zyxwvutsr", 123_456, "urwvyzxts")]
    [InlineData("abcdefghijklmnopqrstu", long.MaxValue, "dqrmbulenfjtcpikahgos")]
    [InlineData("abcdefghijklmnopqrstu", 2_432_902_008_176_640_000, "autsrqponmlkjihgfedcb")]
    [InlineData("abcdefghijklmnopqrstuv", long.MaxValue, "aersncvmfogkudqjlbihpt")]
    [InlineData("0123456789abcdefghijklmnopqrstuvwxyz", long.MaxValue, "0123456789abcdeivwrgzqjskoyhunpfmltx")]
    public void Problem024_matches_independently_computed_permutations(string characters, long position, string expected) =>
        Assert.Equal(expected, Problem024.Solve(characters, position));

    [Fact]
    public void Problem024_treats_a_surrogate_pair_as_one_character()
    {
        // U+1F600 and U+1F601 share their first UTF-16 code unit, and each takes two.
        const string Grin = "\U0001F600";
        const string Beam = "\U0001F601";

        Assert.Equal("a" + Grin, Problem024.Solve(Grin + "a", 1));
        Assert.Equal(Grin + "a", Problem024.Solve(Grin + "a", 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve(Grin + "a", 3)); // Two characters, so 2! permutations.

        Assert.Equal(Grin + Beam, Problem024.Solve(Beam + Grin, 1));
        Assert.Equal(Beam + Grin, Problem024.Solve(Beam + Grin, 2));

        // Three characters taking one, one and two UTF-16 code units: 1 < é < U+1F600.
        string[] expected = ["1é" + Grin, "1" + Grin + "é", "é1" + Grin, "é" + Grin + "1", Grin + "1é", Grin + "é1"];
        for (var position = 1; position <= expected.Length; position++)
        {
            Assert.Equal(expected[position - 1], Problem024.Solve("é" + Grin + "1", position));
        }

        // Order is by code point: U+FFFD comes before U+1F600 although its code unit is larger than both surrogates.
        Assert.Equal("�" + Grin, Problem024.Solve(Grin + "�", 1));

        var exception = Assert.Throws<ArgumentException>(() => Problem024.Solve(Grin + "a" + Grin, 1));
        Assert.Equal("digits", exception.ParamName);
        Assert.Contains(Grin, exception.Message);
    }

    [Fact]
    public void Problem024_rejects_text_that_is_not_well_formed()
    {
        // Listed here rather than as theory data because an attribute argument cannot hold an unpaired surrogate.
        (string Characters, int Index)[] cases =
        [
            ("a\uD83D", 1), // A high surrogate with nothing after it.
            ("\uDE00a", 0), // A low surrogate with nothing before it.
            ("b\uD83Da\uDE00", 1), // The two halves of a pair, separated.
        ];

        foreach (var (characters, index) in cases)
        {
            var exception = Assert.Throws<ArgumentException>(() => Problem024.Solve(characters, 1));
            Assert.Equal("digits", exception.ParamName);
            Assert.Contains($"unpaired surrogate at index {index}", exception.Message);
        }
    }

    [Fact]
    public void Problem024_knows_the_first_and_last_of_twenty_characters()
    {
        Assert.Equal("abcdefghijklmnopqrst", Problem024.Solve("tsrqponmlkjihgfedcba", 1));
        Assert.Equal("tsrqponmlkjihgfedcba", Problem024.Solve("abcdefghijklmnopqrst", 2_432_902_008_176_640_000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve("abcdefghijklmnopqrst", 2_432_902_008_176_640_001));
    }

    [Fact]
    public void Problem024_rejects_repeated_characters_and_positions_out_of_range()
    {
        var exception = Assert.Throws<ArgumentException>(() => Problem024.Solve("0120", 1));
        Assert.Equal("digits", exception.ParamName);

        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve("012", 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve("012", -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve("012", 7));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem024.Solve("abcdefghijklmnopqrstuvwxyz", 0));
        Assert.Throws<ArgumentNullException>(() => Problem024.Solve(null!, 1));
    }

    // Problem 25: "The 12th term, F12, is the first term to contain three digits."
    [Fact]
    public void Problem025_matches_the_statement_example()
    {
        Assert.Equal(1, Problem025.Solve(1));
        Assert.Equal(7, Problem025.Solve(2)); // F7 = 13
        Assert.Equal(12, Problem025.Solve(3)); // F12 = 144
    }

    [Fact]
    public void Problem025_matches_term_by_term_iteration()
    {
        // Walk the sequence once, checking each digit count as its first term comes up.
        int[] spotChecks = [301, 512, 777, 2500, 5000];
        var largest = spotChecks.Max();
        BigInteger previous = 0;
        BigInteger current = 1;
        var index = 1;
        var threshold = BigInteger.One;
        for (var digits = 1; digits <= largest; digits++)
        {
            while (current < threshold)
            {
                (previous, current) = (current, previous + current);
                index++;
            }

            if (digits <= 300 || spotChecks.Contains(digits))
            {
                Assert.Equal(index, Problem025.Solve(digits));
            }

            threshold *= 10;
        }
    }

    [Fact]
    public void Problem025_handles_a_hundred_thousand_digits() =>
        Assert.Equal(478_495, Problem025.Solve(100_000));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1_000_001)]
    public void Problem025_rejects_digit_counts_out_of_range(int digits) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem025.Solve(digits));

    /// <summary>Length of the recurring cycle of 1/d, by long division until a remainder repeats.</summary>
    private static int RecurringCycleLength(int d)
    {
        var firstSeenAt = new int[d];
        long remainder = 1 % d;
        var step = 1;
        while (remainder != 0 && firstSeenAt[remainder] == 0)
        {
            firstSeenAt[remainder] = step++;
            remainder = remainder * 10 % d;
        }

        return remainder == 0 ? 0 : step - firstSeenAt[remainder];
    }

    // Problem 26: of the unit fractions 1/2 to 1/10, "it can be seen that 1/7 has a 6-digit recurring cycle".
    [Fact]
    public void Problem026_matches_the_statement_example()
    {
        Assert.Equal(6, RecurringCycleLength(7));
        Assert.Equal(7, Problem026.Solve(11));
        Assert.Equal(7, Problem026.Solve(8));
    }

    [Theory]
    [InlineData(2, 1)] // Only 1/1, which does not recur.
    [InlineData(3, 1)] // Neither 1/1 nor 1/2 recurs, so the smaller wins.
    [InlineData(4, 3)]
    [InlineData(7, 3)] // 1/3 and 1/6 both have a one-digit cycle.
    public void Problem026_returns_the_smallest_d_on_ties(int limit, int expected) =>
        Assert.Equal(expected, Problem026.Solve(limit));

    [Fact]
    public void Problem026_matches_brute_force_for_small_and_larger_limits()
    {
        const int Largest = 10_000;
        int[] spotChecks = [1000, 2500, 5000, 9967, 9968, Largest];
        var bestD = 0;
        var bestCycle = -1;
        for (var limit = 2; limit <= Largest; limit++)
        {
            var cycle = RecurringCycleLength(limit - 1);
            if (cycle > bestCycle)
            {
                bestCycle = cycle;
                bestD = limit - 1;
            }

            if (limit <= 400 || spotChecks.Contains(limit))
            {
                Assert.Equal(bestD, Problem026.Solve(limit));
            }
        }
    }

    [Fact]
    public void Problem026_handles_a_limit_of_a_million()
    {
        // 1/999983 has the longest cycle any smaller d could have (d - 1 digits), so only the few larger d could beat it.
        Assert.Equal(999_982, RecurringCycleLength(999_983));
        Assert.All(Enumerable.Range(999_984, 16), d => Assert.True(RecurringCycleLength(d) < 999_982));
        Assert.Equal(999_983, Problem026.Solve(1_000_000));
    }

    // Expected values from cycle lengths computed as the multiplicative order of 10, with no long division.
    [Theory]
    [InlineData(14, 7)] // 1/7 and 1/13 both have six-digit cycles.
    [InlineData(65_536, 65_447)]
    [InlineData(999_983, 999_953)]
    [InlineData(999_984, 999_983)]
    [InlineData(10_000_000, 9_999_943)]
    public void Problem026_matches_independently_computed_answers(int limit, int expected) =>
        Assert.Equal(expected, Problem026.Solve(limit));

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(10_000_001)]
    public void Problem026_rejects_limits_out_of_range(int limit) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem026.Solve(limit));

    /// <summary>Tries every pair in range, in the documented tie-break order: smallest b, then smallest a.</summary>
    private static long BestQuadraticByBruteForce(int aLimit, int bLimit)
    {
        long bestProduct = 0;
        var bestRun = 0;
        for (var b = -bLimit; b <= bLimit; b++)
        {
            for (var a = 1 - aLimit; a < aLimit; a++)
            {
                var n = 0;
                while (IsPrime((long)n * n + (long)a * n + b))
                {
                    n++;
                }

                if (n > bestRun)
                {
                    bestRun = n;
                    bestProduct = (long)a * b;
                }
            }
        }

        return bestProduct;
    }

    // Problem 27: n² + n + 41 yields 40 primes for n = 0..39, and n² - 79n + 1601 yields 80 primes for n = 0..79.
    [Fact]
    public void Problem027_finds_the_quadratics_from_the_statement()
    {
        Assert.All(Enumerable.Range(0, 40), n => Assert.True(IsPrime(n * n + n + 41)));
        Assert.False(IsPrime(40 * 40 + 40 + 41));
        Assert.All(Enumerable.Range(0, 80), n => Assert.True(IsPrime(n * n - 79 * n + 1601)));
        Assert.False(IsPrime(80 * 80 - 79 * 80 + 1601));

        // With a = 1 allowed so is a = -1, and n² - n + 41 repeats 41 at n = 1 before following n² + n + 41: 41 primes.
        Assert.Equal(-41, Problem027.Solve(aLimit: 2, bLimit: 41));

        // Nothing with |a| < 80 and |b| ≤ 1601 beats the 80 primes of n² - 79n + 1601.
        Assert.Equal(-79 * 1601, Problem027.Solve(aLimit: 80, bLimit: 1601));
        Assert.NotEqual(-79 * 1601, Problem027.Solve(aLimit: 79, bLimit: 1601));
        Assert.NotEqual(-79 * 1601, Problem027.Solve(aLimit: 80, bLimit: 1600));
    }

    [Fact]
    public void Problem027_matches_brute_force_on_a_grid_of_small_limits()
    {
        for (var aLimit = 1; aLimit <= 12; aLimit++)
        {
            for (var bLimit = 2; bLimit <= 45; bLimit++)
            {
                Assert.Equal(BestQuadraticByBruteForce(aLimit, bLimit), Problem027.Solve(aLimit, bLimit));
            }
        }
    }

    [Theory]
    [InlineData(1, 2, 0)] // n² + 2 gives 2, 3: the only choice with a = 0 that reaches two primes.
    [InlineData(2, 2, -2)] // n² - n + 2 and n² + 2 both give two primes; the smaller a wins.
    [InlineData(1, 50, 0)]
    [InlineData(3, 3, -6)] // n² - 2n + 3 gives 3, 2, 3.
    public void Problem027_breaks_ties_by_smallest_b_then_smallest_a(int aLimit, int bLimit, long expected)
    {
        Assert.Equal(expected, Problem027.Solve(aLimit, bLimit));
        Assert.Equal(expected, BestQuadraticByBruteForce(aLimit, bLimit));
    }

    [Theory]
    [InlineData(2000, 2000)]
    [InlineData(40, 3000)]
    [InlineData(3000, 40)]
    public void Problem027_matches_brute_force_beyond_the_statement(int aLimit, int bLimit) =>
        Assert.Equal(BestQuadraticByBruteForce(aLimit, bLimit), Problem027.Solve(aLimit, bLimit));

    [Fact]
    public void Problem027_keeps_the_same_winner_far_beyond_the_statement() =>
        Assert.Equal(-79 * 1601, Problem027.Solve(aLimit: 30_000, bLimit: 30_000));

    // Expected values from a separate search over every pair in range.
    [Theory]
    [InlineData(79, 1601, -77 * 1523)] // 79 primes, one short of the statement's quadratic, which is just out of range.
    [InlineData(5000, 3, -6)] // 109 pairs yield three primes; n² - 2n + 3 has the smallest b and then the smallest a.
    [InlineData(20_000, 200, -25 * 197)]
    [InlineData(100_000, 2, -2)]
    [InlineData(1, 100_000, 0)]
    [InlineData(100_000, 100_000, -79 * 1601)] // Both limits at their largest.
    public void Problem027_matches_independently_computed_answers(int aLimit, int bLimit, long expected) =>
        Assert.Equal(expected, Problem027.Solve(aLimit, bLimit));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Problem027_has_no_answer_when_no_b_is_prime(int bLimit) =>
        Assert.Throws<InvalidOperationException>(() => Problem027.Solve(aLimit: 1000, bLimit));

    [Theory]
    [InlineData(0, 1000)]
    [InlineData(-1, 1000)]
    [InlineData(100_001, 1000)]
    [InlineData(1000, -1)]
    [InlineData(1000, 100_001)]
    public void Problem027_rejects_limits_out_of_range(int aLimit, int bLimit) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem027.Solve(aLimit, bLimit));

    // Problem 28: the statement's 5 by 5 spiral has diagonals adding up to 101.
    [Fact]
    public void Problem028_matches_the_statement_example() =>
        Assert.Equal(101, Problem028.Solve(5));

    [Fact]
    public void Problem028_matches_a_spiral_built_cell_by_cell()
    {
        for (var size = 1; size <= 41; size += 2)
        {
            // Start in the centre and walk right, down, left, up, with runs of 1, 1, 2, 2, 3, 3, ... cells.
            var grid = new int[size, size];
            var (row, column) = (size / 2, size / 2);
            (int Row, int Column)[] directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];
            var value = 1;
            grid[row, column] = value;
            for (var run = 0; value < size * size; run++)
            {
                var (rowStep, columnStep) = directions[run % 4];
                for (var step = 0; step < run / 2 + 1 && value < size * size; step++)
                {
                    row += rowStep;
                    column += columnStep;
                    grid[row, column] = ++value;
                }
            }

            if (size == 5)
            {
                Assert.Equal(new[] { 21, 22, 23, 24, 25 }, Enumerable.Range(0, 5).Select(c => grid[0, c]));
                Assert.Equal(new[] { 17, 16, 15, 14, 13 }, Enumerable.Range(0, 5).Select(c => grid[4, c]));
            }

            var expected = Enumerable.Range(0, size).Sum(i => (long)grid[i, i] + grid[i, size - 1 - i]) - 1; // The centre is on both.
            Assert.Equal(expected, Problem028.Solve(size));
        }
    }

    [Fact]
    public void Problem028_matches_ring_by_ring_summation_for_large_sizes()
    {
        BigInteger total = 1;
        BigInteger corner = 1;
        for (var side = 3; side <= 100_001; side += 2)
        {
            for (var i = 0; i < 4; i++)
            {
                corner += side - 1;
                total += corner;
            }

            if (side is 1001 or 4097 or 100_001)
            {
                Assert.Equal(total, Problem028.Solve(side));
            }
        }

        // The well-known cubic for the same sum, evaluated at the largest odd int.
        BigInteger largest = int.MaxValue;
        Assert.Equal((4 * largest * largest * largest + 3 * largest * largest + 8 * largest - 9) / 6, Problem028.Solve(int.MaxValue));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-3)]
    [InlineData(2)]
    [InlineData(1000)]
    public void Problem028_rejects_sizes_that_are_not_positive_and_odd(int size) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem028.Solve(size));

    // Problem 29: for 2 ≤ a ≤ 5 and 2 ≤ b ≤ 5 the statement lists 15 distinct terms.
    [Fact]
    public void Problem029_matches_the_statement_example() =>
        Assert.Equal(15, Problem029.Solve(maxBase: 5, maxExponent: 5));

    private static int DistinctPowersByBruteForce(int maxBase, int maxExponent)
    {
        var powers = new HashSet<BigInteger>();
        for (var a = 2; a <= maxBase; a++)
        {
            for (var b = 2; b <= maxExponent; b++)
            {
                powers.Add(BigInteger.Pow(a, b));
            }
        }

        return powers.Count;
    }

    [Fact]
    public void Problem029_matches_brute_force_on_a_grid_of_small_limits()
    {
        for (var maxBase = 2; maxBase <= 36; maxBase++)
        {
            for (var maxExponent = 2; maxExponent <= 20; maxExponent++)
            {
                Assert.Equal(DistinctPowersByBruteForce(maxBase, maxExponent), Problem029.Solve(maxBase, maxExponent));
            }
        }
    }

    [Theory]
    [InlineData(64, 200)]
    [InlineData(200, 37)]
    [InlineData(260, 260)]
    public void Problem029_matches_brute_force_beyond_the_statement(int maxBase, int maxExponent) =>
        Assert.Equal(DistinctPowersByBruteForce(maxBase, maxExponent), Problem029.Solve(maxBase, maxExponent));

    [Theory]
    [InlineData(1000, 1000)]
    [InlineData(4100, 150)] // Includes 4096 = 2^12, the twelfth power of its root.
    public void Problem029_matches_a_count_of_root_and_exponent_pairs(int maxBase, int maxExponent)
    {
        // Write each base as root^k with the smallest possible root; a^b is then identified by (root, k·b).
        var seen = new HashSet<(int Root, int Exponent)>();
        for (var a = 2; a <= maxBase; a++)
        {
            var (root, k) = (a, 1);
            for (var candidate = 2; candidate * candidate <= a; candidate++)
            {
                var power = candidate;
                var exponent = 1;
                while (power < a)
                {
                    power *= candidate;
                    exponent++;
                }

                if (power == a)
                {
                    (root, k) = (candidate, exponent);
                    break;
                }
            }

            for (var b = 2; b <= maxExponent; b++)
            {
                seen.Add((root, k * b));
            }
        }

        Assert.Equal(seen.Count, Problem029.Solve(maxBase, maxExponent));
    }

    [Fact]
    public void Problem029_handles_the_extremes_of_its_range()
    {
        Assert.Equal(1, Problem029.Solve(2, 2));
        Assert.Equal(999_999, Problem029.Solve(2, 1_000_000));

        // Squares of different bases are different.
        Assert.Equal(int.MaxValue - 1L, Problem029.Solve(int.MaxValue, 2));

        // Bases 2 and 4 share the powers of two with exponents 2..E and the even exponents up to 2E; base 3 adds E - 1.
        const int E = 1_000_000;
        Assert.Equal((E - 1) + (E / 2) + (E - 1), Problem029.Solve(4, E));
    }

    // Expected values from a separate count that finds each base's root by integer root extraction.
    [Theory]
    [InlineData(1000, 1000, 977_358)]
    [InlineData(65_535, 100, 6_473_059)]
    [InlineData(65_536, 100, 6_473_091)] // 65536 = 2^16 = 4^8 = 16^4 = 256^2.
    [InlineData(100_000, 1000, 99_713_710)]
    [InlineData(1_073_741_824, 30_000, 32_210_672_788_051)] // 2^30, the largest power of two that is an int.
    [InlineData(int.MaxValue, 999_999, 2_147_455_483_914_268)]
    [InlineData(int.MaxValue, 1_000_000, 2_147_457_631_351_312)] // Both limits at their largest.
    public void Problem029_matches_independently_computed_counts(int maxBase, int maxExponent, long expected) =>
        Assert.Equal(expected, Problem029.Solve(maxBase, maxExponent));

    [Theory]
    [InlineData(1, 100)]
    [InlineData(0, 100)]
    [InlineData(-5, 100)]
    [InlineData(100, 1)]
    [InlineData(100, -1)]
    [InlineData(100, 1_000_001)]
    public void Problem029_rejects_limits_out_of_range(int maxBase, int maxExponent) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem029.Solve(maxBase, maxExponent));

    // Problem 30: "1634, 8208 and 9474 ... The sum of these numbers is 19316" for fourth powers.
    [Fact]
    public void Problem030_matches_the_statement_example() =>
        Assert.Equal(1634 + 8208 + 9474, Problem030.Solve(4));

    [Theory]
    [InlineData(1)] // No number with two or more digits equals its digit sum, so the total is zero.
    [InlineData(2)] // Nor the sum of the squares of its digits.
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void Problem030_matches_brute_force(int power)
    {
        // A number with power + 2 digits or more is too big: 10^(power + 1) > (power + 2)·9^power.
        var limit = (long)BigInteger.Pow(10, power + 1);
        Assert.True(limit > (power + 2) * BigInteger.Pow(9, power));

        var digitPowers = Enumerable.Range(0, 10).Select(digit => (long)BigInteger.Pow(digit, power)).ToArray();
        long expected = 0;
        for (long n = 10; n < limit; n++)
        {
            long sum = 0;
            for (var rest = n; rest > 0; rest /= 10)
            {
                sum += digitPowers[rest % 10];
            }

            if (sum == n)
            {
                expected += n;
            }
        }

        Assert.Equal(expected, Problem030.Solve(power));
        Assert.Equal(power < 3, Problem030.Solve(power).IsZero);
    }

    [Theory]
    [InlineData(7, 1_741_725L + 4_210_818 + 9_800_817 + 9_926_315 + 14_459_929)]
    [InlineData(8, 24_678_050L + 24_678_051 + 88_593_477)]
    [InlineData(9, 146_511_208L + 472_335_975 + 534_494_836 + 912_985_153)]
    [InlineData(10, 4_679_307_774)]
    [InlineData(17, 233_411_150_132_317 + 21_897_142_587_612_075 + 35_641_594_208_964_132 + 35_875_699_062_250_035)]
    [InlineData(18, 0)] // The largest supported power happens to have no such numbers.
    public void Problem030_finds_the_known_numbers_for_higher_powers(int power, long expected) =>
        Assert.Equal(expected, Problem030.Solve(power));

    // Expected values from a separate search over digit strings of each exact length, zeros included.
    [Theory]
    [InlineData(11, 418_030_478_906)]
    [InlineData(12, 0)]
    [InlineData(13, 564_240_140_138)]
    [InlineData(14, 28_116_440_335_967)]
    [InlineData(15, 0)]
    [InlineData(16, 8_676_563_538_782_741)]
    public void Problem030_matches_independently_computed_sums(int power, long expected) =>
        Assert.Equal(expected, Problem030.Solve(power));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(19)]
    public void Problem030_rejects_powers_out_of_range(int power) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem030.Solve(power));
}
