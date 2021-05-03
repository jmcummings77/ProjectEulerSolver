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
    public class Problem52 : BaseProblem, IProblem
    {
        public Problem52()
        {
            Number = 52;
            Prompt = "It can be seen that the number, 125874, and its double, 251748, contain exactly the same digits, but in a different order. " +
                     "Find the smallest positive integer, x, such that 2x, 3x, 4x, 5x, and 6x, contain the same digits.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            bool NoMatch = true;
            int value = 0;
            while(NoMatch)
            {
                value++;
                Console.WriteLine("Value    : " + value.ToString());
                var firstNumber = new Number(value);
                int[] firstArray = firstNumber.ToIntArray();
                bool isMatch = true;
                for (int i = 2; i < 7; i++)
                {
                    if (isMatch)
                    {
                        var secondNumber = new Number(value * i);
                        Console.WriteLine("Multiple    : " + secondNumber.Value.ToString());
                        int[] secondArray = secondNumber.ToIntArray();
                        if (secondArray.Length == firstArray.Length)
                        {
                            isMatch = new HashSet<int>(firstArray).SetEquals(secondArray);
                        }
                        else
                        {
                            isMatch = false;
                        }
                    }
                }
                NoMatch = !isMatch;
            }
            Output = value.ToString();
        }
    }
}
