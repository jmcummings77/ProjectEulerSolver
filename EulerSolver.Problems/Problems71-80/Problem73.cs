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
    public class Problem73 : BaseProblem, IProblem
    {
        public Problem73()
        {
            Number = 73;
            Prompt = "";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            decimal rightRatio = (decimal)1.0 / (decimal)2.0;
            decimal leftRatio = (decimal)1.0 / (decimal)3.0;
            long fractionCount = 0;

            for (int i = 1; i < 12001; i++)
            {
                int max = (int)Math.Ceiling(((double)1.0 * (double)i) / (double)2.0);
                int min = (int)Math.Floor(((double)1.0 * (double)i) / (double)3.0);
                for (int j = min; j < max; j++)
                {
                    decimal ratio = (decimal)j / (decimal)i;
                    if (ratio > leftRatio && ratio < rightRatio)
                    {
                        fractionCount++;
                    }
                }
            }
            Output = fractionCount.ToString();
        }
    }
}
