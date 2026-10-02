using System.Numerics;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests;

public class ToolsTests
{
    [Theory]
    [InlineData(-7, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(97, true)]
    [InlineData(1_000_003, true)]
    [InlineData(1_000_001, false)] // 101 × 9901
    [InlineData(2_147_483_647, true)] // Mersenne prime 2^31 − 1
    [InlineData(3_215_031_751, false)] // Strong pseudoprime to bases 2, 3, 5 and 7
    [InlineData(4_759_123_141, false)] // 48781 × 97561; first boundary of the witness tiers
    [InlineData(341_550_071_728_321, false)] // 10670053 × 32010157; second boundary of the witness tiers
    [InlineData(9_223_372_036_854_775_783, true)] // Largest prime below 2^63
    public void IsPrime_agrees_with_known_values(long n, bool expected) =>
        Assert.Equal(expected, Primes.IsPrime(n));

    [Fact]
    public void Sieve_matches_trial_division()
    {
        var sieve = Primes.Sieve(10_000);
        for (var n = 0; n <= 10_000; n++)
        {
            Assert.Equal(Primes.IsPrime(n), sieve[n]);
        }
    }

    [Fact]
    public void FromSieve_lists_the_marked_primes() =>
        Assert.Equal(new[] { 2, 3, 5, 7, 11, 13, 17, 19 }, Primes.FromSieve(Primes.Sieve(20)));

    [Fact]
    public void Factor_returns_primes_with_exponents() =>
        Assert.Equal([(2L, 3), (3L, 2), (5L, 1)], Primes.Factor(360));

    [Theory]
    [InlineData(9_223_372_036_854_775_783)] // A large prime must not be trial-divided to its square root.
    [InlineData(4_611_686_018_427_387_904)] // 2^62
    [InlineData(6_000_000_042_000_000_000)] // 2^10 × 3 × 5^9 × 1,000,000,007: large prime cofactor is detected early.
    public void Factor_handles_values_near_long_MaxValue(long n)
    {
        var factors = Primes.Factor(n).ToArray();
        Assert.Equal(n, factors.Aggregate(1L, (product, f) => product * (long)BigInteger.Pow(f.Prime, f.Exponent)));
        Assert.All(factors, f => Assert.True(Primes.IsPrime(f.Prime)));
        Assert.Equal(factors.OrderBy(f => f.Prime), factors);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(12, 6)]
    [InlineData(28, 6)]
    [InlineData(360, 24)]
    public void DivisorCount_counts_all_divisors(long n, int expected) =>
        Assert.Equal(expected, Primes.DivisorCount(n));

    [Fact]
    public void DivisorCount_rejects_values_below_one() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Primes.DivisorCount(0));

    [Fact]
    public void DistinctPrimeFactorCounts_sieve_matches_factorisation()
    {
        var counts = Primes.DistinctPrimeFactorCounts(1000);
        for (var n = 2; n <= 1000; n++)
        {
            Assert.Equal(Primes.Factor(n).Count(), counts[n]);
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(15, 3)]
    [InlineData(16, 4)]
    [InlineData(999_999_999_999, 999_999)]
    [InlineData(long.MaxValue, 3_037_000_499)]
    public void ISqrt_floors_the_square_root(long n, long expected) =>
        Assert.Equal(expected, NumberTheory.ISqrt(n));

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(-4, false)]
    [InlineData(3_037_000_499L * 3_037_000_499L, true)] // Largest square that fits in a long
    [InlineData(long.MaxValue, false)]
    public void IsSquare_handles_the_whole_long_range(long n, bool expected) =>
        Assert.Equal(expected, NumberTheory.IsSquare(n));

    [Theory]
    [InlineData(4, 6, 12)]
    [InlineData(-4, 6, 12)]
    [InlineData(0, 5, 0)]
    [InlineData(0, 0, 0)]
    public void Lcm_is_non_negative_and_handles_zero(long a, long b, long expected) =>
        Assert.Equal(expected, NumberTheory.Lcm(a, b));

    [Fact]
    public void ProperDivisorSums_sieve_matches_direct_computation()
    {
        var sums = NumberTheory.ProperDivisorSums(500);
        for (var n = 1; n <= 500; n++)
        {
            Assert.Equal(NumberTheory.ProperDivisorSum(n), sums[n]);
        }
    }

    [Fact]
    public void Totients_sieve_matches_definition()
    {
        var phi = NumberTheory.Totients(200);
        for (var n = 1; n <= 200; n++)
        {
            var expected = Enumerable.Range(1, n).Count(k => NumberTheory.Gcd(k, n) == 1);
            Assert.Equal(expected, phi[n]);
        }
    }

    [Theory]
    [InlineData(0, "zero")]
    [InlineData(13, "thirteen")]
    [InlineData(21, "twenty-one")]
    [InlineData(100, "one hundred")]
    [InlineData(115, "one hundred and fifteen")]
    [InlineData(342, "three hundred and forty-two")]
    [InlineData(1000, "one thousand")]
    [InlineData(1_001, "one thousand and one")]
    [InlineData(2_500, "two thousand five hundred")]
    [InlineData(1_000_001, "one million and one")]
    [InlineData(1_234_567, "one million two hundred and thirty-four thousand five hundred and sixty-seven")]
    [InlineData(2_000_000_000, "two billion")]
    public void ToWords_follows_the_british_and_convention(int n, string expected) =>
        Assert.Equal(expected, NumberWords.ToWords(n));

    [Theory]
    [InlineData(3, 1, true)]
    [InlineData(3, 2, false)]
    [InlineData(3, 55, true)]
    [InlineData(5, 92, true)]
    [InlineData(5, 48, false)]
    [InlineData(6, 40755, true)]
    [InlineData(8, 65, true)]
    [InlineData(5, 0, false)]
    [InlineData(5, 5_482_660, true)]
    [InlineData(6, long.MaxValue, false)] // Would overflow a naive 8(s−2)x discriminant.
    public void IsPolygonal_recognises_figurate_numbers(int sides, long value, bool expected) =>
        Assert.Equal(expected, Figurate.IsPolygonal(sides, value));

    [Fact]
    public void Polygonal_generators_match_their_closed_forms()
    {
        for (long n = 1; n <= 100; n++)
        {
            Assert.Equal(n * (n + 1) / 2, Figurate.Triangle(n));
            Assert.Equal(n * n, Figurate.Square(n));
            Assert.Equal(n * (3 * n - 1) / 2, Figurate.Pentagonal(n));
            Assert.Equal(n * (2 * n - 1), Figurate.Hexagonal(n));
            Assert.Equal(n * (5 * n - 3) / 2, Figurate.Heptagonal(n));
            Assert.Equal(n * (3 * n - 2), Figurate.Octagonal(n));
            Assert.True(Figurate.IsTriangle(Figurate.Triangle(n)));
            Assert.True(Figurate.IsPentagonal(Figurate.Pentagonal(n)));
            Assert.True(Figurate.IsHexagonal(Figurate.Hexagonal(n)));
        }
    }

    [Fact]
    public void Figurate_rejects_fewer_than_three_sides()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Figurate.Polygonal(2, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Figurate.IsPolygonal(2, 1));
    }

    [Fact]
    public void Permutations_are_lexicographic_and_complete()
    {
        var permutations = Combinatorics.Permutations([1, 2, 3]).Select(p => string.Concat(p)).ToArray();
        Assert.Equal(new[] { "123", "132", "213", "231", "312", "321" }, permutations);
    }

    [Fact]
    public void Combinations_are_lexicographic_and_complete()
    {
        var combinations = Combinatorics.Combinations([1, 2, 3, 4, 5], 3).Select(c => string.Concat(c)).ToArray();
        Assert.Equal(new[] { "123", "124", "125", "134", "135", "145", "234", "235", "245", "345" }, combinations);
        Assert.Single(Combinatorics.Combinations([1, 2, 3], 0));
        Assert.Empty(Combinatorics.Combinations([1, 2, 3], 4));
    }

    [Fact]
    public void CountPartitions_counts_coin_change() =>
        Assert.Equal(new BigInteger(4), Combinatorics.CountPartitions(10, [1, 5, 10]));

    [Fact]
    public void CountPartitions_rejects_invalid_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Combinatorics.CountPartitions(-1, [1]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Combinatorics.CountPartitions(5, [0]));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(23, 4)]
    [InlineData(25, 0)]
    public void SqrtPeriodLength_matches_known_expansions(int n, int expected) =>
        Assert.Equal(expected, ContinuedFractions.SqrtPeriodLength(n));

    [Fact]
    public void SqrtExpansion_of_23_is_4_then_1_3_1_8()
    {
        var (leading, period) = ContinuedFractions.SqrtExpansion(23);
        Assert.Equal(4, leading);
        Assert.Equal(new[] { 1, 3, 1, 8 }, period);
    }

    [Fact]
    public void Convergents_of_sqrt2_are_the_classic_sequence()
    {
        var convergents = ContinuedFractions.Convergents(ContinuedFractions.SqrtTerms(2))
            .Take(5)
            .Select(c => $"{c.Numerator}/{c.Denominator}")
            .ToArray();
        Assert.Equal(new[] { "1/1", "3/2", "7/5", "17/12", "41/29" }, convergents);
    }

    [Theory]
    [InlineData(123456789, 9, true)]
    [InlineData(1234, 4, true)]
    [InlineData(1123, 4, false)]
    [InlineData(1230, 4, false)]
    public void IsPandigital_checks_digits_one_to_n(long n, int length, bool expected) =>
        Assert.Equal(expected, Digits.IsPandigital(n, length));

    [Fact]
    public void Digits_ignore_the_sign_and_cover_long_MinValue()
    {
        Assert.Equal(new[] { 1, 2, 3 }, Digits.Of(-123L));
        Assert.Equal(new[] { 0 }, Digits.Of(0L));
        Assert.Equal(6, Digits.Sum(-123L));
        Assert.Equal(3, Digits.Count(-123L));
        Assert.Equal(19, Digits.Count(long.MinValue));
        Assert.Equal(-21, Digits.Reverse(-120L));
        Assert.Equal(new BigInteger(-21), Digits.Reverse(new BigInteger(-120)));
        Assert.False(Digits.IsPalindrome(-121L));
        Assert.True(Digits.ArePermutations(-123, 321));
    }

    [Fact]
    public void AlphabetValue_scores_upper_case_words()
    {
        Assert.Equal(53, Words.AlphabetValue("COLIN"));
        Assert.Equal(55, Words.AlphabetValue("SKY"));
        Assert.Throws<ArgumentException>(() => Words.AlphabetValue("sky"));
    }

    [Fact]
    public void MaximumPathSum_solves_the_example_triangle() =>
        Assert.Equal(23, NumberTriangle.MaximumPathSum(["3", "7 4", "2 4 6", "8 5 9 3"]));

    [Fact]
    public void MaximumPathSum_handles_negatives_and_totals_beyond_an_int()
    {
        Assert.Equal(-1, NumberTriangle.MaximumPathSum(["-1"]));
        Assert.Equal(-4, NumberTriangle.MaximumPathSum(["-3", "-1 -5"]));
        Assert.Equal(2L * int.MaxValue, NumberTriangle.MaximumPathSum([$"{int.MaxValue}", $"1 {int.MaxValue}"]));
    }

    [Fact]
    public void MaximumPathSum_rejects_malformed_triangles()
    {
        Assert.Throws<ArgumentException>(() => NumberTriangle.MaximumPathSum([]));
        Assert.Throws<ArgumentException>(() => NumberTriangle.MaximumPathSum(["1", "2"]));
        Assert.Throws<ArgumentException>(() => NumberTriangle.MaximumPathSum(["1", "2 x"]));
        Assert.Throws<ArgumentException>(() => NumberTriangle.MaximumPathSum(["1", null!]));
        Assert.Throws<ArgumentNullException>(() => NumberTriangle.MaximumPathSum(null!));
    }

    [Fact]
    public void Enumerate_yields_the_same_primes_as_the_plain_sieve_across_block_edges()
    {
        // 200,000 spans three of the segmented sieve's 65,536-number blocks.
        Assert.Equal(Primes.UpTo(200_000), Primes.Enumerate(200_000));
        Assert.Empty(Primes.Enumerate(1));
        Assert.Equal(new[] { 2 }, Primes.Enumerate(2));
        Assert.Equal(new[] { 2, 3 }, Primes.Enumerate(4));
        Assert.Equal(65_537, Primes.Enumerate(65_537).Last());
    }

    [Fact]
    public void Enumerate_works_at_the_top_of_the_int_range()
    {
        // The Mersenne prime 2^31 − 1 is the last value an int can hold; skip ahead rather than walk two billion numbers.
        Assert.True(Primes.IsPrime(int.MaxValue));
        Assert.Equal(new[] { 2, 3, 5, 7 }, Primes.Enumerate(int.MaxValue).Take(4));
    }

    [Fact]
    public void PowerOfTen_covers_every_power_that_fits_in_a_long()
    {
        Assert.Equal(1, NumberTheory.PowerOfTen(0));
        Assert.Equal(1_000, NumberTheory.PowerOfTen(3));
        Assert.Equal(1_000_000_000_000_000_000, NumberTheory.PowerOfTen(18));
        Assert.Throws<ArgumentOutOfRangeException>(() => NumberTheory.PowerOfTen(19));
        Assert.Throws<ArgumentOutOfRangeException>(() => NumberTheory.PowerOfTen(-1));
    }

    [Theory]
    [InlineData(12, 345, 12_345)]
    [InlineData(7, 0, 70)]
    [InlineData(0, 42, 42)]
    [InlineData(39, 186, 39_186)]
    public void Concatenate_joins_decimal_digits(long left, long right, long expected) =>
        Assert.Equal(expected, Digits.Concatenate(left, right));

    [Fact]
    public void Concatenate_rejects_negatives_and_overflow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Digits.Concatenate(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Digits.Concatenate(1, -2));
        Assert.Throws<OverflowException>(() => Digits.Concatenate(long.MaxValue, 1));
    }

    [Theory]
    [InlineData(585, 2, true)] // 1001001001 in binary: the statement example of Problem 36.
    [InlineData(585, 10, true)]
    [InlineData(6, 2, false)] // 110
    [InlineData(0, 2, true)]
    [InlineData(8, 3, true)] // 22 in base 3
    [InlineData(long.MaxValue, 2, true)] // 63 ones
    [InlineData(long.MaxValue, 10, false)] // Its reversal does not fit in a long.
    [InlineData(-5, 10, false)]
    public void IsPalindrome_works_in_any_radix(long n, int radix, bool expected) =>
        Assert.Equal(expected, Digits.IsPalindrome(n, radix));

    [Fact]
    public void IsPalindrome_in_a_radix_matches_string_reversal()
    {
        for (var n = 0; n < 5_000; n++)
        {
            foreach (var radix in new[] { 2, 8, 10, 16 })
            {
                var text = Convert.ToString(n, radix);
                Assert.Equal(Digits.IsPalindrome(text), Digits.IsPalindrome(n, radix));
            }
        }
    }

    [Fact]
    public void MultisetKey_is_shared_exactly_by_digit_permutations()
    {
        Assert.Equal(Digits.MultisetKey(125_874), Digits.MultisetKey(251_748));
        Assert.NotEqual(Digits.MultisetKey(1), Digits.MultisetKey(10));
        Assert.NotEqual(Digits.MultisetKey(112), Digits.MultisetKey(122));
        Assert.Equal(Digits.MultisetKey(-321), Digits.MultisetKey(123));
        Assert.Equal(Digits.MultisetKey(1_999_999_999_999_999_999), Digits.MultisetKey(9_199_999_999_999_999_999)); // Eighteen nines each.

        // Agrees with the sorted-string key for every pair in a range.
        var numbers = Enumerable.Range(0, 2_000).Select(n => (long)n * 7919 % 100_000).ToArray();
        foreach (var a in numbers.Take(200))
        {
            foreach (var b in numbers)
            {
                Assert.Equal(Digits.SortedKey(a) == Digits.SortedKey(b), Digits.MultisetKey(a) == Digits.MultisetKey(b));
            }
        }
    }
}
