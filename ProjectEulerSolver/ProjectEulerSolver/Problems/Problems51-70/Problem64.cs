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
    public class Problem64 : BaseProblem, IProblem
    {
        public Problem64()
        {
            Number = 64;
            Prompt = "https://projecteuler.net/problem=64"
                    + "  How many continued fractions for N ≤ 10000 have an odd period?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int oddPeriodMatches = 0;
            for(int i = 1; i < 10001; i++)
            {
                if(!IsPerfectSquare(i))
                {
                    if(!IsEven(GetContinuedFractionPeriod(i)))
                    {
                        oddPeriodMatches++;
                    }
                }
            }
            Output = oddPeriodMatches.ToString();
        }
        public Tuple<int, int, int> GetNextExpansion(int Value, int NumeratorAdjustment, int NumeratorMultiplier)
        {
            int Denominator = (int)Math.Abs((double)Value - Math.Pow(NumeratorAdjustment, 2));
            int remainder = (int)Math.Floor(NumeratorMultiplier * (Math.Sqrt(Value) + NumeratorAdjustment) / (double)Denominator);
            int nextNumeratorAdjustment = (int)Math.Abs(((NumeratorMultiplier * NumeratorAdjustment) - (Denominator * remainder))/NumeratorMultiplier);
            int nextDenominator = (int)(Denominator / NumeratorMultiplier);

            return new Tuple<int, int, int>(nextNumeratorAdjustment, nextDenominator, remainder);

        }
        public int GetContinuedFractionPeriod(int Value)
        {
            int startingDigit = (int)Math.Floor(Math.Sqrt((double)Value));
            var seriesTerm = GetNextExpansion(Value, startingDigit, 1);
            int termsCount = 1;
            if (seriesTerm.Item2 != 1)
            {
                while (seriesTerm.Item2 != 1)
                {
                    seriesTerm = GetNextExpansion(Value, seriesTerm.Item1, seriesTerm.Item2);
                    termsCount++;
                }
            }
            return termsCount;
        }
        private static bool IsPerfectSquare(int Value)
        {
            return (Math.Floor(Math.Sqrt(Value)) == Math.Sqrt(Value));
        }
        private static bool IsEven(int Value)
        {
            return (Value % 2 == 0);
        }
    }
}
