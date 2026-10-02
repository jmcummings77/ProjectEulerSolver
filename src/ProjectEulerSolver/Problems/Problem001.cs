namespace ProjectEulerSolver.Problems;

/// <summary>Sum of all the multiples of 3 or 5 below 1000.</summary>
public sealed class Problem001 : Problem
{
    public override int Number => 1;

    public override string Title => "Multiples of 3 or 5";

    public override object Solve() =>
        Enumerable.Range(1, 999).Where(n => n % 3 == 0 || n % 5 == 0).Sum();
}
