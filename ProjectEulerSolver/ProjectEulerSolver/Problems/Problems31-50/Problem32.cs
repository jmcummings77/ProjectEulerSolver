using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
{
    public class Problem32 : BaseProblem, IProblem
    {
        public Problem32()
        {
            Number = 32;
            Prompt = "We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once; for example, the 5-digit number, 15234, is 1 through 5 pandigital. " +
                        "The product 7254 is unusual, as the identity, 39 × 186 = 7254, containing multiplicand, multiplier, and product is 1 through 9 pandigital. " +
                        "Find the sum of all products whose multiplicand/multiplier/product identity can be written as a 1 through 9 pandigital. " +
                        "HINT: Some products can be obtained in more than one way so be sure to only include it once in your sum.";
        }
        public override void Solve()
        {
            for (int pandigitalCount = 4; pandigitalCount < 10; pandigitalCount++)
            {
                HashSet<int> results = new HashSet<int>();
                var excludedDigits = GetExcludedDigits(pandigitalCount);
                LogList = new List<string>();
                for (int i = 1; i < 7853; i++)
                {
                    var digitList = i.ToString().ToCharArray();
                    if (digitList.Distinct().Count() == digitList.Count())
                    {
                        if (!HasExcludedDigits(i.ToString(), excludedDigits))
                        {
                            var factorPairs = GetFactorPairs(i, excludedDigits);
                            foreach (Tuple<int, int> pair in factorPairs)
                            {
                                if(i == 54321)
                                {
                                    Console.WriteLine("here");
                                }
                                    string fullList = pair.Item1.ToString() + pair.Item2.ToString() + i.ToString();
                                if (fullList.ToCharArray().Distinct().Count() == fullList.Length)
                                {
                                    if (fullList.Length == pandigitalCount)
                                    {
                                        results.Add(i);
                                        LogList.Add("N: " + pandigitalCount.ToString() + " Number: " + i.ToString() + " Factors: [" + pair.Item1.ToString() + ", " + pair.Item2.ToString() + "]");
                                    }
                                }
                            }
                        }
                    }
                    Output = results.Distinct().Sum().ToString();
                }
                LogToFile();
                LogList.Clear();
            }
        }
        public static bool HasExcludedDigits(string fullList, string[] excludedDigits)
        {
            for (int j = 0; j < excludedDigits.Length; j++)
            {
                if (fullList.Contains(excludedDigits[j]))
                {
                    return true;
                }
            }
            return false;
        }
        public static string[] GetExcludedDigits(int digitCount)
        {
            switch(digitCount)
            {
                case 4:
                    return new string[] { "5", "6", "7", "8", "9", "0" };
                case 5:
                    return new string[] {"6", "7", "8", "9", "0" };
                case 6:
                    return new string[] { "7", "8", "9", "0" };
                case 7:
                    return new string[] { "8", "9", "0" };
                case 8:
                    return new string[] {"9", "0" };
                default:
                    return new string[] { "0" };
            }
        }
        public static List<Tuple<int, int>> GetFactorPairs(int Value, string[] excludedDigits)
        {
            var factorPairs = new List<Tuple<int, int>>();

            factorPairs.Add(new Tuple<int, int>(1, Value));

            for (int i = 2; i < Math.Sqrt(Value) + 1; i++)
            {
                if (Value % i == 0)
                {
                    if (!HasExcludedDigits(i.ToString(), excludedDigits))
                    {
                        if (!HasExcludedDigits((Value / i).ToString(), excludedDigits))
                        {
                            factorPairs.Add(new Tuple<int, int>(i, Value / i));
                        }
                    }
                }
            }

            return factorPairs;
        }
    }
}