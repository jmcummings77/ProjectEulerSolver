using System.Numerics;

namespace ProjectEulerSolver.Problems;

/// <summary>Product of the digits d1 × d10 × ... × d1000000 of Champernowne's constant.</summary>
public sealed class Problem040 : Problem
{
    public override int Number => 40;

    public override string Title => "Champernowne's Constant";

    public override object Solve() => Solve(positions: [1, 10, 100, 1000, 10_000, 100_000, 1_000_000]);

    /// <summary>
    /// The product of the digits found at the given <paramref name="positions"/> of the fractional part of
    /// Champernowne's constant 0.123456789101112..., which is written by concatenating the positive integers.
    /// </summary>
    /// <param name="positions">
    /// One or more digit positions, counted from 1, each from 1 to 2,147,483,647 (any positive int).
    /// A position that is listed more than once is multiplied in each time.
    /// </param>
    public static BigInteger Solve(IReadOnlyList<int> positions)
    {
        if (positions.Count == 0)
        {
            throw new ArgumentException("Expected at least one position.", nameof(positions));
        }

        foreach (var position in positions)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(position, 1, nameof(positions));
        }

        var product = BigInteger.One;
        foreach (var position in positions)
        {
            product *= DigitAt(position);
        }

        return product;
    }

    private static int DigitAt(int position)
    {
        // The numbers with `length` digits start at `first` = 10^(length - 1); there are 9 * first of them and
        // together they fill 9 * first * length positions. Skip whole blocks until the position falls inside
        // one. An int position is reached by the nine-digit block at the latest, so a long holds every term.
        long offset = position - 1;
        long first = 1;
        var length = 1;
        while (offset >= 9 * first * length)
        {
            offset -= 9 * first * length;
            first *= 10;
            length++;
        }

        var number = first + offset / length;
        for (var digitsToDrop = length - 1 - offset % length; digitsToDrop > 0; digitsToDrop--)
        {
            number /= 10;
        }

        return (int)(number % 10);
    }
}
