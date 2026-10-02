namespace ProjectEulerSolver.Problems;

/// <summary>Difference between the square of the sum and the sum of the squares of the first 100 natural numbers.</summary>
public sealed class Problem006 : Problem
{
    public override int Number => 6;

    public override string Title => "Sum Square Difference";

    public override object Solve()
    {
        long sum = 0;
        long sumOfSquares = 0;
        for (var n = 1; n <= 100; n++)
        {
            sum += n;
            sumOfSquares += (long)n * n;
        }

        return sum * sum - sumOfSquares;
    }
}
