using EulerSolver.Interfaces;
using EulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EulerSolver.Problems
{
    public class Problem33 : BaseProblem, IProblem
    {
        public Problem33()
        {
            Number = 33;
            Prompt = "The fraction 49/98 is a curious fraction, as an inexperienced mathematician in attempting to simplify it may incorrectly believe that 49/98 = 4/8, which is correct, is obtained by cancelling the 9s. " +
                       "We shall consider fractions like, 30/50 = 3/5, to be trivial examples. " +
                       "There are exactly four non-trivial examples of this type of fraction, less than one in value, and containing two digits in the numerator and denominator. " +
                       "If the product of these four fractions is given in its lowest common terms, find the value of the denominator.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            LogFilePath = @"";
            List<Fraction> results = new List<Fraction>();
            int count = 0;
            for(int i = 1; i < 10; i++)
            {
                for(int j = 1; j < 10; j++)
                {
                    for (int k = 1; k < 10; k++)
                    {
                        count++;
                        string Numerator = i.ToString() + j.ToString();
                        string Denominator = i.ToString() + k.ToString();
                        LogList.Add("Fraction Number " + count.ToString());
                        LogList.Add(Numerator + "/" + Denominator);
                        var fraction = new Fraction(long.Parse(Numerator), long.Parse(Denominator));
                        var simplifiedFraction = new Fraction((long)j, (long)k);
                        if (fraction.ReducedDenominator == simplifiedFraction.ReducedDenominator && fraction.ReducedNumerator == simplifiedFraction.ReducedNumerator)
                        {
                            results.Add(fraction);
                            LogList.Add("IS MATCH<<<<<<<<<");

                        }
                        LogList.Add("-------------------------------------");

                        count++;
                        Numerator = j.ToString() + i.ToString();
                        Denominator = i.ToString() + k.ToString();
                        LogList.Add("Fraction Number " + count.ToString());
                        LogList.Add(Numerator + "/" + Denominator);
                        fraction = new Fraction(long.Parse(Numerator), long.Parse(Denominator));
                        if (fraction.ReducedDenominator == simplifiedFraction.ReducedDenominator && fraction.ReducedNumerator == simplifiedFraction.ReducedNumerator)
                        {
                            results.Add(fraction);
                            LogList.Add("IS MATCH<<<<<<<<<");

                        }
                        LogList.Add("-------------------------------------");

                        count++;
                        Numerator = j.ToString() + i.ToString();
                        Denominator = k.ToString() + i.ToString();
                        LogList.Add("Fraction Number " + count.ToString());
                        LogList.Add(Numerator + "/" + Denominator);
                        fraction = new Fraction(long.Parse(Numerator), long.Parse(Denominator));
                        if (fraction.ReducedDenominator == simplifiedFraction.ReducedDenominator && fraction.ReducedNumerator == simplifiedFraction.ReducedNumerator)
                        {
                            results.Add(fraction);
                            LogList.Add("IS MATCH<<<<<<<<<");

                        }
                        LogList.Add("-------------------------------------");
                    }
                }
            }
            LogToFile();
            long numerator = 1;
            long denominator = 1;
            count = 0;
            foreach (Fraction fraction in results)
            {
                count++;
                numerator *= fraction.ReducedNumerator;
                denominator *= fraction.ReducedDenominator;

                LogList.Add("Result Number " + count.ToString());
                LogList.Add(fraction.Numerator.ToString() + "/" + fraction.Numerator.ToString());
                LogList.Add(fraction.ReducedNumerator.ToString() + "/" + fraction.ReducedDenominator.ToString());
                LogList.Add("Running Numerator: " + numerator.ToString());
                LogList.Add("Running Denominator: " + denominator.ToString());
                LogList.Add("-------------------------------------");

            }
            var result = new Fraction(numerator, denominator);
            Output = result.ReducedDenominator.ToString();
        }
    }
}