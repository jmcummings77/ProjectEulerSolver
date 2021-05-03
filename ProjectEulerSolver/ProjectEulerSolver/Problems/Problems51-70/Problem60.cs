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
    public class Problem60 : BaseProblem, IProblem
    {
        public Problem60()
        {
            Number = 60;
            Prompt = "The primes 3, 7, 109, and 673, are quite remarkable. By taking any two primes and concatenating them in any order the result will always be prime. For example, taking 7 and 109, both 7109 and 1097 are prime. The sum of these four primes, 792, represents the lowest sum for a set of four primes with this property. " +
                     "Find the lowest sum for a set of five primes for which any two primes concatenate to produce another prime.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            Output = SumOfFivePrimes().ToString();   
        }
        private static long SumOfFivePrimes()
        {
            List<int> primes = new List<int>();
            for (int i = 1; i < 10000; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            List<int> candidateSums = new List<int>();
            for (int i = 0; i < primes.Count(); i++)
            {
                for (int j = i + 1; j < primes.Count; j++)
                {
                    if (ConcatenatedTermsArePrime(primes[i], primes[j]))
                    {
                        for (int k = j; k < primes.Count; k++)
                        {
                            if (ConcatenatedTermsArePrime(primes[i], primes[k]) && ConcatenatedTermsArePrime(primes[j], primes[k]))
                            {
                                for (int l = k; l < primes.Count; l++)
                                {
                                    if (ConcatenatedTermsArePrime(primes[i], primes[l]) && ConcatenatedTermsArePrime(primes[j], primes[l]) && ConcatenatedTermsArePrime(primes[k], primes[l]))
                                    {
                                        for (int m = l; m < primes.Count; m++)
                                        {
                                            if (ConcatenatedTermsArePrime(primes[i], primes[m]) && ConcatenatedTermsArePrime(primes[j], primes[m]) && ConcatenatedTermsArePrime(primes[k], primes[m]) && ConcatenatedTermsArePrime(primes[l], primes[m]))
                                            {
                                                Console.WriteLine(primes[i] + primes[j] + primes[k] + primes[l] + primes[m]);
                                                candidateSums.Add(primes[i] + primes[j] + primes[k] + primes[l] + primes[m]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return candidateSums.Min();
        }
        private static bool ConcatenatedTermsArePrime(long Value1, long Value2)
        {
            return IsPrime(Int64.Parse(Value1.ToString() + Value2.ToString())) && IsPrime(Int64.Parse(Value2.ToString() + Value1.ToString())); 
        }
        private static bool IsPrime(long Value)
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
