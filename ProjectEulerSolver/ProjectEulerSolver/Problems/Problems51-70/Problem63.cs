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
    public class Problem63 : BaseProblem, IProblem
    {
        public Problem63()
        {
            Number = 63;
            Prompt = "The 5-digit number, 16807=75, is also a fifth power. Similarly, the 9-digit number, 134217728=89, is a ninth power. " +
                     "How many n-digit positive integers exist which are also an nth power?";
            ///This is a bad solution but it works
            ///Too many magic numbers. I know that the series diverges above base 10 and power 10, but I don't know why so I brute forced it
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int outsideIterationsWithoutMatch = 0;

            int currentInteger = 2;
            int matchCount = 1;
            
            while (outsideIterationsWithoutMatch < 100)
            {
                int currentPower = 1;
                int powerLength = 0;
                int insideIterationsWithoutMatch = 0;
                while (insideIterationsWithoutMatch < 100)
                {
                    var power = BigInteger.Pow(currentInteger, currentPower);
                    powerLength = power.ToString().Length;
                    if(powerLength == currentPower)
                    {
                        matchCount++;
                        Console.WriteLine(matchCount.ToString());
                        Console.WriteLine("Current Base : " + currentInteger.ToString() + " Power : " + currentPower.ToString() + " Length : " + powerLength.ToString() + " Result : " + power.ToString());
                        insideIterationsWithoutMatch = 0;
                    }
                    else
                    {
                        insideIterationsWithoutMatch++;
                        Console.WriteLine("No match : " + currentInteger.ToString() + " ^ " + currentPower.ToString());
                    }
                    currentPower++;
                }
                outsideIterationsWithoutMatch++;
                currentInteger++;
            }
            Output = matchCount.ToString();
        }
        public BigInteger GetValueToDigitLengthPower(int Value)
        {
            int digitCount = Value.ToString().Length;
            BigInteger result = 1;
            for(int i = 0; i < digitCount; i++)
            {
                result *= Value;
            }
            return result;
        }
    }
}
