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
    public class Problem69 : BaseProblem, IProblem
    {
        public Problem69()
        {
            Number = 69;
            Prompt = "https://projecteuler.net/problem=69"
                   + "   Find the value of n ≤ 1,000,000 for which n/φ(n) is a maximum.";
        }
        public override void Solve()
        {
            LogList = new List<string>();

        }
    }
}
