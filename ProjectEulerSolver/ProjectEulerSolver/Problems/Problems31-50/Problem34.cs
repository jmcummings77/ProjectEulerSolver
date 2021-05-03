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
    public class Problem34 : BaseProblem, IProblem
    {
        public Problem34()
        {
            Number = 34;
            Prompt = "145 is a curious number, as 1! + 4! + 5! = 1 + 24 + 120 = 145. " +
                     "Find the sum of all numbers which are equal to the sum of the factorial of their digits. " +
                     "Note: as 1! = 1 and 2! = 2 are not sums they are not included. ";
        }
        public void testSolve()
        {
            int[] factorials = new int[10];
            factorials[0] = 1;
            factorials[1] = 1;
            factorials[2] = 2;
            factorials[3] = 6;
            factorials[4] = 24;
            factorials[5] = 120;
            factorials[6] = 720;
            factorials[7] = 5040;
            factorials[8] = 40320;
            factorials[9] = 362880;
            int result = 0;
            for (int i = 3; i < 2540161; i++)
            {
                int sum = 0;
                int number = i;
                while(number > 0)
                {
                    int div = number % 10;
                    number /= 10;
                    sum += factorials[div];
                }
                

                if (i == sum)
                {
                    result += i;
                    Console.WriteLine("Match found: " + i.ToString());
                }
            }
            Output = result.ToString();
        }
        private static BigInteger EvaluateRange(int[] LoopStarts, int[] MaxLoops)
        {
            BigInteger result = 0;

            int LoopStart0 = LoopStarts[0];
            int LoopStart1 = LoopStarts[1];
            int LoopStart2 = LoopStarts[2];
            int LoopStart3 = LoopStarts[3];
            int LoopStart4 = LoopStarts[4];
            int LoopStart5 = LoopStarts[5];
            int LoopStart6 = LoopStarts[6];
            int LoopStart7 = LoopStarts[7];
            int LoopStart8 = LoopStarts[8];
            int LoopStart9 = LoopStarts[9];
            int MaxLoops0 = MaxLoops[0];
            int MaxLoops1 = MaxLoops[1];
            int MaxLoops2 = MaxLoops[2];
            int MaxLoops3 = MaxLoops[3];
            int MaxLoops4 = MaxLoops[4];
            int MaxLoops5 = MaxLoops[5];
            int MaxLoops6 = MaxLoops[6];
            int MaxLoops7 = MaxLoops[7];
            int MaxLoops8 = MaxLoops[8];
            int MaxLoops9 = MaxLoops[9];
            for (int Count0 = LoopStart0; Count0 < MaxLoops0; Count0++)
            {
                for (int Count1 = LoopStart1; Count1 < MaxLoops1; Count1++)
                {
                    for (int Count2 = LoopStart2; Count2 < MaxLoops2; Count2++)
                    {
                        for (int Count3 = LoopStart3; Count3 < MaxLoops3; Count3++)
                        {
                            for (int Count4 = LoopStart4; Count4 < MaxLoops4; Count4++)
                            {
                                for (int Count5 = LoopStart5; Count5 < MaxLoops5; Count5++)
                                {
                                    for (int Count6 = LoopStart6; Count6 < MaxLoops6; Count6++)
                                    {
                                        for (int Count7 = LoopStart7; Count7 < MaxLoops7; Count7++)
                                        {
                                            for (int Count8 = LoopStart8; Count8 < MaxLoops8; Count8++)
                                            {
                                                for (int Count9 = LoopStart9; Count9 < MaxLoops9; Count9++)
                                                {
                                                    BigInteger i = (Count0)         + 
                                                                   (Count1)         +
                                                                   (Count2 * 2)     +
                                                                   (Count3 * 6)     +
                                                                   (Count4 * 24)    +
                                                                   (Count5 * 120)   +
                                                                   (Count6 * 720)   +
                                                                   (Count7 * 5040)  +
                                                                   (Count8 * 40320) +
                                                                   (Count9 * 362880);
                                                    if(i.ToString().Count(x => x == '0') == Count0)
                                                    {
                                                        if (i.ToString().Count(x => x == '1') == Count1)
                                                        {
                                                            if (i.ToString().Count(x => x == '2') == Count2)
                                                            {
                                                                if (i.ToString().Count(x => x == '3') == Count3)
                                                                {
                                                                    if (i.ToString().Count(x => x == '4') == Count4)
                                                                    {
                                                                        if (i.ToString().Count(x => x == '5') == Count5)
                                                                        {
                                                                            if (i.ToString().Count(x => x == '6') == Count6)
                                                                            {
                                                                                if (i.ToString().Count(x => x == '7') == Count7)
                                                                                {
                                                                                    if (i.ToString().Count(x => x == '8') == Count8)
                                                                                    {
                                                                                        if (i.ToString().Count(x => x == '9') == Count9)
                                                                                        {
                                                                                            result += i;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        private static async Task InsertRangeAsync(int[] LoopStarts, int[] MaxLoops)
        {
            int LoopStart1 = LoopStarts[0];
            int LoopStart2 = LoopStarts[1];
            int LoopStart3 = LoopStarts[2];
            int LoopStart4 = LoopStarts[3];
            int LoopStart5 = LoopStarts[4];
            int LoopStart6 = LoopStarts[5];
            int LoopStart7 = LoopStarts[6];
            int LoopStart8 = LoopStarts[7];
            int LoopStart9 = LoopStarts[8];
            int MaxLoops1 = MaxLoops[0];
            int MaxLoops2 = MaxLoops[1];
            int MaxLoops3 = MaxLoops[2];
            int MaxLoops4 = MaxLoops[3];
            int MaxLoops5 = MaxLoops[4];
            int MaxLoops6 = MaxLoops[5];
            int MaxLoops7 = MaxLoops[6];
            int MaxLoops8 = MaxLoops[7];
            int MaxLoops9 = MaxLoops[8];
            for (int Count9 = LoopStart9; Count9 < MaxLoops9; Count9++)
            {
                for (int Count8 = LoopStart8; Count8 < MaxLoops8; Count8++)
                {
                    for (int Count7 = LoopStart7; Count7 < MaxLoops7; Count7++)
                    {
                        for (int Count6 = LoopStart6; Count6 < MaxLoops6; Count6++)
                        {
                            for (int Count5 = LoopStart5; Count5 < MaxLoops5; Count5++)
                            {
                                List<FactorialResult> results = new List<FactorialResult>();
                                for (int Count4 = LoopStart4; Count4 < MaxLoops4; Count4++)
                                {
                                    for (int Count3 = LoopStart3; Count3 < MaxLoops3; Count3++)
                                    {
                                        for (int Count2 = LoopStart2; Count2 < MaxLoops2; Count2++)
                                        {
                                            for (int Count1 = LoopStart1; Count1 < MaxLoops1; Count1++)
                                            {
                                                
                                                BigInteger i = (Count1) +
                                                            (Count2 * 2) +
                                                            (Count3 * 6) +
                                                            (Count4 * 24) +
                                                            (Count5 * 120) +
                                                            (Count6 * 720) +
                                                            (Count7 * 5040) +
                                                            (Count8 * 40320) +
                                                            (Count9 * 362880);
                                                var factorialResult = new FactorialResult();
                                                factorialResult.Count1 = Count1;
                                                factorialResult.Count2 = Count2;
                                                factorialResult.Count3 = Count3;
                                                factorialResult.Count4 = Count4;
                                                factorialResult.Count5 = Count5;
                                                factorialResult.Count6 = Count6;
                                                factorialResult.Count7 = Count7;
                                                factorialResult.Count8 = Count8;
                                                factorialResult.Count9 = Count9;
                                                factorialResult.FactorialSum = i.ToString();
                                                factorialResult.FactorialSumCount1 = factorialResult.FactorialSum.Count(x => x == '1');
                                                factorialResult.FactorialSumCount2 = factorialResult.FactorialSum.Count(x => x == '2');
                                                factorialResult.FactorialSumCount3 = factorialResult.FactorialSum.Count(x => x == '3');
                                                factorialResult.FactorialSumCount4 = factorialResult.FactorialSum.Count(x => x == '4');
                                                factorialResult.FactorialSumCount5 = factorialResult.FactorialSum.Count(x => x == '5');
                                                factorialResult.FactorialSumCount6 = factorialResult.FactorialSum.Count(x => x == '6');
                                                factorialResult.FactorialSumCount7 = factorialResult.FactorialSum.Count(x => x == '7');
                                                factorialResult.FactorialSumCount8 = factorialResult.FactorialSum.Count(x => x == '8');
                                                factorialResult.FactorialSumCount9 = factorialResult.FactorialSum.Count(x => x == '9');
                                                factorialResult.IsMatch = false;
                                                if (factorialResult.FactorialSumCount1 == Count1)
                                                {
                                                    if (factorialResult.FactorialSumCount2 == Count2)
                                                    {
                                                        if (factorialResult.FactorialSumCount3 == Count3)
                                                        {
                                                            if (factorialResult.FactorialSumCount4 == Count4)
                                                            {
                                                                if (factorialResult.FactorialSumCount5 == Count5)
                                                                {
                                                                    if (factorialResult.FactorialSumCount6 == Count6)
                                                                    {
                                                                        if (factorialResult.FactorialSumCount7 == Count7)
                                                                        {
                                                                            if (factorialResult.FactorialSumCount8 == Count8)
                                                                            {
                                                                                if (factorialResult.FactorialSumCount9 == Count9)
                                                                                {
                                                                                    factorialResult.IsMatch = true;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                results.Add(factorialResult);
                                            }
                                        }
                                    }
                                }
                                using (var EulerContext = new ProjectEulerSolverDataContext())
                                {
                                    EulerContext.FactorialResults.AddRange(results);
                                    int x = await EulerContext.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
            
        }
        private static async Task SaveResultAsync(FactorialResult factorialResult)
        {
            using (var EulerContext = new ProjectEulerSolverDataContext())
            {
                EulerContext.FactorialResults.Add(factorialResult);
                int x = await EulerContext.SaveChangesAsync();
            }
        }
        public override void Solve()
        {
            testSolve();
        }
        private void aSolve()
        {
            using (var EulerContext = new ProjectEulerSolverDataContext())
            {
                EulerContext.Database.EnsureCreated();
            }
            int[] startLoops1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,      0 };
            int[] maxLoops1 =   { 10, 10, 10, 10, 10, 10, 5, 5, 3, 2 };
            int[] startLoops2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,      2 };
            int[] maxLoops2 =   { 10, 10, 10, 10, 10, 10, 5, 5, 3, 3 };
            int[] startLoops3 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,      3 };
            int[] maxLoops3 =   { 10, 10, 10, 10, 10, 10, 5, 5, 3, 4 };
            int[] startLoops4 = { 0, 0, 0, 0, 0, 0, 0, 0, 0,      4 };
            int[] maxLoops4 =   { 10, 10, 10, 10, 10, 10, 5, 5, 3, 5 };
            int[] startLoops5 = { 0, 0, 0, 0, 0, 0, 0, 0, 3,      0 };
            int[] maxLoops5 =   { 10, 10, 10, 10, 10, 10, 5, 5, 5, 2 };
            int[] startLoops6 = { 0, 0, 0, 0, 0, 0, 0, 0, 3,      2 };
            int[] maxLoops6 =   { 10, 10, 10, 10, 10, 10, 5, 5, 5, 3 };
            int[] startLoops7 = { 0, 0, 0, 0, 0, 0, 0, 0, 3,      3 };
            int[] maxLoops7 =   { 10, 10, 10, 10, 10, 10, 5, 5, 5, 5 };
            
            Task[] taskArray = { Task.Factory.StartNew(() => InsertRangeAsync(startLoops1, maxLoops1)),
                                 Task.Factory.StartNew(() => InsertRangeAsync(startLoops2, maxLoops2)),
                                 Task.Factory.StartNew(() => InsertRangeAsync(startLoops3, maxLoops3)),
                                 Task.Factory.StartNew(() => InsertRangeAsync(startLoops4, maxLoops4)),
                                 Task.Factory.StartNew(() => InsertRangeAsync(startLoops5, maxLoops5)),
                                 Task.Factory.StartNew(() => InsertRangeAsync(startLoops6, maxLoops6)),
                                 Task.Factory.StartNew(() => InsertRangeAsync(startLoops7, maxLoops7))   };
            Output = "Task Complete";
        }
        public void SolveAsync()
        { 
            BigInteger result = 0;
            int[] startLoops1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] maxLoops1 = { 10, 10, 10, 10, 10, 10, 5, 5, 3, 2 };
            int[] startLoops2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 };
            int[] maxLoops2 = { 10, 10, 10, 10, 10, 10, 5, 5, 3, 3 };
            int[] startLoops3 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 };
            int[] maxLoops3 = { 10, 10, 10, 10, 10, 10, 5, 5, 3, 4 };
            int[] startLoops4 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 };
            int[] maxLoops4 = { 10, 10, 10, 10, 10, 10, 5, 5, 3, 5 };
            int[] startLoops5 = { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0 };
            int[] maxLoops5 = { 10, 10, 10, 10, 10, 10, 5, 5, 5, 2 };
            int[] startLoops6 = { 0, 0, 0, 0, 0, 0, 0, 0, 3, 2 };
            int[] maxLoops6 = { 10, 10, 10, 10, 10, 10, 5, 5, 5, 3 };
            int[] startLoops7 = { 0, 0, 0, 0, 0, 0, 0, 0, 3, 3 };
            int[] maxLoops7 = { 10, 10, 10, 10, 10, 10, 5, 5, 5, 5 };

            Task<BigInteger>[] taskArray = { Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops1, maxLoops1)),
                                             Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops2, maxLoops2)),
                                             Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops3, maxLoops3)),
                                             Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops4, maxLoops4)),
                                             Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops5, maxLoops5)),
                                             Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops6, maxLoops6)),
                                             Task<BigInteger>.Factory.StartNew(() => EvaluateRange(startLoops7, maxLoops7))   };
            var results = new BigInteger[taskArray.Length];

            for (int i = 0; i < taskArray.Length; i++)
            {
                results[i] = taskArray[i].Result;
                result += results[i];
            }
            Output = result.ToString();
        }
        
        public void VectorizedSolution()
        {

            var factorialDictionary = new Dictionary<int, int>();
            factorialDictionary[0] = 1;
            factorialDictionary[1] = 1;
            factorialDictionary[2] = 2;
            factorialDictionary[3] = 6;
            factorialDictionary[4] = 24;
            factorialDictionary[5] = 120;
            factorialDictionary[6] = 720;
            factorialDictionary[7] = 5040;
            factorialDictionary[8] = 40320;
            factorialDictionary[9] = 362880;

            int[] vector = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for(int i = 0; i < 10; i++)
            {

            }

        }
    }
}