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
    public class Problem53 : BaseProblem, IProblem
    {
        public Problem53()
        {
            Number = 53;
            Prompt = "There are exactly ten ways of selecting three from five, 12345: " +
                        "	123, 124, 125, 134, 135, 145, 234, 235, 245, and 345 " +
                        "In combinatorics, we use the notation, 5C3 = 10. " +
                        "In general, " +
                        "	nCr =	n! / r!(n−r)! " +
                        "	,where r ≤ n, n! = n×(n−1)×...×3×2×1, and 0! = 1. " +
                        "It is not until n = 23, that a value exceeds one-million: 23C10 = 1144066. " +
                        "How many, not necessarily distinct, values of  nCr, for 1 ≤ n ≤ 100, are greater than one-million?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int result = 0;
            for(int i = 1; i < 101; i++)
            {
                for(int j = 1; j < i; j++)
                {
                    var n = new Number(i);
                    var nr = new Number(i - j);
                    var r = new Number(j);
                    if(n.GetFactorial()/(r.GetFactorial() * nr.GetFactorial()) > 1000000)
                    {
                        result++;
                    }
                }
            }
            Output = result.ToString();
        }
    }
}
