using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using ProjectEulerSolver.Model;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ProjectEulerSolver.Problems
{
    public class Problem54 : BaseProblem, IProblem
    {
        public Problem54()
        {
            Number = 54;
            Prompt = "In the card game poker, a hand consists of five cards and are ranked, from lowest to highest, in the following way: " +
                        "High Card: Highest value card. " +
                        "One Pair: Two cards of the same value. " +
                        "Two Pairs: Two different pairs. " +
                        "Three of a Kind: Three cards of the same value. " +
                        "Straight: All cards are consecutive values. " +
                        "Flush: All cards of the same suit. " +
                        "Full House: Three of a kind and a pair. " +
                        "Four of a Kind: Four cards of the same value. " + 
                        "Straight Flush: All cards are consecutive values of same suit. " +
                        "Royal Flush: Ten, Jack, Queen, King, Ace, in same suit. " +
                        "The cards are valued in the order: " +
                        "2, 3, 4, 5, 6, 7, 8, 9, 10, Jack, Queen, King, Ace. " +
                        "If two players have the same ranked hands then the rank made up of the highest value wins; for example, a pair of eights beats a pair of fives (see example 1 below). But if two ranks tie, for example, both players have a pair of queens, then highest cards in each hand are compared (see example 4 below); if the highest cards tie then the next highest cards are compared, and so on. " +
                        "Consider the following five hands dealt to two players: " +
                        "Hand	 	Player 1	 	Player 2	 	Winner " +
                        "1	 	5H 5C 6S 7S KD " +
                        "Pair of Fives " +
                        " 	2C 3S 8S 8D TD " +
                        "Pair of Eights " +
                        " 	Player 2 " +
                        "2	 	5D 8C 9S JS AC " +
                        "Highest card Ace " +
                        " 	2C 5C 7D 8S QH " +
                        "Highest card Queen " +
                        " 	Player 1 " +
                        "3	 	2D 9C AS AH AC " +
                        "Three Aces " +
                        " 	3D 6D 7D TD QD " +
                        "Flush with Diamonds " +
                        " 	Player 2 " +
                        "4	 	4D 6S 9H QH QC " +
                        "Pair of Queens " +
                        "Highest card Nine " +
                        " 	3D 6D 7H QD QS " +
                        "Pair of Queens " +
                        "Highest card Seven " +
                        " 	Player 1 " +
                        "5	 	2H 2D 4C 4D 4S " +
                        "Full House " +
                        "With Three Fours " +
                        " 	3C 3D 3S 9S 9D " +
                        "Full House " +
                        "with Three Threes " +
                        " 	Player 1 " +
                        "The file, poker.txt, contains one-thousand random hands dealt to two players. Each line of the file contains ten cards (separated by a single space): the first five are Player 1's cards and the last five are Player 2's cards. You can assume that all hands are valid (no invalid characters or repeated cards), each player's hand is in no specific order, and in each hand there is a clear winner. " +
                        "How many hands does Player 1 win?"; 
        }
        public override void Solve()
        {
            LogList = new List<string>();
            Player1Hands = new List<PokerHand>();
            Player2Hands = new List<PokerHand>();
            ReadLinesFromFile();
            int result = 0;
            for (int i = 0; i < Player1Hands.Count(); i++)
            {
                List<int> player1Ranks = Player1Hands[i].GetHighCardList();
                List<int> player2Ranks = Player2Hands[i].GetHighCardList();
                string ranks1 = @"'{";
                for (int k = 0; k < player1Ranks.Count() - 1; k++)
                {
                    ranks1 += player1Ranks[k].ToString() + ",";
                }
                ranks1 += player1Ranks.Last().ToString() + @"}'";
                string ranks2 = @"'{";
                for (int k = 0; k < player2Ranks.Count() - 1; k++)
                {
                    ranks2 += player2Ranks[k].ToString() + ",";
                }
                ranks2 += player2Ranks.Last().ToString() + @"}'";
                string winner = "";
                
                if (Player1Hands[i].HandScore > Player2Hands[i].HandScore)
                {
                    result++;
                    winner = @"'Player 1'";
                }
                else if (Player1Hands[i].HandScore == Player2Hands[i].HandScore)
                {
                    bool winnerIs1 = false;
                    for (int j = 0; j < player1Ranks.Count(); j++)
                    {
                        if (player1Ranks[j] != player2Ranks[j])
                        {
                            if (player1Ranks[j] > player2Ranks[j])
                            {
                                result++;
                                winnerIs1 = true;
                            }
                            
                            break;
                        }
                    }
                    if(winnerIs1)
                    {
                        winner = @"'Player 1'";
                    }
                    else
                    {
                        winner = @"'Player 2'";
                    }
                }
                else
                {
                    winner = @"'Player 2'";
                }
                LogList.Add(i.ToString() + "," + Player1Hands[i].InputString + "," + Player2Hands[i].InputString + "," + Player1Hands[i].HandType + "," + Player2Hands[i].HandType + "," + Player1Hands[i].HandScore.ToString() + "," + Player2Hands[i].HandScore.ToString() + "," + ranks1 + "," + ranks2 + "," + winner + "," + Player1Hands[i].RanksString + "," + Player2Hands[i].RanksString);
                LogToFile();
                LogList.Clear();
            }
            Output = result.ToString();
        }
        private List<string> PokerHands { get; set; }
        private List<PokerHand> Player1Hands { get; set; }
        private List<PokerHand> Player2Hands { get; set; }

        private void ReadLinesFromFile()
        {
            string SourceFile = @"C:\Users\us48610\Desktop\p054.txt";
            string line;
            using (System.IO.StreamReader file = new System.IO.StreamReader(SourceFile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    var hands = new List<string>();
                    hands = line.Split(' ').ToList();
                    var hands1 = new List<string>();
                    for(int i = 0; i < 5; i++)
                    {
                        hands1.Add(hands[i]);
                    }
                    Player1Hands.Add(new PokerHand(hands1));
                    var hands2 = new List<string>();
                    for (int i = 5; i < 10; i++)
                    {
                        hands2.Add(hands[i]);
                    }
                    Player2Hands.Add(new PokerHand(hands2));
                }
            }
            
        }
    }
    public class PokerHand
    {
        public int RankSum { get; private set; }
        public int RankCount { get; private set; }
        public int SuitCount { get; private set; }
        public List<int> Suits { get; private set; }
        public List<int> Ranks { get; private set; }
        public List<PlayingCard> Cards { get; private set; }
        public int HandScore { get; private set; }
        public string HandType { get; private set; }
        public string InputString { get; private set; }
        public string RanksString { get; private set; }
        public PokerHand(List<PlayingCard> cards)
        {
            Cards = cards;
            Suits = new List<int>(Cards.Select(x => x.SuitAsInteger).OrderBy(y => y).ToList());
            Ranks = new List<int>(Cards.Select(x => x.RankAsInteger).OrderBy(y => y).ToList());
            RankCount = Ranks.Distinct().Count();
            SuitCount = Suits.Distinct().Count();
            RankSum = Ranks.Sum();
            SetHandScore();
            SetHandType();
        }
        public PokerHand(List<string> cards)
        {
            Cards = new List<PlayingCard>();
            foreach(string card in cards)
            {
                Cards.Add(new PlayingCard(card));
            }
            Suits = new List<int>(Cards.Select(x => x.SuitAsInteger).OrderBy(y => y).ToList());
            Ranks = new List<int>(Cards.Select(x => x.RankAsInteger).OrderByDescending(y => y).ToList());
            RankCount = Ranks.Distinct().Count();
            SuitCount = Suits.Distinct().Count();
            RankSum = Ranks.Sum();
            SetHandScore();
            SetHandType();
            InputString = @"'";
            for(int i = 0; i < 4; i++)
            {
                InputString += cards[i] + " ";
            }
            InputString += cards[4] + @"'";
            RanksString = @"";
            for (int i = 0; i < 4; i++)
            {
                RanksString += Ranks[i].ToString() + ",";
            }
            RanksString += Ranks[4].ToString() + @"";
        }
        private void SetHandType()
        {
            switch (HandScore)
            {
                case 10:
                    HandType = @"'Royal Flush'";
                    break;
                case 9:
                    HandType = @"'Straight Flush'";
                    break;
                case 8:
                    HandType = @"'Four of a Kind'";
                    break;
                case 7:
                    HandType = @"'Full House'";
                    break;
                case 6:
                    HandType = @"'Flush'";
                    break;
                case 5:
                    HandType = @"'Straight'";
                    break;
                case 4:
                    HandType = @"'Three of a Kind'";
                    break;
                case 3:
                    HandType = @"'Two Pairs'";
                    break;
                case 2:
                    HandType = @"'One Pair'";
                    break;
                case 1:
                    HandType = @"'High Card'";
                    break;
                case 0:
                    HandType = @"'Error'";
                    break;
                default:
                    HandType = @"'Error'";
                    break;
            }
        }
        private void SetHandScore()
        {
            HandScore = 0;
            if(IsRoyalFlush())
            {
                HandScore = 10;
            }
            else if (IsStraightFlush())
            {
                HandScore = 9;
            }
            else if (IsFourKind())
            {
                HandScore = 8;
            }
            else if (IsFullHouse())
            {
                HandScore = 7;
            }
            else if(IsFlush())
            {
                HandScore = 6;
            }
            else if (IsStraight())
            {
                HandScore = 5;
            }
            else if (IsThreeKind())
            {
                HandScore = 4;
            }
            else if (IsTwoPair())
            {
                HandScore = 3;
            }
            else if (IsOnePair())
            {
                HandScore = 2;
            }
            else
            {
                HandScore = 1;
            }
        }
        private bool IsRoyalFlush()
        {
            return (SuitCount == 1 && RankSum == 65);
        }
        private bool IsStraightFlush()
        {
            return (IsStraight() && IsFlush());
        }
        private bool IsFourKind()
        {
            if(RankCount == 2)
            {
                return (Ranks.Where(x => x == Ranks.First()).Count() == 1 || Ranks.Where(x => x == Ranks.First()).Count() == 4);
            }
            return false;
        }
        private bool IsFullHouse()
        {
            if (RankCount == 2)
            {
                return (Ranks.Where(x => x == Ranks.First()).Count() == 2 || Ranks.Where(x => x == Ranks.First()).Count() == 3);
            }
            return false;
        }
        private bool IsFlush()
        {
            return (SuitCount == 1);
        }
        private bool IsStraight()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Ranks[i] - 1 != Ranks[i + 1])
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsThreeKind()
        {
            foreach (int rank in Ranks.Distinct())
            {
                if (Ranks.Where(x => x == rank).Count() == 3)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsTwoPair()
        {
            int pairs = 0;
            foreach (int rank in Ranks.Distinct())
            {
                if (Ranks.Where(x => x == rank).Count() == 2)
                {
                    pairs++;
                }
            }
            return (pairs == 2);
        }
        private bool IsOnePair()
        {
            int pairs = 0;
            foreach (int rank in Ranks.Distinct())
            {
                if (Ranks.Where(x => x == rank).Count() == 2)
                {
                    pairs++;
                }
            }
            return (pairs == 1);
        }
        public List<int> GetHighCardList()
        {
            switch(HandScore)
            {
                case 10:
                    return new List<int>(Ranks);
                case 9:
                    return new List<int>(Ranks);
                case 8:
                    return GetFourKindHighCardList();
                case 7:
                    return GetFullHouseHighCardList();
                case 6:
                    return new List<int>(Ranks);
                case 5:
                    return new List<int>(Ranks);
                case 4:
                    return GetThreeKindHighCardList();
                case 3:
                    return GetTwoPairHighCardList();
                case 2:
                    return GetOnePairHighCardList();
                case 1:
                    return new List<int>(Ranks);
                case 0:
                    return new List<int>(Ranks);
                default:
                    return new List<int>(Ranks);
            }
        }
        private List<int> GetFourKindHighCardList()
        {
            int firstRank = Ranks.First();
            int firstRankCount = Ranks.Where(x => x == firstRank).Count();
            if (firstRankCount != 4)
            {
                firstRank = Ranks.Where(x => x != firstRank).FirstOrDefault();
            }
            List<int> results = new List<int>();
            results.Add(firstRank);
            results.Add(Ranks.Where(x => x != firstRank).FirstOrDefault());
            return results;
        }
        private List<int> GetFullHouseHighCardList()
        {
            int firstRank = Ranks.First();
            int firstRankCount = Ranks.Where(x => x == firstRank).Count();
            if (firstRankCount != 3)
            {
                firstRank = Ranks.Where(x => x != firstRank).FirstOrDefault();
            }
            List<int> results = new List<int>();
            results.Add(firstRank);
            results.Add(Ranks.Where(x => x != firstRank).FirstOrDefault());
            return results;
        }
        private List<int> GetThreeKindHighCardList()
        {
            int firstRank = 0;
            foreach (int rank in Ranks.Distinct().ToList())
            {
                if(Ranks.Where(x => x == rank).Count() == 3)
                {
                    firstRank = rank;
                }
            }
            List<int> results = new List<int>();
            results.Add(firstRank);
            List<int> otherRanks = Ranks.Where(x => x != firstRank).OrderByDescending(x => x).ToList();
            results.AddRange(otherRanks);
            return results;
        }
        private List<int> GetTwoPairHighCardList()
        {
            List<int> results = new List<int>();
            List<int> otherRanks = new List<int>();
            foreach (int rank in Ranks.Distinct().ToList())
            {
                if (Ranks.Where(x => x == rank).Count() == 2)
                {
                    results.Add(rank);
                }
                else
                {
                    otherRanks.Add(rank);
                }
            }
            results = results.OrderByDescending(x => x).ToList();
            otherRanks = otherRanks.OrderByDescending(x => x).ToList();
            results.AddRange(otherRanks);
            return results;
        }
        private List<int> GetOnePairHighCardList()
        {
            List<int> results = new List<int>();
            List<int> otherRanks = new List<int>();
            foreach (int rank in Ranks.Distinct().ToList())
            {
                if (Ranks.Where(x => x == rank).Count() == 2)
                {
                    results.Add(rank);
                }
                else
                {
                    otherRanks.Add(rank);
                }
            }
            results = results.OrderByDescending(x => x).ToList();
            otherRanks = otherRanks.OrderByDescending(x => x).ToList();
            results.AddRange(otherRanks);
            return results;
        }
        public Tuple<int, List<int>> GetHandScore()
        {
            List<int> suits = Cards.Select(x => x.SuitAsInteger).OrderBy(y => y).ToList();
            List<int> ranks = Cards.Select(x => x.RankAsInteger).OrderBy(y => y).ToList();
            int kindCount = ranks.Distinct().Count();
            int suitCount = suits.Distinct().Count();

            if (suitCount == 1)
            {
                if (ranks.Sum() == 65)
                {
                    return new Tuple<int, List<int>>(10, ranks);
                }
                bool straightFlush = true;
                for (int i = 0; i < 4; i++)
                {
                    if (ranks[i] + 1 != ranks[i + 1])
                    {
                        straightFlush = false;
                    }
                }
                if (straightFlush)
                {
                    return new Tuple<int, List<int>>(9, ranks);
                }
            }
            if (kindCount == 2)
            {
                int firstRank = ranks.First();
                int firstRankCount = ranks.Where(x => x == firstRank).Count();
                if (firstRankCount == 4)
                {
                    List<int> results = new List<int>();
                    results.Add(firstRank);
                    results.Add(firstRank);
                    results.Add(firstRank);
                    results.Add(firstRank);
                    results.Add(ranks.Where(x => x != firstRank).FirstOrDefault());
                    return new Tuple<int, List<int>>(8, results);
                }
                else if (firstRankCount == 1)
                {
                    int highCard = ranks.Where(x => x != firstRank).FirstOrDefault();
                    List<int> results = new List<int>();
                    results.Add(highCard);
                    results.Add(highCard);
                    results.Add(highCard);
                    results.Add(highCard);
                    results.Add(firstRank);
                    return new Tuple<int, List<int>>(8, results);
                }
                else if (firstRankCount == 3)
                {
                    List<int> results = new List<int>();
                    results.Add(firstRank);
                    results.Add(firstRank);
                    results.Add(firstRank);
                    results.Add(firstRank);
                    results.Add(ranks.Where(x => x != firstRank).FirstOrDefault());
                    return new Tuple<int, List<int>>(7, results);
                }
                else
                {
                    int highCard = ranks.Where(x => x != firstRank).FirstOrDefault();
                    List<int> results = new List<int>();
                    results.Add(highCard);
                    results.Add(highCard);
                    results.Add(highCard);
                    results.Add(highCard);
                    results.Add(firstRank);
                    return new Tuple<int, List<int>>(7, results);
                }
            }
            if (suitCount == 1)
            {
                return new Tuple<int, List<int>>(6, ranks);
            }
            bool straight = true;
            for (int i = 0; i < 4; i++)
            {
                if (ranks[i] + 1 != ranks[i + 1])
                {
                    straight = false;
                }
            }
            if (straight)
            {
                return new Tuple<int, List<int>>(5, ranks);
            }
            int pairs = 0;
            List<int> pairResults = new List<int>();
            foreach (int rank in ranks.Distinct().ToList())
            {
                int rankCount = ranks.Where(x => x == rank).Count();
                if (rankCount == 3)
                {
                    List<int> results = new List<int>();
                    results.Add(rank);
                    results.Add(rank);
                    results.Add(rank);
                    foreach(int otherRank in ranks.Where(x => x != rank).ToList())
                    {
                        results.Add(otherRank);
                    }
                    return new Tuple<int, List<int>>(4, results);
                }
                if (rankCount == 2)
                {
                    pairResults.Add(rank);
                    pairResults.Add(rank);
                    pairResults = pairResults.OrderBy(x => x).ToList();
                    pairs++;
                }
            }
            foreach (int rank in ranks)
            {
                if (!pairResults.Contains(rank))
                {
                    pairResults.Add(rank);
                }
            }
            if (pairs == 2)
            {
                return new Tuple<int, List<int>>(3, pairResults);
            }
            if (pairs == 1)
            {
                return new Tuple<int, List<int>>(2, pairResults);

            }
            return new Tuple<int, List<int>>(1, ranks);
        }
    }
    public class PlayingCard
    {
        public string SuitAsString { get; private set; }
        public int SuitAsInteger { get; private set; }
        public int RankAsInteger { get; private set; }
        public string RankAsString { get; private set; }
        private int ParseSuit(string suit)
        {
            switch (suit)
            {
                case "C":
                    return 1;
                case "D":
                    return 2;
                case "H":
                    return 3;
                case "S":
                    return 4;
                default:
                    return int.Parse(suit);
            }
        }
        private string ParseSuit(int suit)
        {
            switch (suit)
            {
                case 1:
                    return "C";
                case 2:
                    return "D";
                case 3:
                    return "H";
                case 4:
                    return "S";
                default:
                    return suit.ToString();
            }
        }
        private int ParseRank(string rank)
        {
            switch (rank)
            {
                case "T":
                    return 10;
                case "J":
                    return 11;
                case "Q":
                    return 12;
                case "K":
                    return 13;
                case "A":
                    return 14;
                default:
                    return int.Parse(rank);
            }
        }
        private string ParseRank(int rank)
        {
            switch (rank)
            {
                case 10:
                    return "T";
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                case 14:
                    return "A";
                default:
                    return rank.ToString(); 
            }
        }
        public PlayingCard(string suitRank)
        {
            SuitAsString = suitRank.Substring(1, 1);
            SuitAsInteger = ParseSuit(SuitAsString);
            RankAsString = suitRank.Substring(0, 1);
            RankAsInteger = ParseRank(RankAsString);
        }
        public PlayingCard(string suit, string rank)
        {
            SuitAsString = suit;
            SuitAsInteger = ParseSuit(suit);
            RankAsString = rank;
            RankAsInteger = ParseRank(rank);
        }
        public PlayingCard(string suit, int rank)
        {
            SuitAsString = suit;
            SuitAsInteger = ParseSuit(suit);
            RankAsString = ParseRank(rank);
            RankAsInteger = rank;
        }
        public PlayingCard(int suit, string rank)
        {
            SuitAsString = ParseSuit(suit);
            SuitAsInteger = suit;
            RankAsString = rank;
            RankAsInteger = ParseRank(rank);
        }
        public PlayingCard(int suit, int rank)
        {
            SuitAsString = ParseSuit(suit);
            SuitAsInteger = suit;
            RankAsString = ParseRank(rank);
            RankAsInteger = rank;
        }
    }
}
