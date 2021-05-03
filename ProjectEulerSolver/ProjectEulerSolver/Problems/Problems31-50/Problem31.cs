using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEulerSolver.Problems
{
    public class Problem31 : BaseProblem, IProblem
    {
        public Problem31()
        {
            Number = 31;
            Prompt = "In England the currency is made up of pound, £, and pence, p, and there are eight coins in general circulation: " +

                        "1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p). " +
                        "It is possible to make £2 in the following way: " +

                        "1×£1 + 1×50p + 2×20p + 1×5p + 1×2p + 3×1p " +
                    "How many different ways can £2 be made using any number of coins? ";
        }
        public override void Solve()
        {
            List<int> coins = new List<int>();
            var Coins = new int[] { 1, 2, 5, 10, 20, 50, 100, 200 };
            var result = Combinator.CountWaysCoinsCanTotal(Coins, 200);
            Output = result.ToString();
        }
    }
}