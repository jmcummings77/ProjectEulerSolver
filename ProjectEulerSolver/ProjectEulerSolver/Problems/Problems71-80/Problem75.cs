using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using ProjectEulerSolver.Model;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using ProjectEulerSolver.ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems
{
    public class Problem75 : BaseProblem, IProblem
    {
        public Problem75()
        {
            Number = 75;
            Prompt = "It turns out that 12 cm is the smallest length of wire that can be bent to form an integer sided right angle triangle in exactly one way, but there are many more examples. " +
                     "  12 cm: (3,4,5) " +
                     "  24 cm: (6,8,10) " +
                     "  30 cm: (5,12,13) " +
                     "  36 cm: (9,12,15) " +
                     "  40 cm: (8,15,17) " +
                     "  48 cm: (12,16,20) " +
                     "In contrast, some lengths of wire, like 20 cm, cannot be bent to form an integer sided right angle triangle, and other lengths allow more than one solution to be found; for example, using 120 cm it is possible to form exactly three different integer sided right angle triangles. " +
                     "  120 cm: (30,40,50), (20,48,52), (24,45,51) " +
                     "Given that L is the length of the wire, for how many values of L ≤ 1,500,000 can exactly one integer sided right angle triangle be formed?";
        }
        public override void Solve()
        {
            int limit = 1500001;
            Output = GetPythagoreanTriplesCount(limit).ToString();
            //Output = GetTriadSums(GetTriangleNumbers(1, limit), limit).ToString();
            //int result = PermutationExtensions.Combinations(GetTriangleNumbers(1, limit),3).Select(x => x.Sum()).ToList().GroupBy(x => x).Where(x => x.Count() == 1).Count();
            //Output = result.ToString();
        }
        private static int GetPythagoreanTriplesCount(int UpperBound)
        {
            long[] results = new long[UpperBound + 1];
            long iteratorLimit = (long)Math.Sqrt(UpperBound / 2);
            int result = 0;
            for (long m = 2; m < iteratorLimit; m++)
            { 
                for (long n = 1; n < m; n++)
                {
                    if (GetGreatestCommonDivisor(m, n) == 1 && (m + n) % 2 == 1)
                    {
                        long sum = 2 * m * (m + n);
                        long ksum = sum;
                        while (ksum <= UpperBound)
                        {
                            results[ksum]++;
                            if(results[ksum] == 1)
                            {
                                result++;
                            }
                            else if (results[ksum] == 2)
                            {
                                result--;
                            }
                            ksum += sum;
                        }
                    }
                }
            }
            return result;

        }
        private static long GetGreatestCommonDivisor(long a, long b)
        {
            if (a < b)
            {
                long temp = a;
                a = b;
                b = temp;
            }
            long remainder;
            long priorRemainder;
            long nextRemainder;
            long quotient = Math.DivRem(a, b, out priorRemainder);
            if (priorRemainder != 0)
            {
                quotient = Math.DivRem(b, priorRemainder, out remainder);
                if (remainder != 0)
                {
                    nextRemainder = remainder;
                    while (remainder != 0)
                    {
                        quotient = Math.DivRem(priorRemainder, nextRemainder, out remainder);
                        priorRemainder = nextRemainder;
                        nextRemainder = remainder;
                    }
                    return priorRemainder;
                }
                else
                {
                    return priorRemainder;
                }
            }
            else
            {
                return b;
            }
        }

        public static int GetTriadSums(List<int> list, int limit)
        {
            Dictionary<int, int> results = new Dictionary<int, int>();

            for (int i = 0; i < list.Count() - 2; i++)
            {
                for (int j = i + 1; j < list.Count() - 1; j++)
                {
                    for (int k = j + 1; k < list.Count(); k++)
                    {
                        int sum = list[i] + list[j] + list[k];
                        if (sum < limit)
                        {
                            if (results.ContainsKey(sum))
                            {
                                results[sum]++;
                            }
                            else
                            {
                                results[sum] = 1;
                            }
                        }
                    }
                }
            }
            return results.Where(x => x.Value == 1).Count();
        }
        public static List<int> GetTriangleNumbers(int lowerBound, int upperBound)
        {
            List<int> Numbers = new List<int>();
            int result = 0;
            int n = 0;
            while (result < upperBound)
            {
                n++;
                result = n * (n + 1) / 2;
                if (result > lowerBound)
                {
                    Numbers.Add(result);
                }
            }
            return Numbers;
        }
        public static int GetTriangleNumberTriadSums(int upperBound)
        {
            List<int> Numbers = new List<int>();
            Dictionary<int, int> results = new Dictionary<int, int>();
            int result = 0;
            int n = 0;
            n++;
            result = n * (n + 1) / 2;
            Numbers.Add(result);
            n++;
            result = n * (n + 1) / 2;
            Numbers.Add(result);

            while (result < upperBound)
            {
                n++;
                result = n * (n + 1) / 2;
                for (int i = n - 2; i > 0; i--)
                {
                    for (int j = i - 1; j > -1; j--)
                    {
                        int sum = Numbers[i] + Numbers[j] + result;
                        if (sum < upperBound)
                        {
                            if (results.ContainsKey(sum))
                            {
                                results[sum]++;
                            }
                            else
                            {
                                results[sum] = 1;
                            }
                        }
                    }
                }
                Numbers.Add(result);
            }
            return results.Where(x => x.Value == 1).Count();
        }
    }
}
