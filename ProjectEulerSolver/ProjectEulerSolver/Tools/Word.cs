using ProjectEulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectEulerSolver.Tools
{
    public class Word
    {
        public string Value { get; private set; }
        public Word(string _Value)
        {
            Value = _Value;
            Length = _Value.Length;
        }
        public BigInteger GetSimpleAlphabetScore()
        {
            return GetCharacterScore(GetAlphabetScoreDictionary(), "[^a-zA-Z -]", false);
        }
        public int Length { get; private set; }
        public string Clean(string InvalidCharacterRegEx)
        {
            Regex regx = new Regex(InvalidCharacterRegEx);
            string cleanedValue = regx.Replace(Value, "");
            return cleanedValue;
        }
        public BigInteger GetCharacterScore(Dictionary<string, int> ScoreLookup, string InvalidCharacterRegEx, bool matchCase = false)
        {
            BigInteger result = 0;
            string cleanedWord = Clean(InvalidCharacterRegEx);
            if(matchCase)
            {
                for(int j = 0; j < cleanedWord.Length; j++)
                {
                    result += ScoreLookup[cleanedWord.Substring(j,1)];
                }
            }
            else
            {
                for(int j = 0; j < cleanedWord.Length; j++)
                {
                    result += ScoreLookup[cleanedWord.Substring(j,1).ToLower()];
                }
            }
            return result;
        }
        public Dictionary<string, int> GetAlphabetScoreDictionary()
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
    }
}