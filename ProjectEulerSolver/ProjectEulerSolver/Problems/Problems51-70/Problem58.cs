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
    public class Problem58 : BaseProblem, IProblem
    {
        public Problem58()
        {
            Number = 58;
            Prompt = "Starting with 1 and spiralling anticlockwise in the following way, a square spiral with side length 7 is formed. " +
                        "37 36 35 34 33 32 31 " +
                        "38 17 16 15 14 13 30 " +
                        "39 18  5  4  3 12 29 " +
                        "40 19  6  1  2 11 28 " +
                        "41 20  7  8  9 10 27 " +
                        "42 21 22 23 24 25 26 " +
                        "43 44 45 46 47 48 49 " +
                     "It is interesting to note that the odd squares lie along the bottom right diagonal, but what is more interesting is that 8 out of the 13 numbers lying along both diagonals are prime; that is, a ratio of 8/13 ≈ 62%. " +
                     "If one complete new layer is wrapped around the spiral above, a square spiral with side length 9 will be formed. If this process is continued, what is the side length of the square spiral for which the ratio of primes along both diagonals first falls below 10%?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            long result = 1;
            decimal primeRatio = 1;
            int primeCount = 0;
            int diagonalsCount = 1;
            int sides = 1;
            int i = 1;
            while (primeRatio > (decimal)0.1)
            {
                sides += 2;
                diagonalsCount += 4;
                result = result + (2 * i);
                if (IsPrime(result))
                {
                    primeCount++;
                }
                result = result + (2 * i);
                if (IsPrime(result))
                {
                    primeCount++;
                }
                result = result + (2 * i);
                if (IsPrime(result))
                {
                    primeCount++;
                }
                result = result + (2 * i);
                if (IsPrime(result))
                {
                    primeCount++;
                }
                primeRatio = (decimal)primeCount / (decimal)diagonalsCount;
                i++;
                Console.WriteLine("Sides : " + sides.ToString() + " Prime Count : " + primeCount.ToString() + " Diagonals Count : " + diagonalsCount.ToString() + " Prime Ratio : " + primeRatio.ToString());
                LogList.Add(i.ToString() + "," + sides.ToString() + "," + primeCount.ToString() + "," + diagonalsCount.ToString() + "," + primeRatio.ToString());
                LogToFile();
                LogList.Clear();

            }
            Output = sides.ToString();
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
