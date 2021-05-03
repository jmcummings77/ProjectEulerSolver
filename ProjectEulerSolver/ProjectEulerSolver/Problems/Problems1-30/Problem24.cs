using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Problems
{
    public class Problem24 : BaseProblem, IProblem
    {
        public List<string> InputList { get; set; }
        public Problem24()
        {
            Number = 24;
            Prompt = "A permutation is an ordered arrangement of objects. For example, 3124 is one possible permutation of the digits 1, 2, 3 and 4. If all of the permutations are listed numerically or alphabetically, we call it lexicographic order. The lexicographic permutations of 0, 1 and 2 are: " +
                     "012   021   102   120   201   210 " +
                     "What is the millionth lexicographic permutation of the digits 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9 ? ";
            Input = "0123456789";
        }
        public override void Solve()
        {
            double NthIteration = 999999;
            string output = "";
            List<char> inputList = new List<char>(Input.ToCharArray());
            inputList.Sort();
            var length = new Number(Input.Length - 1);
            
            while (NthIteration > 0)
            {
                double remainder = Math.Floor(NthIteration / (double)length.GetFactorial());
                output += inputList[(int)remainder];
                inputList.RemoveAt((int)remainder);
                NthIteration -= (remainder * (double)length.GetFactorial());
                length.Value--;
            }
            if (inputList.Any())
            {
                foreach (char item in inputList)
                {
                    output += item;
                }
            }
           
            Output = output.ToString();
        }
    }
}