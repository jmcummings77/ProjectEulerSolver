using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;

namespace ProjectEulerSolver.Problems
{
    public class Problem17 : BaseProblem, IProblem
    {
        public Problem17()
        {
            Number = 17;
            Prompt = "If the numbers 1 to 5 are written out in words: one, two, three, four, five, then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in total. If all the numbers from 1 to 1000 (one thousand) inclusive were written out in words, how many letters would be used?";
            Notes = " Do not count spaces or hyphens. For example, 342 (three hundred and forty-two) contains 23 letters and 115 (one hundred and fifteen) contains 20 letters. The use of 'and' when writing out numbers is in compliance with British usage.";
        }
        public override void Solve()
        {
            BigInteger result = 0;
            for(BigInteger i = 1; i < 1001; i++)
            {
                result += new VeryLargeNumber(i).ToWords().Replace(" ", "").Replace("-","").Length;
            }
            Output = result.ToString();
        }
    }
}