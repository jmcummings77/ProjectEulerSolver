namespace ProjectEulerSolver.Problems;

/// <summary>The least n for which the partition number p(n) is divisible by one million.</summary>
public sealed class Problem078 : Problem
{
    private const int Modulus = 1_000_000;

    public override int Number => 78;

    public override string Title => "Coin Partitions";

    public override object Solve()
    {
        // Euler's pentagonal number theorem: p(n) = Σ (-1)^(k+1) [p(n - k(3k-1)/2) + p(n - k(3k+1)/2)].
        var partitions = new List<int> { 1 };
        for (var n = 1; ; n++)
        {
            long total = 0;
            for (var k = 1; ; k++)
            {
                var sign = k % 2 == 1 ? 1 : -1;
                var first = n - k * (3 * k - 1) / 2;
                if (first < 0)
                {
                    break;
                }

                total += sign * partitions[first];
                var second = n - k * (3 * k + 1) / 2;
                if (second >= 0)
                {
                    total += sign * partitions[second];
                }
            }

            var value = (int)((total % Modulus + Modulus) % Modulus);
            if (value == 0)
            {
                return n;
            }

            partitions.Add(value);
        }
    }
}
