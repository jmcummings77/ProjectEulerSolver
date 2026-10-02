using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The index of the first Fibonacci term to contain 1000 digits.</summary>
public sealed class Problem025 : Problem
{
    public override int Number => 25;

    public override string Title => "1000-digit Fibonacci Number";

    public override object Solve()
    {
        var threshold = BigInteger.Pow(10, 999); // The smallest 1000-digit number.
        BigInteger previous = 1;
        BigInteger current = 1;
        var index = 2;
        while (current < threshold)
        {
            (previous, current) = (current, previous + current);
            index++;
        }

        return index;
    }
}
