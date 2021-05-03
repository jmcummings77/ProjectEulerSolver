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
    public class Problem70 : BaseProblem, IProblem
    {
        public Problem70()
        {
            Number = 70;
            Prompt = "Euler's Totient function, φ(n) [sometimes called the phi function], is used to determine the number of positive numbers less than or equal to n which are relatively prime to n. For example, as 1, 2, 4, 5, 7, and 8, are all less than nine and relatively prime to nine, φ(9)=6. " +
                     "The number 1 is considered to be relatively prime to every positive number, so φ(1)=1. " +
                     "Interestingly, φ(87109)=79180, and it can be seen that 87109 is a permutation of 79180. " +
                     "Find the value of n, 1 < n < 107, for which φ(n) is a permutation of n and the ratio n/φ(n) produces a minimum.";
        }
        public override void Solve()
        {
            // can't use my prior solution because this requires checking all values of phi
            // so I can't just build the number from prior primes
            // instead I think I need to set reasonable boundaries on the solution
            // and then brute force/trial test integers in that range
            // I can't think of a good constraint on what might produce permutations
            // But a minimum n/φ(n) should be one with a very large φ(n) relative
            // to n (yeah, yeah by definition relative to n).
            // But maximizing φ(n) may be a good starting place. 
            // We want as few prime factors as possible
            // Which would make me think a prime near the limit would be the answer
            // except that can't be a permutation because it's phi will be equal to n-1
            // So we need to look at numbers near the limit but which are products of more than one prime
            // And the prime factors need to be large so they produce a very large n,
            // as close to the limit as possible
            // So primes proximate to the square root of the limit which is around 3162.
            // Which makes me think I should search the products of two primes between 1k and 10k
            // in order to stay in the correct order of magnitude for n
            // Then, check those for permutations and return the one with the smallest n/phin
            // Also since we are starting with the two prime factors of n, 
            // we can calculate phi n much faster than using the GCD algorithm
            // by multiplying the two primes' phis together. 
            // which for primes is equal to the prime itself minus 1. 

            long limit = 10000000;
            long searchPrimesLowerLimit = 1000;
            long searchPrimesUpperLimit = 9999;

            double currentMinimumRatio = (double)limit;
            long result = 0;

            var primes = PrimeSieve(searchPrimesLowerLimit, searchPrimesUpperLimit);

            for(int i = 0; i < primes.Length; i++)
            {
                for(int j = i + 1; j < primes.Length; j++)
                {
                    long n = primes[i] * primes[j];
                    if(n > limit)
                    {
                        break;
                    }
                    long phin = (primes[i] - 1) * (primes[j] - 1);
                    if (AreDigitPermutations(n, phin))
                    {
                        double currentRatio = (double)n / (double)phin;
                        if (currentRatio < currentMinimumRatio)
                        {
                            currentMinimumRatio = currentRatio;
                            result = n;
                        }
                    }
                }
            }

            Output = result.ToString();
        }
        private static long[] PrimeSieve(long LowerBound, long UpperBound)
        {
            var primes = new List<long>();
            for (long i = LowerBound; i < UpperBound; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            return primes.ToArray();
        }
        private static bool AreDigitPermutations(long a, long b)
        {
            List<int> aList = a.ToString().ToCharArray().Select(x => int.Parse(x.ToString())).OrderBy(x => x).ToList();
            List<int> bList = b.ToString().ToCharArray().Select(x => int.Parse(x.ToString())).OrderBy(x => x).ToList();
            if(aList.Count() != bList.Count())
            {
                return false;
            }
            for(int i = 0; i < aList.Count(); i++)
            {
                if(aList[i] != bList[i])
                {
                    return false;
                }
            }
            return true;
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
            for (long i = 3; i <= Math.Sqrt(Value); i = i + 2)
            {
                if (Value % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static long GetEulersTotient(long a)
        {
            int result = 1;
            for(long i = 2; i < a; i++)
            {
                if(GetGreatestCommonDivisor(a,i) == 1)
                {
                    result++;
                }
            }
            return result;
        }
        private static long GetGreatestCommonDivisor(long a, long b)
        {
            if (a < b)
            {
                long temp = a;
                a = b;
                b = temp;
            }
            long remainder;
            long priorRemainder;
            long nextRemainder;
            long quotient = Math.DivRem(a, b, out priorRemainder);
            if (priorRemainder != 0)
            {
                quotient = Math.DivRem(b, priorRemainder, out remainder);
                if (remainder != 0)
                {
                    nextRemainder = remainder;
                    while (remainder != 0)
                    {
                        quotient = Math.DivRem(priorRemainder, nextRemainder, out remainder);
                        priorRemainder = nextRemainder;
                        nextRemainder = remainder;
                    }
                    return priorRemainder;
                }
                else
                {
                    return priorRemainder;
                }
            }
            else
            {
                return b;
            }
        }


    }
}
