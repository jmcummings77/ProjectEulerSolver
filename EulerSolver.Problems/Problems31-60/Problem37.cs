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
    public class Problem37 : BaseProblem, IProblem
    {
        public Problem37()
        {
            Number = 37;
            Prompt = "The number 3797 has an interesting property. Being prime itself, it is possible to continuously remove digits from left to right, and remain prime at each stage: 3797, 797, 97, and 7. Similarly we can work from right to left: 3797, 379, 37, and 3. " +
                     "Find the sum of the only eleven primes that are both truncatable from left to right and right to left. " +
                     "NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int result = 0;
            

            LogList.Add("Total: " + result.ToString());

            LogToFile();
            Output = result.ToString();
        }
    }
}