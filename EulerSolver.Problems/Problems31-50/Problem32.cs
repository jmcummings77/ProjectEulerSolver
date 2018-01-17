using EulerSolver.Interfaces;
using EulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EulerSolver.Problems
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
            List<int> results = new List<int>();
            LogList = new List<string>();
            for(int i = 1; i < 1000000; i++)
            {
                var number = new Number(i);
                var digitList = i.ToString().ToCharArray();
                if(digitList.Distinct().Count() == digitList.Count())
                {
                    if (i.ToString().Contains("0"))
                    {
                    }
                    else
                    { 
                        var factorPairs = number.GetFactorPairs();
                        foreach(Tuple<int, int> pair in factorPairs)
                        {
                            string fullList = pair.Item1.ToString() + pair.Item2.ToString() + i.ToString();
                            if(fullList.Length == 9 && !fullList.Contains("0"))
                            {
                                if(fullList.ToCharArray().Distinct().Count() == 9)
                                {
                                    results.Add(i);
                                    LogList.Add("Number: " + i.ToString() + " Factors: [" + pair.Item1.ToString() + ", " + pair.Item2.ToString() + "]");
                                }
                            }
                        }
                    }
                }
            }
            LogFilePath = @"C:\Users\user\Desktop\Multipliers.txt";
            LogToFile();
            Output = results.Distinct().Sum().ToString();
        }
    }
}