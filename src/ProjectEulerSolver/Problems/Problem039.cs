namespace ProjectEulerSolver.Problems;

/// <summary>The perimeter p ≤ 1000 with the most integer-sided right triangle solutions.</summary>
public sealed class Problem039 : Problem
{
    public override int Number => 39;

    public override string Title => "Integer Right Triangles";

    public override object Solve()
    {
        var bestPerimeter = 0;
        var bestCount = 0;
        for (var p = 12; p <= 1000; p += 2) // The perimeter of a Pythagorean triple is always even.
        {
            var count = 0;
            for (var a = 1; a < p / 3; a++)
            {
                // From a + b + c = p and a² + b² = c²: b = p(p - 2a) / (2(p - a)).
                var numerator = p * (p - 2 * a);
                var denominator = 2 * (p - a);
                if (numerator % denominator == 0 && numerator / denominator > a)
                {
                    count++;
                }
            }

            if (count > bestCount)
            {
                bestCount = count;
                bestPerimeter = p;
            }
        }

        return bestPerimeter;
    }
}
