using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The minimal difference D = |Pk − Pj| for pentagonal numbers whose sum and difference are both pentagonal.</summary>
public sealed class Problem044 : Problem
{
    public override int Number => 44;

    public override string Title => "Pentagon Numbers";

    public override object Solve()
    {
        var best = long.MaxValue;
        var pentagonals = new List<long> { 0, 1 }; // pentagonals[n] = P(n)
        var isPentagonal = new bool[2];
        var marked = 1; // P(1) .. P(marked) have been marked in the table.

        // Consecutive pentagonal numbers differ by P(k) − P(k−1) = 3k − 2, which is the smallest difference any
        // pair with larger index k can have, so once that gap reaches the best difference found the search is
        // provably complete. Within a row, differences grow as j decreases, so the inner loop stops early too.
        for (long k = 2; 3 * k - 2 < best; k++)
        {
            var pk = Figurate.Pentagonal(k);
            pentagonals.Add(pk);

            // Every difference examined in this row is below min(pk, best); keep the lookup table that large.
            var needed = Math.Min(pk, best);
            if (needed >= isPentagonal.Length)
            {
                Array.Resize(ref isPentagonal, (int)Math.Max(needed + 1, 2L * isPentagonal.Length));
                for (var p = Figurate.Pentagonal(marked + 1); p < isPentagonal.Length; p = Figurate.Pentagonal(++marked + 1))
                {
                    isPentagonal[p] = true;
                }
            }

            for (var j = k - 1; j >= 1; j--)
            {
                var pj = pentagonals[(int)j];
                var difference = pk - pj;
                if (difference >= best)
                {
                    break;
                }

                if (isPentagonal[difference] && Figurate.IsPentagonal(pk + pj))
                {
                    best = difference;
                }
            }
        }

        return best;
    }
}
