using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>The last ten digits of 1^1 + 2^2 + ... + 1000^1000.</summary>
public sealed class Problem048 : Problem
{
    private const long MaxWork = 100_000_000_000;

    public override int Number => 48;

    public override string Title => "Self Powers";

    public override object Solve() => Solve(limit: 1000, digits: 10);

    /// <summary>
    /// The last <paramref name="digits"/> digits of 1^1 + 2^2 + ... + limit^limit, padded with leading zeros to
    /// exactly that many characters.
    /// </summary>
    /// <param name="limit">
    /// The last base and exponent in the series, from 1 to 10,000,000. One modular power is computed per term,
    /// so the run time is proportional to it (about a second per million terms at ten digits).
    /// </param>
    /// <param name="digits">
    /// How many trailing digits to keep, from 1 to 1000, and few enough that limit × digits² is at most 10^11:
    /// up to 100 digits at the largest limit, and all 1000 for limits up to 100,000. A modular power costs time
    /// roughly quadratic in the digit count, so that product measures the work; the slowest supported calls
    /// take a few minutes.
    /// </param>
    public static string Solve(int limit, int digits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 10_000_000);
        ArgumentOutOfRangeException.ThrowIfLessThan(digits, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(digits, 1000);

        // Without this the two maxima together would be accepted and then run for well over an hour (a modular
        // power at 1000 digits takes around half a millisecond). The product is at most 10^13, so it fits a long.
        if ((long)limit * digits * digits > MaxWork)
        {
            throw new ArgumentOutOfRangeException(
                nameof(digits),
                digits,
                "limit × digits² must be at most 10^11 to keep the run time reasonable; use fewer digits or a smaller limit.");
        }

        var modulus = BigInteger.Pow(10, digits);
        BigInteger total = 0;
        for (var n = 1; n <= limit; n++)
        {
            total = (total + BigInteger.ModPow(n, n, modulus)) % modulus;
        }

        return total.ToString().PadLeft(digits, '0');
    }
}
