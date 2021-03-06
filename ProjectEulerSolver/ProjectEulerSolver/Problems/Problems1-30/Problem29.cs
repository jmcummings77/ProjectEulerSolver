using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
{
    public class Problem29 : BaseProblem, IProblem
    {
        public Problem29()
        {
            Number = 29;
            Prompt = "Consider all integer combinations of a^b for 2 ≤ a ≤ 5 and 2 ≤ b ≤ 5: " +

                "2^2=4, 2^3=8, 2^4=16, 2^5=32 " +
                "3^2=9, 3^3=27, 3^4=81, 3^5=243 " +
                "4^2=16, 4^3=64, 4^4=256, 4^5=1024 " +
                "5^2=25, 5^3=125, 5^4=625, 5^5=3125 " +
                "If they are then placed in numerical order, with any repeats removed, we get the following sequence of 15 distinct terms: " +

                "4, 8, 9, 16, 25, 27, 32, 64, 81, 125, 243, 256, 625, 1024, 3125 " +

                "How many distinct terms are in the sequence generated by a^b for 2 ≤ a ≤ 100 and 2 ≤ b ≤ 100? ";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int limit = 10000;
            HashSet<BigInteger> results = new HashSet<BigInteger>();
            for(BigInteger a = 2; a <= limit; a++)
            {
                for(BigInteger b = 2; b <= limit; b++)
                {
                    BigInteger x = BigInteger.Pow(a, (int)b);
                    results.Add(x);
                }
            }
            Output = results.Count().ToString();
        }
    }
}