using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The next triangle number after 40755 that is also pentagonal and hexagonal.</summary>
public sealed class Problem045 : Problem
{
    public override int Number => 45;

    public override string Title => "Triangular, Pentagonal, and Hexagonal";

    public override object Solve()
    {
        // Every hexagonal number is triangular (H(n) = T(2n − 1)), so only pentagonality needs checking.
        for (long n = 144; ; n++)
        {
            var hexagonal = Figurate.Hexagonal(n);
            if (Figurate.IsPentagonal(hexagonal))
            {
                return hexagonal;
            }
        }
    }
}
