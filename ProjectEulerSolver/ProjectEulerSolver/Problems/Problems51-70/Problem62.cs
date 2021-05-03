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
    public class Problem62 : BaseProblem, IProblem
    {
        public Problem62()
        {
            Number = 62;
            Prompt = "The cube, 41063625 (3453), can be permuted to produce two other cubes: 56623104 (3843) and 66430125 (4053). In fact, 41063625 is the smallest cube which has exactly three permutations of its digits which are also cube. " +
                     "Find the smallest cube for which exactly five permutations of its digits are cube.";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            bool NoMatch = true;
            BigInteger result = 0;
            int currentValue = 1;
            Dictionary<BigInteger, Pair> cubes = new Dictionary<BigInteger, Pair>();
            while (NoMatch)
            {
                BigInteger cube = GetCube(currentValue);
                string digitsAsString = "";
                cube.ToString().ToCharArray().Select(x => int.Parse(x.ToString())).OrderByDescending(y => y).ToList().ForEach(z => digitsAsString += z.ToString());
                BigInteger digitsAsLong = BigInteger.Parse(digitsAsString);

                if (cubes.Keys.Contains(digitsAsLong))
                {
                    cubes[digitsAsLong].Count++;
                }
                else
                {
                    cubes[digitsAsLong] = new Pair(cube, 1);
                }
                
                if(cubes[digitsAsLong].Count == 5)
                {
                    result = cubes[digitsAsLong].LowestValue;
                    NoMatch = false;
                }
                currentValue++;
            }
            Output = result.ToString();
        }
        public BigInteger GetCube(int Value)
        {
            BigInteger n = (BigInteger)Value;
            return n * n * n;
        }
        
    }
    public class Pair
    {
        public BigInteger LowestValue { get; set; }
        public int Count { get; set; }
        public Pair(BigInteger lowestValue, int count)
        {
            LowestValue = lowestValue;
            Count = count;
        }
    }
}
