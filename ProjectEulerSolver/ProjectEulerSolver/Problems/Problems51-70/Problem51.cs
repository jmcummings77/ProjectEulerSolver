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
    public class Problem51 : BaseProblem, IProblem
    {
        public Problem51()
        {
            Number = 51;
            Prompt = "By replacing the 1st digit of the 2-digit number *3, it turns out that six of the nine possible values: 13, 23, 43, 53, 73, and 83, are all prime. " +
                     "By replacing the 3rd and 4th digits of 56**3 with the same digit, this 5-digit number is the first example having seven primes among the ten generated numbers, yielding the family: 56003, 56113, 56333, 56443, 56663, 56773, and 56993. Consequently 56003, being the first member of this family, is the smallest prime with this property. " +
                     "Find the smallest prime which, by replacing part of the number (not necessarily adjacent digits) with the same digit, is part of an eight prime value family.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            CurrentValueLength = 0;
            int currentValue = 1;
            bool NoMatch = true;

            while(NoMatch)
            {
                if (DigitReplacementPrime(currentValue))
                {
                    NoMatch = false;
                }
                else
                {
                    currentValue++;
                }
            }
        }
        private static int ListToInteger(List<int> digits)
        {
            string result = "";
            foreach(int digit in digits)
            {
                result += digit.ToString();
            }
            return int.Parse(result);
        }
        private static List<int> IntegerToList(int Value)
        {
            var values = new List<int>();
            string value = Value.ToString();
            for (int i = 0; i < value.Length; i++)
            {
                values.Add(int.Parse(value.Substring(i, 1)));
            }
            return values;
        }
        private bool DigitReplacementPrime(int Value)
        {
            var values = IntegerToList(Value);
            if (values.Count() > CurrentValueLength)
            {
                CurrentValueLength = values.Count();
                Combos = GetAllCombos(GetPositionDigits(values.Count()));
            }
            foreach (List<int> positionList in Combos)
            {
                int currentPositionPrimeCount = 0;
                List<int> currentPositionPrimeResults = new List<int>();
                for (int i = 0; i < 10; i++)
                {
                    if (i != 0)
                    {
                        var tempValues = new List<int>();
                        tempValues.AddRange(values);
                        foreach (int position in positionList)
                        {
                            tempValues[position] = i;
                        }
                        int testValue = ListToInteger(tempValues);
                        if (IsPrime(testValue))
                        {
                            currentPositionPrimeCount++;
                            currentPositionPrimeResults.Add(testValue);
                        }
                    }
                    else
                    {
                        if (!positionList.Contains(0))
                        {
                            var tempValues = new List<int>();
                            tempValues.AddRange(values);
                            foreach (int position in positionList)
                            {
                                tempValues[position] = i;
                            }
                            int testValue = ListToInteger(tempValues);
                            if (IsPrime(testValue))
                            {
                                currentPositionPrimeCount++;
                                currentPositionPrimeResults.Add(testValue);
                            }
                        }
                    }
                }
                if (currentPositionPrimeCount == 8)
                {
                    Output = currentPositionPrimeResults.OrderBy(x => x).First().ToString();
                    return true;
                }
            }
            return false;
        }

        private bool DigitReplacementPrimeLoggable(int Value)
        {
            var values = IntegerToList(Value);
            if (values.Count() > CurrentValueLength)
            {
                CurrentValueLength = values.Count();
                Combos = GetAllCombos(GetPositionDigits(values.Count()));
            }
            foreach (List<int> positionList in Combos)
            {
                List<int> currentPositionValues = new List<int>();
                for (int i = 0; i < 10; i++)
                {
                    var tempValues = new List<int>();
                    tempValues.AddRange(values);
                    if(positionList.Contains(0) && i == 0)
                    {

                    }
                    else
                    {
                        foreach (int position in positionList)
                        {
                            tempValues[position] = i;
                        }
                        currentPositionValues.Add(ListToInteger(tempValues));
                    }
                }
                currentPositionValues = currentPositionValues.Distinct().ToList();

                int currentPositionPrimeCount = 0;
                string item = Value.ToString() + " : ";
                for (int i = 0; i < currentPositionValues.Count() - 1; i++)
                {
                    item += currentPositionValues[i].ToString() + ", ";
                    if (IsPrime(currentPositionValues[i]))
                    {
                        currentPositionPrimeCount++;
                    }
                }
                item += currentPositionValues.Last().ToString();
                if (IsPrime(currentPositionValues.Last()))
                {
                    currentPositionPrimeCount++;
                }
                LogList.Clear();
                LogList.Add(item);
                LogToFile();
                if (currentPositionPrimeCount == 8)
                {
                    return true;
                }
            }
            return false;
        }
        private int CurrentValueLength { get; set; }
        private List<List<int>> Combos { get; set; }
        public static List<List<int>> GetAllCombos(List<int> list)
        {
            int comboCount = (int)Math.Pow(2, list.Count) - 1;
            List<List<int>> result = new List<List<int>>();
            for (int i = 1; i < comboCount + 1; i++)
            {
                result.Add(new List<int>());
                for (int j = 0; j < list.Count; j++)
                {
                    if ((i >> j) % 2 != 0)
                    {
                        result.Last().Add(list[j]);
                    }
                }
            }
            return result;
        }
        private static List<int> GetPositionDigits(int digitCount)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < digitCount; i++)
            {
                result.Add(i);
            }
            return result;
        }
        private static bool IsPrime(int Value)
        {
            if (Value < 1)
            {
                return false;
            }
            if (Value < 4)
            {
                return true;
            }
            if (Value % 2 == 0)
            {
                {
                    return false;
                }
            }
            for (int i = 3; i <= Math.Sqrt(Value); i = i + 2)
            {
                if (Value % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
