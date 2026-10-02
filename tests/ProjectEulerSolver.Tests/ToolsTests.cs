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
}
