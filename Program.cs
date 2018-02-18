using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using EulerSolver.Problems;
using EulerSolver.Tools;

namespace EulerSolver.Core
{
    class Program
    {
        public static ulong[] coinSizes = new ulong[] { 1, 2, 5, 10, 20, 50, 100, 200 };
        public static ulong modifier = (ulong)(Math.Pow(10, 9) + 7);
        public static Dictionary<ulong, ulong> results = new Dictionary<ulong, ulong>();
        private static void RunCurrentSolution()
        {
            var problem = new Problem33();
            problem.Solve();
            Console.WriteLine(problem.Output);
            Console.ReadLine();
        }
        static void Main(String[] args)
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int t = Convert.ToInt32(tokens_n[1]);
            int[] tm = new int[n];
            for (int tm_i = 0; tm_i < n; tm_i++)
            {
                tm[tm_i] = Convert.ToInt32(Console.ReadLine());
            }
            int result = examRush(tm, t);
            Console.WriteLine(result);
            Console.ReadLine();
        }
        static int examRush(int[] tm, int t)
        {
            int[] times = tm.OrderBy(x => x).ToArray();
            int max = tm.Length;
            int i = -1;
            while(t >= 0)
            {
                i++;
                t -= times[i];
            }
            return i;
        }
        public static void testsomecrap()
            { 
            BigInteger max = BigInteger.Pow(10, 18);
            bool answer = (long.MaxValue < max);
            Console.WriteLine(answer);
            Console.ReadLine();
            string test = "";
            int x = 1;
            while (test.Length < max)
            {
                test += x.ToString();
                int y = test.Length;
                x++;
            }
            Console.WriteLine(x);

            
            Console.ReadLine();
        }
        public static void runstuff()
        { 
            var results = new List<int>();
            int pMax = 0, tMax = 0;
            int m = 0, k = 0;

            for (int s = 12; s <= 5000000; s++)
            {
                int t = 0;
                int mlimit = (int)Math.Sqrt(s / 2);
                for (m = 2; m <= mlimit; m++)
                {
                    if ((s / 2) % m == 0)
                    { // m found
                        if (m % 2 == 0)
                        { // ensure that we find an odd number for k
                            k = m + 1;
                        }
                        else
                        {
                            k = m + 2;
                        }
                        while (k < 2 * m && k <= s / (2 * m)) { if (s / (2 * m) % k == 0 && gcd(k, m) == 1) { t++; } k += 2; }
                    }
                }
                if (t > tMax)
                {
                    tMax = t;
                    pMax = s;
                    results.Add(pMax);

                }
            }
            
            using (System.IO.StreamWriter file = File.AppendText(@"C:\Users\user\Desktop\testfile.txt"))
            {
                foreach (int result in results)
                {
                    file.WriteLine(result.ToString());
                }
            }
        }
        public static int gcd(int a, int b)
        {
            int y = 0;
            int x = 0;

            if (a > b)
            {
                x = a;
                y = b;
            }
            else
            {
                x = b;
                y = a;
            }

            while (x % y != 0)
            {
                int temp = x;
                x = y;
                y = temp % x;
            }
            return y;
        }
        public static int GetSolutionsCount(int perimeter)
        {
            int count = 0;
            for (int i = 1; i < perimeter - 2; i++)
            {
                for (int j = perimeter - i - 1; j > 0; j--)
                {
                    if ((perimeter * perimeter) - (2 * perimeter * i) - (2 * perimeter * j) + (2 * i * j) == 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        public static bool IsPalindrome(int InBase, int value)
        {
            string rebasedInt = ToBaseX(value, InBase);
            return rebasedInt.SequenceEqual(rebasedInt.Reverse());
        }
        public static string ToBaseX(int value, int toBase)
        {
            return Convert.ToString(Convert.ToInt32(value.ToString(), 10), toBase);
        }
        private static void GetResults()
        {
            for (ulong target = 1; target < 100001; target++)
            {
                ulong[] ways = new ulong[target + 1];
                ways[0] = 1;
                for (ulong i = 0; i < (ulong)coinSizes.Length; i++)
                {
                    for (ulong j = coinSizes[i]; j <= target; j++)
                    {
                        ways[j] += ways[j - coinSizes[i]];
                    }
                }
                ulong result = ways[target] % modifier;
                results[target] = result;
            }
            using (System.IO.StreamWriter file = File.AppendText(@"C:\Users\user\Desktop\test.txt"))
            {
                foreach (ulong item in results.OrderByDescending(x => x.Value).Select(x => x.Key).ToList()) 
                {
                    file.WriteLine(item.ToString() + "," + results[item].ToString());
                }
            }
        }

        
        
        private static void AwaitUserInput()
        {
            Console.WriteLine(@"Enter query or type /q to exit.");
            string result = Console.ReadLine();
            if (result != @"/q")
            {
                string[] query = result.Split(' ');
                Solve45(query);
            }
        }
        private static void Solve31()
        {
            ulong modifier = (ulong)(Math.Pow(10, 9) + 7);
            Dictionary<int, ulong> results = new Dictionary<int, ulong>();
            var coinSizes = new int[] { 1, 2, 5, 10, 20, 50, 100, 200 };
            int limit = 10001;
            int target = 0;
            
                ulong[] ways = new ulong[target + 1];
                ways[0] = 1;
                for (int i = 0; i < coinSizes.Length; i++)
                {
                    for (int j = coinSizes[i]; j <= target; j++)
                    {
                        ways[j] += ways[j - coinSizes[i]];
                    }
                }
                ulong result = ways[target] % modifier;
                results[target] = result;
            
        }


        private static void Solve30()
        {
            BigInteger result = 0;
            for (int n = 6; n < 7; n++)
            {
                for (int i = 2; i < 10000000; i++)
                {
                    var item = new Number(i);
                    if (item.ToBigInteger() == item.GetSumOfDigitsToNthPower(n))
                    {
                        Console.WriteLine(n.ToString() + ", " + item.ToString());
                    }
                }
            }
        }
        private static void Solve45(string[] query)
        {
            ulong limit = UInt64.Parse(query[0]);
            List<ulong> results = new List<ulong>();
            if (query[1] == '3'.ToString())
            {
                results = GetTrianglePentagonNumbers(limit);
            }
            else
            {
                results = GetPentagonHexagonNumbers(limit);
            }

            if (results.Any())
            {
                results.OrderBy(x => x).ToList().ForEach(x => Console.WriteLine(x.ToString()));
            }
            AwaitUserInput();
        }
        private static List<ulong> GetPentagonHexagonNumbers(ulong limit)
        {
            List<ulong> results = new List<ulong>();
            ulong i = 1;
            ulong pentagonalNumber = 0;
            while (pentagonalNumber < limit)
            {
                pentagonalNumber = (i * (3 * i - 1) / (ulong)2);
                if (IsHexagonNumber(pentagonalNumber))
                {
                    results.Add(pentagonalNumber);
                }
                i++;
            }
            return results;
        }
        private static List<ulong> GetTrianglePentagonNumbers(ulong limit)
        {
            List<ulong> results = new List<ulong>();
            ulong i = 1;
            ulong triangleNumber = 0;
            while (triangleNumber < limit)
            {
                triangleNumber = (i * (i + (ulong)1)) / (ulong)2;
                if (IsPentagonNumber(triangleNumber))
                {
                    results.Add(triangleNumber);
                }
                i++;
            }
            return results;
        }
        private static bool IsPentagonNumber(ulong Value)
        {
            double n = (1 + Math.Sqrt(24 * Value + 1)) / 6;
            return (Math.Floor(n) == n);
        }
        private static bool IsHexagonNumber(ulong Value)
        {
            double n = (1 + Math.Sqrt(8 * Value + 1)) / 4;
            return (Math.Floor(n) == n);
        }

        private static void LogAbundantIntegerComposites()
        {
            List<int> AbundantIntegers = new List<int>();
            for (int i = 1; i < 28124; i++)
            {
                if (IsAbundant(i))
                {
                    AbundantIntegers.Add(i);
                }
            }
            int[] AbundantIntegersArray = AbundantIntegers.ToArray();
            List<int> Composites = new List<int>();
            for (int i = 0; i < AbundantIntegersArray.Length; i++)
            {
                for (int j = i; j < AbundantIntegersArray.Length; j++)
                {
                    if (AbundantIntegersArray[i] + AbundantIntegersArray[j] < 21824)
                    {
                        Composites.Add(AbundantIntegersArray[i] + AbundantIntegersArray[j]);
                    }
                }
            }
            int[] CompositesArray = Composites.Distinct().ToArray();
            List<string> results = AbundantIntegersArray.Select(x => x.ToString()).ToList();
            var problem = new Problem();
            problem.LogFilePath = @"C:\Users\user\Desktop\Abundants.txt";
            problem.LogList = results;
            problem.LogToFile();
            problem.LogFilePath = @"C:\Users\user\Desktop\Composites.txt";

            results = CompositesArray.Select(x => x.ToString()).ToList();
            problem.Number = 1111;
            problem.LogList = results;
            problem.LogToFile();

        }
        public static bool IsAbundant(int Value)
        {
            int result = 1;
            if(Value < 12)
            {
                return false;
            }
            for (int i = 2; i < Math.Sqrt(Value) + 1; i++)
            {
                if (Value % i == 0)
                {
                    if (Value / i == i)
                    {
                        result += i;
                    }
                    else
                    {
                        result += i;
                        result += Value / i;
                    }
                }
                if (result > Value)
                {
                    return true;
                }
            }
            return false;
        }
        private static void LogAmicablePairs(int maxValue)
        {
            Dictionary<int, int> NumberDivisorSumPairs = new Dictionary<int, int>();
            List<string> results = new List<string>();
            for (int i = 1; i < maxValue; i++)
            {
                int divisorsSum = GetDivisorsSum(i);
                if (divisorsSum < maxValue)
                {
                    NumberDivisorSumPairs.Add(i, divisorsSum);
                }
            }

            foreach (int key in NumberDivisorSumPairs.Keys)
            {
                if (NumberDivisorSumPairs.ContainsKey(NumberDivisorSumPairs[key]))
                {
                    if (NumberDivisorSumPairs[NumberDivisorSumPairs[key]] == key)
                    {
                        if (NumberDivisorSumPairs[key] != key)
                        {
                            results.Add(NumberDivisorSumPairs[key].ToString() + ", " + key.ToString());
                        }
                    }
                }
            }
            var problem = new Problem();
            problem.LogList = results;
            problem.LogToFile();
        }

        private static string GetResult(int maxValue)
        {
            int result = 0;
            Dictionary<int, int> NumberDivisorSumPairs = new Dictionary<int, int>();

            for (int i = 1; i < maxValue; i++)
            {
                int divisorsSum = GetDivisorsSum(i);
                if (divisorsSum < maxValue)
                {
                    NumberDivisorSumPairs.Add(i, divisorsSum);
                }
            }

            foreach (int key in NumberDivisorSumPairs.Keys)
            {
                if (NumberDivisorSumPairs.ContainsKey(NumberDivisorSumPairs[key]))
                {
                    if (NumberDivisorSumPairs[NumberDivisorSumPairs[key]] == key)
                    {
                        if (NumberDivisorSumPairs[key] != key)
                        {
                            result += key;
                        }
                    }
                }
            }
            return result.ToString();
        }
        private static int GetDivisorsSum(int Value)
        {
            int sum = 1;
            for (int i = 2; i < Math.Sqrt(Value) + 1; i++)
            {
                if (Value % i == 0)
                {
                    if (Value / i == i)
                    {
                        sum += i;
                    }
                    else
                    {
                        sum += i;
                        sum += Value / i;
                    }
                }
            }
            return sum;
        }
        
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        private static void testc()
        {
            int t = 1;
            
            Dictionary<int, long> results = new Dictionary<int, long>();
            for (int a0 = 0; a0 < t; a0++)
            {
                results[a0] = Convert.ToInt64(Console.ReadLine());
            }

            List<long> primes = new List<long>();
            primes.Add(2);
            primes.Add(3);
            long counter = 3;
            foreach (int a0 in results.Keys.OrderBy(x => x).ToList())
            {
                long n = results[a0];
                if (n == 1)
                {
                    results[a0] = 2;
                }
                else if (n == 2)
                {
                    results[a0] = 3;
                }
                else
                {
                    while (primes.Count() < n)
                    {
                        counter += 2;
                        int j = 0;
                        bool isPrime = true;
                        while (primes[j] * primes[j] <= counter)
                        {
                            if (counter % primes[j] == 0)
                            {
                                isPrime = false;
                                break;
                            }
                            j++;
                        }
                        if (isPrime)
                        {
                            primes.Add(counter);
                        }
                    }
                    results[a0] = counter;
                }
            }
            for (int a0 = 0; a0 < t; a0++)
            {
                Console.WriteLine(results[a0]);
            }

            

            Console.ReadLine();
        }
        private static int[] PrimeSieve(int LowerBound, int UpperBound)
        {
            var primes = new List<int>();
            for (int i = LowerBound; i < UpperBound; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            return primes.ToArray();
        }

        private static bool IsPrime(int Value)
        {
            if (Value < 1)
            {
                return false;
            }
            if (Value < 4)
            {
                return true;
            }
            if (Value % 2 == 0)
            {
                {
                    return false;
                }
            }
            for (int i = 3; i <= Math.Sqrt(Value); i = i + 2)
            {
                if (Value % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
