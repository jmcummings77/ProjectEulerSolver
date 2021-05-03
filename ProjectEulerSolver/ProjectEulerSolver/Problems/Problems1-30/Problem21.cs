using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace ProjectEulerSolver.Problems
{
    public class Problem21 : BaseProblem, IProblem
    {
        public Problem21()
        {
            Number = 21;
            Prompt = "Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n). " +
                     "If d(a) = b and d(b) = a, where a ≠ b, then a and b are an amicable pair and each of a and b are called amicable numbers. " +
                     "For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110; therefore d(220) = 284. The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220. " +
                     "Evaluate the sum of all the amicable numbers under 10000.";
            Input = "10000";
        }
        public override void Solve()
        {
            int maxValue = int.Parse(Input);
            int result = 0;
            Dictionary<int, int> NumberDivisorSumPairs = new Dictionary<int, int>();

            for(int i = 1; i < maxValue; i++)
            {
                var number = new VeryLargeNumber((BigInteger)i).GetSumOfDivisors();
                List<int> divs = new VeryLargeNumber((BigInteger)i).GetDivisors();
                
                if(number < maxValue)
                {
                    NumberDivisorSumPairs.Add(i, number);
                }
                
            }
        
            foreach(int key in NumberDivisorSumPairs.Keys)
            {
                if(NumberDivisorSumPairs.ContainsKey(NumberDivisorSumPairs[key]))
                {
                    if(NumberDivisorSumPairs[NumberDivisorSumPairs[key]] == key)
                    {
                        if(NumberDivisorSumPairs[key] != key)
                        {
                            result += key;
                        }
                    }
                }
            }
            Output = result.ToString();
        }
    }
}