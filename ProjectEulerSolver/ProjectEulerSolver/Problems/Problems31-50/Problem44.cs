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
    public class Problem44 : BaseProblem, IProblem
    {
        public Problem44()
        {
            Number = 44;
            Prompt = "Pentagonal numbers are generated by the formula, Pn=n(3n−1)/2. The first ten pentagonal numbers are: " +
                     "1, 5, 12, 22, 35, 51, 70, 92, 117, 145, ... " +
                     "It can be seen that P4 + P7 = 22 + 70 = 92 = P8. However, their difference, 70 − 22 = 48, is not pentagonal. " +
                     "Find the pair of pentagonal numbers, Pj and Pk, for which their sum and difference are pentagonal and D = |Pk − Pj| is minimised; what is the value of D?";

        }
        public override void Solve()
        {
            Output = GetCompositePentagonNumber().ToString();
        }
        private long GetCompositePentagonNumber()
        {
            bool NoMatch = true;
            long result = 0;
            int i = 1;
            while(NoMatch)
            {
                int pentagonNumber = (i * (3 * i - 1)) / 2;
                for(int j = i - 1; j > 0; j--)
                {
                    int previousNumber = (j * (3 * j - 1)) / 2;
                    if (IsPentagonNumber(pentagonNumber - previousNumber) && IsPentagonNumber(pentagonNumber + previousNumber))
                    {
                        NoMatch = false;
                        result = pentagonNumber - previousNumber;
                        break;
                    }
                }
                i++;
            }
            return result;
        }
        private static bool IsPentagonNumber(long Value)
        {
            double n = (1 + Math.Sqrt(24 * Value + 1)) / 6;
            return (Math.Floor(n) == n);
        }
    }
}