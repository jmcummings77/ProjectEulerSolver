using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>The general solvers of problems 51 to 60, checked against their statements' examples and brute force.</summary>
public class Problems051To060Tests
{
    private static readonly string[] PokerDeals = Resources.ReadLines("0054_poker.txt");

    // One hand per category, weakest first. The low hands and the high hands share no card.
    private static readonly string[] LowHands =
    [
        "2C 4D 5C 6D 7C", // High card.
        "2C 2D 4C 5D 7C", // One pair.
        "2C 2D 4C 4D 7C", // Two pairs.
        "2C 2D 2H 5D 7C", // Three of a kind.
        "2C 3D 4C 5D 6C", // Straight.
        "2C 3C 4C 5C 7C", // Flush.
        "2C 2D 2H 4C 4D", // Full house.
        "2C 2D 2H 2S 4C", // Four of a kind.
        "2C 3C 4C 5C 6C", // Straight flush.
    ];

    private static readonly string[] HighHands =
    [
        "8H TS JH QS AH",
        "8H 8S TH JS AH",
        "8H 8S TH TS AH",
        "8H 8S 8D JS AH",
        "9H TS JH QS KH",
        "8H 9H TH JH KH",
        "8H 8S 8D TH TS",
        "8H 8S 8D 8C TH",
        "9H TH JH QH KH",
    ];

    private const string PlainText =
        "The lighthouse keeper climbed the stairs every evening, lit the lamp and wrote the weather in a ledger. "
        + "Ships passing in the night never knew his name; they only saw the light, and steered by it. "
        + "When the storm of 1887 took the roof, he rebuilt it himself (twice!) and went on writing: wind, tide, cloud.";

    private static Dictionary<string, string> Arguments(params (string Name, string Value)[] pairs) =>
        pairs.ToDictionary(pair => pair.Name, pair => pair.Value);

    [Fact]
    public void Runner_sees_the_parameters_of_every_problem()
    {
        Assert.Equal("familySize", ParameterizedSolver.Signature(new Problem051()));
        Assert.Equal("maxMultiplier", ParameterizedSolver.Signature(new Problem052()));
        Assert.Equal("maxN, threshold", ParameterizedSolver.Signature(new Problem053()));
        Assert.Equal("hands", ParameterizedSolver.Signature(new Problem054()));
        Assert.Equal("limit, maxIterations", ParameterizedSolver.Signature(new Problem055()));
        Assert.Equal("maxBase, maxExponent", ParameterizedSolver.Signature(new Problem056()));
        Assert.Equal("expansions", ParameterizedSolver.Signature(new Problem057()));
        Assert.Equal("percent", ParameterizedSolver.Signature(new Problem058()));
        Assert.Equal("cipher, keyLength", ParameterizedSolver.Signature(new Problem059()));
        Assert.Equal("setSize", ParameterizedSolver.Signature(new Problem060()));
    }

    [Fact]
    public void Runner_binds_text_arguments()
    {
        Assert.Equal(4, ParameterizedSolver.Solve(new Problem053(), Arguments(("maxN", "23"), ("threshold", "1_000_000"))));
        Assert.Equal(1, ParameterizedSolver.Solve(new Problem054(), Arguments(("hands", "5D 8C 9S JS AC 2C 5C 7D 8S QH"))));
        Assert.Equal(
            PlainText.Sum(c => (long)c),
            ParameterizedSolver.Solve(new Problem059(), Arguments(("cipher", string.Join(", ", Encrypt(PlainText, "k"))), ("keyLength", "1"))));
    }

    // ---- Problem 51 ----

    [Fact]
    public void Problem051_matches_the_statement_examples()
    {
        Assert.Equal(13, Problem051.Solve(6)); // *3 gives the six primes 13, 23, 43, 53, 73 and 83.
        Assert.Equal(56003, Problem051.Solve(7)); // 56**3 is the first family of seven.
    }

    [Fact]
    public void Problem051_matches_a_search_of_every_pattern()
    {
        // Every pattern of two to six characters with at least one wildcard and at least one fixed digit.
        const int MaxLength = 6;
        var isPrime = Primes.Sieve(1_000_000);
        var smallest = new Dictionary<int, int>();
        for (var length = 2; length <= MaxLength; length++)
        {
            Fill(length, 0, 0, 0, false);
        }

        for (var familySize = 2; familySize <= 8; familySize++)
        {
            Assert.Equal(smallest[familySize], Problem051.Solve(familySize));
        }

        // fixedPart is the number with zeros in the wildcard positions; wildcardPart has ones there instead.
        void Fill(int remaining, int fixedPart, int wildcardPart, int fixedDigits, bool leadingWildcard)
        {
            if (remaining == 0)
            {
                if (wildcardPart == 0 || fixedDigits == 0)
                {
                    return;
                }

                // Members in ascending order, so the first prime met is the family's smallest.
                var size = 0;
                var least = 0;
                for (var digit = leadingWildcard ? 1 : 0; digit <= 9; digit++)
                {
                    var member = fixedPart + (digit * wildcardPart);
                    if (isPrime[member])
                    {
                        least = size == 0 ? member : least;
                        size++;
                    }
                }

                if (size > 0 && (!smallest.TryGetValue(size, out var known) || least < known))
                {
                    smallest[size] = least;
                }

                return;
            }

            var first = fixedPart == 0 && wildcardPart == 0;
            Fill(remaining - 1, fixedPart * 10, wildcardPart * 10 + 1, fixedDigits, leadingWildcard || first);
            for (var digit = first ? 1 : 0; digit <= 9; digit++)
            {
                Fill(remaining - 1, fixedPart * 10 + digit, wildcardPart * 10, fixedDigits + 1, leadingWildcard);
            }
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(9)]
    public void Problem051_rejects_unsupported_family_sizes(int familySize) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem051.Solve(familySize));

    // ---- Problem 52 ----

    [Fact]
    public void Problem052_matches_the_statement_example() =>
        Assert.Equal(125874, Problem052.Solve(2)); // 125874 and its double 251748 contain the same digits.

    [Fact]
    public void Problem052_matches_brute_force()
    {
        // One pass over x records, for each multiplier, the first x whose multiples up to it are all rearrangements.
        var first = new Dictionary<int, long>();
        for (long x = 1; first.Count < 5; x++)
        {
            var key = SortedDigits(x);
            for (var multiplier = 2; multiplier <= 6 && SortedDigits(multiplier * x) == key; multiplier++)
            {
                first.TryAdd(multiplier, x);
            }
        }

        for (var maxMultiplier = 2; maxMultiplier <= 6; maxMultiplier++)
        {
            Assert.Equal(first[maxMultiplier], Problem052.Solve(maxMultiplier));
        }

        static string SortedDigits(long n)
        {
            var digits = n.ToString().ToCharArray();
            Array.Sort(digits);
            return new string(digits);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(7)]
    public void Problem052_rejects_unsupported_multipliers(int maxMultiplier) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem052.Solve(maxMultiplier));

    // ---- Problem 53 ----

    [Fact]
    public void Problem053_matches_the_statement_examples()
    {
        // C(5, 3) = 10, and with C(5, 2) it is one of the only two values above 9 in the first five rows.
        Assert.Equal(2, Problem053.Solve(5, 9));
        Assert.Equal(0, Problem053.Solve(5, 10));

        // "It is not until n = 23 that a value exceeds one-million": C(23, 10) to C(23, 13).
        Assert.Equal(0, Problem053.Solve(22, 1_000_000));
        Assert.Equal(4, Problem053.Solve(23, 1_000_000));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(9)]
    [InlineData(100)]
    [InlineData(1_000_000)]
    [InlineData(1_000_000_000_000)]
    [InlineData(long.MaxValue - 1)]
    [InlineData(long.MaxValue)]
    public void Problem053_matches_brute_force(long threshold)
    {
        // Exact rows of Pascal's triangle; maxN = 400 is well past the statement's 100.
        var expected = 0;
        var row = new BigInteger[] { 1 };
        Assert.Equal(0, Problem053.Solve(0, threshold));
        for (var n = 1; n <= 400; n++)
        {
            var next = new BigInteger[n + 1];
            for (var r = 0; r <= n; r++)
            {
                next[r] = (r > 0 ? row[r - 1] : 0) + (r < n ? row[r] : 0);
            }

            row = next;
            expected += row.Count(value => value > threshold);
            if (n <= 70 || n % 50 == 0)
            {
                Assert.Equal(expected, Problem053.Solve(n, threshold));
            }
        }
    }

    [Fact]
    public void Problem053_counts_every_entry_above_a_zero_threshold() =>
        Assert.Equal((10_001 * 10_002 / 2) - 1, Problem053.Solve(10_000, 0)); // Rows 1 to 10,000 of the triangle.

    [Theory]
    [InlineData(1)]
    [InlineData(5_000)]
    [InlineData(1_000_000)]
    [InlineData(long.MaxValue)]
    public void Problem053_matches_a_count_by_symmetry_at_the_largest_maxN(long threshold)
    {
        // A row rises to its middle and mirrors itself, so its entries above the threshold are exactly those from
        // the first r that exceeds it to n - r. Only that first stretch of each row is computed, by C(n, r) =
        // C(n, r - 1) × (n - r + 1) / r, which keeps maxN = 10,000 affordable.
        const int MaxN = 10_000;
        var expected = 0;
        for (var n = 1; n <= MaxN; n++)
        {
            BigInteger value = 1;
            var r = 0;
            while (r <= n / 2 && value <= threshold)
            {
                r++;
                value = value * (n - r + 1) / r;
            }

            expected += r <= n / 2 ? n - (2 * r) + 1 : 0;
            if (n is 1 or 2 or 99 or 100 or 101 or 1_000)
            {
                Assert.Equal(expected, Problem053.Solve(n, threshold));
            }
        }

        Assert.Equal(expected, Problem053.Solve(MaxN, threshold));
    }

    [Fact]
    public void Problem053_rejects_out_of_range_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem053.Solve(-1, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem053.Solve(10_001, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem053.Solve(10, -1));
    }

    // ---- Problem 54 ----

    [Theory] // The five example deals in the statement.
    [InlineData("5H 5C 6S 7S KD 2C 3S 8S 8D TD", 0)]
    [InlineData("5D 8C 9S JS AC 2C 5C 7D 8S QH", 1)]
    [InlineData("2D 9C AS AH AC 3D 6D 7D TD QD", 0)]
    [InlineData("4D 6S 9H QH QC 3D 6D 7H QD QS", 1)]
    [InlineData("2H 2D 4C 4D 4S 3C 3D 3S 9S 9D", 1)]
    public void Problem054_matches_the_statement_examples(string deal, int expectedWins) =>
        Assert.Equal(expectedWins, Problem054.Solve([deal]));

    [Fact]
    public void Problem054_counts_wins_over_several_deals()
    {
        string[] examples =
        [
            "5H 5C 6S 7S KD 2C 3S 8S 8D TD",
            "5D 8C 9S JS AC 2C 5C 7D 8S QH",
            "2D 9C AS AH AC 3D 6D 7D TD QD",
            "4D 6S 9H QH QC 3D 6D 7H QD QS",
            "2H 2D 4C 4D 4S 3C 3D 3S 9S 9D",
        ];
        Assert.Equal(3, Problem054.Solve(examples));
        Assert.Equal(0, Problem054.Solve([]));

        // Three times the statement's thousand deals.
        string[] tripled = [.. PokerDeals, .. PokerDeals, .. PokerDeals];
        Assert.Equal(3 * Problem054.Solve(PokerDeals), Problem054.Solve(tripled));
    }

    [Fact]
    public void Problem054_ranks_every_category_above_the_ones_below_it()
    {
        for (var low = 0; low < LowHands.Length; low++)
        {
            for (var high = 0; high < HighHands.Length; high++)
            {
                // Within a category the high hand wins on its ranks.
                Assert.Equal(low > high ? 1 : 0, Problem054.Solve([$"{LowHands[low]} {HighHands[high]}"]));
                Assert.Equal(high >= low ? 1 : 0, Problem054.Solve([$"{HighHands[high]} {LowHands[low]}"]));
            }
        }
    }

    [Fact]
    public void Problem054_agrees_with_an_independent_evaluator()
    {
        var deck = "23456789TJQKA".SelectMany(rank => "CDHS".Select(suit => $"{rank}{suit}")).ToArray();
        var random = new Random(54);
        var deals = PokerDeals.ToList();
        for (var i = 0; i < 4000; i++)
        {
            random.Shuffle(deck);
            deals.Add(string.Join(' ', deck[..10]));
        }

        // Sorting each player's cards must not matter either; it also puts a few hands in a different order.
        foreach (var deal in deals)
        {
            var cards = deal.Split(' ');
            var expected = HandStrength(cards[..5]) > HandStrength(cards[5..]) ? 1 : 0;
            Assert.Equal(expected, Problem054.Solve([deal]));
            Assert.Equal(expected, Problem054.Solve([string.Join(' ', cards[..5].Order().Concat(cards[5..].Order()))]));
        }

        Assert.Equal(deals.Count(deal => HandStrength(deal.Split(' ')[..5]) > HandStrength(deal.Split(' ')[5..])), Problem054.Solve(deals));
    }

    [Fact]
    public void Problem054_gives_a_tied_deal_to_neither_player()
    {
        Assert.Equal(0, Problem054.Solve(["2H 3D 5S 9C KD 2C 3H 5D 9S KH"]));
        Assert.Equal(0, Problem054.Solve(["2C 3H 5D 9S KH 2H 3D 5S 9C KD"]));
    }

    [Fact]
    public void Problem054_tolerates_extra_spaces() =>
        Assert.Equal(1, Problem054.Solve(["  5D 8C 9S JS AC   2C 5C 7D 8S QH "]));

    [Theory]
    [InlineData("")]
    [InlineData("5D 8C 9S JS AC 2C 5C 7D 8S")] // Nine cards.
    [InlineData("5D 8C 9S JS AC 2C 5C 7D 8S QH KH")] // Eleven cards.
    [InlineData("1D 8C 9S JS AC 2C 5C 7D 8S QH")] // No such rank.
    [InlineData("5X 8C 9S JS AC 2C 5C 7D 8S QH")] // No such suit.
    [InlineData("5d 8C 9S JS AC 2C 5C 7D 8S QH")] // Lowercase.
    [InlineData("10D 8C 9S JS AC 2C 5C 7D 8S QH")] // Ten is written T.
    [InlineData("5D 8C 9S JS AC 2C 5D 7D 8S QH")] // 5D dealt twice.
    public void Problem054_rejects_malformed_deals(string deal)
    {
        var exception = Assert.Throws<ArgumentException>(() => Problem054.Solve(["5D 8C 9S JS AC 2C 5C 7D 8S QH", deal]));
        Assert.Equal("hands", exception.ParamName);
        Assert.Contains("Deal 2", exception.Message);
    }

    [Fact]
    public void Problem054_rejects_a_missing_deal()
    {
        var exception = Assert.Throws<ArgumentException>(() => Problem054.Solve(["5D 8C 9S JS AC 2C 5C 7D 8S QH", null!]));
        Assert.Equal("hands", exception.ParamName);
        Assert.Contains("Deal 2", exception.Message);
    }

    [Fact]
    public void Problem054_rejects_a_missing_list() =>
        Assert.Throws<ArgumentNullException>(() => Problem054.Solve(null!));

    /// <summary>
    /// A deliberately different evaluator: the number of equal-rank pairs among the five cards identifies the
    /// pattern (0 none, 1 one pair, 2 two pairs, 3 three of a kind, 4 full house, 6 four of a kind), and the
    /// result packs category and tie-break ranks into one number.
    /// </summary>
    private static long HandStrength(string[] cards)
    {
        var ranks = cards.Select(card => "23456789TJQKA".IndexOf(card[0]) + 2).OrderDescending().ToArray();
        var flush = cards.All(card => card[1] == cards[0][1]);
        var equalPairs = 0;
        for (var i = 0; i < 5; i++)
        {
            for (var j = i + 1; j < 5; j++)
            {
                equalPairs += ranks[i] == ranks[j] ? 1 : 0;
            }
        }

        var wheel = ranks.SequenceEqual([14, 5, 4, 3, 2]);
        var straight = equalPairs == 0 && (ranks[0] - ranks[4] == 4 || wheel);
        var category = straight && flush ? 8
            : equalPairs == 6 ? 7
            : equalPairs == 4 ? 6
            : flush ? 5
            : straight ? 4
            : equalPairs;

        var tieBreak = wheel
            ? [5, 4, 3, 2, 1]
            : ranks.OrderByDescending(rank => ranks.Count(other => other == rank)).ThenByDescending(rank => rank).ToArray();
        return tieBreak.Aggregate((long)category, (strength, rank) => strength * 15 + rank);
    }

    // ---- Problem 55 ----

    [Fact]
    public void Problem055_matches_the_statement_examples()
    {
        Assert.False(IsCounted(47, 1)); // 47 + 74 = 121.
        Assert.True(IsCounted(349, 2)); // 349 takes three iterations to arrive at 7337.
        Assert.False(IsCounted(349, 3));
        Assert.Equal(0, Problem055.Solve(196, 50)); // 196 is the first Lychrel number.
        Assert.True(IsCounted(196, 50));
        Assert.True(IsCounted(4994, 50)); // A palindrome that is itself a Lychrel number.
        Assert.True(IsCounted(10677, 52)); // 10677 is the first to need over fifty iterations: 53 of them.
        Assert.False(IsCounted(10677, 53));

        static bool IsCounted(int number, int maxIterations) =>
            Problem055.Solve(number + 1, maxIterations) - Problem055.Solve(number, maxIterations) == 1;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(24)]
    [InlineData(50)]
    [InlineData(120)] // Past the statement's fifty; the numbers reach about sixty digits.
    public void Problem055_matches_brute_force(int maxIterations)
    {
        var expected = 0;
        for (var number = 1; number <= 12_000; number++)
        {
            if (number is 1 or 2 or 11 or 90 or 197 or 1_000 or 4_995 or 12_000)
            {
                Assert.Equal(expected, Problem055.Solve(number, maxIterations));
            }

            BigInteger value = number;
            var palindrome = false;
            for (var iteration = 0; iteration < maxIterations && !palindrome; iteration++)
            {
                value += BigInteger.Parse(string.Concat(value.ToString().Reverse()));
                palindrome = value.ToString().SequenceEqual(value.ToString().Reverse());
            }

            expected += palindrome ? 0 : 1;
        }
    }

    [Fact]
    public void Problem055_matches_a_separate_implementation_at_the_largest_arguments()
    {
        // Counted independently with arbitrary-precision integers and string reversal.
        Assert.Equal(6_208, Problem055.Solve(100_000, 50));
        Assert.Equal(118_689, Problem055.Solve(1_000_000, 50));
        Assert.Equal(39, Problem055.Solve(3_000, 500)); // The numbers pass two hundred digits.
    }

    [Fact]
    public void Problem055_rejects_out_of_range_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem055.Solve(0, 50));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem055.Solve(1_000_001, 50));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem055.Solve(100, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem055.Solve(100, 501));
    }

    // ---- Problem 56 ----

    [Fact]
    public void Problem056_matches_small_cases_worked_by_hand()
    {
        Assert.Equal(1, Problem056.Solve(2, 2)); // Only 1^1.
        Assert.Equal(9, Problem056.Solve(10, 2)); // 9^1.
        Assert.Equal(8, Problem056.Solve(3, 5)); // 2, 4, 8, 16.
        Assert.Equal(13, Problem056.Solve(11, 3)); // 7^2 = 49; the statement's point is that 10^b only sums to 1.
    }

    [Fact]
    public void Problem056_matches_brute_force()
    {
        // best[a, b] is the largest digit sum over bases up to a and exponents up to b, from exact powers.
        const int Size = 40;
        var best = new int[Size + 1, Size + 1];
        for (var a = 1; a <= Size; a++)
        {
            for (var b = 1; b <= Size; b++)
            {
                var digitSum = BigInteger.Pow(a, b).ToString().Sum(c => c - '0');
                best[a, b] = Math.Max(digitSum, Math.Max(best[a - 1, b], best[a, b - 1]));
            }
        }

        for (var maxBase = 2; maxBase <= Size + 1; maxBase++)
        {
            for (var maxExponent = 2; maxExponent <= Size + 1; maxExponent++)
            {
                Assert.Equal(best[maxBase - 1, maxExponent - 1], Problem056.Solve(maxBase, maxExponent));
            }
        }
    }

    [Theory]
    [InlineData(150, 150)]
    [InlineData(1000, 12)]
    [InlineData(12, 1000)]
    public void Problem056_matches_brute_force_past_the_statement_values(int maxBase, int maxExponent)
    {
        var expected = 0;
        for (var a = 1; a < maxBase; a++)
        {
            for (var b = 1; b < maxExponent; b++)
            {
                expected = Math.Max(expected, BigInteger.Pow(a, b).ToString().Sum(c => c - '0'));
            }
        }

        Assert.Equal(expected, Problem056.Solve(maxBase, maxExponent));
    }

    [Fact]
    public void Problem056_rejects_out_of_range_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem056.Solve(1, 100));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem056.Solve(100, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem056.Solve(1_001, 100));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem056.Solve(100, 1_001));
    }

    // ---- Problem 57 ----

    [Fact]
    public void Problem057_matches_the_statement_example()
    {
        // 1393/985, the eighth expansion, is the first whose numerator has more digits.
        Assert.Equal(0, Problem057.Solve(0));
        Assert.Equal(0, Problem057.Solve(7));
        Assert.Equal(1, Problem057.Solve(8));
    }

    [Fact]
    public void Problem057_matches_fractions_evaluated_from_the_inside_out()
    {
        var expected = 0;
        for (var expansions = 1; expansions <= 150; expansions++)
        {
            // 1 + 1/(2 + 1/(2 + ... + 1/2)) with this many twos.
            BigInteger numerator = 2;
            BigInteger denominator = 1;
            for (var level = 1; level < expansions; level++)
            {
                (numerator, denominator) = (2 * numerator + denominator, numerator);
            }

            (numerator, denominator) = (numerator + denominator, numerator);
            Assert.Equal(BigInteger.One, BigInteger.GreatestCommonDivisor(numerator, denominator));
            expected += numerator.ToString().Length > denominator.ToString().Length ? 1 : 0;
            Assert.Equal(expected, Problem057.Solve(expansions));
        }
    }

    [Fact]
    public void Problem057_matches_digit_counting_past_the_statement_value()
    {
        var expected = 0;
        BigInteger numerator = 1;
        BigInteger denominator = 1;
        for (var expansions = 1; expansions <= 4000; expansions++)
        {
            (numerator, denominator) = (numerator + 2 * denominator, numerator + denominator);
            expected += numerator.ToString().Length > denominator.ToString().Length ? 1 : 0;
            if (expansions % 500 == 0)
            {
                Assert.Equal(expected, Problem057.Solve(expansions));
            }
        }
    }

    [Fact]
    public void Problem057_matches_the_closed_form_far_past_the_statement_value()
    {
        // The k-th expansion is the pair nearest to (1 + √2)^(k + 1) / 2 and (1 + √2)^(k + 1) / (2√2), so both digit
        // counts follow from logarithms; these counts were obtained that way, with eighty-digit arithmetic.
        Assert.Equal(756, Problem057.Solve(5_000));
        Assert.Equal(3_018, Problem057.Solve(20_000));
    }

    [Fact]
    public void Problem057_rejects_out_of_range_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem057.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem057.Solve(100_001));
    }

    // ---- Problem 58 ----

    [Fact]
    public void Problem058_matches_a_spiral_that_is_actually_drawn()
    {
        // Walk the spiral anticlockwise from the centre, as in the statement: right, up, left, down, with the
        // run length growing by one after every second turn.
        const int Size = 1001;
        var grid = new int[Size, Size];
        var (x, y) = (Size / 2, Size / 2);
        var (dx, dy) = (1, 0);
        var value = 1;
        grid[y, x] = value;
        for (var run = 1; value < Size * Size; run++)
        {
            for (var turn = 0; turn < 2; turn++)
            {
                for (var step = 0; step < run && value < Size * Size; step++)
                {
                    (x, y) = (x + dx, y + dy);
                    grid[y, x] = ++value;
                }

                (dx, dy) = (dy, -dx);
            }
        }

        // The statement's picture: 37 at the top left of the side-7 spiral, 49 at its bottom right.
        var centre = Size / 2;
        Assert.Equal(37, grid[centre - 3, centre - 3]);
        Assert.Equal(49, grid[centre + 3, centre + 3]);

        var firstSideBelow = new int[101];
        var primes = 0;
        for (var ring = 1; ring <= centre; ring++)
        {
            int[] corners =
            [
                grid[centre - ring, centre - ring],
                grid[centre - ring, centre + ring],
                grid[centre + ring, centre - ring],
                grid[centre + ring, centre + ring],
            ];
            primes += corners.Count(IsPrimeByTrialDivision);
            var side = 2 * ring + 1;
            if (side == 7)
            {
                Assert.Equal(8, primes); // "8 out of the 13 numbers lying along both diagonals are prime".
            }

            for (var percent = 1; percent <= 100; percent++)
            {
                if (firstSideBelow[percent] == 0 && primes * 100 < percent * (2 * side - 1))
                {
                    firstSideBelow[percent] = side;
                }
            }
        }

        // The drawn spiral is big enough for 15% (side 981) and everything above it.
        for (var percent = 15; percent <= 100; percent++)
        {
            Assert.NotEqual(0, firstSideBelow[percent]);
            Assert.Equal(firstSideBelow[percent], Problem058.Solve(percent));
        }

        static bool IsPrimeByTrialDivision(int n)
        {
            for (var d = 2; d * d <= n; d++)
            {
                if (n % d == 0)
                {
                    return false;
                }
            }

            return n >= 2;
        }
    }

    [Fact]
    public void Problem058_handles_ratios_at_and_around_the_first_layer()
    {
        // Side 3 has primes 3, 5 and 7 among 1, 3, 5, 7, 9: exactly 60%, which is below 61% but not below 60%.
        Assert.Equal(3, Problem058.Solve(100));
        Assert.Equal(3, Problem058.Solve(61));
        Assert.Equal(5, Problem058.Solve(60));
    }

    [Fact]
    public void Problem058_reaches_spirals_larger_than_the_statements() =>
        Assert.Equal(74_373, Problem058.Solve(9)); // Confirmed with a separate implementation; 10% needs side 26,241.

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(101)]
    public void Problem058_rejects_unsupported_percentages(int percent) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem058.Solve(percent));

    // ---- Problem 59 ----

    [Theory] // The statement's rule: XOR with the key encrypts, and the same XOR again restores the text.
    [InlineData("k")]
    [InlineData("qz")]
    [InlineData("god")]
    [InlineData("abcde")]
    [InlineData("lanterns")]
    public void Problem059_recovers_text_encrypted_with_a_known_key(string key)
    {
        var cipher = Encrypt(PlainText, key);
        Assert.Equal(PlainText.Sum(c => (long)c), Problem059.Solve(cipher, key.Length));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Problem059_matches_a_search_of_every_key(int keyLength)
    {
        var statementCipher = Resources.ReadQuotedList("0059_cipher_original.txt").Select(int.Parse).ToArray();
        Assert.Equal(SumUnderBestKey(Encrypt(PlainText, "pew"[..keyLength]), keyLength), Problem059.Solve(Encrypt(PlainText, "pew"[..keyLength]), keyLength));
        Assert.Equal(SumUnderBestKey(Encrypt("Hi, Bob", "xy"), keyLength), Problem059.Solve(Encrypt("Hi, Bob", "xy"), keyLength));
        if (keyLength == 3)
        {
            Assert.Equal(SumUnderBestKey(statementCipher, keyLength), Problem059.Solve(statementCipher, keyLength));
        }
    }

    [Fact]
    public void Problem059_prefers_the_earliest_key_among_equals()
    {
        // 'a' ^ 'A' is 32: every lowercase key turns it into an uppercase letter, so all 26 tie and 'a' is used.
        Assert.Equal((long)'A', Problem059.Solve(['a' ^ 'A'], 1));
    }

    [Fact]
    public void Problem059_accepts_the_smallest_and_largest_codes()
    {
        // Code 0 decrypts to the key letter itself, so every key scores one letter and 'a' is used.
        Assert.Equal((long)'a', Problem059.Solve([0], 1));

        // Code 127 becomes a control character under every letter; 'r' is the first to give a permitted one (CR).
        Assert.Equal((long)'\r', Problem059.Solve([127], 1));
        Assert.Equal('a' + (long)'\r', Problem059.Solve([0, 127], 2));
    }

    [Fact]
    public void Problem059_reports_a_cipher_that_no_key_makes_printable()
    {
        // Codes 97 and 99 become control characters under every lowercase letter, never both white space.
        Assert.Throws<InvalidOperationException>(() => Problem059.Solve([97, 99], 1));
        Assert.Throws<InvalidOperationException>(() => Problem059.Solve([.. Encrypt(PlainText, "key"), 97, 99, 97, 99, 97, 99], 3));
    }

    [Fact]
    public void Problem059_rejects_malformed_arguments()
    {
        Assert.Throws<ArgumentNullException>(() => Problem059.Solve(null!, 3));
        Assert.Throws<ArgumentException>(() => Problem059.Solve([], 1));
        Assert.Throws<ArgumentException>(() => Problem059.Solve([65, -1, 66], 1));
        Assert.Throws<ArgumentException>(() => Problem059.Solve([65, 128, 66], 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem059.Solve([65, 66, 67], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem059.Solve([65, 66, 67], 4));
    }

    private static int[] Encrypt(string text, string key) => text.Select((c, i) => c ^ key[i % key.Length]).ToArray();

    /// <summary>Tries all 26^keyLength keys on the whole text; the first of the best-scoring ones wins.</summary>
    private static long SumUnderBestKey(int[] cipher, int keyLength)
    {
        var bestScore = -1;
        var bestSum = 0L;
        var key = new char[keyLength];
        for (var index = 0; index < (int)Math.Pow(26, keyLength); index++)
        {
            // Most significant letter first, so keys are visited in alphabetical order.
            for (int position = keyLength - 1, rest = index; position >= 0; position--, rest /= 26)
            {
                key[position] = (char)('a' + (rest % 26));
            }

            var score = 0;
            var sum = 0L;
            for (var i = 0; i < cipher.Length; i++)
            {
                var plain = (char)(cipher[i] ^ key[i % keyLength]);
                if (plain is not ((>= ' ' and <= '~') or '\t' or '\n' or '\r'))
                {
                    score = -1; // Not text at all: this key is out.
                    break;
                }

                sum += plain;
                score += plain == ' ' || char.IsAsciiLetter(plain) ? 1 : 0;
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestSum = sum;
            }
        }

        Assert.True(bestScore >= 0, "No key gave printable text.");
        return bestSum;
    }

    // ---- Problem 60 ----

    [Fact]
    public void Problem060_matches_the_statement_example() =>
        Assert.Equal(792, Problem060.Solve(4)); // 3, 7, 109 and 673.

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void Problem060_matches_brute_force(int setSize)
    {
        var answer = Problem060.Solve(setSize);

        // Every member of a set with a sum of at most the answer is itself below the answer, so searching all
        // sets of primes below it both finds the answer's own set and rules out anything smaller.
        var primes = Enumerable.Range(2, answer).Where(IsPrimeByTrialDivision).ToArray();
        var lowest = int.MaxValue;
        Choose([], 0);
        Assert.Equal(lowest, answer);

        void Choose(int[] chosen, int from)
        {
            if (chosen.Length == setSize)
            {
                lowest = Math.Min(lowest, chosen.Sum());
                return;
            }

            for (var i = from; i < primes.Length; i++)
            {
                var prime = primes[i];
                if (chosen.All(member => IsPrimeByTrialDivision(long.Parse($"{member}{prime}")) && IsPrimeByTrialDivision(long.Parse($"{prime}{member}"))))
                {
                    Choose([.. chosen, prime], i + 1);
                }
            }
        }

        static bool IsPrimeByTrialDivision<T>(T n)
            where T : INumber<T>
        {
            var two = T.One + T.One;
            for (var d = two; d * d <= n; d++)
            {
                if (T.IsZero(n % d))
                {
                    return false;
                }
            }

            return n >= two;
        }
    }

    [Fact]
    public void Problem060_five_set_is_the_lowest_of_every_set_within_its_sum()
    {
        // Sets are built in ascending order from the primes that pair with every member chosen so far. A set whose
        // sum is at most the answer cannot contain a prime above answer - sum - (members still to come) × prime,
        // because those members are all larger than the one just chosen; that keeps the lists short.
        const int SetSize = 5;
        var answer = Problem060.Solve(SetSize);
        var lowest = int.MaxValue;
        Choose(Primes.UpTo(answer), 0, 0);
        Assert.Equal(26_033, lowest); // 13 + 5,197 + 5,701 + 6,733 + 8,389.
        Assert.Equal(lowest, answer);

        void Choose(int[] candidates, int count, int sum)
        {
            if (count == SetSize)
            {
                lowest = Math.Min(lowest, sum);
                return;
            }

            var toCome = SetSize - count - 1;
            for (var i = 0; i < candidates.Length && sum + ((toCome + 1) * (long)candidates[i]) <= answer; i++)
            {
                var prime = candidates[i];
                var largest = answer - sum - (toCome * prime);
                var compatible = candidates
                    .Skip(i + 1)
                    .TakeWhile(other => other <= largest)
                    .Where(other => Primes.IsPrime(Join(prime, other)) && Primes.IsPrime(Join(other, prime)))
                    .ToArray();
                Choose(compatible, count + 1, sum + prime);
            }
        }

        static long Join(long left, long right)
        {
            var scale = 10L;
            while (scale <= right)
            {
                scale *= 10;
            }

            return (left * scale) + right;
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    public void Problem060_rejects_unsupported_set_sizes(int setSize) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem060.Solve(setSize));
}
