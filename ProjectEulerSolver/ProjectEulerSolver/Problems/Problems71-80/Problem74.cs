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
    public class Problem74 : BaseProblem, IProblem
    {
        public Problem74()
        {
            Number = 74;
            Prompt = "The number 145 is well known for the property that the sum of the factorial of its digits is equal to 145: " +
                     "          1! + 4! + 5! = 1 + 24 + 120 = 145 " +
                     "Perhaps less well known is 169, in that it produces the longest chain of numbers that link back to 169; it turns out that there are only three such loops that exist: " +
                     "          169 → 363601 → 1454 → 169 " +
                     "          871 → 45361 → 871 " +
                     "          872 → 45362 → 872 " +
                     "It is not difficult to prove that EVERY starting number will eventually get stuck in a loop. For example, " +
                     "          69 → 363600 → 1454 → 169 → 363601 (→ 1454) " +
                     "          78 → 45360 → 871 → 45361 (→ 871) " +
                     "          540 → 145 (→ 145) " +
                     "Starting with 69 produces a chain of five non-repeating terms, but the longest non-repeating chain with a starting number below one million is sixty terms. " +
                     "How many chains, with a starting number below one million, contain exactly sixty non-repeating terms?";
        }
        private int[] factorials = new int[10];
        public override void Solve()
        {
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
            int limit = 1000001;
            int result = 0;
            for(int i = 1; i < limit; i++)
            {
                if(GetFactorialChainLength(i) == 60)
                {
                    result++;
                }
            }
            Output = result.ToString();
        }
        private int GetFactorialChainLength(long ChainStart)
        {
            long currentChainItem = ChainStart;
            bool NoRepeats = true;
            List<long> chain = new List<long>();
            chain.Add(currentChainItem);
            while(NoRepeats)
            {
                currentChainItem = NextFactorialChainItem(currentChainItem);
                NoRepeats = !chain.Contains(currentChainItem);
                chain.Add(currentChainItem);
            }
            return chain.Count() - 1;
        }
        private long NextFactorialChainItem(long CurrentChainItem)
        {
            long result = 0;
            int[] digitArray = ConvertToDigitArray(CurrentChainItem);
            foreach(int digit in digitArray)
            {
                result += factorials[digit];
            }
            return result;
        }
        private static int[] ConvertToDigitArray(long Value)
        {
            return Value.ToString().ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
        }
        
    }
}
