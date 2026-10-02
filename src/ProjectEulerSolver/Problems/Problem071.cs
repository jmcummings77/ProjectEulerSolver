namespace ProjectEulerSolver.Problems;

/// <summary>The numerator of the reduced proper fraction immediately to the left of 3/7 for d ≤ 1,000,000.</summary>
public sealed class Problem071 : Problem
{
    public override int Number => 71;

    public override string Title => "Ordered Fractions";

    public override object Solve() => Solve(numerator: 3, denominator: 7, maxDenominator: 1_000_000);

    /// <summary>
    /// The numerator of the reduced proper fraction immediately to the left of
    /// <paramref name="numerator"/>/<paramref name="denominator"/> when the reduced proper fractions with
    /// denominators up to <paramref name="maxDenominator"/> are listed in ascending order.
    /// </summary>
    /// <param name="numerator">Numerator of the target fraction: at least 1 and less than <paramref name="denominator"/>. The target need not be in lowest terms.</param>
    /// <param name="denominator">Denominator of the target fraction: greater than <paramref name="numerator"/>, up to <see cref="long.MaxValue"/>.</param>
    /// <param name="maxDenominator">The largest denominator in the list: from 1 to <see cref="long.MaxValue"/>.</param>
    /// <exception cref="InvalidOperationException">No proper fraction with such a denominator is smaller than the target.</exception>
    public static long Solve(long numerator, long denominator, long maxDenominator)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(numerator, 1);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(denominator, numerator);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxDenominator, 1);

        // Walk down the Stern–Brocot tree towards the target, keeping lower < target ≤ upper and
        // upperN·lowerD − lowerN·upperD = 1. Every fraction strictly between such a pair has a denominator of at
        // least lowerD + upperD, so once that sum exceeds maxDenominator no listed fraction lies between lower
        // and the target: lower is the answer, and the determinant shows it is already in lowest terms.
        long lowerN = 0;
        long lowerD = 1;
        long upperN = 1;
        long upperD = 1;
        while (lowerD <= maxDenominator - upperD)
        {
            // How far the target is above lower (positive) and below upper (zero when upper is the target), as
            // cross products. Every factor is below 2^63, so the products and their difference fit in an Int128.
            var gapBelow = (Int128)numerator * lowerD - (Int128)denominator * lowerN;
            var gapAbove = (Int128)denominator * upperN - (Int128)numerator * upperD;

            // lower + k·upper (k mediant steps at once) is still below the target while k·gapAbove < gapBelow.
            var steps = (maxDenominator - lowerD) / upperD;
            if (gapAbove > 0)
            {
                steps = (long)Int128.Min(steps, (gapBelow - 1) / gapAbove);
            }

            lowerN += steps * upperN;
            lowerD += steps * upperD;
            gapBelow -= steps * gapAbove;

            // upper + k·lower is still at or above the target while k·gapBelow ≤ gapAbove. One of the two moves
            // always advances, and the gaps shrink as in Euclid's algorithm, so the walk takes O(log) rounds.
            steps = (long)Int128.Min((maxDenominator - upperD) / lowerD, gapAbove / gapBelow);
            upperN += steps * lowerN;
            upperD += steps * lowerD;
        }

        if (lowerN == 0)
        {
            throw new InvalidOperationException(
                $"No proper fraction with a denominator up to {maxDenominator} is smaller than {numerator}/{denominator}.");
        }

        return lowerN;
    }
}
