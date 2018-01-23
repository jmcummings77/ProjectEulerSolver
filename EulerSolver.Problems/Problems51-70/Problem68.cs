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
    public class Problem68 : BaseProblem, IProblem
    {
        public Problem68()
        {
            Number = 68;
            Prompt = "https://projecteuler.net/problem=68"
                    + "     Using the numbers 1 to 10, and depending on arrangements, it is possible to form 16- and 17-digit strings. What is the maximum 16-digit string for a 'magic' 5-gon ring?";
        }
        public override void Solve()
        {
            LogList = new List<string>();

        }
    }
}
