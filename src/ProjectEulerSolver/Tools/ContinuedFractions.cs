using System.Numerics;

namespace ProjectEulerSolver.Tools;

/// <summary>Continued-fraction expansions of square roots and their convergents.</summary>
public static class ContinuedFractions
{
    /// <summary>
    /// The continued fraction of √n as (a0, [a1, a2, ..., ak]) where the bracketed terms repeat forever.
    /// For a perfect square the periodic part is empty.
    /// </summary>
    public static (int Leading, int[] Period) SqrtExpansion(int n)
    {
        var a0 = (int)NumberTheory.ISqrt(n);
        if (a0 * a0 == n)
        {
            return (a0, []);
        }

        // Standard recurrence: m_{k+1} = d_k a_k - m_k, d_{k+1} = (n - m_{k+1}^2) / d_k, a_{k+1} = floor((a0 + m_{k+1}) / d_{k+1}).
        var period = new List<int>();
        var m = 0;
        var d = 1;
        var a = a0;
        do
        {
            m = d * a - m;
            d = (n - m * m) / d;
            a = (a0 + m) / d;
            period.Add(a);
        }
        while (a != 2 * a0);

        return (a0, [.. period]);
    }

    /// <summary>Length of the repeating block in the continued fraction of √n (0 for perfect squares).</summary>
    public static int SqrtPeriodLength(int n) => SqrtExpansion(n).Period.Length;

    /// <summary>
    /// Successive convergents h/k of the continued fraction [a0; a1, a2, ...] given by <paramref name="terms"/>.
    /// </summary>
    public static IEnumerable<(BigInteger Numerator, BigInteger Denominator)> Convergents(IEnumerable<int> terms)
    {
        BigInteger hPrev = 0, h = 1;
        BigInteger kPrev = 1, k = 0;
        foreach (var term in terms)
        {
            (hPrev, h) = (h, term * h + hPrev);
            (kPrev, k) = (k, term * k + kPrev);
            yield return (h, k);
        }
    }

    /// <summary>The terms a0, a1, a2, ... of √n's continued fraction, repeating the periodic block forever.</summary>
    public static IEnumerable<int> SqrtTerms(int n)
    {
        var (leading, period) = SqrtExpansion(n);
        yield return leading;
        if (period.Length == 0)
        {
            yield break;
        }

        while (true)
        {
            foreach (var term in period)
            {
                yield return term;
            }
        }
    }
}
