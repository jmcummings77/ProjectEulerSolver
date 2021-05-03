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
    public class Problem68 : BaseProblem, IProblem
    {
        public Problem68()
        {
            Number = 68;
            Prompt = "https://projecteuler.net/problem=68"
                    + "     Using the numbers 1 to 10, and depending on arrangements, it is possible to form 16- and 17-digit strings. What is the maximum 16-digit string for a 'magic' 5-gon ring?";
        }
        public override void Solve()
        {
            LogList = new List<string>();

            int[] sourceDigits = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var Permutations = sourceDigits.GetPermutations();
            BigInteger max = new BigInteger(0);
            string result = "";
            foreach (int[] permutation in Permutations)
            {
                if(CheckCombo(permutation.ToArray()))
                {
                    string combo = GetIdentity(permutation.ToArray());
                    if(combo.Length == 16)
                    {
                        BigInteger value = BigInteger.Parse(combo);
                        if(value > max)
                        {
                            max = value;
                            result = combo;
                        }
                    }
                }
            }
            Output = result;
        }
        private bool CheckCombo(int[] digits)
        {
            int triplet1 = digits[0] + digits[1] + digits[2];
            if (triplet1 != digits[3] + digits[2] + digits[4])
            {
                return false;
            }
            if (triplet1 != digits[5] + digits[4] + digits[6])
            {
                return false;
            }
            if (triplet1 != digits[7] + digits[6] + digits[8])
            {
                return false;
            }
            if (triplet1 != digits[9] + digits[8] + digits[1])
            {
                return false;
            }
            
            return  true;
        }
        private string GetIdentity(int[] digits)
        {
            List<int> outerNodes = new List<int>();
            outerNodes.Add(digits[0]);
            outerNodes.Add(digits[3]);
            outerNodes.Add(digits[5]);
            outerNodes.Add(digits[7]);
            outerNodes.Add(digits[9]);
            string triplet1 = digits[0].ToString() + digits[1].ToString() + digits[2].ToString();
            string triplet2 = digits[3].ToString() + digits[2].ToString() + digits[4].ToString();
            string triplet3 = digits[5].ToString() + digits[4].ToString() + digits[6].ToString();
            string triplet4 = digits[7].ToString() + digits[6].ToString() + digits[8].ToString();
            string triplet5 = digits[9].ToString() + digits[8].ToString() + digits[1].ToString();
            switch(Array.IndexOf(digits, outerNodes.Min()))
            {
                case 0:
                    return triplet1 + triplet2 + triplet3 + triplet4 + triplet5;
                case 3:
                    return triplet2 + triplet3 + triplet4 + triplet5 + triplet1;
                case 5:
                    return triplet3 + triplet4 + triplet5 + triplet1 + triplet2;
                case 7:
                    return triplet4 + triplet5 + triplet1 + triplet2 + triplet3;
                case 9:
                    return triplet5 + triplet1 + triplet2 + triplet3 + triplet4;
            }
            return "";
           
        }
        
    }
    public static class PermutationExtension
    {
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();

            var factorials = Enumerable.Range(0, array.Length + 1)
                .Select(Factorial)
                .ToArray();

            for (var i = 0L; i < factorials[array.Length]; i++)
            {
                var sequence = GenerateSequence(i, array.Length - 1, factorials);

                yield return GeneratePermutation(array, sequence);
            }
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var clone = (T[])array.Clone();

            for (int i = 0; i < clone.Length - 1; i++)
            {
                Swap(ref clone[i], ref clone[i + sequence[i]]);
            }

            return clone;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var facto = factorials[sequence.Length - j];

                sequence[j] = (int)(number / facto);
                number = (int)(number % facto);
            }

            return sequence;
        }

        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private static long Factorial(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }
    }
}
