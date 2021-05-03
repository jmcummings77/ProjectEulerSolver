using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEulerSolver.ProjectEulerSolver.Tools
{
    class Solitaire
    {
        public class MessagePlayingCard
        {
            private string Input;
            private string Output;
            private List<string> LogList;
            private void LogToFile()
            {

            }
            private void DecodeSolitaire()
            {
                ReadLinesFromFile();
                Input = "YUORUGKSXCGUVTMORPWPQBFXKULHYVUXOAH";
                Input = Input.ToLower();
                Dictionary<string, int> scores = GetAlphabetScoreDictionary();
                var revScores = GetReverseAlphabetScoreDictionary();
                CardDeck = new List<MessagePlayingCard>(deck);
                for (int i = 0; i < Input.Length; i++)
                {
                    var result = GetKey(CardDeck);
                    if (i == 1)
                    {
                        Console.WriteLine("here");
                    }
                    if (result.Item1 > 26)
                    {
                        CardDeck = new List<MessagePlayingCard>(result.Item2);
                        result = GetKey(CardDeck);
                    }
                    int score = 26;
                    if ((result.Item1) >= scores[Input.Substring(i, 1)])
                    {
                        score = scores[Input.Substring(i, 1)] + 26 - (result.Item1);
                    }
                    else
                    {
                        score = scores[Input.Substring(i, 1)] - (result.Item1);
                    }
                    CardDeck = new List<MessagePlayingCard>(result.Item2);
                    Output += revScores[score];
                    Console.WriteLine(Output);
                }


            }
            private Dictionary<int, string> GetReverseAlphabetScoreDictionary()
            {
                var scores = GetAlphabetScoreDictionary();
                var revScores = new Dictionary<int, string>();
                foreach (int val in scores.Values)
                {
                    foreach (string letter in scores.Keys)
                    {
                        if (scores[letter] == val)
                        {
                            revScores[val] = letter;
                        }
                    }
                }

                return revScores;

            }
            private Dictionary<string, int> GetAlphabetScoreDictionary()
            {
                var scores = new Dictionary<string, int>();
                scores["a"] = 1;
                scores["b"] = 2;
                scores["c"] = 3;
                scores["d"] = 4;
                scores["e"] = 5;
                scores["f"] = 6;
                scores["g"] = 7;
                scores["h"] = 8;
                scores["i"] = 9;
                scores["j"] = 10;
                scores["k"] = 11;
                scores["l"] = 12;
                scores["m"] = 13;
                scores["n"] = 14;
                scores["o"] = 15;
                scores["p"] = 16;
                scores["q"] = 17;
                scores["r"] = 18;
                scores["s"] = 19;
                scores["t"] = 20;
                scores["u"] = 21;
                scores["v"] = 22;
                scores["w"] = 23;
                scores["x"] = 24;
                scores["y"] = 25;
                scores["z"] = 26;
                return scores;

            }
            private static List<MessagePlayingCard> IterateAJokerPosition(List<MessagePlayingCard> workingDeck)
            {
                var iteratedDeck = new List<MessagePlayingCard>();
                if (workingDeck.Last().IsAJoker)
                {
                    workingDeck[0].Position = 0;
                    iteratedDeck.Add(workingDeck[0]);
                    iteratedDeck.Add(workingDeck.Where(x => x.IsAJoker).FirstOrDefault());
                    iteratedDeck[1].Position = 1;
                    for (int i = 1; i < workingDeck.Count() - 1; i++)
                    {
                        workingDeck[i].Position = i + 1;
                        iteratedDeck.Add(workingDeck[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < workingDeck.Count(); i++)
                    {

                        if (workingDeck[i].IsAJoker)
                        {
                            workingDeck[i].Position = i + 1;
                            iteratedDeck.Add(workingDeck[i]);
                            workingDeck[i + 1].Position = i;
                            iteratedDeck.Add(workingDeck[i + 1]);
                            i++;
                        }
                        else
                        {
                            workingDeck[i].Position = i;
                            iteratedDeck.Add(workingDeck[i]);
                        }
                    }
                }
                return iteratedDeck;
            }
            private static List<MessagePlayingCard> IterateBJokerPosition(List<MessagePlayingCard> workingDeck)
            {
                var iteratedDeck = new List<MessagePlayingCard>();
                int bPosition = workingDeck.Where(x => x.IsBJoker).FirstOrDefault().Position;
                if (bPosition == 51)
                {
                    workingDeck.Where(x => x.IsBJoker).FirstOrDefault().Position = 0;
                    iteratedDeck.Add(workingDeck.Where(x => x.IsBJoker).FirstOrDefault());
                    for (int i = 0; i < 51; i++)
                    {
                        workingDeck[i].Position++;
                        iteratedDeck.Add(workingDeck[i]);
                    }
                    iteratedDeck.Add(workingDeck[52]);
                    iteratedDeck.Add(workingDeck[53]);

                }
                else if (bPosition == 52)
                {
                    iteratedDeck.Add(workingDeck[0]);
                    workingDeck.Where(x => x.IsBJoker).FirstOrDefault().Position = 1;
                    iteratedDeck.Add(workingDeck.Where(x => x.IsBJoker).FirstOrDefault());
                    for (int i = 1; i < 52; i++)
                    {
                        workingDeck[i].Position++;
                        iteratedDeck.Add(workingDeck[i]);
                    }
                    iteratedDeck.Add(workingDeck[53]);
                }
                else if (bPosition == 53)
                {
                    iteratedDeck.Add(workingDeck[0]);
                    iteratedDeck.Add(workingDeck[1]);
                    workingDeck.Where(x => x.IsBJoker).FirstOrDefault().Position = 2;
                    iteratedDeck.Add(workingDeck.Where(x => x.IsBJoker).FirstOrDefault());
                    for (int i = 2; i < 53; i++)
                    {
                        workingDeck[i].Position++;
                        iteratedDeck.Add(workingDeck[i]);
                    }
                }
                else
                {
                    workingDeck.Where(x => x.Position == bPosition + 1).FirstOrDefault().Position = bPosition;
                    workingDeck.Where(x => x.Position == bPosition + 2).FirstOrDefault().Position = bPosition + 1;
                    workingDeck.Where(x => x.IsBJoker).FirstOrDefault().Position = bPosition + 2;
                    iteratedDeck.AddRange(workingDeck.OrderBy(x => x.Position));
                }
                return iteratedDeck;
            }
            private static List<MessagePlayingCard> TripleCut(List<MessagePlayingCard> workingDeck)
            {
                var iteratedDeck = new List<MessagePlayingCard>();
                int aPosition = workingDeck.Where(x => x.IsAJoker).FirstOrDefault().Position;
                int bPosition = workingDeck.Where(x => x.IsBJoker).FirstOrDefault().Position;
                int firstCutPosition = Math.Min(aPosition, bPosition);
                int secondCutPosition = Math.Max(aPosition, bPosition);
                int currentPosition = 0;
                for (int i = secondCutPosition + 1; i <= 53; i++)
                {
                    workingDeck[i].Position = currentPosition;
                    iteratedDeck.Add(workingDeck[i]);
                    currentPosition++;
                }
                for (int i = firstCutPosition; i <= secondCutPosition; i++)
                {
                    workingDeck[i].Position = currentPosition;
                    iteratedDeck.Add(workingDeck[i]);
                    currentPosition++;
                }
                for (int i = 0; i < firstCutPosition; i++)
                {
                    workingDeck[i].Position = currentPosition;
                    iteratedDeck.Add(workingDeck[i]);
                    currentPosition++;
                }
                return iteratedDeck;
            }
            private static List<MessagePlayingCard> CountCut(List<MessagePlayingCard> workingDeck)
            {
                var iteratedDeck = new List<MessagePlayingCard>();
                if (workingDeck.Last().IsAJoker || workingDeck.Last().IsBJoker)
                {
                    return workingDeck;
                }
                int countCut = workingDeck.Last().RankAsInteger - 1;
                int currentPosition = 0;
                for (int i = countCut + 1; i < 53; i++)
                {
                    workingDeck[i].Position = currentPosition;
                    iteratedDeck.Add(workingDeck[i]);
                    currentPosition++;
                }
                for (int i = 0; i <= countCut; i++)
                {
                    workingDeck[i].Position = currentPosition;
                    iteratedDeck.Add(workingDeck[i]);
                    currentPosition++;
                }
                iteratedDeck.Add(workingDeck[53]);


                return iteratedDeck;
            }
            private Tuple<int, List<MessagePlayingCard>> GetKey(List<MessagePlayingCard> workingDeck)
            {
                LogList = new List<string>();

                //Move A down one
                workingDeck = IterateAJokerPosition(workingDeck);
                foreach (MessagePlayingCard card in workingDeck)
                {
                    LogList.Add(card.RankAsString + card.SuitAsString);
                }
                LogList.Add(" ");
                LogToFile();
                LogList.Clear();
                //Move B down two
                workingDeck = IterateBJokerPosition(workingDeck);
                foreach (MessagePlayingCard card in workingDeck)
                {
                    LogList.Add(card.RankAsString + card.SuitAsString);
                }
                LogList.Add(" ");
                LogToFile();
                LogList.Clear();
                //swap beginning to first joker with second joker to end
                workingDeck = TripleCut(workingDeck);
                foreach (MessagePlayingCard card in workingDeck)
                {
                    LogList.Add(card.RankAsString + card.SuitAsString);
                }
                LogList.Add(" ");
                LogToFile();
                LogList.Clear();
                //convert bottom card to number
                //value + suit value (c =0, d = 13, h = 26, s = 39)
                //count that number down from top, cut from there to end minus one
                workingDeck = CountCut(workingDeck);
                foreach (MessagePlayingCard card in workingDeck)
                {
                    LogList.Add(card.RankAsString + card.SuitAsString);
                }
                LogList.Add(" ");
                LogToFile();
                LogList.Clear();
                //look at top card, get value, count down from there to get ouput card. convert output card to number

                return new Tuple<int, List<MessagePlayingCard>>(GetOutputCardValue(workingDeck), workingDeck);
            }
            private static int GetOutputCardValue(List<MessagePlayingCard> workingDeck)
            {
                int finalCardPosition = workingDeck.First().RankAsInteger - 1;
                int finalCardRank = workingDeck[finalCardPosition].RankAsInteger;
                int finalCardSuitValue = workingDeck[finalCardPosition].SuitAsInteger * 13;
                int finalCardValue = finalCardRank - finalCardSuitValue;
                if (finalCardValue < 0)
                {
                    return 53;
                }
                return finalCardValue;
            }
            private List<MessagePlayingCard> CardDeck { get; set; }
            private List<MessagePlayingCard> deck { get; set; }
            private void ReadLinesFromFile()
            {
                deck = new List<MessagePlayingCard>();
                string SourceFile = @"C:\Users\us48610\Desktop\cards.txt";
                string line;
                int linecount = 0;
                using (System.IO.StreamReader file = new System.IO.StreamReader(SourceFile))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        deck.Add(new MessagePlayingCard(line, linecount));
                        linecount++;
                    }
                }
            }
            public int Position { get; set; }
            public string SuitAsString { get; private set; }
            public int SuitAsInteger { get; private set; }
            public int RankAsInteger { get; private set; }
            public string RankAsString { get; private set; }
            public bool IsAJoker { get; set; }
            public bool IsBJoker { get; set; }

            private int ParseSuit(string suit)
            {
                switch (suit)
                {
                    case "C":
                        return 0;
                    case "D":
                        return 1;
                    case "H":
                        return 2;
                    case "S":
                        return 3;
                    default:
                        return int.Parse(suit);
                }
            }
            private string ParseSuit(int suit)
            {
                switch (suit)
                {
                    case 0:
                        return "C";
                    case 1:
                        return "D";
                    case 2:
                        return "H";
                    case 3:
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
                        return 1;
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
                    case 1:
                        return "A";
                    default:
                        return rank.ToString();
                }
            }
            public MessagePlayingCard(string suitRank, int position)
            {
                Position = position;
                if (suitRank == "A Joker")
                {
                    SuitAsString = "A";
                    SuitAsInteger = 5;
                    RankAsString = "A Joker";
                    RankAsInteger = 53;
                    IsAJoker = true;
                }
                else if (suitRank == "B Joker")
                {
                    SuitAsString = "B";
                    SuitAsInteger = 6;
                    RankAsString = "B Joker";
                    RankAsInteger = 53;
                    IsBJoker = true;
                }
                else
                {
                    RankAsString = suitRank.Substring(0, 1);
                    RankAsInteger = ParseRank(RankAsString);
                    SuitAsString = suitRank.Substring(1, 1);
                    SuitAsInteger = ParseSuit(SuitAsString);
                    switch (SuitAsInteger)
                    {
                        case 0:
                            break;
                        case 1:
                            RankAsInteger += 13;
                            break;

                        case 2:
                            RankAsInteger += 26;
                            break;

                        case 3:
                            RankAsInteger += 39;
                            break;

                    }

                }
            }
        }

    }
}
