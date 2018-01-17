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
    public class Problem50 : BaseProblem, IProblem
    {
        public Problem50()
        {
            Number = 50;
            Prompt = "The prime 41, can be written as the sum of six consecutive primes: " +
                     "      41 = 2 + 3 + 5 + 7 + 11 + 13 " +
                     "This is the longest sum of consecutive primes that adds to a prime below one-hundred. " +
                     "The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953. " +
                     "Which prime, below one-million, can be written as the sum of the most consecutive primes?";
        }
        public override void Solve()
        {
            var primes = new List<int>();
            for (int i = 1; i < 1000000; i++)
            {
                if (IsPrime(i)) { primes.Add(i); }
            }
            int currentMaximum = 0;
            for(int i = primes.Count() - 1; i > 0; i--)
            {
                var lesserPrimes = primes.Where(x => x < primes[i]).ToList();
                int count = MaximumCountOfSummingPrimes(primes[i], lesserPrimes);
                if (count > currentMaximum)
                {
                    currentMaximum = count;
                    Output = primes[i].ToString();
                }
            }
        }
        private static int MaximumCountOfSummingPrimes(int Value, List<int> Candidates)
        {
            int max = Candidates.Count() - 1;
            int currentStreak = 0;
            for(int i = max; i > 0; i--)
            {
                int j = i - 1;
                int n = Candidates[i];
                int count = 0;
                while(n < Value && j > 0)
                {
                    count++;
                    n += Candidates[j];
                    if(n == Value)
                    {
                        currentStreak = count;
                    }
                    j--;
                }
            }

            return currentStreak;
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
        public static IEnumerable<IEnumerable<T>> Subsets<T>(IEnumerable<T> source)
        {
            List<T> list = source.ToList();
            int length = list.Count;
            int max = (int)Math.Pow(2, list.Count);

            for (int count = 0; count < max; count++)
            {
                List<T> subset = new List<T>();
                uint rs = 0;
                while (rs < length)
                {
                    if ((count & (1u << (int)rs)) > 0)
                    {
                        subset.Add(list[(int)rs]);
                    }
                    rs++;
                }
                yield return subset;
            }
        }
    }
}
