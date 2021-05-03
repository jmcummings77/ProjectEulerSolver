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
    public class Problem36 : BaseProblem, IProblem
    {
        public Problem36()
        {
            Number = 36;
            Prompt = "The decimal number, 585 = 10010010012 (binary), is palindromic in both bases. " +
                     "Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2. " +
                     "(Please note that the palindromic number, in either base, may not include leading zeros.)";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int result = 0;
            for(int i = 1; i < 1000000; i++)
            {
                var number = new Number(i);
                if(number.IsPalindrome(10) && number.IsPalindrome(2))
                {
                    result += i;
                    LogList.Add(i.ToString() + " : " + number.ToBase2());
                }
            }
            LogList.Add("Total: " + result.ToString());

            LogToFile();
            Output = result.ToString();
        }
    }
}