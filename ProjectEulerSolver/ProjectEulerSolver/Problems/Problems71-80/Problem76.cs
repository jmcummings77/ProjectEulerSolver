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
    public class Problem76 : BaseProblem, IProblem
    {
        public Problem76()
        {
            Number = 76;
            Prompt = "It is possible to write five as a sum in exactly six different ways: " +
                     "  4 + 1 " +
                     "  3 + 2 " +
                     "  3 + 1 + 1 " +
                     "  2 + 2 + 1 " +
                     "  2 + 1 + 1 + 1 " +
                     "  1 + 1 + 1 + 1 + 1 " +
                     "How many different ways can one hundred be written as a sum of at least two positive integers?";
        }
        public override void Solve()
        {
            int[] sums = new int[101];
            sums[0] = 1;
            for (int i = 1; i < 100; i++)
            {
                for (int j = i; j < 101; j++)
                {
                    sums[j] += sums[j - i];
                }
            }
            Output = sums.Last().ToString();
        }
    }
}
