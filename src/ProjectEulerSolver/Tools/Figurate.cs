namespace ProjectEulerSolver.Tools;

/// <summary>Polygonal (figurate) numbers: triangle, square, pentagonal, hexagonal, heptagonal and octagonal.</summary>
public static class Figurate
{
    /// <summary>The n-th s-gonal number, P(s, n) = ((s - 2)n² - (s - 4)n) / 2.</summary>
    public static long Polygonal(int sides, long n) => ((sides - 2) * n * n - (sides - 4) * n) / 2;

    /// <summary>
    /// True when x is an s-gonal number. Solves the quadratic for n exactly using integer arithmetic:
    /// n = (sqrt(8(s-2)x + (s-4)²) + (s-4)) / (2(s-2)).
    /// </summary>
    public static bool IsPolygonal(int sides, long x)
    {
        if (x < 1)
        {
            return false;
        }

        var a = sides - 2;
        var b = sides - 4;
        var discriminant = 8L * a * x + (long)b * b;
        if (!NumberTheory.IsSquare(discriminant))
        {
            return false;
        }

        var numerator = NumberTheory.ISqrt(discriminant) + b;
        return numerator % (2 * a) == 0;
    }

    public static long Triangle(long n) => Polygonal(3, n);

    public static long Pentagonal(long n) => Polygonal(5, n);

    public static long Hexagonal(long n) => Polygonal(6, n);

    public static long Heptagonal(long n) => Polygonal(7, n);

    public static long Octagonal(long n) => Polygonal(8, n);

    public static bool IsTriangle(long x) => IsPolygonal(3, x);

    public static bool IsPentagonal(long x) => IsPolygonal(5, x);

    public static bool IsHexagonal(long x) => IsPolygonal(6, x);
}
