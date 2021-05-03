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
    public class Problem47 : BaseProblem, IProblem
    {
        public Problem47()
        {
            Number = 47;
            Prompt = "The first two consecutive numbers to have two distinct prime factors are: " +
                        "14 = 2 × 7 " +
                        "15 = 3 × 5 " +
                        "The first three consecutive numbers to have three distinct prime factors are: " +
                        "644 = 2² × 7 × 23 " +
                        "645 = 3 × 5 × 43 " +
                        "646 = 2 × 17 × 19. " +
                        "Find the first four consecutive integers to have four distinct prime factors each. What is the first of these numbers?";

        }
        public override void Solve()
        {
            int streak = 0;
            int i = 1;
            int result = 0;
            while(streak < 4)
            {
                int divisorsCount = GetPrimeFactorsCount(i);
                Console.WriteLine("Integer: " + i.ToString() + ", Current Streak: " + streak.ToString() + ", Divisors Count: " + divisorsCount.ToString());
                if (divisorsCount == 4)
                {
                    if(streak == 0)
                    {
                        result = i;
                    }
                    streak++;
                }
                else
                {
                    streak = 0;
                }
                i++;
            }
            Output = result.ToString();
        }
        private int GetPrimeFactorsCount(int n)
        {
            int result = 0;
            if(n % 2 == 0)
            {
                result++;
                while (n % 2 == 0)
                {
                    n = n / 2;
                }
            }
            for (int i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                if (n % i == 0)
                {
                    result++;
                    while (n % i == 0)
                    {
                        n = n / i;
                    }
                }
            }
            if (n > 2)
            {
                result++;
            }
            return result;
        }
    }
}