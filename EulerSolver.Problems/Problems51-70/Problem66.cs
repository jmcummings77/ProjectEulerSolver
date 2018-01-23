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
    public class Problem66 : BaseProblem, IProblem
    {
        public Problem66()
        {
            Number = 66;
            Prompt = "Consider quadratic Diophantine equations of the form: " +
                     "x2 – Dy2 = 1 " +
                     "For example, when D=13, the minimal solution in x is 6492 – 13×1802 = 1. " +
                     "It can be assumed that there are no solutions in positive integers when D is square. " +
                     "By finding minimal solutions in x for D = {2, 3, 5, 6, 7}, we obtain the following: " +
                     "32 – 2×22 = 1 " +
                     "22 – 3×12 = 1 " +
                     "92 – 5×42 = 1 " +
                     "52 – 6×22 = 1 " +
                     "82 – 7×32 = 1 " +
                     "Hence, by considering minimal solutions in x for D ≤ 7, the largest x is obtained when D=5. " +
                     "Find the value of D ≤ 1000 in minimal solutions of x for which the largest value of x is obtained.";
        }
        public override void Solve()
        {
            LogList = new List<string>();

        }
    }
}
