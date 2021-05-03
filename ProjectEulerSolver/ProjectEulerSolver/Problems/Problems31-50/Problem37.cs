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
    public class Problem37 : BaseProblem, IProblem
    {
        public Problem37()
        {
            Number = 37;
            Prompt = "The number 3797 has an interesting property. Being prime itself, it is possible to continuously remove digits from " + 
                     "left to right, and remain prime at each stage: " + 
                     "3797, 797, 97, and 7. " + 
                     "Similarly we can work from right to left: " + 
                     "3797, 379, 37, and 3. " +
                     "Find the sum of the only eleven primes that are both truncatable from left to right and right to left. " +
                     "NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes.";
        }
        public override void Solve()
        {
            //has to start with one of 2,3,5,7 bc first and last have to be single digit primes
            //must also end with 3,7 bc 2 or 5 means it isnt prime
            //boundaries? not sure, but the problem only calls for the first 11 after 7
            LogList = new List<string>();
            int result = 0;
            int count = 0;
            int numberToTest = 9;
            while (count < 11)
            {
                numberToTest+=2;

                var number = new Number(numberToTest);
                int[] digitArray = number.ToIntArray();
                int n = digitArray.Length;

                bool isTrunctable = true;
                switch (digitArray[n - 1])
                {
                    case 0:
                        isTrunctable = false;
                        break;
                    case 1:
                        isTrunctable = false;
                        break;
                    case 2:
                        isTrunctable = false;
                        break;
                    case 3:

                        break;
                    case 4:
                        isTrunctable = false;
                        break;
                    case 5:
                        isTrunctable = false;
                        break;
                    case 6:
                        isTrunctable = false;
                        break;
                    case 7:

                        break;
                    case 8:
                        isTrunctable = false;
                        break;
                    case 9:
                        isTrunctable = false;
                        break;
                    default:
                        isTrunctable = false;
                        break;
                }
                if (isTrunctable)
                {
                    switch (digitArray[0])
                    {
                        case 0:
                            isTrunctable = false;
                            break;
                        case 1:
                            isTrunctable = false;
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:
                            isTrunctable = false;
                            break;
                        case 5:

                            break;
                        case 6:
                            isTrunctable = false;
                            break;
                        case 7:

                            break;
                        case 8:
                            isTrunctable = false;
                            break;
                        case 9:
                            isTrunctable = false;
                            break;
                        default:
                            isTrunctable = false;
                            break;
                    }
                    if (isTrunctable)
                    {
                        if (number.IsPrime())
                        {
                            LogList.Add("       Current Count >>>>>> " + count.ToString());
                            LogList.Add("     Candidate Prime >>>>>> " + numberToTest.ToString());
                            Console.WriteLine("     Candidate Prime >>>>>> " + numberToTest.ToString());
                            while (n > 0 && isTrunctable)
                            {
                                string truncatedDigits = "";
                                for (int j = 0; j < n; j++)
                                {
                                    truncatedDigits += digitArray[j].ToString();
                                }
                                LogList.Add("          Truncation >>>>>> " + truncatedDigits);
                                Console.WriteLine("          Truncation >>>>>> " + truncatedDigits);

                                var truncatedNumber = new Number(Int64.Parse(truncatedDigits));
                                if(!truncatedNumber.IsPrime())
                                {
                                    isTrunctable = false;
                                }
                                if (isTrunctable)
                                {
                                    truncatedDigits = "";
                                    for (int j = digitArray.Length - n; j < digitArray.Length; j++)
                                    {
                                        truncatedDigits += digitArray[j].ToString();
                                    }
                                    LogList.Add("          Truncation >>>>>> " + truncatedDigits);
                                    Console.WriteLine("          Truncation >>>>>> " + truncatedDigits);

                                    truncatedNumber = new Number(Int64.Parse(truncatedDigits));
                                    if (!truncatedNumber.IsPrime())
                                    {
                                        isTrunctable = false;
                                    }
                                    n--;
                                }
                            }
                            if (isTrunctable)
                            {
                                LogList.Add("    Trunctable Prime >>>>>> " + numberToTest.ToString());
                                LogList.Add("----------------------------");
                                Console.WriteLine("    Trunctable Prime >>>>>> " + numberToTest.ToString());
                                Console.WriteLine("----------------------------");
                                LogList.Add(numberToTest.ToString());
                                result += numberToTest;
                                count++;
                            }
                            else
                            {
                                LogList.Add("Non-Trunctable Prime >>>>>> " + numberToTest.ToString());
                                LogList.Add("----------------------------");
                                Console.WriteLine("Non-Trunctable Prime >>>>>> " + numberToTest.ToString());
                                Console.WriteLine("----------------------------");
                            }
                            LogToFile();
                            LogList.Clear();
                        }
                    }
                }
                if(numberToTest > 10000000)
                {
                    count = 111;
                }
            }

            LogList.Add("Result: " + result.ToString());

            LogToFile();
            Output = result.ToString();
        }
    }
}