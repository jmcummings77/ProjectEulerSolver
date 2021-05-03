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
    public class Problem43 : BaseProblem, IProblem
    {
        public Problem43()
        {
            Number = 43;
            Prompt = "The number, 1406357289, is a 0 to 9 pandigital number because it is made up of each of the digits 0 to 9 in some order, but it also has a rather interesting sub-string divisibility property. " +
                     "Let d1 be the 1st digit, d2 be the 2nd digit, and so on. In this way, we note the following: " +
                     "	d2d3d4=406 is divisible by 2 " +
                     "	d3d4d5=063 is divisible by 3 " +
                     "	d4d5d6=635 is divisible by 5 " +
                     "	d5d6d7=357 is divisible by 7 " +
                     "	d6d7d8=572 is divisible by 11 " +
                     "	d7d8d9=728 is divisible by 13 " +
                     "	d8d9d10=289 is divisible by 17 " +
                     "Find the sum of all 0 to 9 pandigital numbers with this property.";

        }
        public override void Solve()
        {
            Output = SumSubdivisiblePandigitals().ToString();
        }
        public long SumSubdivisiblePandigitals()
        {
            long result = 0;
            var digits = new Number(9876543210);
            List<long> permutations = digits.GetAllDigitPermutations();
            int[] divisors = new int[] { 2, 3, 5, 7, 11, 13, 17 };
            foreach (long permutation in permutations)
            {
                var number = new Number(permutation);
                int[] numberDigits = number.ToIntArray();
                if(numberDigits.Length == 10)
                {
                    bool divisible = true;
                    for (int i = 0; i < 7; i++)
                    {
                        string divisee = numberDigits[1+i].ToString() + numberDigits[2 + i].ToString() + numberDigits[3 + i].ToString();
                        if (int.Parse(divisee) % divisors[i] != 0)
                        {
                            divisible = false;
                            break;
                        }

                    }
                    if(divisible)
                    {
                        result += permutation;
                    }
                }
            }
            return result;
        }
    }
}