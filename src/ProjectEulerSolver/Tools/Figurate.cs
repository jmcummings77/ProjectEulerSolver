namespace ProjectEulerSolver.Tools;

/// <summary>Polygonal (figurate) numbers: triangle, square, pentagonal, hexagonal, heptagonal and octagonal.</summary>
public static class Figurate
{
    /// <summary>The n-th s-gonal number, P(s, n) = ((s - 2)n² - (s - 4)n) / 2, for s ≥ 3. Throws when the result does not fit in a long.</summary>
    public static long Polygonal(int sides, long n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(sides, 3);
        return checked((long)(((Int128)(sides - 2) * n * n - (Int128)(sides - 4) * n) / 2));
    }

    /// <summary>
    /// True when x is an s-gonal number (s ≥ 3). Solves the quadratic for n exactly in integer arithmetic:
    /// n = (sqrt(8(s-2)x + (s-4)²) + (s-4)) / (2(s-2)) must be a whole number.
    /// </summary>
    public static bool IsPolygonal(int sides, long x)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(sides, 3);
        if (x < 1)
        {
            return false;
        }

        var a = sides - 2;
        var b = sides - 4;
        var discriminant = (Int128)8 * a * x + (Int128)b * b;
        var root = ISqrt(discriminant);
        if (root * root != discriminant)
        {
            return false;
        }

        return (root + b) % (2 * a) == 0;
    }

    /// <summary>The n-th triangle number, n(n + 1)/2.</summary>
    public static long Triangle(long n) => Polygonal(3, n);

    /// <summary>The n-th square number, n².</summary>
    public static long Square(long n) => Polygonal(4, n);

    /// <summary>The n-th pentagonal number, n(3n − 1)/2.</summary>
    public static long Pentagonal(long n) => Polygonal(5, n);

    /// <summary>The n-th hexagonal number, n(2n − 1).</summary>
    public static long Hexagonal(long n) => Polygonal(6, n);

    /// <summary>The n-th heptagonal number, n(5n − 3)/2.</summary>
    public static long Heptagonal(long n) => Polygonal(7, n);

    /// <summary>The n-th octagonal number, n(3n − 2).</summary>
    public static long Octagonal(long n) => Polygonal(8, n);

    /// <summary>True when x is a triangle number.</summary>
    public static bool IsTriangle(long x) => IsPolygonal(3, x);

    /// <summary>True when x is a pentagonal number.</summary>
    public static bool IsPentagonal(long x) => IsPolygonal(5, x);

    /// <summary>True when x is a hexagonal number.</summary>
    public static bool IsHexagonal(long x) => IsPolygonal(6, x);

    private static Int128 ISqrt(Int128 n)
    {
        // The discriminant is below 2^67, so a double estimate is within a few units; correct it exactly.
        var s = (Int128)Math.Sqrt((double)n);
        while (s * s > n)
        {
            s--;
        }

        while ((s + 1) * (s + 1) <= n)
        {
            s++;
        }

        return s;
    }
}
