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
    public class Problem41 : BaseProblem, IProblem
    {
        public Problem41()
        {
            Number = 41;
            Prompt = "We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once. For example, 2143 is a 4-digit pandigital and is also prime. " +
                     "What is the largest n-digit pandigital prime that exists?";
        }
        public override void Solve()
        {
            long result = GetHighestPandigitalPrime();
            Output = result.ToString();
        }
        public long GetHighestPandigitalPrime()
        {
            List<int> digits = new List<int>();
            for(int i = 9; i > 0; i--)
            {
                digits.Add(i);
            }
            for(int i = 9; i > 0; i--)
            {
                long result = GetHighestPandigitalPrimeInDigitSet(digits);
                if (result != 0)
                {
                    return result;
                }
                digits.Remove(i);
            }
            return 0;
        }
        public long GetHighestPandigitalPrimeInDigitSet(List<int> Digits)
        {
            string digitList = "";
            Digits = Digits.OrderByDescending(x => x).ToList();
            foreach (int digit in Digits)
            {
                digitList += digit.ToString();
            }
            var digits = new Number(int.Parse(digitList));
            List<long> permutations = digits.GetAllDigitPermutations();
            foreach(long permutation in permutations)
            {
                var number = new Number(permutation);
                if (number.IsPrime())
                {
                    return permutation;
                }
            }
            return 0;
        }
    }
}