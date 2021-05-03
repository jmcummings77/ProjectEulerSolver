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
    public class Problem72 : BaseProblem, IProblem
    {
        public Problem72()
        {
            Number = 72;
            Prompt = "Consider the fraction, n/d, where n and d are positive integers. If n<d and HCF(n,d)=1, it is called a reduced proper fraction. " +
                    "If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get: " +
                    "       1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8 " +
                    "It can be seen that there are 21 elements in this set. " +
                    "How many elements would be contained in the set of reduced proper fractions for d ≤ 1,000,000?";
        }
        public override void Solve()
        {
            int limit = 1000001;
            // I tried doing this with some overly complicated ways of calculating phiN efficeintly for odds, evens, primes, etc. 
            // Those solutions took insanely long
            // So I went back to the totient formula and realized I just need to count integers once the 
            // prime factors have been removed
            
            // create an array with all of the positive integers below the limit

            int[] numbers = Enumerable.Range(0, limit).ToArray();
            long result = 0;
            // loop through the array, stopping only at numbers that are the same as their index, 
            // i.e. numbers that are prime, because any lesser prime factors should have been removed
            // remove that prime factor from any subsequent integer
            // add the reduced integers to the total
            for (int i = 2; i < limit; i++)
            {
                if (numbers[i] == i)
                {
                    int divisor = i * (i - 1);
                    for (int j = i; j < limit; j += i)
                    {
                        numbers[j] = numbers[j] / divisor;
                    }
                }
                result += numbers[i];

            }
            Output = result.ToString();
                
        }
        public void test()
        { 
            int limit = 1000001;
            BigInteger n = 13;
            var primes = new List<int>();
            primes.Add(1);
            primes.Add(2);
            primes.Add(3);
            primes.Add(5);
            primes.Add(7);


            var nonPrimeOdds = new List<int>();
            var evens = new List<int>();
            evens.Add(4);
            evens.Add(6);
            evens.Add(8);

            nonPrimeOdds.Add(9);

            for (int i = 11; i < limit; i+=2)
            {
                evens.Add(i - 1);
                n += primes.Count();

                if (IsPrime(i))
                {
                    primes.Add(i);
                    n += i - 1;
                }
                else
                {
                    n += primes.Count();
                    foreach(int odd in nonPrimeOdds)
                    {
                        if (GetGreatestCommonDivisor(i, odd) == 1)
                        {
                            n++;
                        }
                    }
                    foreach (int even in evens)
                    {
                        if (GetGreatestCommonDivisor(i, even) == 1)
                        {
                            n++;
                        }
                    }
                    nonPrimeOdds.Add(i);
                }
            }
            for (int i = 4; i < limit; i+=2)
            {
                if(!primes.Contains(i))
                {
                    n++;
                    for(int j = i - 1; j > 1; j--)
                    {
                        if(primes.Contains(j))
                        {
                            n++;
                        }
                        else
                        {
                            if (GetGreatestCommonDivisor(i, j) == 1)
                            {
                                n++;
                            }
                        }
                    }
                }
            }
            Output = n.ToString();
        }
        private void GetResult()
        {
            double max = 0;
            int result = 0;
            List<int> primes = new List<int>();
            primes.Add(1);
            primes.Add(2);
            List<int> nonPrimeOdds = new List<int>();

            for (int i = 4; i < 1000000; i += 2)
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
                foreach (int odd in nonPrimeOdds)
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
