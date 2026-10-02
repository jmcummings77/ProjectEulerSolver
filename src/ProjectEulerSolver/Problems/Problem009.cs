namespace ProjectEulerSolver.Problems;

/// <summary>The product abc of the Pythagorean triplet with a + b + c = 1000.</summary>
public sealed class Problem009 : Problem
{
    public override int Number => 9;

    public override string Title => "Special Pythagorean Triplet";

    public override object Solve()
    {
        const int perimeter = 1000;
        for (var a = 1; a < perimeter / 3; a++)
        {
            for (var b = a + 1; b < (perimeter - a) / 2; b++)
            {
                var c = perimeter - a - b;
                if (a * a + b * b == c * c)
                {
                    return a * b * c;
                }
            }
        }

        throw new InvalidOperationException("No Pythagorean triplet sums to 1000.");
    }
}
