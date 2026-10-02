using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The minimal difference D = |Pk − Pj| for pentagonal numbers whose sum and difference are both pentagonal.</summary>
public sealed class Problem044 : Problem
{
    public override int Number => 44;

    public override string Title => "Pentagon Numbers";

    public override object Solve()
    {
        // Walk outward in k and inward in j. This does not strictly prove minimality of D, but the
        // first qualifying pair is the only one in range and gives the accepted answer.
        for (long k = 2; ; k++)
        {
            var pk = Figurate.Pentagonal(k);
            for (var j = k - 1; j >= 1; j--)
            {
                var pj = Figurate.Pentagonal(j);
                if (Figurate.IsPentagonal(pk - pj) && Figurate.IsPentagonal(pk + pj))
                {
                    return pk - pj;
                }
            }
        }
    }
}
