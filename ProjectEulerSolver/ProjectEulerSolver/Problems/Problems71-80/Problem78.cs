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
    public class Problem78 : BaseProblem, IProblem
    {
        public Problem78()
        {
            Number = 78;
            Prompt = "https://projecteuler.net/problem=78";
        }
        public override void Solve()
        {
            Output = FindPartitionUsingPentagonalSeries().ToString();
        }

        private static int FindPartitionUsingPentagonalSeries()
        {
            List<int> pentagonalNumbers = new List<int>();
            int n = 1;
            pentagonalNumbers.Add(n);

            while (true)
            {
                int index = 0;
                int pentagonalNumber = 1;
                pentagonalNumbers.Add(0);

                while (pentagonalNumber <= n)
                {
                    int signModifier = (index % 4 > 1) ? -1 : 1;
                    
                    pentagonalNumbers[n] += signModifier * pentagonalNumbers[n - pentagonalNumber];
                    pentagonalNumbers[n] %= 1000000;
                    index++;

                    int nextIndex = (index % 2 == 0) ? index / 2 + 1 : -1 * (index / 2 + 1);
                    pentagonalNumber = nextIndex * (3 * nextIndex - 1) / 2;
                }

                if (pentagonalNumbers[n] == 0)
                {
                    return n;
                }
                n++;
            }
        }
        private static BigInteger EnumerateCountingPiles(BigInteger Value)
        {
            BigInteger[] sums = new BigInteger[(int)Value + 1];
            sums[0] = 1;
            for (int i = 1; i < Value; i++)
            {
                for (int j = i; j < Value + 1; j++)
                {
                    sums[j] += sums[j - i];
                }
            }
            return sums.Last();
        }
    }
}
