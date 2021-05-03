using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
{
    public class Problem28 : BaseProblem, IProblem
    {
        public Problem28()
        {
            Number = 28;
            Prompt = "Starting with the number 1 and moving to the right in a clockwise direction a 5 by 5 spiral is formed as follows: " +
                        
                        "21 22 23 24 25 " +
                        "20  7  8  9 10 " +
                        "19  6  1  2 11 " +
                        "18  5  4  3 12 " +
                        "17 16 15 14 13 " +
                        
                     "It can be verified that the sum of the numbers on the diagonals is 101. " +
                     "What is the sum of the numbers on the diagonals in a 1001 by 1001 spiral formed in the same way? ";
        }
        public override void Solve()
        {
            BigInteger result = 1;
            BigInteger total = 1;
            int sides = 1;
            int i = 1;
            while(sides < 1001)
            {
                sides += 2;
                Console.WriteLine("Side Count: " + sides.ToString());
                Console.WriteLine("Multiplier: " + i.ToString());
                result = result + (2 * i);
                total += result;
                Console.WriteLine("       First Diagonal: " + result.ToString());

                result = result + (2 * i);
                total += result;
                Console.WriteLine("       Second Diagonal: " + result.ToString());

                result = result + (2 * i);
                total += result;
                Console.WriteLine("       Third Diagonal: " + result.ToString());

                result = result + (2 * i);
                total += result;
                Console.WriteLine("       Fourth Diagonal: " + result.ToString());
                Console.WriteLine("-----------------------------------");
                i++;
            }
            
            Output = total.ToString();
        }
    }
}