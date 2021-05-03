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
    public class Problem77 : BaseProblem, IProblem
    {
        public Problem77()
        {
            Number = 77;
            Prompt = "It is possible to write ten as the sum of primes in exactly five different ways: " +
                     "	7 + 3 " +
                     "	5 + 5 " +
                     "	5 + 3 + 2 " +
                     "	3 + 3 + 2 + 2 " +
                     "	2 + 2 + 2 + 2 + 2 " +
                     "What is the first value which can be written as the sum of primes in over five thousand different ways?";
        }
        public override void Solve()
        {
            int upperLimit = 6;
            List<int> primes = new List<int>();
            primes.Add(1);
            for(int result = 2; result < upperLimit; result++)
            {
                int[] sums = new int[result + 1];
                sums[0] = 1;
                for (int i = 1; i < primes.Count(); i++)
                {
                    for (int j = primes[i]; j < result + 1; j++)
                    {
                        sums[j] += sums[j - primes[i]];
                    }
                }
                Output = sums.Last().ToString();
                if (sums.Last() > 5000)
                {
                    Output = result.ToString();
                    break;
                }
                else if(IsPrime(result))
                {
                    primes.Add(result);
                }
            }
            


        }
        
        private static int[] PrimeSieve(int LowerBound, int UpperBound)
        {
            var primes = new List<int>();
            for (int i = LowerBound; i < UpperBound; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            return primes.ToArray();
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
