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
    public class Problem39 : BaseProblem, IProblem
    {
        public Problem39()
        {
            Number = 39;
            Prompt = "If p is the perimeter of a right angle triangle with integral length sides, {a,b,c}, " + 
                     "there are exactly three solutions for p = 120. " +
                     "{20,48,52}, {24,45,51}, {30,40,50} " +
                     "For which value of p ≤ 1000, is the number of solutions maximised?";
        }
        public override void Solve()
        {

            LogList = new List<string>();
            int result = 0;
            int highestResult = 0;
            for(int perimeter = 12; perimeter < 1001; perimeter++)
            {
                int count = GetSolutionsCount(perimeter); 
                if(count > highestResult)
                {
                    result = perimeter;
                    highestResult = count;
                }
            }


            Output = result.ToString();
        }
        public int GetSolutionsCount(int perimeter)
        {
            int count = 0;
            for(int i = 1; i < perimeter - 2; i++)
            {
                for(int j = perimeter - i - 1; j > 0; j--)
                {
                    if((perimeter * perimeter) - (2 * perimeter * i) - (2 * perimeter * j) + (2 * i * j) == 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        
    }
}