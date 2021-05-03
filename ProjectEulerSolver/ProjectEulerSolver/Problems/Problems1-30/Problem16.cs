using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;

namespace ProjectEulerSolver.Problems
{
    public class Problem16 : BaseProblem, IProblem
    {
        public Problem16()
        {
            Number = 16;
            Prompt = "215 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26. What is the sum of the digits of the number 2^1000?";
        }
        public override void Solve()
        {
            var x = new VeryLargeNumber(BigInteger.Pow(2,1000));
            Output = x.SumEachDigitInValue().ToString();
        }
    }
}