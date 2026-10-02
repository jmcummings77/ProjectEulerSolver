using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>
/// The general solvers of problems 1 to 10: the examples from the problem statements, cross-checks against
/// brute force, argument validation, and arguments beyond the statement's to exercise the bounds derived from them.
/// (The statement examples of problems 1 and 8 are in <see cref="ParameterizedSolverTests"/>.)
/// </summary>
public class Problems001To010Tests
{
    // Trial division is slow but leaves nothing to get wrong. The sieve blocks of problems 7 and 10 start at 2,
    // 65,538 and 131,074, so the first 140,000 numbers cover two whole blocks and the start of a third.
    private static readonly int[] SmallPrimes = Enumerable.Range(0, 140_000).Where(IsPrimeByTrialDivision).ToArray();

    [Theory]
    [InlineData(2, "maxValue")]
    [InlineData(3, "n")]
    [InlineData(4, "digits")]
    [InlineData(5, "n")]
    [InlineData(6, "n")]
    [InlineData(7, "n")]
    [InlineData(9, "perimeter")]
    [InlineData(10, "limit")]
    public void General_solver_takes_the_agreed_parameters(int number, string expected) =>
        Assert.Equal(expected, ParameterizedSolver.Signature(ProblemCatalog.Find(number)!));

    // The shape the runner's binder relies on: one overload, bindable parameter types, nothing optional, a concrete result.
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void General_solver_follows_the_convention(int number)
    {
        Type[] parameterTypes = [typeof(int), typeof(long), typeof(BigInteger), typeof(string), typeof(IReadOnlyList<int>), typeof(IReadOnlyList<string>)];
        Type[] resultTypes = [typeof(int), typeof(long), typeof(BigInteger), typeof(string), typeof(IReadOnlyList<string>)];

        var solver = ParameterizedSolver.Find(ProblemCatalog.Find(number)!)!;
        Assert.Contains(solver.ReturnType, resultTypes);
        Assert.All(solver.GetParameters(), parameter =>
        {
            Assert.Contains(parameter.ParameterType, parameterTypes);
            Assert.False(parameter.IsOptional);
        });
    }

    // Problem 1. The statement's own example (the multiples below 10 add up to 23) is asserted in ParameterizedSolverTests.
    [Fact]
    public void Problem001_matches_brute_force()
    {
        long expected = 0;
        for (var limit = 0; limit <= 3000; limit++)
        {
            Assert.Equal(expected, Problem001.Solve(limit));
            if (limit % 3 == 0 || limit % 5 == 0)
            {
                expected += limit;
            }
        }
    }

    [Theory]
    [InlineData(15)]
    [InlineData(150)]
    [InlineData(1_500_000_000)]
    [InlineData(2_999_999_985)]
    [InlineData(3_000_000_000)] // The largest supported limit.
    public void Problem001_does_not_overflow_up_to_its_largest_limit(long limit)
    {
        // A different route to the same sum, in arbitrary precision. Each run of 15 consecutive numbers starting at a
        // multiple of 15, say 15j, holds seven multiples of 3 or 5: 15j + 0, 3, 5, 6, 9, 10 and 12, which add up to
        // 105j + 45. Below 15q that is 105·q(q − 1)/2 + 45q.
        BigInteger blocks = limit / 15;
        Assert.Equal(0, limit % 15);
        Assert.Equal(105 * blocks * (blocks - 1) / 2 + 45 * blocks, Problem001.Solve(limit));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3_000_000_001)]
    [InlineData(long.MaxValue)]
    public void Problem001_rejects_limits_outside_its_range(long limit) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem001.Solve(limit));

    // Problem 2. The statement lists the first ten terms, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89; the even ones are 2, 8 and 34.
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(2, 2)] // The bound is inclusive.
    [InlineData(7, 2)]
    [InlineData(8, 10)]
    [InlineData(33, 10)]
    [InlineData(34, 44)]
    [InlineData(89, 44)]
    public void Problem002_sums_the_even_terms_listed_in_the_statement(long maxValue, long expected) =>
        Assert.Equal(expected, Problem002.Solve(maxValue));

    [Fact]
    public void Problem002_matches_brute_force() =>
        Assert.Empty(Enumerable.Range(0, 3001).Where(maxValue => EvenFibonacciSum(maxValue) != Problem002.Solve(maxValue)));

    [Theory]
    [InlineData(4_000_000_000_000)]
    [InlineData(2_880_067_194_370_816_119)] // One short of the largest even term a long can hold.
    [InlineData(2_880_067_194_370_816_120)]
    [InlineData(7_540_113_804_746_346_428)] // One short of the largest term a long can hold; the term after it overflows.
    [InlineData(7_540_113_804_746_346_429)]
    [InlineData(long.MaxValue)]
    public void Problem002_handles_every_long_without_overflow(long maxValue) =>
        Assert.Equal(EvenFibonacciSum(maxValue), Problem002.Solve(maxValue));

    [Fact]
    public void Problem002_rejects_a_negative_bound() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem002.Solve(-1));

    // Problem 3. The statement: "The prime factors of 13195 are 5, 7, 13 and 29."
    [Fact]
    public void Problem003_matches_the_statement_example() =>
        Assert.Equal(29, Problem003.Solve(13_195));

    [Fact]
    public void Problem003_matches_brute_force() =>
        Assert.Empty(Enumerable.Range(2, 5000).Where(n => LargestPrimeFactorByTrialDivision(n) != Problem003.Solve(n)));

    [Theory]
    [InlineData(2, 2)]
    [InlineData(1L << 62, 2)]
    [InlineData(1_000_006_000_009, 1_000_003)] // The square of a prime.
    [InlineData(6_857_000_047_999, 1_000_000_007)] // 6857 × 1000000007
    [InlineData(600_851_475_143L * 15_350_000, 6857)] // The statement's number times 2^4 · 5^5 · 307, close to the top of the range.
    [InlineData(long.MaxValue, 649_657)] // 7^2 · 73 · 127 · 337 · 92737 · 649657
    [InlineData(9_223_372_036_854_775_783, 9_223_372_036_854_775_783)] // The largest prime below 2^63.
    public void Problem003_handles_numbers_up_to_the_largest_long(long n, long expected) =>
        Assert.Equal(expected, Problem003.Solve(n));

    // The factorisation stops early once the remaining cofactor tests as prime, so a composite that slipped through
    // the primality test would be returned as the answer. Each of these passes Miller-Rabin for several bases, and
    // the middle two are the exact points at which the set of bases tried has to grow.
    [Theory]
    [InlineData(3_215_031_751, 28_351)] // 151 × 751 × 28351, a strong pseudoprime to bases 2, 3, 5 and 7.
    [InlineData(4_759_123_141, 97_561)] // 48781 × 97561, the smallest strong pseudoprime to bases 2, 7 and 61.
    [InlineData(341_550_071_728_321, 32_010_157)] // 10670053 × 32010157, the smallest strong pseudoprime to bases 2 to 17.
    [InlineData(3_825_123_056_546_413_051, 34_233_211)] // 149491 × 747451 × 34233211, a strong pseudoprime to every prime base up to 31.
    public void Problem003_is_not_fooled_by_strong_pseudoprimes(long n, long expected)
    {
        Assert.Equal(expected, Problem003.Solve(n));
        Assert.Equal(0, n % expected);
        Assert.True(IsPrimeByTrialDivision((int)expected));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-13_195)]
    public void Problem003_rejects_numbers_without_a_prime_factor(long n) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem003.Solve(n));

    // Problem 4. The statement: "The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 × 99."
    [Fact]
    public void Problem004_matches_the_statement_example() =>
        Assert.Equal(9009, Problem004.Solve(2));

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Problem004_matches_brute_force(int digits)
    {
        var smallest = (long)Math.Pow(10, digits - 1);
        var largest = smallest * 10 - 1;
        long expected = 0;
        for (var x = smallest; x <= largest; x++)
        {
            for (var y = x; y <= largest; y++)
            {
                if (IsPalindrome(x * y))
                {
                    expected = Math.Max(expected, x * y);
                }
            }
        }

        Assert.Equal(expected, Problem004.Solve(digits));
    }

    // The single-digit answer is the only one with an odd number of digits; nine digits is the most a long allows.
    [Theory]
    [InlineData(1, 9, 9)]
    [InlineData(2, 9009, 99)]
    [InlineData(3, 906_609, 993)]
    [InlineData(4, 99_000_099, 9999)]
    [InlineData(5, 9_966_006_699, 99_979)]
    [InlineData(6, 999_000_000_999, 999_999)]
    [InlineData(7, 99_956_644_665_999, 9_998_017)]
    [InlineData(8, 9_999_000_000_009_999, 99_999_999)]
    [InlineData(9, 999_900_665_566_009_999, 999_980_347)]
    public void Problem004_finds_the_known_answers_across_its_whole_range(int digits, long expected, long factor)
    {
        Assert.Equal(expected, Problem004.Solve(digits));

        // Guard the expected values themselves: each must be a palindrome that splits into two factors of the right size.
        Assert.True(IsPalindrome(expected));
        Assert.Equal(0, expected % factor);
        Assert.Equal(digits, factor.ToString().Length);
        Assert.Equal(digits, (expected / factor).ToString().Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(10)]
    public void Problem004_rejects_digit_counts_outside_its_range(int digits) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem004.Solve(digits));

    // Problem 5. The statement: "2520 is the smallest number that can be divided by each of the numbers from 1 to 10."
    [Fact]
    public void Problem005_matches_the_statement_example() =>
        Assert.Equal(2520, Problem005.Solve(10));

    [Fact]
    public void Problem005_matches_a_search_for_the_smallest_multiple()
    {
        for (var n = 0; n <= 14; n++)
        {
            var candidate = 1;
            while (Enumerable.Range(1, n).Any(divisor => candidate % divisor != 0))
            {
                candidate++;
            }

            Assert.Equal(candidate, Problem005.Solve(n));
        }
    }

    [Fact]
    public void Problem005_matches_a_running_least_common_multiple()
    {
        BigInteger expected = 1;
        for (var n = 1; n <= 600; n++)
        {
            expected = expected / BigInteger.GreatestCommonDivisor(expected, n) * n;
            Assert.Equal(expected, Problem005.Solve(n));
        }
    }

    [Fact]
    public void Problem005_keeps_going_past_the_range_of_a_long()
    {
        Assert.Equal(219_060_189_739_591_200, Problem005.Solve(42));
        Assert.Equal(BigInteger.Parse("9419588158802421600"), Problem005.Solve(43)); // 43 times the previous one, and above 2^63.
        Assert.Equal(BigInteger.Parse("69720375229712477164533808935312303556800"), Problem005.Solve(100));
    }

    [Fact]
    public void Problem005_handles_its_largest_argument()
    {
        var multiple = Problem005.Solve(100_000);
        Assert.Equal(43_452, multiple.ToString().Length);
        Assert.Equal(0, multiple % 99_991); // The largest prime below 100,000.

        // 2^16 ≤ 100,000 < 2^17 and 3^10 ≤ 100,000 < 3^11, so those are the highest powers that may appear.
        Assert.Equal(0, multiple % (1 << 16));
        Assert.NotEqual(0, multiple % (1 << 17));
        Assert.Equal(0, multiple % 59_049);
        Assert.NotEqual(0, multiple % 177_147);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100_001)]
    public void Problem005_rejects_arguments_outside_its_range(int n) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem005.Solve(n));

    // Problem 6. The statement: for the first ten natural numbers, 55^2 − 385 = 3025 − 385 = 2640.
    [Fact]
    public void Problem006_matches_the_statement_example() =>
        Assert.Equal(2640, Problem006.Solve(10));

    [Fact]
    public void Problem006_matches_brute_force()
    {
        BigInteger sum = 0;
        BigInteger sumOfSquares = 0;
        Assert.Equal(0, Problem006.Solve(0));
        for (var n = 1; n <= 78_000; n++)
        {
            sum += n;
            sumOfSquares += (long)n * n;

            // Every n at first, then a sample that still reaches past the range of a long (n = 77,936 onwards).
            if (n <= 500 || n % 97 == 0 || n >= 77_930)
            {
                Assert.Equal(sum * sum - sumOfSquares, Problem006.Solve(n));
            }
        }

        Assert.True(Problem006.Solve(77_935) <= long.MaxValue);
        Assert.True(Problem006.Solve(77_936) > long.MaxValue);
    }

    [Fact]
    public void Problem006_handles_the_largest_int() =>
        Assert.Equal(BigInteger.Parse("5316911974886729899838124255256510464"), Problem006.Solve(int.MaxValue));

    [Theory]
    [InlineData(2)]
    [InlineData(100)]
    [InlineData(77_936)]
    [InlineData(1_000_000_007)]
    [InlineData(int.MaxValue - 1)]
    [InlineData(int.MaxValue)]
    public void Problem006_matches_the_sum_over_pairs(int n)
    {
        // Squaring the sum gives every product i·j; taking the squares away leaves the pairs with i ≠ j, twice the
        // sum over i < j. Summing j·(1 + ... + (j − 1)) over j gives (n − 1)·n·(n + 1)·(3n + 2) / 24 for that sum.
        BigInteger count = n;
        Assert.Equal((count - 1) * count * (count + 1) * (3 * count + 2) / 12, Problem006.Solve(n));
    }

    [Fact]
    public void Problem006_rejects_a_negative_count() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem006.Solve(-1));

    // Problem 7. The statement: "By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13."
    [Fact]
    public void Problem007_matches_the_statement_example() =>
        Assert.Equal([2, 3, 5, 7, 11, 13], Enumerable.Range(1, 6).Select(Problem007.Solve));

    [Fact]
    public void Problem007_matches_trial_division()
    {
        // The first 1500 positions, then the stretches where the answer crosses from one sieve block into the next:
        // 65,537 is the 6543rd prime and the last number of the first block, and 131,071 is the 12,251st prime and
        // the last one in the second block, which ends at 131,073.
        var positions = Enumerable.Range(1, 1500).Concat(Enumerable.Range(6500, 100)).Concat(Enumerable.Range(12_200, 100));
        Assert.Empty(positions.Where(n => SmallPrimes[n - 1] != Problem007.Solve(n)));
        Assert.Equal(65_537, Problem007.Solve(6543));
        Assert.Equal(65_539, Problem007.Solve(6544));
        Assert.Equal(131_071, Problem007.Solve(12_251));
        Assert.Equal(131_101, Problem007.Solve(12_252));
    }

    // The sieve bound comes from p_n < n (ln n + ln ln n), which is only claimed from n = 6 on and is tightest
    // there; were it ever short, the solver would run out of numbers. The primes it returns must also keep rising.
    [Fact]
    public void Problem007_bound_always_reaches_the_requested_prime()
    {
        var previous = 0;
        foreach (var n in Enumerable.Range(1, 3000).Concat(Enumerable.Range(1, 60).Select(step => step * 5003)))
        {
            var prime = Problem007.Solve(n);
            Assert.True(prime > previous);
            Assert.True(IsPrimeByTrialDivision(prime));
            previous = prime;
        }
    }

    [Theory]
    [InlineData(100_000, 1_299_709)]
    [InlineData(1_000_000, 15_485_863)]
    public void Problem007_finds_primes_well_beyond_the_statement(int n, int expected) =>
        Assert.Equal(expected, Problem007.Solve(n));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(105_097_566)] // One past 2^31 − 1, the last prime that fits in an int.
    [InlineData(int.MaxValue)]
    public void Problem007_rejects_positions_outside_its_range(int n) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem007.Solve(n));

    // Problem 8. The statement's 1000-digit number, every window length the solver supports.
    [Fact]
    public void Problem008_matches_brute_force_for_every_window_length()
    {
        var digits = string.Concat(Resources.ReadLines("0008_number.txt"));
        Assert.Equal(1000, digits.Length);
        for (var windowLength = 1; windowLength <= 19; windowLength++)
        {
            var expected = Enumerable.Range(0, digits.Length - windowLength + 1)
                .Max(start => digits.Substring(start, windowLength).Aggregate(BigInteger.One, (product, digit) => product * (digit - '0')));
            Assert.Equal(expected, Problem008.Solve(digits, windowLength));
        }
    }

    [Fact]
    public void Problem008_handles_the_longest_window_and_the_largest_product()
    {
        Assert.Equal(BigInteger.Pow(9, 19), Problem008.Solve(new string('9', 19), 19)); // 1,350,851,717,672,992,089
        Assert.Equal(BigInteger.Pow(9, 19), Problem008.Solve("1" + new string('9', 19) + "0", 19));
        Assert.Equal(7, Problem008.Solve("7", 1));
        Assert.Equal(0, Problem008.Solve("0", 1));
        Assert.Equal(24, Problem008.Solve("1234", 4)); // A window as long as the whole string.
    }

    [Fact]
    public void Problem008_rejects_arguments_outside_its_range()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem008.Solve(new string('9', 20), 20)); // 9^20 does not fit in a long.
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem008.Solve("1234", 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem008.Solve("1234", 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem008.Solve("1234", -1));
        Assert.Throws<ArgumentException>(() => Problem008.Solve(string.Empty, 1));
        Assert.Throws<ArgumentException>(() => Problem008.Solve("12 4", 2));
        Assert.Throws<ArgumentException>(() => Problem008.Solve("-124", 2));
        Assert.Throws<ArgumentException>(() => Problem008.Solve("١٢٣٤", 2)); // Arabic-Indic digits are not decimal digits here.
    }

    // Problem 9. The statement: "For example, 3^2 + 4^2 = 9 + 16 = 25 = 5^2."
    [Fact]
    public void Problem009_matches_the_statement_example() =>
        Assert.Equal(60, Problem009.Solve(12));

    [Fact]
    public void Problem009_matches_brute_force()
    {
        for (var perimeter = 1; perimeter <= 500; perimeter++)
        {
            long expected = 0;
            for (var a = 1; a < perimeter; a++)
            {
                for (var b = a + 1; a + b < perimeter; b++)
                {
                    var c = perimeter - a - b;
                    if (a * a + b * b == c * c)
                    {
                        expected = Math.Max(expected, (long)a * b * c);
                    }
                }
            }

            var p = perimeter;
            if (expected == 0)
            {
                Assert.Throws<InvalidOperationException>(() => Problem009.Solve(p));
            }
            else
            {
                Assert.Equal(expected, Problem009.Solve(p));
            }
        }
    }

    [Fact]
    public void Problem009_returns_the_largest_product_when_several_triplets_fit() =>
        Assert.Equal(60_000, Problem009.Solve(120)); // 30·40·50 beats 24·45·51 = 55,080 and 20·48·52 = 49,920.

    [Fact]
    public void Problem009_handles_perimeters_near_the_top_of_its_range()
    {
        // 12 × (2^19 − 1): the only triplet is 3-4-5 scaled by that prime.
        const long Scale = 524_287;
        Assert.Equal(60 * Scale * Scale * Scale, Problem009.Solve(6_291_444));

        // 34 triplets share this perimeter. The winner is the one nearest to isosceles, and its product is 96% of long.MaxValue.
        Assert.Equal(1_821_204L * 1_864_128 * 2_606_100, Problem009.Solve(6_291_432));
    }

    [Fact]
    public void Problem009_matches_euclids_formula()
    {
        // Every perimeter up to 4000, perimeters rich in divisors (and so in triplets), and the end of the range.
        var perimeters = Enumerable.Range(1, 4000)
            .Concat([27_720, 55_440, 720_720, 4_324_320, 5_765_760, 6_126_120, 6_289_920])
            .Concat(Enumerable.Range(6_291_446, 10));
        foreach (var perimeter in perimeters)
        {
            var expected = LargestTripletProductByEuclidsFormula(perimeter);
            if (expected.IsZero)
            {
                Assert.Throws<InvalidOperationException>(() => Problem009.Solve(perimeter));
            }
            else
            {
                Assert.Equal(expected, Problem009.Solve(perimeter));
            }
        }
    }

    [Fact]
    public void Problem009_fits_the_largest_product_in_its_range()
    {
        // Of all triplets with a supported perimeter, this one has the largest product: 96% of long.MaxValue.
        const long Expected = 1_834_980L * 1_850_400 * 2_605_980;
        Assert.Equal(6_291_360, 1_834_980 + 1_850_400 + 2_605_980);
        Assert.Equal(Expected, Problem009.Solve(6_291_360));
        Assert.Equal(Expected, LargestTripletProductByEuclidsFormula(6_291_360));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(13)]
    [InlineData(6_291_454)]
    [InlineData(6_291_455)] // The largest supported perimeter; odd, and every triplet's perimeter is even.
    public void Problem009_reports_perimeters_without_a_triplet(int perimeter) =>
        Assert.Throws<InvalidOperationException>(() => Problem009.Solve(perimeter));

    [Theory]
    [InlineData(0)]
    [InlineData(-12)]
    [InlineData(6_291_456)]
    [InlineData(int.MaxValue)]
    public void Problem009_rejects_perimeters_outside_its_range(int perimeter) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem009.Solve(perimeter));

    // Problem 10. The statement: "The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17."
    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 0)] // The bound is exclusive.
    [InlineData(3, 2)]
    [InlineData(7, 10)]
    [InlineData(8, 17)]
    [InlineData(10, 17)]
    public void Problem010_matches_the_statement_example(int limit, long expected) =>
        Assert.Equal(expected, Problem010.Solve(limit));

    [Fact]
    public void Problem010_matches_trial_division()
    {
        // Every small limit, then the limits either side of the first two sieve block edges (65,538 and 131,074).
        var limits = Enumerable.Range(0, 3000)
            .Concat(Enumerable.Range(65_520, 40))
            .Concat(Enumerable.Range(131_050, 40))
            .Append(SmallPrimes[^1] + 1);
        Assert.Empty(limits.Where(limit => SmallPrimes.TakeWhile(prime => prime < limit).Sum(prime => (long)prime) != Problem010.Solve(limit)));
    }

    [Theory]
    [InlineData(10_000_000, 3_203_324_994_356)]
    [InlineData(100_000_000, 279_209_790_387_276)]
    public void Problem010_sums_well_beyond_the_statement(int limit, long expected) =>
        Assert.Equal(expected, Problem010.Solve(limit));

    [Fact]
    public void Problem010_rejects_a_negative_limit() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem010.Solve(-1));

    private static bool IsPrimeByTrialDivision(int n) =>
        n >= 2 && Enumerable.Range(2, Math.Max(0, (int)Math.Sqrt(n) - 1)).All(divisor => n % divisor != 0);

    private static long LargestPrimeFactorByTrialDivision(long n)
    {
        long largest = 1;
        for (long divisor = 2; n > 1; divisor++)
        {
            while (n % divisor == 0)
            {
                largest = divisor;
                n /= divisor;
            }
        }

        return largest;
    }

    /// <summary>
    /// The largest product abc over the Pythagorean triplets with the given perimeter, or zero when there is none.
    /// Every triplet is k(m² − n²), 2kmn, k(m² + n²) for exactly one k and one pair m &gt; n ≥ 1 that is coprime and of
    /// opposite parity, and its perimeter is 2km(m + n); so the triplets are the ways to split half the perimeter.
    /// </summary>
    private static BigInteger LargestTripletProductByEuclidsFormula(int perimeter)
    {
        BigInteger largest = 0;
        if (perimeter % 2 != 0)
        {
            return largest;
        }

        long half = perimeter / 2;
        for (long m = 2; m * m < half; m++)
        {
            if (half % m != 0)
            {
                continue;
            }

            // sum = m + n is odd (opposite parity), lies strictly between m and 2m, and is coprime to m exactly when n is.
            for (var sum = m + 1; sum < 2 * m; sum++)
            {
                if (sum % 2 == 0 || half / m % sum != 0 || BigInteger.GreatestCommonDivisor(m, sum) != 1)
                {
                    continue;
                }

                var k = half / m / sum;
                var n = sum - m;
                BigInteger a = k * (m * m - n * n);
                BigInteger b = 2 * k * m * n;
                BigInteger c = k * (m * m + n * n);
                Assert.Equal(perimeter, a + b + c);
                Assert.Equal(c * c, a * a + b * b);
                largest = BigInteger.Max(largest, a * b * c);
            }
        }

        return largest;
    }

    private static bool IsPalindrome(long n)
    {
        var text = n.ToString();
        return text.SequenceEqual(text.Reverse());
    }

    /// <summary>Sums the even Fibonacci terms up to the bound with arbitrary-precision terms, so nothing can overflow.</summary>
    private static BigInteger EvenFibonacciSum(BigInteger maxValue)
    {
        BigInteger sum = 0;
        for (BigInteger previous = 1, current = 2; current <= maxValue; (previous, current) = (current, previous + current))
        {
            if (current.IsEven)
            {
                sum += current;
            }
        }

        return sum;
    }
}
