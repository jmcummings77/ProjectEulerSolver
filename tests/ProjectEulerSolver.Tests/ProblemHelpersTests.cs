using ProjectEulerSolver.Problems;
using Xunit;

namespace ProjectEulerSolver.Tests;

/// <summary>Unit tests for the helper types that live inside individual problems.</summary>
public class ProblemHelpersTests
{
    [Theory]
    [InlineData("5H 5C 6S 7S KD", "2C 3S 8S 8D TD", -1)] // Pair of fives loses to pair of eights.
    [InlineData("5D 8C 9S JS AC", "2C 5C 7D 8S QH", 1)] // Ace high beats queen high.
    [InlineData("2D 9C AS AH AC", "3D 6D 7D TD QD", -1)] // Three aces loses to a flush.
    [InlineData("4D 6S 9H QH QC", "3D 6D 7H QD QS", 1)] // Pair of queens, nine kicker beats seven kicker.
    [InlineData("2H 2D 4C 4D 4S", "3C 3D 3S 9S 9D", 1)] // Full house, fours over threes.
    [InlineData("TH JH QH KH AH", "9S TS JS QS KS", 1)] // Royal flush beats king-high straight flush.
    [InlineData("AS 2D 3C 4H 5S", "2S 3D 4C 5H 6S", -1)] // Five-high straight loses to six-high straight.
    [InlineData("2H 3D 5S 9C KD", "2C 3H 5D 9S KH", 0)] // Identical ranks tie.
    public void PokerHand_ranks_hands_like_the_problem_statement(string player1, string player2, int expectedSign)
    {
        var hand1 = Problem054.PokerHand.Parse(player1.Split(' '));
        var hand2 = Problem054.PokerHand.Parse(player2.Split(' '));
        Assert.Equal(expectedSign, Math.Sign(hand1.CompareTo(hand2)));
        Assert.Equal(-expectedSign, Math.Sign(hand2.CompareTo(hand1)));
    }

    [Theory]
    [InlineData(145, 1)]
    [InlineData(169, 3)]
    [InlineData(871, 2)]
    [InlineData(69, 5)]
    [InlineData(78, 4)]
    [InlineData(540, 2)]
    [InlineData(3, 16)]
    public void DigitFactorialChains_measure_the_examples_from_the_statement(int start, int expectedLength)
    {
        var chains = new Problem074.DigitFactorialChains();
        Assert.Equal(expectedLength, chains.Length(start));
        Assert.Equal(expectedLength, chains.Length(start)); // Cached answers must agree with the first.
    }

    [Fact]
    public void DigitFactorialChains_cache_agrees_with_an_uncached_walk()
    {
        var chains = new Problem074.DigitFactorialChains();
        for (var start = 1; start < 20_000; start++)
        {
            var seen = new HashSet<int>();
            for (var term = start; seen.Add(term); term = Problem074.DigitFactorialChains.Next(term))
            {
            }

            Assert.Equal(seen.Count, chains.Length(start));
        }
    }

    [Fact]
    public void DerivePasscode_recovers_the_example_passcode()
    {
        string[] attempts = ["317", "531", "127", "278", "538", "512", "317"];
        Assert.Equal("531278", Problem079.DerivePasscode(attempts));
    }

    [Fact]
    public void DerivePasscode_rejects_cyclic_attempts() =>
        Assert.Throws<InvalidOperationException>(() => Problem079.DerivePasscode(["123", "231"]));
}
