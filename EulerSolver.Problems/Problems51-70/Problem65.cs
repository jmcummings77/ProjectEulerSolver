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

        }
    }
}
