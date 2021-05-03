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
    public class Problem69 : BaseProblem, IProblem
    {
        public Problem69()
        {
            Number = 69;
            Prompt = "https://projecteuler.net/problem=69"
                   + "   Find the value of n ≤ 1,000,000 for which n/φ(n) is a maximum.";
        }
        public override void Solve()
        {
            int limit = 1000001;
            var primes = new List<int>();
            for (int i = 1; i < limit; i++)
            {
                if(IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            int result = 1;
            int j = 0;
            while(result * primes[j] < limit)
            {
                result *= primes[j];
                j++;
            }
            Output = result.ToString();
        }

        private void GetResult()
        { 
            LogList = new List<string>();
            double max = 0;
            int result = 0;
            List<int> primes = new List<int>();
            primes.Add(1);
            primes.Add(2);
            List<int> nonPrimeOdds = new List<int>();

            for (int i = 4; i < 1000000; i+=2)
            {
                int n = primes.Count();
                if (IsPrime(i - 1))
                {
                    primes.Add(i - 1);
                    n++;
                }
                else
                {
                    nonPrimeOdds.Add(i - 1);
                }
                foreach(int odd in nonPrimeOdds)
                {
                    if (GetGreatestCommonDivisor(i, odd) == 1)
                    {
                        n++;
                    }
                }
                
                double phi = (double)i / (double)n;
                if (phi > max)
                {
                    max = phi;
                    result = i;
                    Console.WriteLine(i.ToString());
                }
            }
            Output = result.ToString();
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

        private static int GetGreatestCommonDivisor(int a, int b)
        {
            if (a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            int remainder;
            int priorRemainder;
            int nextRemainder;
            int quotient = Math.DivRem(a, b, out priorRemainder);
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
