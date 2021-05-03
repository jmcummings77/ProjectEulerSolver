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
    public class Problem57 : BaseProblem, IProblem
    {
        public Problem57()
        {
            Number = 57;
            Prompt = "It is possible to show that the square root of two can be expressed as an infinite continued fraction. " +
                     "      √ 2 = 1 + 1/(2 + 1/(2 + 1/(2 + ... ))) = 1.414213... " +
                     "By expanding this for the first four iterations, we get: " +
                     "      1 + 1/2 = 3/2 = 1.5 " +
                     "      1 + 1/(2 + 1/2) = 7/5 = 1.4 " +
                     "      1 + 1/(2 + 1/(2 + 1/2)) = 17/12 = 1.41666... " +
                     "      1 + 1/(2 + 1/(2 + 1/(2 + 1/2))) = 41/29 = 1.41379... " +
                     "The next three expansions are 99/70, 239/169, and 577/408, but the eighth expansion, 1393/985, is the first example where the number of digits in the numerator exceeds the number of digits in the denominator. " +
                     "In the first one-thousand expansions, how many fractions contain a numerator with more digits than denominator?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int result = 0;
            var currentTerm = new Fraction((BigInteger)1, (BigInteger)2);
            var modifier = new Fraction((BigInteger)2, (BigInteger)1);
            for (int i = 0; i < 1000; i++)
            {
                var divisee = new Fraction((BigInteger)1, (BigInteger)1);
                currentTerm.Add(modifier);
                divisee.DivideBy(currentTerm);
                currentTerm = divisee;
                var resultTerm = new Fraction((BigInteger)1, (BigInteger)1);
                resultTerm.Add(currentTerm);
                if (resultTerm.GetDenominatorDigitCount() < resultTerm.GetNumeratorDigitCount())
                {
                    result++;
                }
            }
            Output = result.ToString();
        }
    }
}
