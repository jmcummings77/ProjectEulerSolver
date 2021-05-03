using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using ProjectEulerSolver.Model;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ProjectEulerSolver.Problems
{
    public class Problem49 : BaseProblem, IProblem
    {
        public Problem49()
        {
            Number = 49;
            Prompt = "The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways: (i) each of the three terms are prime, and, (ii) each of the 4-digit numbers are permutations of one another. " +
                        "There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property, but there is one other 4-digit increasing sequence. " +
                        "What 12-digit number do you form by concatenating the three terms in this sequence?";
        }
        public override void Solve()
        {
            Output = "";
            var primes = new List<int>();
            for(int i = 1000; i < 10000; i++)
            {
                if(IsPrime(i)) { primes.Add(i); }
            }
            foreach (int i in primes)
            {
                var num = new Number(i);
                var permutations = num.GetAllDigitPermutations();
                permutations = permutations.OrderBy(x => x).ToList();
                var candidates = new List<int>();

                foreach (int j in permutations)
                {
                    if(IsPrime(j))
                    {
                        candidates.Add(j);
                    }
                }
                candidates = candidates.Distinct().ToList();
                if (candidates.Count() > 2)
                {
                    for (int j = candidates.Count() - 1; j > 0; j--)
                    {
                        for (int k = 0; k < j; k++)
                        {
                            int difference = candidates[j] - candidates[k];
                            if (candidates.Contains(candidates[j] + difference))
                            {
                                Output = candidates[k].ToString() + candidates[j].ToString() + (candidates[j] + difference).ToString();
                            }
                            else if (candidates.Contains(candidates[k] - difference))
                            {
                                Output = (candidates[k] - difference).ToString() + candidates[k].ToString() + candidates[j].ToString();
                            }
                        }
                    }
                }
            }
        }
        private static bool IsPrime(int Value)
        {
            if (Value < 1)
            {
                return false;
            }
            if (Value < 4)
            {
                return true;
            }
            if (Value % 2 == 0)
            {
                {
                    return false;
                }
            }
            for (int i = 3; i <= Math.Sqrt(Value); i = i + 2)
            {
                if (Value % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
