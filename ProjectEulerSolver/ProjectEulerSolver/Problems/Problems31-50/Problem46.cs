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
    public class Problem46 : BaseProblem, IProblem
    {
        public Problem46()
        {
            Number = 46;
            Prompt = "It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square. " +
                        "9 = 7 + 2×12 " +
                        "15 = 7 + 2×22 " +
                        "21 = 3 + 2×32 " +
                        "25 = 7 + 2×32 " +
                        "27 = 19 + 2×22 " +
                        "33 = 31 + 2×12 " +
                        "It turns out that the conjecture was false. " +
                        "What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?";

        }
        public override void Solve()
        {
            int i = 5;
            bool NoMatch = true;
            List<int> primes = new List<int>();
            primes.Add(1);
            primes.Add(3);
            while (NoMatch)
            {
                if(IsPrime(i))
                {
                    primes.Add(i);
                }
                else
                {
                    bool result = false;
                    foreach(int prime in primes)
                    {
                        double n = Math.Sqrt((i - prime)/2);
                        result = (Math.Floor(n) == n);
                        if(result)
                        {
                            break;
                        }
                    }
                    if(!result)
                    {
                        Output = i.ToString();
                    }
                    NoMatch = result;
                }
                i += 2;
            }
        }
        private static bool IsPrime(Int64 Value)
        {
            if (Value < 1)
            {
                return false;
            }
            if (Value < 4)
            {
                return true;
            }
            var primes = GetPrimeFactors(Value);
            foreach (int factor in primes)
            {
                if (factor != 1 && factor != (int)Value)
                {
                    return false;
                }
            }
            return true;
        }
        private static List<int> GetPrimeFactors(Int64 Value)
        {
            Int64 n = Value;
            var PrimeFactors = new List<int>();
            PrimeFactors.Add(1);
            if (n % 2 == 0)
            {
                while (n % 2 == 0)
                {
                    PrimeFactors.Add(2);
                    n = n / 2;
                }
            }
            for (int i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                if (n % i == 0)
                {
                    while (n % i == 0)
                    {
                        PrimeFactors.Add(i);
                        n = n / i;
                    }
                }
            }
            return PrimeFactors;
        }
    }
}