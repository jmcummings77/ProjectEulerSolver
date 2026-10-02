using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests;

public class ToolsTests
{
    [Theory]
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
    public void Factor_returns_primes_with_exponents() =>
        Assert.Equal([(2L, 3), (3L, 2), (5L, 1)], Primes.Factor(360));

    [Theory]
    [InlineData(1, 1)]
    [InlineData(12, 6)]
    [InlineData(28, 6)]
    [InlineData(360, 24)]
    public void DivisorCount_counts_all_divisors(long n, int expected) =>
        Assert.Equal(expected, Primes.DivisorCount(n));

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
    public void ToWords_uses_british_spelling(int n, string expected) =>
        Assert.Equal(expected, NumberWords.ToWords(n));

    [Theory]
    [InlineData(3, 1, true)]
    [InlineData(3, 2, false)]
    [InlineData(3, 55, true)]
    [InlineData(5, 92, true)]
    [InlineData(5, 48, false)]
    [InlineData(6, 40755, true)]
    [InlineData(8, 65, true)]
    public void IsPolygonal_recognises_figurate_numbers(int sides, long value, bool expected) =>
        Assert.Equal(expected, Figurate.IsPolygonal(sides, value));

    [Fact]
    public void Permutations_are_lexicographic_and_complete()
    {
        var permutations = Combinatorics.Permutations([1, 2, 3]).Select(p => string.Concat(p)).ToArray();
        Assert.Equal(new[] { "123", "132", "213", "231", "312", "321" }, permutations);
    }

    [Fact]
    public void Combinations_choose_k_items() =>
        Assert.Equal(10, Combinatorics.Combinations([1, 2, 3, 4, 5], 3).Count());

    [Fact]
    public void CountPartitions_counts_coin_change() =>
        Assert.Equal(new BigInteger(4), Combinatorics.CountPartitions(10, [1, 5, 10]));

    [Theory]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(23, 4)]
    [InlineData(25, 0)]
    public void SqrtPeriodLength_matches_known_expansions(int n, int expected) =>
        Assert.Equal(expected, ContinuedFractions.SqrtPeriodLength(n));

    [Theory]
    [InlineData(123456789, 9, true)]
    [InlineData(1234, 4, true)]
    [InlineData(1123, 4, false)]
    [InlineData(1230, 4, false)]
    public void IsPandigital_checks_digits_one_to_n(long n, int length, bool expected) =>
        Assert.Equal(expected, Digits.IsPandigital(n, length));

    [Theory]
    [InlineData("5H 5C 6S 7S KD", "2C 3S 8S 8D TD", false)] // Pair of fives loses to pair of eights.
    [InlineData("5D 8C 9S JS AC", "2C 5C 7D 8S QH", true)] // Ace high beats queen high.
    [InlineData("2D 9C AS AH AC", "3D 6D 7D TD QD", false)] // Three aces loses to a flush.
    [InlineData("4D 6S 9H QH QC", "3D 6D 7H QD QS", true)] // Pair of queens, nine kicker beats seven kicker.
    [InlineData("2H 2D 4C 4D 4S", "3C 3D 3S 9S 9D", true)] // Full house, fours over threes.
    [InlineData("TH JH QH KH AH", "9S TS JS QS KS", true)] // Royal flush beats king-high straight flush.
    [InlineData("AS 2D 3C 4H 5S", "2S 3D 4C 5H 6S", false)] // Five-high straight loses to six-high straight.
    public void PokerHand_ranks_hands_like_the_problem_statement(string player1, string player2, bool player1Wins)
    {
        var hand1 = Problem054.PokerHand.Parse(player1.Split(' '));
        var hand2 = Problem054.PokerHand.Parse(player2.Split(' '));
        Assert.Equal(player1Wins, hand1.CompareTo(hand2) > 0);
    }

    [Fact]
    public void DerivePasscode_recovers_the_example_passcode()
    {
        string[] attempts = ["317", "531", "127", "278", "538", "512", "317"];
        Assert.Equal("531278", Problem079.DerivePasscode(attempts));
    }
}
