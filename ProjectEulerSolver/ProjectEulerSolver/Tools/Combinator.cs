using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ProjectEulerSolver.Tools
{
    public static class Combinator
    {
        public static BigInteger CountWaysCoinsCanTotal(int[] Coins, int TotalValue)
        {
            int CountOfCoinTypes = Coins.Length;
            var table = new BigInteger[TotalValue + 1];
            table[0] = 1;
            for (int i = 0; i < CountOfCoinTypes; i++)
            {
                for (int j = Coins[i]; j <= TotalValue; j++)
                {
                    table[j] += table[j - Coins[i]];
                }
            }
            return table[TotalValue];
        }
    }
}
