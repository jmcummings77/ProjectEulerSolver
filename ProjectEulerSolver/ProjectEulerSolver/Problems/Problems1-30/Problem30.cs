using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
{
    public class Problem30 : BaseProblem, IProblem
    {
        public Problem30()
        {
            Number = 30;
            Prompt = "Surprisingly there are only three numbers that can be written as the sum of fourth powers of their digits: " +

                         "1634 = 1^4 + 6^4 + 3^4 + 4^4 " +
                         "8208 = 8^4 + 2^4 + 0^4 + 8^4 " +
                         "9474 = 9^4 + 4^4 + 7^4 + 4^4 " +
                     "As 1 = 14 is not a sum it is not included. " +

                     "The sum of these numbers is 1634 + 8208 + 9474 = 19316. " +

                     "Find the sum of all the numbers that can be written as the sum of fifth powers of their digits. ";
        }
        public override void Solve()
        {
            BigInteger result = 0;
            for(int i = 1; i < 1000000; i++)
            {
                var item = new Number(i);
                if(item.ToBigInteger() == item.GetSumOfDigitsToNthPower(5))
                {
                    result += i;
                }
            }
            Output = result.ToString();
        }
    }
}