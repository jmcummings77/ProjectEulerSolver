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
    public class Problem56 : BaseProblem, IProblem
    {
        public Problem56()
        {
            Number = 56;
            Prompt = "A googol (10^100) is a massive number: one followed by one-hundred zeros; 100^100 is almost unimaginably large: one followed by two-hundred zeros. Despite their size, the sum of the digits in each number is only 1. " +
                     "Considering natural numbers of the form, a^b, where a, b < 100, what is the maximum digital sum?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            long result = 0;
            for(int a = 1; a < 100; a++)
            {
                for(int b = 1; b < 100; b++)
                {
                    var number = new VeryLargeNumber(BigInteger.Pow(a, b));
                    long sum = (long)number.SumEachDigitInValue();
                    if(sum > result)
                    {
                        result = sum;
                    }
                }
            }
            Output = result.ToString();
        }
    }
}
