using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
{
    public class Problem26 : BaseProblem, IProblem
    {
        public Problem26()
        {
            Number = 26;
            Prompt = "A unit fraction contains 1 in the numerator. The decimal representation of the unit fractions with denominators 2 to 10 are given: " +
                            "1 / 2 = 0.5  " +
                            "1 / 3 = 0.(3) " +
                            "1 / 4 = 0.25 " +
                            "1 / 5 = 0.2 " +
                            "1 / 6 = 0.1(6) " +
                            "1 / 7 = 0.(142857) " +
                            "1 / 8 = 0.125 " +
                            "1 / 9 = 0.(1) " +
                            "1 / 10 = 0.1 " +
                    "Where 0.1(6) means 0.166666..., and has a 1 - digit recurring cycle. It can be seen that 1 / 7 has a 6 - digit recurring cycle. " +
                    "Find the value of d < 1000 for which 1 / d contains the longest recurring cycle in its decimal fraction part.";
        }
        public override void Solve()
        {
            //loop from 2 to 1000
            var LongestReciprocal = new Tuple<int, int>(1,1);
            for(int i = 2; i < 1001; i++)
            {
                var x = new Number(i);
                var primeFactors = x.GetPrimeFactors();
                bool isFinite = true;
                foreach(int factor in primeFactors)
                {
                    if(factor != 5 && factor != 2 && factor != 1)
                    {
                        isFinite = false;
                    }
                }
                if(isFinite)
                {
                    List<Tuple<double, double>> ratios = new List<Tuple<double, double>>();
                    double divisor = i;
                    double divisee = 1;
                    while (divisee != 0)
                    {
                        var ratio = new Tuple<double, double>(divisee, divisor);
                        if(!ratios.Contains(ratio))
                        {
                            ratios.Add(ratio);

                            while (divisor > divisee)
                            {
                                divisee *= 10;
                            }
                            divisee = divisee - Math.Floor((double)(divisee / divisor)) * divisor;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(ratios.Count > LongestReciprocal.Item2)
                    {
                        LongestReciprocal = new Tuple<int, int>(i, ratios.Count);
                    }
                }
            }
            Output = "Index: [" + LongestReciprocal.Item1.ToString() + "] Value: [" + LongestReciprocal.Item2.ToString() + "]";
        }
    }
}