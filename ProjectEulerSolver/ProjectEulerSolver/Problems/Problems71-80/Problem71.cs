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
    public class Problem71 : BaseProblem, IProblem
    {
        public Problem71()
        {
            Number = 71;
            Prompt = "Consider the fraction, n/d, where n and d are positive integers. If n<d and HCF(n,d)=1, it is called a reduced proper fraction." +
                    "If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get:" +
                    "	1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8" +
                    "It can be seen that 2/5 is the fraction immediately to the left of 3/7." +
                    "By listing the set of reduced proper fractions for d ≤ 1,000,000 in ascending order of size, find the numerator of the fraction immediately to the left of 3/7.";
        }
        public override void Solve()
        {
            decimal goalRatio = (decimal)3.0 / (decimal)7.0;
            decimal currentLeftRatio = (decimal)0.0;
            int currentLeftNumerator = 2;
            int currentLeftDenominator = 5;

            for (int i = 3; i < 1000001; i++)
            {
                int max = (int)Math.Ceiling(((double)3.0 * (double)i) / (double)7.0);
                int min = (int)Math.Floor(((double)currentLeftNumerator * (double)i) / (double)currentLeftDenominator);
                for (int j = min; j < max; j++)
                {
                    decimal ratio = (decimal)j / (decimal)i;
                    if(ratio > currentLeftRatio)
                    {
                        currentLeftNumerator = j;
                        currentLeftDenominator = i;
                        currentLeftRatio = ratio;
                    }
                }
            }
            Output = currentLeftNumerator.ToString();
        }
    }
}
