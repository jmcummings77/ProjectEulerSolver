using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>How many different ways £2 can be made using any number of the eight British coins.</summary>
public sealed class Problem031 : Problem
{
    public override int Number => 31;

    public override string Title => "Coin Sums";

    public override object Solve() => Solve(target: 200, coins: [1, 2, 5, 10, 20, 50, 100, 200]);

    /// <summary>
    /// How many different ways <paramref name="target"/> can be made using any number of coins of the given denominations.
    /// </summary>
    /// <param name="target">The amount to make; zero or more.</param>
    /// <param name="coins">The distinct, positive coin denominations.</param>
    public static BigInteger Solve(int target, IReadOnlyList<int> coins)
    {
        if (coins.Distinct().Count() != coins.Count)
        {
            throw new ArgumentException("Coin denominations must be distinct.", nameof(coins));
        }

        return Combinatorics.CountPartitions(target, coins);
    }
}
