using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many of the one thousand poker hands in the data file are won by Player 1.</summary>
public sealed class Problem054 : Problem
{
    public override int Number => 54;

    public override string Title => "Poker Hands";

    public override object Solve() =>
        Resources.ReadLines("0054_poker.txt")
            .Select(line => line.Split(' '))
            .Count(cards => PokerHand.Parse(cards[..5]).CompareTo(PokerHand.Parse(cards[5..])) > 0);

    /// <summary>A five-card hand ranked by the standard poker categories, with kickers for tie-breaks.</summary>
    internal sealed class PokerHand : IComparable<PokerHand>
    {
        private const string RankOrder = "23456789TJQKA";

        // Category (higher is better), then ranks ordered by how they should be compared.
        private readonly int category;
        private readonly int[] tieBreak;

        private PokerHand(int category, int[] tieBreak)
        {
            this.category = category;
            this.tieBreak = tieBreak;
        }

        public static PokerHand Parse(IReadOnlyList<string> cards)
        {
            var ranks = cards.Select(c => RankOrder.IndexOf(c[0]) + 2).ToArray();
            var isFlush = cards.All(c => c[1] == cards[0][1]);

            // Group ranks by multiplicity: quads, trips and pairs come first, then kickers high to low.
            var groups = ranks
                .GroupBy(r => r)
                .Select(g => (Count: g.Count(), Rank: g.Key))
                .OrderByDescending(g => g.Count)
                .ThenByDescending(g => g.Rank)
                .ToArray();
            var ordered = groups.Select(g => g.Rank).ToArray();

            var distinct = ordered.Length;
            var isStraight = distinct == 5 && ordered[0] - ordered[4] == 4;
            if (distinct == 5 && ordered.SequenceEqual([14, 5, 4, 3, 2]))
            {
                isStraight = true; // A-2-3-4-5 counts as a five-high straight.
                ordered = [5, 4, 3, 2, 1];
            }

            var category = (groups[0].Count, distinct) switch
            {
                _ when isStraight && isFlush => 8,
                (4, _) => 7,
                (3, 2) => 6,
                _ when isFlush => 5,
                _ when isStraight => 4,
                (3, _) => 3,
                (2, 3) => 2,
                (2, _) => 1,
                _ => 0,
            };

            return new PokerHand(category, ordered);
        }

        public int CompareTo(PokerHand? other)
        {
            ArgumentNullException.ThrowIfNull(other);
            if (category != other.category)
            {
                return category.CompareTo(other.category);
            }

            for (var i = 0; i < tieBreak.Length; i++)
            {
                if (tieBreak[i] != other.tieBreak[i])
                {
                    return tieBreak[i].CompareTo(other.tieBreak[i]);
                }
            }

            return 0;
        }
    }
}
