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
    public class Problem73 : BaseProblem, IProblem
    {
        public Problem73()
        {
            Number = 73;
            Prompt = "Consider the fraction, n/d, where n and d are positive integers. If n<d and HCF(n,d)=1, it is called a reduced proper fraction. " +
                     "If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get: " +
                     "       1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8 " +
                     "It can be seen that there are 3 fractions between 1/3 and 1/2. " +
                     "How many fractions lie between 1/3 and 1/2 in the sorted set of reduced proper fractions for d ≤ 12,000?";
        }
        public override void Solve()
        {
            int minimumDenominator = 3;
            int maximumDenoninator = 2;
            int limit = 12001;
            long result = 0;

            for (int i = 1; i < limit; i++)
            {
                int max = ((i - 1) / maximumDenoninator) + 1;
                int min = (i / minimumDenominator) + 1;
                for (int j = min; j < max; j++)
                {
                    if (GetGreatestCommonDivisor(i,j) == 1)
                    {
                        result++;
                    }
                }
            }
            Output = result.ToString();
        }
        private static int GetGreatestCommonDivisor(int a, int b)
        {
            if (a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            int remainder;
            int priorRemainder;
            int nextRemainder;
            int quotient = Math.DivRem(a, b, out priorRemainder);
            if (priorRemainder != 0)
            {
                quotient = Math.DivRem(b, priorRemainder, out remainder);
                if (remainder != 0)
                {
                    nextRemainder = remainder;
                    while (remainder != 0)
                    {
                        quotient = Math.DivRem(priorRemainder, nextRemainder, out remainder);
                        priorRemainder = nextRemainder;
                        nextRemainder = remainder;
                    }
                    return priorRemainder;
                }
                else
                {
                    return priorRemainder;
                }
            }
            else
            {
                return b;
            }
        }

    }
}
