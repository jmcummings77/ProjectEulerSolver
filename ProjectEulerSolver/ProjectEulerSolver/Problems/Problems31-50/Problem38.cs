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
    public class Problem38 : BaseProblem, IProblem
    {
        public Problem38()
        {
            Number = 38;
            Prompt = "Take the number 192 and multiply it by each of 1, 2, and 3: " +
                     "192 × 1 = 192 " +
                     "192 × 2 = 384 " +
                     "192 × 3 = 576 " +
                     "By concatenating each product we get the 1 to 9 pandigital, 192384576. " + 
                     "We will call 192384576 the concatenated product of 192 and (1,2,3) " + 
                     "The same can be achieved by starting with 9 and multiplying by 1, 2, 3, 4, and 5, giving the pandigital, 918273645, " + 
                     "which is the concatenated product of 9 and (1,2,3,4,5). " + 
                     "What is the largest 1 to 9 pandigital 9-digit number that can be formed as the concatenated product of an integer " +
                     "with (1,2, ... , n) where n > 1?";
        }
        public override void Solve()
        {
           
            LogList = new List<string>();
            string result = "";
            Int64 highestResult = 0;
            
            for(int i = 1; i < 10000; i++)
            {
                for (int j = 1; j < 100; j++)
                {
                    string concatenatedProduct = GenerateConcatenatedProducts(i, j);
                    if(concatenatedProduct.Length == 9)
                    {
                        bool isPandigital = (concatenatedProduct.Contains("1"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("2"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("3"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("4"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("5"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("6"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("7"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("8"));
                        isPandigital = isPandigital && (concatenatedProduct.Contains("9"));
                        
                        if (isPandigital)
                        {
                            if(Int64.Parse(concatenatedProduct) > highestResult)
                            {
                                highestResult = Int64.Parse(concatenatedProduct);
                                result = concatenatedProduct;
                            }
                        }
                    }
                }
            }
            Output = result;
        }
        public string GenerateConcatenatedProducts(int multiplier, int maxN)
        {
            string result = "";
            for(int i = 1; i <= maxN; i++)
            {
                result += (i * multiplier).ToString();
            }
            return result;
        }
    }
}