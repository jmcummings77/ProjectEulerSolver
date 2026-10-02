namespace ProjectEulerSolver.Problems;

/// <summary>Sum of the even-valued Fibonacci terms that do not exceed four million.</summary>
public sealed class Problem002 : Problem
{
    public override int Number => 2;

    public override string Title => "Even Fibonacci Numbers";

    public override object Solve()
    {
        long sum = 0;
        long previous = 1;
        long current = 2;
        while (current <= 4_000_000)
        {
            if (current % 2 == 0)
            {
                sum += current;
            }

            (previous, current) = (current, previous + current);
        }

        return sum;
    }
}
