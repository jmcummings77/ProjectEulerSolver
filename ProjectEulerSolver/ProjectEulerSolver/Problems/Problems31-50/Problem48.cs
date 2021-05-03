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
    public class Problem48 : BaseProblem, IProblem
    {
        public Problem48()
        {
            Number = 48;
            Prompt = "The series, 1^1 + 2^2 + 3^3 + ... + 10^10 = 10405071317. " +
                     "Find the last ten digits of the series, 1^1 + 2^2 + 3^3 + ... + 1000^1000 ";
        }
        public override void Solve()
        {
            BigInteger total = 0;
            for(int i = 1; i < 1001; i++)
            {
                total += BigInteger.Pow(i, i);
            }
            Output = total.ToString().Substring(total.ToString().Length - 10);
        }
    }
}
