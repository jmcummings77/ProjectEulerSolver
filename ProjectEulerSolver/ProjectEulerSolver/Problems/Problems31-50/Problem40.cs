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
    public class Problem40 : BaseProblem, IProblem
    {
        public Problem40()
        {
            Number = 40;
            Prompt = "An irrational decimal fraction is created by concatenating the positive integers: " +
                     "0.123456789101112131415161718192021... " +
                     "It can be seen that the 12th digit of the fractional part is 1. " +
                     "If dn represents the nth digit of the fractional part, find the value of the following expression. " +
                     "d1 × d10 × d100 × d1000 × d10000 × d100000 × d1000000";
        }
        public override void Solve()
        {
            int currentLength = 0;
            long product = 1;
            for (int i = 1; i <1000001; i++)
            {
                List<char> items = i.ToString().ToCharArray().ToList();
                foreach(char item in items)
                {
                    currentLength++;
                    switch(currentLength)
                    {
                        case 1:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                        case 10:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                        case 100:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                        case 1000:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                        case 10000:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                        case 100000:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                        case 1000000:
                            product *= Int64.Parse(Convert.ToString(item));
                            break;
                    }
                }
            }
            
            Output = product.ToString();
        }
        public string PrintResults()
        {
            string result = "";
            int length = 0;
            int currentInteger = 1;

            for (double i = 0; i < 7; i++)
            {
                result = "";
                while (length < (int)Math.Pow(10, i))
                {
                    result += currentInteger.ToString();
                    LogList.Add(currentInteger.ToString());
                    length += currentInteger.ToString().Length;
                    currentInteger++;
                }
                LogList.Add("----------------------------");
                LogList.Add("----------------------------");
                LogList.Add("----------------------------" + length.ToString() + "----------------------------");
                LogList.Add("----------------------------");
                LogList.Add("----------------------------");
                LogList.Add("----------------------------");
                LogToFile();
                LogList.Clear();
            }
            return result;
        }
        public int[] GetNthDigit(int n, int length, int currentInteger)
        {
            while (length < n)
            {
                length += currentInteger.ToString().Length;
                currentInteger++;
            }
            int[] result = new int[3];
            if (length == n)
            {
                result[0] = length;
                result[1] = currentInteger;
                result[2] = int.Parse(currentInteger.ToString().Last().ToString());
                return result;
            }
            else if(length < n)
            {
                currentInteger++;
                result[0] = length + currentInteger.ToString().Length;
                result[1] = currentInteger;
                result[2] = int.Parse(currentInteger.ToString().Substring((n - length - 1), 1));
                return result;
            }
            else
            {
                result[0] = length;
                result[1] = currentInteger;
                result[2] = int.Parse(currentInteger.ToString().Substring(currentInteger.ToString().Length - (length - n), 1));
                return result;
            }
        }
    }
}