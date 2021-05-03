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
    public class Problem55 : BaseProblem, IProblem
    {
        public Problem55()
        {
            Number = 55;
            Prompt = "If we take 47, reverse and add, 47 + 74 = 121, which is palindromic. " +
                     "Not all numbers produce palindromes so quickly. For example, " +
                     "		349 + 943 = 1292, " +
                     "		1292 + 2921 = 4213 " +
                     "		4213 + 3124 = 7337 " +
                     "That is, 349 took three iterations to arrive at a palindrome. " +
                     "Although no one has proved it yet, it is thought that some numbers, like 196, never produce a palindrome. " +
                     "A number that never forms a palindrome through the reverse and add process is called a Lychrel number. " +
                     "Due to the theoretical nature of these numbers, and for the purpose of this problem, we shall assume that a number is Lychrel " + 
                     "until proven otherwise. In addition you are given that for every number below ten-thousand, it will either (i) " + 
                     "become a palindrome in less than fifty iterations, or, (ii) no one, with all the computing power that exists, has managed so " + 
                     "far to map it to a palindrome. In fact, 10677 is the first number to be shown to require over fifty iterations before " + 
                     "producing a palindrome: 4668731596684224866951378664 (53 iterations, 28-digits). " +
                     "Surprisingly, there are palindromic numbers that are themselves Lychrel numbers; the first example is 4994. " +
                     "How many Lychrel numbers are there below ten-thousand? " +
                     "NOTE: Wording was modified slightly on 24 April 2007 to emphasise the theoretical nature of Lychrel numbers. ";

        }
        public override void Solve()
        {
            LogList = new List<string>();
            int result = 0;
            for(int i = 1; i < 10001; i++)
            {
                bool IsLychrel = true;
                BigInteger testValue = (BigInteger)i;
                for(int j = 0; j < 50; j++)
                {
                    if(IsPalindrome(AddNumberToReverse(testValue)))
                    {
                        IsLychrel = false;
                        break;
                    }
                    else
                    {
                        testValue = AddNumberToReverse(testValue);
                    }
                }
                if(IsLychrel)
                {
                    result++;
                }
            }
            Output = result.ToString();
        }
        private static BigInteger AddNumberToReverse(BigInteger Value)
        {
            return Value + ReverseNumber(Value);
        }
        private static bool IsPalindrome(BigInteger Value)
        {
            return (Value == ReverseNumber(Value));
        }
        private static BigInteger ReverseNumber(BigInteger Value)
        {
            var NumberAsList = IntegerToList(Value);
            NumberAsList.Reverse();
            return ListToBigInteger(NumberAsList);
        }
        private static BigInteger ListToBigInteger(List<int> digits)
        {
            string result = "";
            foreach (BigInteger digit in digits)
            {
                result += digit.ToString();
            }
            return BigInteger.Parse(result);
        }
        private static List<int> IntegerToList(BigInteger Value)
        {
            var values = new List<int>();
            string value = Value.ToString();
            for (int i = 0; i < value.Length; i++)
            {
                values.Add(int.Parse(value.Substring(i, 1)));
            }
            return values;
        }
    }
}
