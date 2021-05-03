using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
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
            string[] inputs = new string[] { "2", "1" };
            var answer = GetAnswer(inputs);
            Console.WriteLine(answer.Item1.ToString() + " " + answer.Item2.ToString());
            inputs = new string[] { "3", "1" };
            answer = GetAnswer(inputs);
            Console.WriteLine(answer.Item1.ToString() + " " + answer.Item2.ToString());
            inputs = new string[] { "3", "2" };
            answer = GetAnswer(inputs);
            Console.WriteLine(answer.Item1.ToString() + " " + answer.Item2.ToString());
            inputs = new string[] { "4", "1" };
            answer = GetAnswer(inputs);
            Console.WriteLine(answer.Item1.ToString() + " " + answer.Item2.ToString());
            inputs = new string[] { "4", "2" };
            answer = GetAnswer(inputs);
            Console.WriteLine(answer.Item1.ToString() + " " + answer.Item2.ToString());
            inputs = new string[] { "4", "3" };
            answer = GetAnswer(inputs);
            Console.WriteLine(answer.Item1.ToString() + " " + answer.Item2.ToString());



            Output = "";
        }
        public static Tuple<long, long> GetAnswer(string[] inputs)
        {
            var allResults = new List<Tuple<int, int>>();
            var allDenominators = new List<Tuple<int, int>>();
            switch (inputs[0])
            {
                case "2":
                    for (int i = 0; i < 10; i++)
                    {
                        for (int d = 0; d < 10; d++)
                        {
                            for (int n = 0; n < 10; n++)
                            {
                                var numerators = GetPossibleCombos(new int[] { i, n }, 1);
                                var denominators = GetPossibleCombos(new int[] { i, d }, 1);
                                allResults.AddRange(AddMatchingValues(numerators, denominators));
                            }
                        }
                    }
                    break;
                case "3":
                    switch (inputs[1])
                    {
                        case "1":
                            for (int i = 0; i < 10; i++)
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    for (int n = 0; n < 10; n++)
                                    {
                                        for (int d = 0; d < 10; d++)
                                        {
                                            var numerators = GetPossibleCombos(new int[] { i, j, n }, 1);
                                            var denominators = GetPossibleCombos(new int[] { i, j, d }, 1);
                                            allResults.AddRange(AddMatchingValues(numerators, denominators));
                                        }
                                    }
                                }
                            }
                            break;
                        case "2":
                            for (int i = 0; i < 10; i++)
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    for (int n = 1; n < 10; n++)
                                    {
                                        for (int d = 1; d < 10; d++)
                                        {
                                            var numerators = GetPossibleCombos(new int[] { i, j, n }, 2);
                                            var denominators = GetPossibleCombos(new int[] { i, j, d }, 2);
                                            allResults.AddRange(AddMatchingValues(numerators, denominators));
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case "4":
                    switch (inputs[1])
                    {
                        case "1":
                            for (int n = 0; n < 10; n++)
                            {
                                for (int d = 0; d < 10; d++)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        for (int j = 0; j < 10; j++)
                                        {
                                            for (int k = 0; k < 10; k++)
                                            {
                                                var numerators = GetPossibleCombos(new int[] { i, j, k, n }, 1);
                                                var denominators = GetPossibleCombos(new int[] { i, j, k, d }, 1);
                                                allResults.AddRange(AddMatchingValues(numerators, denominators));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "2":
                            for (int n = 0; n < 10; n++)
                            {
                                for (int d = 0; d < 10; d++)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        for (int j = 0; j < 10; j++)
                                        {
                                            for (int k = 0; k < 10; k++)
                                            {
                                                var numerators = GetPossibleCombos(new int[] { i, j, k, n }, 2);
                                                var denominators = GetPossibleCombos(new int[] { i, j, k, d }, 2);
                                                allResults.AddRange(AddMatchingValues(numerators, denominators));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "3":
                            for (int n = 1; n < 10; n++)
                            {
                                for (int d = 1; d < 10; d++)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        for (int j = 0; j < 10; j++)
                                        {
                                            for (int k = 0; k < 10; k++)
                                            {
                                                var numerators = GetPossibleCombos(new int[] { i, j, k, n }, 3);
                                                var denominators = GetPossibleCombos(new int[] { i, j, k, d }, 3);
                                                allResults.AddRange(AddMatchingValues(numerators, denominators));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
            long numeratorResult = allResults.Distinct().Select(x => x.Item1).Sum();
            long denominatorResult = allResults.Distinct().Select(x => x.Item2).Sum();
            return new Tuple<long, long>(numeratorResult, denominatorResult);
            

        }
        public static List<Tuple<int, int>> AddMatchingValues(List<Tuple<int, int>> numerators, List<Tuple<int, int>> denominators)
        {
            var results = new List<Tuple<int, int>>();
            foreach (Tuple<int, int> Numerator in numerators)
            {
                foreach (Tuple<int, int> Denominator in denominators)
                {
                    if (Denominator.Item1 > Numerator.Item1)
                    {
                        if (Denominator.Item1 * Numerator.Item2 == Numerator.Item1 * Denominator.Item2)
                        {
                            results.Add(new Tuple<int, int>(Numerator.Item1, Denominator.Item1));
                        }
                    }
                }
            }
            return results;
        }
        public static List<Tuple<int,int>> GetPossibleCombos(int[] values, int cancelCount)
        {
            var results = new List<Tuple<int, int>>();
            int[] adjustment = new int[cancelCount];
            for(int x = 0; x < cancelCount; x++)
            {
                adjustment[x] = values[x];
            }
                
                
            foreach(int[] combo in values.GetPermutations())
            {
                if (combo.First() != 0)
                {
                    int result = 0;
                    int j = 0;
                    int nresult = 0;
                    for (int i = 0; i < combo.Length; i++)
                    {
                        result += (int)(combo[i] * Math.Pow(10, i));
                        if (adjustment.Contains(combo[i]))
                        {
                            j++;
                        }
                        else
                        {
                            nresult += (int)(combo[i] * Math.Pow(10, i - j));
                        }

                    }
                    if (result > 0 && nresult > 0)
                    {
                        results.Add(new Tuple<int, int>(result, nresult));
                    }
                }
            }

            return results;
        }

        public void LegacySolve()
        {
            LogList = new List<string>();
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
            BigInteger numerator = 1;
            BigInteger denominator = 1;
            count = 0;
            int total = 0;
            foreach (Fraction fraction in results)
            {
                count++;
                numerator *= fraction.ReducedNumerator;
                denominator *= fraction.ReducedDenominator;
                if (fraction.ReducedNumerator < fraction.ReducedDenominator)
                {
                    total += (int)fraction.Denominator;
                }
                LogList.Add("Result Number " + count.ToString());
                LogList.Add(fraction.Numerator.ToString() + "/" + fraction.Numerator.ToString());
                LogList.Add(fraction.ReducedNumerator.ToString() + "/" + fraction.ReducedDenominator.ToString());
                LogList.Add("Running Numerator: " + numerator.ToString());
                LogList.Add("Running Denominator: " + denominator.ToString());
                LogList.Add("-------------------------------------");

            }
            Console.WriteLine(total);
            var result = new Fraction(numerator, denominator);
            Output = result.ReducedDenominator.ToString();
        }
    }
}