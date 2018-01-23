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
    public class Problem63 : BaseProblem, IProblem
    {
        public Problem63()
        {
            Number = 63;
            Prompt = "The 5-digit number, 16807=75, is also a fifth power. Similarly, the 9-digit number, 134217728=89, is a ninth power. " +
                     "How many n-digit positive integers exist which are also an nth power?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            int iterationsWithoutMatch = 0;
            int currentInteger = 1;
            int matchCount = 0;

            while (iterationsWithoutMatch < 10000)
            {
                int currentPower = 1;
                int powerLength = 0;
                while(powerLength <= currentPower)
                {
                    var power = BigInteger.Pow(currentInteger, currentPower);
                    powerLength = power.ToString().Length;
                    if(powerLength == currentPower)
                    {
                        matchCount++;
                        Console.WriteLine(matchCount.ToString());
                        Console.WriteLine("Current Base : " + currentInteger.ToString() + " Power : " + currentPower.ToString() + " Length : " + powerLength.ToString() + " Result : " + power.ToString());
                        iterationsWithoutMatch = 0;
                    }
                    else
                    {
                        iterationsWithoutMatch++;
                        Console.WriteLine("No match : " + iterationsWithoutMatch.ToString());
                    }
                    currentPower++;
                }
                currentInteger++;
            }
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
