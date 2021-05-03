using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;

namespace ProjectEulerSolver.Problems
{
    public class Problem20 : BaseProblem, IProblem
    {
        public Problem20()
        {
            Number = 20;
            Prompt = "n! means n × (n − 1) × ... × 3 × 2 × 1 " +
                     "For example, 10! = 10 × 9 × ... × 3 × 2 × 1 = 3628800, " +
                     "and the sum of the digits in the number 10! is 3 + 6 + 2 + 8 + 8 + 0 + 0 = 27. " +
                     "Find the sum of the digits in the number 100!";
            Input = "100";
        }
        public override void Solve()
        {

            var inputInteger = new VeryLargeNumber(BigInteger.Parse(Input));
            var outputInteger = new VeryLargeNumber(inputInteger.GetFactorial());
            Output = outputInteger.SumEachDigitInValue().ToString();
        }
    }
}