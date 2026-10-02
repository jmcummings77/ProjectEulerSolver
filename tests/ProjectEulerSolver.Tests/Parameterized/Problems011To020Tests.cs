using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>
/// The general solvers of problems 11 to 20: the worked examples from the problem statements, cross-checks against
/// brute force, argument validation, and arguments larger than the ones the statements use.
/// </summary>
public class Problems011To020Tests
{
    // How many letters "one" to "nineteen" and "twenty" to "ninety" have; index 0 is the word left out.
    private static readonly int[] UnitLetters = [0, 3, 3, 5, 4, 4, 3, 5, 5, 4, 3, 6, 6, 8, 8, 7, 7, 9, 8, 8];
    private static readonly int[] TensLetters = [0, 0, 6, 6, 5, 5, 5, 7, 6, 6];

    [Fact]
    public void General_solvers_have_the_agreed_parameter_names()
    {
        Assert.Equal("rows, runLength", ParameterizedSolver.Signature(new Problem011()));
        Assert.Equal("divisors", ParameterizedSolver.Signature(new Problem012()));
        Assert.Equal("numbers, digits", ParameterizedSolver.Signature(new Problem013()));
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem014()));
        Assert.Equal("gridSize", ParameterizedSolver.Signature(new Problem015()));
        Assert.Equal("exponent", ParameterizedSolver.Signature(new Problem016()));
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem017()));
        Assert.Equal("rows", ParameterizedSolver.Signature(new Problem018()));
        Assert.Equal("startYear, endYear", ParameterizedSolver.Signature(new Problem019()));
        Assert.Equal("n", ParameterizedSolver.Signature(new Problem020()));
    }

    [Fact]
    public void General_solvers_accept_text_arguments_from_the_runner()
    {
        var grid = new Dictionary<string, string> { ["rows"] = "1 2,3 4", ["runLength"] = "2" };
        Assert.Equal(new BigInteger(12), ParameterizedSolver.Solve(new Problem011(), grid));

        var triangle = new Dictionary<string, string> { ["rows"] = "3,7 4,2 4 6,8 5 9 3" };
        Assert.Equal(23L, ParameterizedSolver.Solve(new Problem018(), triangle));

        var years = new Dictionary<string, string> { ["startYear"] = "2001", ["endYear"] = "2400" };
        Assert.Equal(688, ParameterizedSolver.Solve(new Problem019(), years));
    }

    [Fact]
    public void Problem011_finds_the_diagonal_from_the_statement()
    {
        // The statement highlights 26 × 63 × 78 × 14 = 1788696 running down a diagonal of its grid.
        string[] rows = ["26 0 0 0", "0 63 0 0", "0 0 78 0", "0 0 0 14"];
        Assert.Equal(1_788_696, Problem011.Solve(rows, 4));
    }

    [Fact]
    public void Problem011_looks_in_every_direction()
    {
        Assert.Equal(125, Problem011.Solve(["1 5 5 5", "1 1 1 1", "1 1 1 1"], 3)); // Across.
        Assert.Equal(125, Problem011.Solve(["1 1 5", "1 1 5", "1 1 5", "1 1 1"], 3)); // Down.
        Assert.Equal(125, Problem011.Solve(["5 1 1", "1 5 1", "1 1 5"], 3)); // Diagonal down to the right.
        Assert.Equal(125, Problem011.Solve(["1 1 5", "1 5 1", "5 1 1"], 3)); // Diagonal down to the left.
    }

    [Fact]
    public void Problem011_handles_grids_that_are_not_square_or_not_small_numbers()
    {
        // Only the rows are long enough for a run of five.
        Assert.Equal(120, Problem011.Solve(["1 2 3 4 5", "9 9 9 9 0"], 5));
        Assert.Equal(9, Problem011.Solve(["7", "9", "8"], 1));
        Assert.Equal(72, Problem011.Solve(["7", "9", "8"], 2));

        // The only line there is has a negative product, and two negatives beat any positive pair.
        Assert.Equal(-10, Problem011.Solve(["-5 2"], 2));
        Assert.Equal(12, Problem011.Solve(["-3 -4", "1 2"], 2));

        // Leading zeros, tabs and repeated spaces, as in the statement's own "08 02 22 97" layout.
        Assert.Equal(24, Problem011.Solve(["08  02", "\t03 01 "], 2));

        // Products are not limited to 64 bits.
        var huge = "100000000000000000000";
        Assert.Equal(BigInteger.Pow(10, 60), Problem011.Solve([$"{huge} {huge} {huge}"], 3));
    }

    [Fact]
    public void Problem011_matches_brute_force_on_random_grids()
    {
        var random = new Random(11);
        for (var trial = 0; trial < 300; trial++)
        {
            var grid = RandomGrid(random, random.Next(1, 7), random.Next(1, 7));
            for (var runLength = 1; runLength <= Math.Max(grid.Length, grid[0].Length); runLength++)
            {
                Assert.Equal(BruteForceGridProduct(grid, runLength), Problem011.Solve(GridRows(grid), runLength));
            }
        }
    }

    [Fact]
    public void Problem011_handles_a_grid_and_run_larger_than_the_statement()
    {
        var grid = RandomGrid(new Random(1111), 45, 60);
        Assert.Equal(BruteForceGridProduct(grid, 9), Problem011.Solve(GridRows(grid), 9));

        // Longer than the grid is tall: across is the only direction left.
        Assert.Equal(BruteForceGridProduct(grid, 60), Problem011.Solve(GridRows(grid), 60));
    }

    [Fact]
    public void Problem011_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentException>(() => Problem011.Solve([], 1));
        Assert.Throws<ArgumentException>(() => Problem011.Solve(["1 2", "3"], 1)); // Ragged.
        Assert.Throws<ArgumentException>(() => Problem011.Solve(["1 2", "   "], 1)); // Blank row.
        Assert.Throws<ArgumentException>(() => Problem011.Solve(["1 x"], 1));
        Assert.Throws<ArgumentException>(() => Problem011.Solve(["1 2.5"], 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem011.Solve(["1 2", "3 4"], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem011.Solve(["1 2", "3 4"], -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem011.Solve(["1 2 3", "4 5 6"], 4)); // No line that long.
    }

    [Fact]
    public void Problem012_matches_the_statement_example()
    {
        // "We can see that 28 is the first triangle number to have over five divisors."
        Assert.Equal(28, Problem012.Solve(5));
        Assert.Equal(1, Problem012.Solve(0));
        Assert.Equal(3, Problem012.Solve(1));
    }

    [Fact]
    public void Problem012_matches_a_direct_search()
    {
        for (var divisors = 0; divisors <= 100; divisors++)
        {
            long triangle = 0;
            for (long n = 1; ; n++)
            {
                triangle += n;
                if (CountDivisors(triangle) > divisors)
                {
                    break;
                }
            }

            Assert.Equal(triangle, Problem012.Solve(divisors));
        }
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(1500)]
    public void Problem012_matches_a_search_beyond_the_statement(int divisors)
    {
        // Far enough to make the solver's table of divisor counts grow several times. Counting the divisors of each
        // triangle number directly would be slow here, so count them for its two coprime halves instead.
        for (long n = 1; ; n++)
        {
            var (a, b) = n % 2 == 0 ? (n / 2, n + 1) : (n, (n + 1) / 2);
            if (CountDivisors(a) * CountDivisors(b) > divisors)
            {
                Assert.Equal(a * b, Problem012.Solve(divisors));
                return;
            }
        }
    }

    [Theory]
    [InlineData(647, 14_399)]
    [InlineData(648, 21_735)]
    [InlineData(767, 21_735)]
    [InlineData(768, 41_040)]
    [InlineData(1023, 41_040)]
    [InlineData(1024, 78_624)]
    [InlineData(1535, 126_224)]
    [InlineData(1536, 165_375)]
    [InlineData(1919, 201_824)]
    [InlineData(1920, 313_599)]
    [InlineData(3071, 453_375)]
    [InlineData(3072, 1_056_159)]
    public void Problem012_is_right_either_side_of_each_growth_of_its_table(int divisors, long index)
    {
        // The solver's table of divisor counts starts with 2^14 entries and doubles whenever the search reaches its
        // end. Each pair of rows is the last threshold answered by a triangle number whose index is below some
        // power of two (2^14 to 2^19) and the first answered by one above it, so the search has to carry on
        // correctly across each of those doublings. The indices come from an independent sieve of smallest
        // prime factors.
        var triangle = index * (index + 1) / 2;
        Assert.True(CountDivisors(triangle) > divisors);
        Assert.Equal(triangle, Problem012.Solve(divisors));
    }

    [Fact]
    public void Problem012_reaches_the_top_of_its_range()
    {
        // The 14,753,024th triangle number, found with an independent sieve.
        var answer = Problem012.Solve(10_000);
        Assert.Equal(108_825_865_948_800, answer);
        Assert.Equal(13_824, CountDivisors(answer));
    }

    [Fact]
    public void Problem012_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem012.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem012.Solve(10_001));
    }

    [Fact]
    public void Problem013_returns_leading_digits_of_small_sums()
    {
        Assert.Equal("1000", Problem013.Solve(["123", "877"], 4));
        Assert.Equal("1", Problem013.Solve(["123", "877"], 1));
        Assert.Equal("12", Problem013.Solve(["0007", "05"], 2)); // Leading zeros are not digits of the sum.
        Assert.Equal("0", Problem013.Solve(["0", "000"], 1));
        Assert.Equal("42", Problem013.Solve(["42"], 2));
    }

    [Fact]
    public void Problem013_matches_column_addition_on_random_numbers()
    {
        var random = new Random(13);
        for (var trial = 0; trial < 200; trial++)
        {
            var numbers = Enumerable.Range(0, random.Next(1, 30)).Select(_ => RandomDigits(random, random.Next(1, 40))).ToArray();
            var sum = AddByColumns(numbers);
            for (var digits = 1; digits <= sum.Length; digits++)
            {
                Assert.Equal(sum[..digits], Problem013.Solve(numbers, digits));
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => Problem013.Solve(numbers, sum.Length + 1));
        }
    }

    [Fact]
    public void Problem013_handles_more_and_longer_numbers_than_the_statement()
    {
        var random = new Random(1313);
        var numbers = Enumerable.Range(0, 500).Select(_ => RandomDigits(random, 200)).ToArray();
        var sum = AddByColumns(numbers);
        Assert.Equal(sum[..150], Problem013.Solve(numbers, 150));
        Assert.Equal(sum, Problem013.Solve(numbers, sum.Length));
    }

    [Fact]
    public void Problem013_matches_column_addition_on_the_statement_numbers()
    {
        var numbers = Resources.ReadLines("0013_numbers.txt");
        var sum = AddByColumns(numbers);
        Assert.Equal(52, sum.Length); // One hundred 50-digit numbers.
        Assert.Equal(sum, Problem013.Solve(numbers, 52));
        Assert.StartsWith(Problem013.Solve(numbers, 10), sum);
    }

    [Fact]
    public void Problem013_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentException>(() => Problem013.Solve([], 1));
        Assert.Throws<ArgumentException>(() => Problem013.Solve(["12", ""], 1));
        Assert.Throws<ArgumentException>(() => Problem013.Solve(["12a"], 1));
        Assert.Throws<ArgumentException>(() => Problem013.Solve(["-5"], 1));
        Assert.Throws<ArgumentException>(() => Problem013.Solve([" 12"], 1));
        Assert.Throws<ArgumentException>(() => Problem013.Solve(["1.5"], 1));
        Assert.Throws<ArgumentException>(() => Problem013.Solve(["١٢"], 1)); // Digits, but not the ASCII ones.
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem013.Solve(["12"], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem013.Solve(["12"], -3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem013.Solve(["12", "13"], 3)); // The sum has two digits.
    }

    [Fact]
    public void Problem014_matches_brute_force_for_every_small_limit()
    {
        // The statement's chain 13 → 40 → 20 → 10 → 5 → 16 → 8 → 4 → 2 → 1 "contains 10 terms".
        Assert.Equal(10, CollatzTerms(13));

        const int Largest = 3000;
        var bestStart = 1;
        for (var limit = 2; limit <= Largest; limit++)
        {
            // limit - 1 is the only start new to this limit; it must be strictly longer to displace an earlier one.
            if (CollatzTerms(limit - 1) > CollatzTerms(bestStart))
            {
                bestStart = limit - 1;
            }

            Assert.Equal(bestStart, Problem014.Solve(limit));
        }
    }

    [Fact]
    public void Problem014_returns_the_smallest_start_when_chains_tie()
    {
        // Below 20 the longest chain has 21 terms and both 18 and 19 produce it.
        Assert.Equal(21, CollatzTerms(18));
        Assert.Equal(21, CollatzTerms(19));
        Assert.Equal(21, Enumerable.Range(1, 19).Max(start => CollatzTerms(start)));
        Assert.Equal(18, Problem014.Solve(20));

        Assert.Equal(1, Problem014.Solve(2)); // 1 is the only start below 2.
    }

    [Theory]
    [InlineData(100_000)]
    [InlineData(262_145)]
    public void Problem014_matches_brute_force_for_a_mid_sized_limit(int limit)
    {
        var bestStart = 1;
        var bestTerms = 1;
        for (var start = 2; start < limit; start++)
        {
            var terms = CollatzTerms(start);
            if (terms > bestTerms)
            {
                bestStart = start;
                bestTerms = terms;
            }
        }

        Assert.Equal(bestStart, Problem014.Solve(limit));
    }

    [Theory]
    [InlineData(2_000_000, 1_723_519)]
    [InlineData(5_000_000, 3_732_423)]
    [InlineData(10_000_000, 8_400_511)]
    public void Problem014_handles_limits_beyond_the_statement(int limit, int expected)
    {
        // Expected starts come from an independent implementation; each is checked here to be a start below the
        // limit whose chain is at least as long as that of the statement's own answer.
        Assert.Equal(expected, Problem014.Solve(limit));
        Assert.True(CollatzTerms(expected) >= CollatzTerms(837_799));
    }

    [Fact]
    public void Problem014_reaches_the_top_of_its_range()
    {
        // The solver keeps chain lengths in 16 bits and terms in 64, relying on two records for starts below 10^8:
        // the longest chain and the largest term. Both record holders are walked again here with overflow checking.
        Assert.Equal(950, CollatzTerms(63_728_127));
        Assert.Equal(2_185_143_829_170_100, CollatzPeak(80_049_391));

        // Found by an independent program that walks every chain below 10^8 in full with 128-bit terms.
        Assert.Equal(63_728_127, Problem014.Solve(100_000_000));
    }

    [Fact]
    public void Problem014_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem014.Solve(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem014.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem014.Solve(-10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem014.Solve(100_000_001));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem014.Solve(int.MaxValue));
    }

    [Fact]
    public void Problem015_matches_the_statement_example()
    {
        // "there are exactly 6 routes to the bottom right corner" of a 2×2 grid.
        Assert.Equal(6, Problem015.Solve(2));
        Assert.Equal(1, Problem015.Solve(0));
        Assert.Equal(2, Problem015.Solve(1));
    }

    [Fact]
    public void Problem015_matches_counting_routes_corner_by_corner()
    {
        // Routes to a corner are the routes to the corner above it plus the routes to the corner on its left.
        // 200 is ten times the statement's size, with a 120-digit count.
        const int Largest = 200;
        var routes = new BigInteger[Largest + 1, Largest + 1];
        for (var row = 0; row <= Largest; row++)
        {
            for (var column = 0; column <= Largest; column++)
            {
                routes[row, column] = row == 0 || column == 0 ? 1 : routes[row - 1, column] + routes[row, column - 1];
            }
        }

        Assert.Equal(120, routes[Largest, Largest].ToString().Length);
        for (var gridSize = 0; gridSize <= Largest; gridSize++)
        {
            Assert.Equal(routes[gridSize, gridSize], Problem015.Solve(gridSize));
        }
    }

    [Theory]
    [InlineData(1_000)]
    [InlineData(5_000)]
    public void Problem015_satisfies_the_central_binomial_recurrence_for_large_grids(int gridSize)
    {
        // C(2n, n) · n = C(2n − 2, n − 1) · 2(2n − 1).
        Assert.Equal(
            Problem015.Solve(gridSize - 1) * 2 * (2 * gridSize - 1),
            Problem015.Solve(gridSize) * gridSize);
    }

    [Fact]
    public void Problem015_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem015.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem015.Solve(100_001));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem015.Solve(int.MaxValue));
    }

    [Fact]
    public void Problem016_matches_the_statement_example()
    {
        // "2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26."
        Assert.Equal(26, Problem016.Solve(15));
        Assert.Equal(1, Problem016.Solve(0));
        Assert.Equal(2, Problem016.Solve(1));
    }

    [Fact]
    public void Problem016_matches_repeated_doubling_on_paper()
    {
        // Doubles a decimal number digit by digit; checked at every small exponent and at two beyond the statement's.
        var digits = new List<int> { 1 };
        for (var exponent = 0; exponent <= 10_000; exponent++)
        {
            if (exponent <= 300 || exponent is 2_500 or 10_000)
            {
                Assert.Equal(digits.Sum(), Problem016.Solve(exponent));
            }

            MultiplyInPlace(digits, 2);
        }
    }

    [Fact]
    public void Problem016_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem016.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem016.Solve(1_000_001));
    }

    [Fact]
    public void Problem017_matches_the_statement_examples()
    {
        // "If the numbers 1 to 5 are written out in words: one, two, three, four, five, then there are
        // 3 + 3 + 5 + 4 + 4 = 19 letters used in total."
        Assert.Equal(19, Problem017.Solve(5));
        Assert.Equal(0, Problem017.Solve(0));

        // "342 (three hundred and forty-two) contains 23 letters and 115 (one hundred and fifteen) contains 20 letters."
        Assert.Equal(23, Problem017.Solve(342) - Problem017.Solve(341));
        Assert.Equal(20, Problem017.Solve(115) - Problem017.Solve(114));
    }

    [Fact]
    public void Problem017_matches_writing_every_number_out()
    {
        // Every limit up to 1,200, then samples, with every limit around the points where "thousand" and "million"
        // first take a multi-digit count or first appear.
        long letters = 0;
        for (var limit = 0; limit <= 1_000_250; limit++)
        {
            if (limit > 0)
            {
                letters += LetterCount(limit);
            }

            if (limit <= 1_200 || limit % 9_973 == 0 || limit is (>= 99_990 and <= 100_110) or >= 999_990)
            {
                Assert.Equal(letters, Problem017.Solve(limit));
            }
        }
    }

    [Fact]
    public void Problem017_matches_counting_letters_without_the_speller()
    {
        // The solver and the test above both lean on NumberWords.ToWords. This count is built from word lengths
        // alone, so a slip in the speller cannot cancel out. It runs past two million to cover "two million and ...".
        Assert.Equal(23, LettersWithoutSpelling(342));
        Assert.Equal(20, LettersWithoutSpelling(115));

        long letters = 0;
        for (var limit = 1; limit <= 2_000_150; limit++)
        {
            letters += LettersWithoutSpelling(limit);
            if (limit <= 1_100 || limit % 7_919 == 0 || limit is (>= 999_990 and <= 1_000_110) or >= 1_999_990)
            {
                Assert.Equal(letters, Problem017.Solve(limit));
            }
        }
    }

    [Theory]
    [InlineData(1_000_000, 50_514_713)]
    [InlineData(12_345_678, 749_013_689)]
    [InlineData(999_999_999, 78_620_999_703)]
    [InlineData(1_000_000_000, 78_620_999_713)]
    [InlineData(1_000_000_099, 78_621_001_854)]
    [InlineData(1_000_000_100, 78_621_001_874)]
    [InlineData(2_000_000_000, 167_241_999_713)]
    [InlineData(2_147_483_647, 179_019_634_329)]
    public void Problem017_matches_totals_counted_one_number_at_a_time(int limit, long expected)
    {
        // Totals from an independent program that spells and counts every number up to the limit, the largest
        // limit being the top of the supported range.
        Assert.Equal(expected, Problem017.Solve(limit));
    }

    [Theory]
    [InlineData(12_345_678)]
    [InlineData(999_999_999)]
    [InlineData(1_000_000_000)]
    [InlineData(1_000_000_001)]
    [InlineData(1_000_000_099)]
    [InlineData(1_000_000_100)]
    [InlineData(1_000_001_000)]
    [InlineData(1_001_000_000)]
    [InlineData(1_999_999_999)]
    [InlineData(2_000_000_000)]
    [InlineData(2_000_000_001)]
    [InlineData(2_147_483_647)]
    public void Problem017_grows_by_exactly_the_letters_of_the_last_number(int limit)
    {
        // Far beyond what can be written out one number at a time, the total for limit must still exceed the total
        // for limit − 1 by the letters in limit itself.
        Assert.Equal(LetterCount(limit), Problem017.Solve(limit) - Problem017.Solve(limit - 1));
    }

    [Fact]
    public void Problem017_adds_up_whole_blocks_of_a_billion()
    {
        // 1,000,000,000 to 1,999,999,999 is "one billion" a billion times over, plus everything below a billion once
        // more, plus "and" for the 99 numbers whose remainder is below 100.
        var belowABillion = Problem017.Solve(999_999_999);
        Assert.Equal(
            (2 * belowABillion) + (1_000_000_000L * "onebillion".Length) + (99 * "and".Length),
            Problem017.Solve(1_999_999_999));
    }

    [Fact]
    public void Problem017_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem017.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem017.Solve(int.MinValue));
    }

    [Fact]
    public void Problem018_matches_the_statement_example()
    {
        // "3 / 7 4 / 2 4 6 / 8 5 9 3 ... 3 + 7 + 4 + 9 = 23."
        Assert.Equal(23, Problem018.Solve(["3", "7 4", "2 4 6", "8 5 9 3"]));
        Assert.Equal(5, Problem018.Solve(["5"]));
        Assert.Equal(23, Problem018.Solve([" 3", "7   4 ", "2\t4 6", "08 5 +9 3"])); // Loose spacing, zeros and signs.
    }

    [Fact]
    public void Problem018_matches_trying_every_route()
    {
        var random = new Random(18);
        for (var trial = 0; trial < 200; trial++)
        {
            var triangle = RandomTriangle(random, random.Next(1, 13), -50, 100);

            // Bit k of route says whether to step right when leaving row k.
            var best = long.MinValue;
            for (var route = 0; route < 1 << (triangle.Length - 1); route++)
            {
                long total = 0;
                var position = 0;
                for (var row = 0; row < triangle.Length; row++)
                {
                    total += triangle[row][position];
                    position += (route >> row) & 1;
                }

                best = Math.Max(best, total);
            }

            Assert.Equal(best, Problem018.Solve(TriangleRows(triangle)));
        }
    }

    [Fact]
    public void Problem018_handles_a_triangle_far_taller_than_the_statement()
    {
        var triangle = RandomTriangle(new Random(1818), 400, -1000, 1000);

        // Work downwards (the solver works upwards): the best total arriving at each entry.
        var arriving = new long[] { triangle[0][0] };
        for (var row = 1; row < triangle.Length; row++)
        {
            var next = new long[row + 1];
            for (var i = 0; i <= row; i++)
            {
                var above = i == 0 ? arriving[0] : i == row ? arriving[i - 1] : Math.Max(arriving[i - 1], arriving[i]);
                next[i] = above + triangle[row][i];
            }

            arriving = next;
        }

        Assert.Equal(arriving.Max(), Problem018.Solve(TriangleRows(triangle)));
    }

    [Fact]
    public void Problem018_does_not_overflow_on_extreme_entries()
    {
        var top = int.MaxValue.ToString();
        var bottom = int.MinValue.ToString();
        Assert.Equal(3L * int.MaxValue, Problem018.Solve([top, $"{top} 0", $"0 {top} 0"]));
        Assert.Equal(3L * int.MinValue, Problem018.Solve([bottom, $"{bottom} {bottom}", $"{bottom} {bottom} {bottom}"]));
        Assert.Equal(-3, Problem018.Solve(["-1", "-5 -1", "-9 -9 -1"])); // The best route can still be negative.
    }

    [Fact]
    public void Problem018_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentException>(() => Problem018.Solve([]));
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["1 2"])); // The top row holds one number.
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["1", "2 3", "4 5"])); // Too short.
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["1", "2 3 4"])); // Too long.
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["1", ""]));
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["1", "2 x"]));
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["1", "2 3.0"]));
        Assert.Throws<ArgumentException>(() => Problem018.Solve(["2147483648"])); // One past int.MaxValue.
    }

    [Fact]
    public void Problem019_matches_a_calendar_built_from_the_statement()
    {
        var byYear = SundaysOnTheFirstByYear();
        var upTo = new int[byYear.Length];
        for (var year = 1; year < byYear.Length; year++)
        {
            upTo[year] = upTo[year - 1] + byYear[year];
        }

        // 1900 is where the statement anchors its calendar; 1 and 9999 are the ends of the supported range, which is
        // far wider than the statement's century.
        (int Start, int End)[] ranges = [(1900, 1900), (1900, 1999), (1899, 1901), (1, 1), (9999, 9999), (1, 9999), (1582, 1583), (2000, 2000), (2100, 2100)];
        var random = new Random(19);
        var randomRanges = Enumerable.Range(0, 60).Select(_ =>
        {
            var start = random.Next(1, 10_000);
            return (Start: start, End: random.Next(start, 10_000));
        });

        foreach (var (start, end) in ranges.Concat(randomRanges))
        {
            Assert.Equal(upTo[end] - upTo[start - 1], Problem019.Solve(start, end));
        }

        for (var year = 1; year <= 9999; year += 7)
        {
            Assert.Equal(byYear[year], Problem019.Solve(year, year));
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1601)]
    [InlineData(1901)]
    [InlineData(2001)]
    [InlineData(9600)]
    public void Problem019_counts_688_in_any_400_years(int startYear)
    {
        // The Gregorian calendar repeats every 400 years, and in each cycle 688 months begin on a Sunday
        // (the same months whose 13th is a Friday).
        Assert.Equal(688, Problem019.Solve(startYear, startYear + 399));
    }

    [Fact]
    public void Problem019_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem019.Solve(0, 2000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem019.Solve(-5, 2000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem019.Solve(1901, 10_000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem019.Solve(10_000, 10_000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem019.Solve(2000, 1999)); // Ends before it starts.
    }

    [Fact]
    public void Problem020_matches_the_statement_example()
    {
        // "10! = 3628800, and the sum of the digits in the number 10! is 3 + 6 + 2 + 8 + 8 + 0 + 0 = 27."
        Assert.Equal(27, Problem020.Solve(10));
        Assert.Equal(1, Problem020.Solve(0));
        Assert.Equal(1, Problem020.Solve(1));
    }

    [Fact]
    public void Problem020_matches_long_multiplication_on_paper()
    {
        // Builds n! digit by digit; checked at every small n and at two far beyond the statement's.
        var digits = new List<int> { 1 };
        for (var n = 0; n <= 2_000; n++)
        {
            if (n > 0)
            {
                MultiplyInPlace(digits, n);
            }

            if (n <= 60 || n is 500 or 2_000)
            {
                Assert.Equal(digits.Sum(), Problem020.Solve(n));
            }
        }
    }

    [Fact]
    public void Problem020_rejects_bad_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem020.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem020.Solve(50_001));
    }

    private static long[][] RandomGrid(Random random, int height, int width) =>
        Enumerable.Range(0, height)
            .Select(_ => Enumerable.Range(0, width).Select(_ => (long)random.Next(-9, 21)).ToArray())
            .ToArray();

    private static string[] GridRows(long[][] grid) => grid.Select(row => string.Join(' ', row)).ToArray();

    private static BigInteger BruteForceGridProduct(long[][] grid, int runLength)
    {
        // Walks runLength cells from every cell towards all eight neighbours, checking each cell is on the grid.
        // That visits every line from both ends, which cannot change the maximum.
        BigInteger? best = null;
        for (var row = 0; row < grid.Length; row++)
        {
            for (var column = 0; column < grid[row].Length; column++)
            {
                for (var direction = 0; direction < 9; direction++)
                {
                    var (dr, dc) = ((direction / 3) - 1, (direction % 3) - 1);
                    if (dr == 0 && dc == 0)
                    {
                        continue;
                    }

                    var product = BigInteger.One;
                    var fits = true;
                    for (var step = 0; step < runLength && fits; step++)
                    {
                        var r = row + (dr * step);
                        var c = column + (dc * step);
                        fits = r >= 0 && r < grid.Length && c >= 0 && c < grid[r].Length;
                        if (fits)
                        {
                            product *= grid[r][c];
                        }
                    }

                    if (fits && (best is null || product > best))
                    {
                        best = product;
                    }
                }
            }
        }

        return best ?? throw new InvalidOperationException("No line of that length fits on the grid.");
    }

    private static int CountDivisors(long n)
    {
        var count = 0;
        for (long d = 1; d * d <= n; d++)
        {
            if (n % d == 0)
            {
                count += d * d == n ? 1 : 2;
            }
        }

        return count;
    }

    private static string RandomDigits(Random random, int length) =>
        string.Concat(Enumerable.Range(0, length).Select(_ => (char)('0' + random.Next(10))));

    /// <summary>Adds numbers the way it is done on paper, a column at a time from the right.</summary>
    private static string AddByColumns(IReadOnlyList<string> numbers)
    {
        var total = new List<int> { 0 }; // Least significant digit first.
        foreach (var number in numbers)
        {
            var carry = 0;
            for (var i = 0; i < number.Length || carry > 0; i++)
            {
                if (i == total.Count)
                {
                    total.Add(0);
                }

                var sum = total[i] + carry + (i < number.Length ? number[^(i + 1)] - '0' : 0);
                total[i] = sum % 10;
                carry = sum / 10;
            }
        }

        while (total.Count > 1 && total[^1] == 0)
        {
            total.RemoveAt(total.Count - 1);
        }

        total.Reverse();
        return string.Concat(total);
    }

    /// <summary>The number of terms in the Collatz chain from <paramref name="start"/> down to 1, counting both ends.</summary>
    private static int CollatzTerms(long start)
    {
        var terms = 1;
        for (var term = start; term != 1; term = term % 2 == 0 ? term / 2 : checked((3 * term) + 1))
        {
            terms++;
        }

        return terms;
    }

    /// <summary>The largest term in the Collatz chain from <paramref name="start"/>; throws if it does not fit a long.</summary>
    private static long CollatzPeak(long start)
    {
        var peak = start;
        for (var term = start; term != 1; term = term % 2 == 0 ? term / 2 : checked((3 * term) + 1))
        {
            peak = Math.Max(peak, term);
        }

        return peak;
    }

    /// <summary>Multiplies a decimal number, held least significant digit first, by a small factor.</summary>
    private static void MultiplyInPlace(List<int> digits, int factor)
    {
        long carry = 0;
        for (var i = 0; i < digits.Count; i++)
        {
            var value = ((long)digits[i] * factor) + carry;
            digits[i] = (int)(value % 10);
            carry = value / 10;
        }

        for (; carry > 0; carry /= 10)
        {
            digits.Add((int)(carry % 10));
        }
    }

    private static int LetterCount(int n) => NumberWords.ToWords(n).Count(char.IsLetter);

    /// <summary>
    /// Letters in <paramref name="n"/> written out in British English, worked out from word lengths without
    /// spelling the number and without <see cref="NumberWords"/>.
    /// </summary>
    private static int LettersWithoutSpelling(int n)
    {
        var billions = n / 1_000_000_000;
        var millions = n / 1_000_000 % 1000;
        var thousands = n / 1000 % 1000;
        var last = n % 1000;

        var letters = GroupLetters(last);
        if (billions > 0)
        {
            letters += GroupLetters(billions) + "billion".Length;
        }

        if (millions > 0)
        {
            letters += GroupLetters(millions) + "million".Length;
        }

        if (thousands > 0)
        {
            letters += GroupLetters(thousands) + "thousand".Length;
        }

        // "one thousand and five": a final part below one hundred takes "and" when anything larger comes before it.
        if (last is > 0 and < 100 && n >= 1000)
        {
            letters += "and".Length;
        }

        return letters;
    }

    /// <summary>Letters in a group of up to three digits, such as "three hundred and forty-two"; none for zero.</summary>
    private static int GroupLetters(int group)
    {
        var hundreds = group / 100;
        var rest = group % 100;
        var letters = rest < 20 ? UnitLetters[rest] : TensLetters[rest / 10] + UnitLetters[rest % 10];
        if (hundreds > 0)
        {
            letters += UnitLetters[hundreds] + "hundred".Length + (rest > 0 ? "and".Length : 0);
        }

        return letters;
    }

    private static int[][] RandomTriangle(Random random, int rows, int minimum, int maximum) =>
        Enumerable.Range(1, rows)
            .Select(length => Enumerable.Range(0, length).Select(_ => random.Next(minimum, maximum)).ToArray())
            .ToArray();

    private static string[] TriangleRows(int[][] triangle) => triangle.Select(row => string.Join(' ', row)).ToArray();

    /// <summary>
    /// For each year 1 to 9999, how many months begin on a Sunday, worked out from the statement alone: 1 January 1900
    /// was a Monday, the rhyme's month lengths, and its leap-year rule.
    /// </summary>
    private static int[] SundaysOnTheFirstByYear()
    {
        var counts = new int[10_000];

        // Day of the week of the first of the month, with Sunday as 0, stepping forwards from January 1900...
        var weekday = 1;
        for (var year = 1900; year <= 9999; year++)
        {
            for (var month = 1; month <= 12; month++)
            {
                if (weekday == 0)
                {
                    counts[year]++;
                }

                weekday = (weekday + DaysInMonth(year, month)) % 7;
            }
        }

        // ...and backwards from it.
        weekday = 1;
        for (var year = 1899; year >= 1; year--)
        {
            for (var month = 12; month >= 1; month--)
            {
                weekday = (((weekday - DaysInMonth(year, month)) % 7) + 7) % 7;
                if (weekday == 0)
                {
                    counts[year]++;
                }
            }
        }

        return counts;
    }

    private static int DaysInMonth(int year, int month) => month switch
    {
        4 or 6 or 9 or 11 => 30,
        2 => (year % 4 == 0 && year % 100 != 0) || year % 400 == 0 ? 29 : 28,
        _ => 31,
    };
}
