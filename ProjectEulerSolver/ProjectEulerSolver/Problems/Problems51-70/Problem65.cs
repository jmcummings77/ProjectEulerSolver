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
    public class Problem65 : BaseProblem, IProblem
    {
        public Problem65()
        {
            Number = 65;
            Prompt = "https://projecteuler.net/problem=65"
                    + "  Find the sum of digits in the numerator of the 100th convergent of the continued fraction for e.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int upperBound = 101;
            BigInteger denominator = 1;
            BigInteger numerator = 2;

            for (int i = 2; i < upperBound; i++)
            {
                BigInteger currentDenominator = denominator;
                int n = (i % 3 == 0) ? 2 * (i / 3) : 1;
                denominator = numerator;
                numerator = n * denominator + currentDenominator;
            }
           
            int result = numerator.ToString().ToCharArray().Select(x => int.Parse(x.ToString())).Sum();
            Output = result.ToString();
        }
    }
}
