using EulerSolver.Interfaces;
using EulerSolver.Tools;
using EulerSolver.Model;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace EulerSolver.Problems
{
    public class Problem64 : BaseProblem, IProblem
    {
        public Problem64()
        {
            Number = 64;
            Prompt = "https://projecteuler.net/problem=64"
                    + "  How many continued fractions for N ≤ 10000 have an odd period?";
        }
        public override void Solve()
        {
            LogList = new List<string>();

        }
    }
}
