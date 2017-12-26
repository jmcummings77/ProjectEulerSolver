using System;
using System.Collections.Generic;
using System.Linq;
using EulerSolver.Problems;

namespace EulerSolver.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var problem = new Problem27();
            problem.Solve();
            Console.WriteLine(problem.Output);
            Console.ReadLine();
        }
    }
}
