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
    public class Problem : BaseProblem, IProblem
    {
        public Problem()
        {
            Number = 0;
            Prompt = "";
        }
        public override void Solve()
        {
            LogList = new List<string>();

        }
    }
}
