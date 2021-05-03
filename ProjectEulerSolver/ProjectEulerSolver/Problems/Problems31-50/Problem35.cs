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
    public class Problem35 : BaseProblem, IProblem
    {
        public Problem35()
        {
            Number = 35;
            Prompt = "The number, 197, is called a circular prime because all rotations of the digits: 197, 971, and 719, are themselves prime. " +
                     "There are thirteen such primes below 100: 2, 3, 5, 7, 11, 13, 17, 31, 37, 71, 73, 79, and 97. " +
                     "How many circular primes are there below one million?";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            LogFilePath = @"C:\Users\us48610\Desktop\Problem35.txt";
            int result = 1;
            for(int i = 3; i < 1000000; i+=2)
            {
                bool isCircular = true;
                var number = new Number(i);
                if(number.IsPrime())
                {
                    int length = number.Length();
                    List<long> rotations = number.GetAllRotations().Where(x => x.ToString().Length == length).ToList();
                    foreach (long rotation in rotations)
                    {
                        var rotationNumber = new Number(rotation);
                        if(!rotationNumber.IsPrime())
                        {
                            isCircular = false;
                        }
                    }
                }
                else
                {
                    isCircular = false;
                }
                if(isCircular)
                {
                    LogList.Add(i.ToString());
                    Console.WriteLine("Match: " + i.ToString());
                    result++;
                }
            }
            LogToFile();
            Output = result.ToString();
        }
    }
}