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
    public class Problem41 : BaseProblem, IProblem
    {
        public Problem41()
        {
            Number = 41;
            Prompt = "We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once. For example, 2143 is a 4-digit pandigital and is also prime. " +
                     "What is the largest n-digit pandigital prime that exists?";
        }
        public override void Solve()
        {
           
        }
    }
}