namespace ProjectEulerSolver.Problems;

/// <summary>The maximum digital sum of a^b for a, b &lt; 100.</summary>
public sealed class Problem056 : Problem
{
    private const uint LimbBase = 1_000_000_000;

    public override int Number => 56;

    public override string Title => "Powerful Digit Sum";

    public override object Solve() => Solve(maxBase: 100, maxExponent: 100);

    /// <summary>
    /// The largest sum of decimal digits of a^b over all 1 ≤ a &lt; <paramref name="maxBase"/> and
    /// 1 ≤ b &lt; <paramref name="maxExponent"/>.
    /// </summary>
    /// <param name="maxBase">Exclusive upper bound on a: from 2 to 1,000.</param>
    /// <param name="maxExponent">Exclusive upper bound on b: from 2 to 1,000. The work grows with maxBase × maxExponent².</param>
    public static int Solve(int maxBase, int maxExponent)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxBase, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBase, 1_000);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxExponent, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(maxExponent, 1_000);

        // Each power is the previous one times a, held in base 10^9 (least significant limb first) so that the
        // decimal digits can be read off without a conversion. The list grows whenever a carry leaves the top limb.
        var best = 0;
        var limbs = new List<uint>();
        for (var a = 1; a < maxBase; a++)
        {
            limbs.Clear();
            limbs.Add(1);
            for (var b = 1; b < maxExponent; b++)
            {
                // limb × a + carry < 10^9 × 1,000 + 1,000, far inside a ulong, and the carry stays below a.
                ulong carry = 0;
                for (var i = 0; i < limbs.Count; i++)
                {
                    var product = (ulong)limbs[i] * (ulong)a + carry;
                    limbs[i] = (uint)(product % LimbBase);
                    carry = product / LimbBase;
                }

                if (carry > 0)
                {
                    limbs.Add((uint)carry);
                }

                best = Math.Max(best, DigitSum(limbs));
            }
        }

        return best;
    }

    private static int DigitSum(List<uint> limbs)
    {
        var sum = 0;
        foreach (var limb in limbs)
        {
            for (var rest = limb; rest > 0; rest /= 10)
            {
                sum += (int)(rest % 10);
            }
        }

        return sum;
    }
}
