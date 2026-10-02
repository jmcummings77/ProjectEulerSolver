using System.Numerics;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>The general solvers of problems 71 to 79, checked against the statements' examples and brute force.</summary>
public class Problems071To079Tests
{
    private static readonly int[] DigitFactorials = [1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880];

    [Fact]
    public void Each_problem_exposes_the_parameters_its_statement_fixes()
    {
        Assert.Equal("numerator, denominator, maxDenominator", ParameterizedSolver.Signature(new Problem071()));
        Assert.Equal("maxDenominator", ParameterizedSolver.Signature(new Problem072()));
        Assert.Equal("maxDenominator", ParameterizedSolver.Signature(new Problem073()));
        Assert.Equal("limit, chainLength", ParameterizedSolver.Signature(new Problem074()));
        Assert.Equal("maxLength", ParameterizedSolver.Signature(new Problem075()));
        Assert.Equal("n", ParameterizedSolver.Signature(new Problem076()));
        Assert.Equal("ways", ParameterizedSolver.Signature(new Problem077()));
        Assert.Equal("divisor", ParameterizedSolver.Signature(new Problem078()));
        Assert.Equal("attempts", ParameterizedSolver.Signature(new Problem079()));
    }

    [Fact]
    public void The_runner_binds_text_arguments_to_the_general_solvers()
    {
        var fraction = new Dictionary<string, string> { ["numerator"] = "3", ["denominator"] = "7", ["maxDenominator"] = "8" };
        Assert.Equal(2L, ParameterizedSolver.Solve(new Problem071(), fraction));

        var keylog = new Dictionary<string, string> { ["attempts"] = "317, 531, 127, 278, 538, 512" };
        Assert.Equal("531278", ParameterizedSolver.Solve(new Problem079(), keylog));
    }

    [Fact]
    public void Problem071_matches_the_statement_example()
    {
        // "It can be seen that 2/5 is the fraction immediately to the left of 3/7" for d ≤ 8.
        Assert.Equal(2, Problem071.Solve(numerator: 3, denominator: 7, maxDenominator: 8));
    }

    [Fact]
    public void Problem071_matches_brute_force_for_small_lists()
    {
        var targets = new List<(long Numerator, long Denominator)>();
        for (var denominator = 2; denominator <= 16; denominator++)
        {
            for (var numerator = 1; numerator < denominator; numerator++)
            {
                targets.Add((numerator, denominator)); // Includes unreduced targets such as 6/8.
            }
        }

        // Targets whose own denominator is far beyond the list, including the extremes of the supported range.
        targets.Add((300_000_000_000_000_001, 700_000_000_000_000_000)); // Just above 3/7.
        targets.Add((299_999_999_999_999_999, 700_000_000_000_000_000)); // Just below 3/7.
        targets.Add((1, long.MaxValue));
        targets.Add((long.MaxValue - 1, long.MaxValue));
        targets.Add((long.MaxValue / 3, long.MaxValue));

        foreach (var (numerator, denominator) in targets)
        {
            for (var maxDenominator = 1; maxDenominator <= 40; maxDenominator++)
            {
                var expected = LeftNeighbourNumerator(numerator, denominator, maxDenominator);
                if (expected == 0)
                {
                    Assert.Throws<InvalidOperationException>(() => Problem071.Solve(numerator, denominator, maxDenominator));
                }
                else
                {
                    Assert.Equal(expected, Problem071.Solve(numerator, denominator, maxDenominator));
                }
            }
        }
    }

    [Theory]
    [InlineData(3, 7, 1_000_000_000_000_000_000)]
    [InlineData(3, 7, long.MaxValue)]
    [InlineData(1, 2, long.MaxValue)]
    [InlineData(999, 1000, long.MaxValue)]
    [InlineData(355, 113 * 4, 987_654_321_987_654_321)]
    public void Problem071_handles_lists_far_larger_than_the_statement(long numerator, long denominator, long maxDenominator)
    {
        // When a reduced a/b is itself in the list, its left neighbour p/q is the solution of aq - bp = 1 with
        // the largest q in the list, so step q down from the top until b divides aq - 1.
        var divisor = NumberTheory.Gcd(numerator, denominator);
        BigInteger a = numerator / divisor;
        BigInteger b = denominator / divisor;
        BigInteger q = maxDenominator;
        while ((a * q - 1) % b != 0)
        {
            q--;
        }

        Assert.Equal((a * q - 1) / b, Problem071.Solve(numerator, denominator, maxDenominator));
    }

    [Fact]
    public void Problem071_reaches_the_top_of_the_supported_range()
    {
        // (M-2)/(M-1) and (M-1)/M are neighbours: their cross products differ by one.
        Assert.Equal(long.MaxValue - 2, Problem071.Solve(long.MaxValue - 1, long.MaxValue, long.MaxValue));
    }

    [Fact]
    public void Problem071_leaves_no_listed_fraction_between_its_answer_and_the_target()
    {
        // Independent of the solver's walk, and valid whether or not the target is itself in the list. If p is
        // the answer, its denominator q is the smallest one that puts p/q below the target a/b. The fraction that
        // follows p/q in the list is r/s with r·q − p·s = 1 and the largest such s in the list, and it must not be
        // below the target. Targets and lists are drawn from every magnitude up to long.MaxValue.
        var random = new Random(71);
        var answered = 0;
        for (var round = 0; round < 4000; round++)
        {
            var denominator = Math.Max(2, random.NextInt64(1, long.MaxValue) >> random.Next(0, 62));
            var numerator = random.NextInt64(1, denominator);
            var maxDenominator = Math.Max(1, random.NextInt64(1, long.MaxValue) >> random.Next(0, 62));
            BigInteger a = numerator;
            BigInteger b = denominator;
            BigInteger last = maxDenominator;

            // 1/maxDenominator is the smallest proper fraction in the list, so there is an answer exactly when
            // it is below the target.
            if (maxDenominator == 1 || b >= a * last)
            {
                Assert.Throws<InvalidOperationException>(() => Problem071.Solve(numerator, denominator, maxDenominator));
                continue;
            }

            BigInteger p = Problem071.Solve(numerator, denominator, maxDenominator);
            var q = p * b / a + 1;
            Assert.True(p >= 1 && q <= last, $"{p}/{q} is not in the list for {numerator}/{denominator}, d ≤ {maxDenominator}");
            Assert.Equal(BigInteger.One, BigInteger.GreatestCommonDivisor(p, q));

            var smallest = (q - ModularInverse(p, q)) % q; // The least s with p·s ≡ −1 (mod q).
            var s = smallest + (last - smallest) / q * q;
            var r = (p * s + 1) / q;
            Assert.Equal(BigInteger.One, r * q - p * s);
            Assert.True(r * b >= a * s, $"{r}/{s} lies between {p}/{q} and {numerator}/{denominator} for d ≤ {maxDenominator}");
            answered++;
        }

        Assert.True(answered >= 2000, $"Only {answered} rounds had an answer.");
    }

    [Fact]
    public void Problem071_rejects_bad_arguments_and_reports_when_nothing_is_smaller()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem071.Solve(0, 7, 8));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem071.Solve(-3, 7, 8));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem071.Solve(7, 7, 8));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem071.Solve(8, 7, 8));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem071.Solve(3, 7, 0));

        // 1/8 is the smallest proper fraction for d ≤ 8, and there are no proper fractions at all for d ≤ 1.
        Assert.Throws<InvalidOperationException>(() => Problem071.Solve(1, 8, 8));
        Assert.Throws<InvalidOperationException>(() => Problem071.Solve(3, 7, 1));
        Assert.Equal(1, Problem071.Solve(1, 8, 9));
    }

    [Fact]
    public void Problem072_matches_the_statement_example()
    {
        // "It can be seen that there are 21 elements in this set" for d ≤ 8.
        Assert.Equal(21, Problem072.Solve(maxDenominator: 8));
    }

    [Fact]
    public void Problem072_matches_brute_force_for_small_denominators()
    {
        long expected = 0;
        for (var d = 1; d <= 200; d++)
        {
            expected += Enumerable.Range(1, d - 1).Count(n => NumberTheory.Gcd(n, d) == 1);
            Assert.Equal(expected, Problem072.Solve(d));
        }
    }

    [Fact]
    public void Problem072_handles_more_denominators_than_the_statement()
    {
        // Independent count: the coprime pairs (x, y) in [1, N]² number Σ μ(g)·⌊N/g⌋², and each reduced proper
        // fraction is two of them (x < y and y < x); the one remaining pair is (1, 1).
        const int MaxDenominator = 3_000_000;
        var moebius = Moebius(MaxDenominator);
        long coprimePairs = 0;
        for (var g = 1; g <= MaxDenominator; g++)
        {
            coprimePairs += moebius[g] * (long)(MaxDenominator / g) * (MaxDenominator / g);
        }

        Assert.Equal((coprimePairs - 1) / 2, Problem072.Solve(MaxDenominator));
    }

    [Fact]
    public void Problem072_rejects_denominators_outside_the_supported_range()
    {
        Assert.Equal(0, Problem072.Solve(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem072.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem072.Solve(-8));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem072.Solve(100_000_001));
    }

    [Fact]
    public void Problem073_matches_the_statement_example()
    {
        // "It can be seen that there are 3 fractions between 1/3 and 1/2" for d ≤ 8: 3/8, 2/5 and 3/7.
        Assert.Equal(3, Problem073.Solve(maxDenominator: 8));
    }

    [Fact]
    public void Problem073_matches_brute_force_for_small_denominators()
    {
        long expected = 0;
        for (var d = 1; d <= 200; d++)
        {
            expected += Enumerable.Range(1, d).Count(n => 3 * n > d && 2 * n < d && NumberTheory.Gcd(n, d) == 1);
            Assert.Equal(expected, Problem073.Solve(d));
        }
    }

    [Fact]
    public void Problem073_matches_the_stern_brocot_tree_beyond_the_statement()
    {
        // 1/3 and 1/2 are neighbours in the Stern–Brocot tree, so the reduced fractions between them are exactly
        // the mediants generated from that pair, and only the denominators are needed to count them.
        const int MaxDenominator = 15_000;
        long expected = 0;
        var pending = new Stack<(int Left, int Right)>();
        pending.Push((3, 2));
        while (pending.TryPop(out var pair))
        {
            var mediant = pair.Left + pair.Right;
            if (mediant <= MaxDenominator)
            {
                expected++;
                pending.Push((pair.Left, mediant));
                pending.Push((mediant, pair.Right));
            }
        }

        Assert.Equal(expected, Problem073.Solve(MaxDenominator));
    }

    [Fact]
    public void Problem073_handles_counts_beyond_an_int()
    {
        // Independent count by Möbius inversion: with F(m) the number of all fractions (reduced or not) in the
        // interval with denominators up to m, the reduced ones number Σ μ(g)·F(⌊N/g⌋). F is tallied by
        // numerator: n/d lies in the interval exactly when 2n < d < 3n.
        const int MaxDenominator = 1_000_000;
        var all = new long[MaxDenominator + 1];
        for (var n = 1; 2 * n < MaxDenominator; n++)
        {
            all[2 * n + 1]++;
            if (3 * n <= MaxDenominator)
            {
                all[3 * n]--;
            }
        }

        // The first running sum turns the range marks into a count per denominator, the second into F.
        for (var pass = 0; pass < 2; pass++)
        {
            for (var d = 1; d <= MaxDenominator; d++)
            {
                all[d] += all[d - 1];
            }
        }

        var moebius = Moebius(MaxDenominator);
        long expected = 0;
        for (var g = 1; g <= MaxDenominator; g++)
        {
            expected += moebius[g] * all[MaxDenominator / g];
        }

        Assert.True(expected > int.MaxValue);
        Assert.Equal(expected, Problem073.Solve(MaxDenominator));
    }

    [Fact]
    public void Problem073_rejects_denominators_outside_the_supported_range()
    {
        Assert.Equal(0, Problem073.Solve(1));
        Assert.Equal(0, Problem073.Solve(4));
        Assert.Equal(1, Problem073.Solve(5)); // 2/5
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem073.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem073.Solve(100_000_001));
    }

    [Fact]
    public void Problem074_counts_the_chains_named_in_the_statement()
    {
        // 145 → 145; 169 → 363601 → 1454 → 169; 871 → 45361 → 871; 872 → 45362 → 872;
        // 69 → 363600 → 1454 → 169 → 363601 (→ 1454); 78 → 45360 → 871 → 45361 (→ 871); 540 → 145 (→ 145).
        Assert.Equal(1, ChainLength(145));
        Assert.Equal(3, ChainLength(169));
        Assert.Equal(2, ChainLength(871));
        Assert.Equal(2, ChainLength(872));
        Assert.Equal(5, ChainLength(69));
        Assert.Equal(4, ChainLength(78));
        Assert.Equal(2, ChainLength(540));

        // Below 146 the only chains of one term are the factorions 1, 2 and 145.
        Assert.Equal(3, Problem074.Solve(limit: 146, chainLength: 1));
        Assert.Equal(2, Problem074.Solve(limit: 145, chainLength: 1));
    }

    [Fact]
    public void Problem074_agrees_that_sixty_terms_is_the_longest_chain_below_one_million()
    {
        // "The longest non-repeating chain with a starting number below one million is sixty terms."
        var chains = new Problem074.DigitFactorialChains();
        Assert.Equal(60, Enumerable.Range(1, 999_999).Max(chains.Length));
        Assert.Equal(0, Problem074.Solve(limit: 1_000_000, chainLength: 61));
    }

    [Fact]
    public void Problem074_matches_an_uncached_walk_for_small_limits()
    {
        var lengths = Enumerable.Range(0, 3000).Select(start => start == 0 ? 0 : ChainLength(start)).ToArray();
        foreach (var limit in new[] { 2, 146, 3000 })
        {
            for (var chainLength = 1; chainLength <= 62; chainLength++)
            {
                var expected = lengths.Take(limit).Count(length => length == chainLength);
                Assert.Equal(expected, Problem074.Solve(limit, chainLength));
            }
        }
    }

    [Fact]
    public void Problem074_matches_an_uncached_walk_just_above_the_statement_limit()
    {
        var expected = Enumerable.Range(1_000_000, 40_000).Count(start => ChainLength(start) == 60);
        Assert.Equal(expected, Problem074.Solve(1_040_000, 60) - Problem074.Solve(1_000_000, 60));
    }

    [Fact]
    public void Problem074_handles_starts_larger_than_any_digit_factorial_sum()
    {
        // Ten digits sum to at most 10 × 9! = 3,628,800, so these starts lie beyond every later term of a chain.
        var chains = new Problem074.DigitFactorialChains();
        int[] starts = [3_628_799, 3_628_800, 3_628_801, 3_999_999, 9_999_999, 1_000_000_000, 1_999_999_999, int.MaxValue - 1, int.MaxValue];
        foreach (var start in starts)
        {
            Assert.Equal(ChainLength(start), chains.Length(start));
        }

        // A chain has exactly two terms when the start is not its own digit factorial sum but its image either
        // is one or maps straight back to the start.
        const int Limit = 3_700_000;
        var twoTerms = 0;
        for (var start = 1; start < Limit; start++)
        {
            var second = DigitFactorialSum(start);
            var third = DigitFactorialSum(second);
            if (second != start && (third == start || third == second))
            {
                twoTerms++;
            }
        }

        Assert.Equal(twoTerms, Problem074.Solve(Limit, chainLength: 2));
    }

    [Fact]
    public void Problem074_rejects_bad_arguments()
    {
        Assert.Equal(0, Problem074.Solve(limit: 1, chainLength: 1));
        Assert.Equal(0, Problem074.Solve(limit: 1000, chainLength: 1000)); // No such chain is a count of zero, not an error.
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem074.Solve(limit: 0, chainLength: 60));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem074.Solve(limit: 1000, chainLength: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Problem074.DigitFactorialChains().Length(0));
    }

    [Fact]
    public void Problem075_matches_the_statement_examples()
    {
        // 12, 24, 30, 36, 40 and 48 each form exactly one triangle; 120 forms three.
        Assert.Equal(0, Problem075.Solve(maxLength: 11));
        Assert.Equal(1, Problem075.Solve(maxLength: 12));
        Assert.Equal(6, Problem075.Solve(maxLength: 48));
        Assert.Equal(Problem075.Solve(119), Problem075.Solve(120));
    }

    [Fact]
    public void Problem075_matches_brute_force_for_short_wires()
    {
        const int Longest = 1000;
        var triangles = new int[Longest + 1];
        for (var a = 1; 3 * a < Longest; a++)
        {
            for (var b = a; a + 2 * b < Longest; b++)
            {
                var c = (int)NumberTheory.ISqrt(a * a + b * b);
                if (c * c == a * a + b * b && a + b + c <= Longest)
                {
                    triangles[a + b + c]++;
                }
            }
        }

        Assert.Equal(3, triangles[120]);
        var expected = 0;
        for (var length = 1; length <= Longest; length++)
        {
            expected += triangles[length] == 1 ? 1 : 0;
            Assert.Equal(expected, Problem075.Solve(length));
        }
    }

    [Fact]
    public void Problem075_matches_the_tree_of_primitive_triples_beyond_the_statement()
    {
        // Independent of Euclid's formula: Berggren's three matrices generate every primitive triple exactly once
        // from (3, 4, 5), and a child's perimeter always exceeds its parent's.
        const int Longest = 5_000_000;
        var triangles = new int[Longest + 1];
        var pending = new Stack<(long A, long B, long C)>();
        pending.Push((3, 4, 5));
        while (pending.TryPop(out var triple))
        {
            var (a, b, c) = triple;
            var perimeter = a + b + c;
            if (perimeter > Longest)
            {
                continue;
            }

            for (var length = perimeter; length <= Longest; length += perimeter)
            {
                triangles[length]++;
            }

            pending.Push((a - 2 * b + 2 * c, 2 * a - b + 2 * c, 2 * a - 2 * b + 3 * c));
            pending.Push((a + 2 * b + 2 * c, 2 * a + b + 2 * c, 2 * a + 2 * b + 3 * c));
            pending.Push((-a + 2 * b + 2 * c, -2 * a + b + 2 * c, -2 * a + 2 * b + 3 * c));
        }

        Assert.Equal(triangles.Count(count => count == 1), Problem075.Solve(Longest));
    }

    [Fact]
    public void Problem075_rejects_lengths_outside_the_supported_range()
    {
        Assert.Equal(0, Problem075.Solve(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem075.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem075.Solve(-12));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem075.Solve(100_000_001));
    }

    [Fact]
    public void Problem076_matches_the_statement_example()
    {
        // "It is possible to write five as a sum in exactly six different ways."
        Assert.Equal(6, Problem076.Solve(n: 5));
    }

    [Fact]
    public void Problem076_matches_brute_force_for_small_numbers()
    {
        for (var n = 1; n <= 35; n++)
        {
            // Capping the largest part at n - 1 leaves exactly the sums with at least two terms.
            Assert.Equal(CountSums(n, largestPart: n - 1), Problem076.Solve(n));
        }
    }

    [Fact]
    public void Problem076_handles_numbers_whose_counts_overflow_a_long()
    {
        // The same count by the coin-change recurrence over the parts 1 .. n-1.
        var expected = Combinatorics.CountPartitions(2000, Enumerable.Range(1, 1999));
        Assert.True(expected > long.MaxValue);
        Assert.Equal(expected, Problem076.Solve(2000));

        // Ramanujan's congruences: p(5k + 4), p(7k + 5) and p(11k + 6) are divisible by 5, 7 and 11.
        // 20,004 is of all three forms, and Solve counts every partition but one.
        Assert.Equal(BigInteger.Zero, (Problem076.Solve(20_004) + 1) % (5 * 7 * 11));
    }

    [Fact]
    public void Problem076_rejects_numbers_outside_the_supported_range()
    {
        Assert.Equal(0, Problem076.Solve(1));
        Assert.Equal(1, Problem076.Solve(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem076.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem076.Solve(-5));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem076.Solve(100_001));
    }

    [Fact]
    public void Problem077_matches_the_statement_example()
    {
        // "It is possible to write ten as the sum of primes in exactly five different ways", and nothing
        // smaller has as many (9 has four: 7+2, 5+2+2, 3+3+3, 3+2+2+2).
        Assert.Equal(10, Problem077.Solve(ways: 4));
        Assert.True(Problem077.Solve(ways: 5) > 10);
    }

    [Fact]
    public void Problem077_matches_brute_force_for_small_counts()
    {
        var counts = Enumerable.Range(0, 66).Select(n => n < 2 ? 0 : CountPrimeSums(n, largestPrime: n)).ToArray();
        Assert.Equal(5, counts[10]);

        // Every threshold at which the answer can change, and its neighbours.
        var thresholds = counts.Take(60).SelectMany(count => new[] { count - 1, count, count + 1 }).Where(ways => ways >= 0).Distinct();
        foreach (var ways in thresholds)
        {
            var expected = Array.FindIndex(counts, count => count > ways);
            Assert.True(expected > 0);
            Assert.Equal(expected, Problem077.Solve(ways));
        }
    }

    [Theory]
    [InlineData(1_000_000)]
    [InlineData(1_000_000_000_000)]
    [InlineData(long.MaxValue - 1)]
    [InlineData(long.MaxValue)]
    public void Problem077_handles_counts_far_beyond_the_statement(long ways)
    {
        var answer = Problem077.Solve(ways);

        // Coin-change over every prime up to the answer is exact for every value up to the answer.
        var counts = new BigInteger[answer + 1];
        counts[0] = 1;
        foreach (var prime in Enumerable.Range(2, answer - 1).Where(IsPrimeByTrialDivision))
        {
            for (var amount = prime; amount <= answer; amount++)
            {
                counts[amount] += counts[amount - prime];
            }
        }

        Assert.True(counts[answer] > ways);
        Assert.All(counts.Skip(1).Take(answer - 1), count => Assert.True(count <= ways));
    }

    [Fact]
    public void Problem077_search_stays_small_at_the_top_of_the_supported_range()
    {
        // The solver's table size is justified by this value: the answer never decreases as ways grows.
        Assert.Equal(1287, Problem077.Solve(long.MaxValue));
    }

    [Fact]
    public void Problem077_is_exact_on_either_side_of_each_table_size()
    {
        // The solver's table starts at 16 values and doubles; these thresholds put the answer just below, at and
        // just above every size it passes through.
        var counts = PrimeSumCounts(1300);
        foreach (var size in new[] { 16, 32, 64, 128, 256, 512, 1024 })
        {
            for (var n = size - 1; n <= size + 2; n++)
            {
                foreach (var ways in new[] { counts[n] - 1, counts[n] })
                {
                    var expected = Array.FindIndex(counts, 2, count => count > ways);
                    Assert.True(expected > 0);
                    Assert.Equal(expected, Problem077.Solve((long)ways));
                }
            }
        }
    }

    [Fact]
    public void Problem077_rejects_negative_counts()
    {
        Assert.Equal(2, Problem077.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem077.Solve(-1));
    }

    [Fact]
    public void Problem078_matches_the_statement_example()
    {
        // "Five coins can be separated into piles in exactly seven different ways, so p(5) = 7."
        Assert.Equal(5, Problem078.Solve(divisor: 7));
    }

    [Fact]
    public void Problem078_matches_exact_partition_numbers_for_small_divisors()
    {
        var partitions = PartitionNumbers(2500);
        for (var divisor = 1; divisor <= 300; divisor++)
        {
            var expected = Array.FindIndex(partitions, 1, p => p % divisor == 0);
            Assert.True(expected > 0, $"No p(n) with n ≤ 2500 is divisible by {divisor}.");
            Assert.Equal(expected, Problem078.Solve(divisor));
        }
    }

    [Fact]
    public void Problem078_finds_each_partition_number_as_its_own_first_multiple()
    {
        // p(n) is positive and strictly increasing from n = 1, so nothing earlier can be a multiple of p(n).
        // p(60) = 966,467 is the last one within the supported range.
        var partitions = PartitionNumbers(61);
        Assert.True(partitions[60] <= 1_000_000 && partitions[61] > 1_000_000);
        for (var n = 1; n <= 60; n++)
        {
            Assert.Equal(n, Problem078.Solve((int)partitions[n]));
        }
    }

    [Fact]
    public void Problem078_search_stays_well_inside_its_cut_off()
    {
        // The solver gives up at 32 × divisor; over every divisor up to 100,000 the largest ratio seen is just
        // under 11. Re-running them all takes minutes, so this samples the low end.
        for (var divisor = 1; divisor <= 1500; divisor++)
        {
            Assert.InRange(Problem078.Solve(divisor), 1, 11 * divisor);
        }
    }

    [Fact]
    public void Problem078_rejects_bad_divisors_and_reports_an_exhausted_search()
    {
        Assert.Equal(1, Problem078.Solve(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem078.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem078.Solve(-7));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem078.Solve(1_000_001));

        // p(5) = 7 is the first multiple of 7, so a search that stops at 4 finds nothing.
        Assert.Equal(5, Problem078.LeastIndex(divisor: 7, searchLimit: 5));
        Assert.Throws<InvalidOperationException>(() => Problem078.LeastIndex(divisor: 7, searchLimit: 4));
    }

    [Fact]
    public void Problem079_recovers_the_statement_example()
    {
        // "If the passcode was 531278, they may ask for the 2nd, 3rd, and 5th characters; the expected reply
        // would be: 317."
        Assert.Equal("317", Problem079.Solve(attempts: ["317"]));
        Assert.Equal("531278", Problem079.Solve(attempts: ["317", "531", "127", "278", "538", "512", "317"]));
    }

    [Fact]
    public void Problem079_matches_brute_force_on_random_attempts()
    {
        var random = new Random(79);
        var answered = 0;
        var contradictory = 0;
        for (var round = 0; round < 400; round++)
        {
            // Odd rounds sample a real passcode, so they are always consistent; even rounds are arbitrary strings
            // over a small alphabet and often contradict each other.
            var attempts = new string[random.Next(1, 7)];
            if (round % 2 == 1)
            {
                var passcode = "0123456789".OrderBy(_ => random.Next()).Take(random.Next(3, 8)).ToArray();
                for (var i = 0; i < attempts.Length; i++)
                {
                    var keep = random.Next(1, 1 << passcode.Length);
                    attempts[i] = string.Concat(passcode.Where((_, position) => (keep & (1 << position)) != 0));
                }
            }
            else
            {
                for (var i = 0; i < attempts.Length; i++)
                {
                    attempts[i] = string.Concat(Enumerable.Range(0, random.Next(1, 4)).Select(_ => (char)('0' + random.Next(5))));
                }
            }

            var expected = ShortestPasscodeByBruteForce(attempts);
            if (expected is null)
            {
                Assert.Throws<InvalidOperationException>(() => Problem079.Solve(attempts));
                contradictory++;
            }
            else
            {
                Assert.Equal(expected, Problem079.Solve(attempts));
                answered++;
            }
        }

        Assert.True(answered >= 200 && contradictory >= 20, $"{answered} answered, {contradictory} contradictory");
    }

    [Fact]
    public void Problem079_handles_longer_passcodes_and_attempts_than_the_statement()
    {
        const string Passcode = "8046213975";
        var triples = Combinatorics.Combinations(Passcode.ToCharArray(), 3).Select(chars => new string(chars)).ToArray();
        Assert.Equal(120, triples.Length);
        Assert.Equal(Passcode, Problem079.Solve(triples));
        Assert.Equal(Passcode, Problem079.Solve([Passcode]));
        Assert.Equal(Passcode, Problem079.Solve(["80462", "2139", "39", "975", "6"]));

        // Nothing orders 1 against 2 here, so the passcode that sorts first is returned.
        Assert.Equal("5123", Problem079.Solve(["51", "52", "13", "23"]));
    }

    [Fact]
    public void Problem079_rejects_malformed_and_contradictory_attempts()
    {
        Assert.Throws<ArgumentException>(() => Problem079.Solve([]));
        Assert.Throws<ArgumentException>(() => Problem079.Solve(["319", ""]));
        Assert.Throws<ArgumentException>(() => Problem079.Solve(["319", "68o"]));
        Assert.Throws<ArgumentException>(() => Problem079.Solve(["3 1"]));

        Assert.Throws<InvalidOperationException>(() => Problem079.Solve(["12", "21"]));
        Assert.Throws<InvalidOperationException>(() => Problem079.Solve(["123", "345", "51"]));
        Assert.Throws<InvalidOperationException>(() => Problem079.Solve(["11"])); // A digit before itself.
        Assert.Throws<InvalidOperationException>(() => Problem079.Solve(["121"]));
    }

    /// <summary>
    /// Numerator of the largest proper fraction below the target with a denominator up to maxDenominator, in
    /// lowest terms; zero when there is none.
    /// </summary>
    private static long LeftNeighbourNumerator(long numerator, long denominator, int maxDenominator)
    {
        long bestN = 0;
        long bestD = 1;
        for (long d = 2; d <= maxDenominator; d++)
        {
            for (long n = 1; n < d; n++)
            {
                // A strict comparison keeps the first, and so the reduced, form of each value.
                if ((Int128)n * denominator < (Int128)numerator * d && n * bestD > bestN * d)
                {
                    bestN = n;
                    bestD = d;
                }
            }
        }

        return bestN;
    }

    /// <summary>The inverse of value modulo modulus (coprime, modulus ≥ 2), by the extended Euclidean algorithm.</summary>
    private static BigInteger ModularInverse(BigInteger value, BigInteger modulus)
    {
        var (previous, current) = (modulus, value % modulus);
        var (previousCoefficient, coefficient) = (BigInteger.Zero, BigInteger.One);
        while (current != 0)
        {
            var quotient = previous / current;
            (previous, current) = (current, previous - quotient * current);
            (previousCoefficient, coefficient) = (coefficient, previousCoefficient - quotient * coefficient);
        }

        return (previousCoefficient % modulus + modulus) % modulus;
    }

    /// <summary>The Möbius function for every n ≤ limit.</summary>
    private static int[] Moebius(int limit)
    {
        var moebius = new int[limit + 1];
        Array.Fill(moebius, 1);
        moebius[0] = 0;
        var isComposite = new bool[limit + 1];
        for (var p = 2; p <= limit; p++)
        {
            if (isComposite[p])
            {
                continue;
            }

            for (var multiple = p; multiple <= limit; multiple += p)
            {
                isComposite[multiple] = true;
                moebius[multiple] = (multiple / p) % p == 0 ? 0 : -moebius[multiple];
            }
        }

        return moebius;
    }

    private static int DigitFactorialSum(int n)
    {
        var sum = 0;
        for (; n > 0; n /= 10)
        {
            sum += DigitFactorials[n % 10];
        }

        return sum;
    }

    /// <summary>Number of terms before the first repeat, by walking the chain with no cache.</summary>
    private static int ChainLength(int start)
    {
        var seen = new HashSet<int>();
        for (var term = start; seen.Add(term); term = DigitFactorialSum(term))
        {
        }

        return seen.Count;
    }

    /// <summary>Ways to write the amount as a sum of positive parts no larger than largestPart, largest part first.</summary>
    private static long CountSums(int remaining, int largestPart)
    {
        if (remaining == 0)
        {
            return 1;
        }

        long count = 0;
        for (var part = Math.Min(remaining, largestPart); part >= 1; part--)
        {
            count += CountSums(remaining - part, part);
        }

        return count;
    }

    private static bool IsPrimeByTrialDivision(int n) => n >= 2 && Enumerable.Range(2, n - 2).All(d => d * d > n || n % d != 0);

    /// <summary>Ways to write the amount as a sum of primes no larger than largestPrime, largest prime first.</summary>
    private static long CountPrimeSums(int remaining, int largestPrime)
    {
        if (remaining == 0)
        {
            return 1;
        }

        long count = 0;
        for (var prime = Math.Min(remaining, largestPrime); prime >= 2; prime--)
        {
            if (IsPrimeByTrialDivision(prime))
            {
                count += CountPrimeSums(remaining - prime, prime);
            }
        }

        return count;
    }

    /// <summary>Ways to write each amount up to max as a sum of primes, by the coin-change recurrence over the primes up to max.</summary>
    private static BigInteger[] PrimeSumCounts(int max)
    {
        var counts = new BigInteger[max + 1];
        counts[0] = 1;
        foreach (var prime in Enumerable.Range(2, max - 1).Where(IsPrimeByTrialDivision))
        {
            for (var amount = prime; amount <= max; amount++)
            {
                counts[amount] += counts[amount - prime];
            }
        }

        return counts;
    }

    /// <summary>Exact partition numbers p(0) .. p(max) by the coin-change recurrence over the parts 1 .. max.</summary>
    private static BigInteger[] PartitionNumbers(int max)
    {
        var partitions = new BigInteger[max + 1];
        partitions[0] = 1;
        for (var part = 1; part <= max; part++)
        {
            for (var amount = part; amount <= max; amount++)
            {
                partitions[amount] += partitions[amount - part];
            }
        }

        return partitions;
    }

    /// <summary>
    /// The first, in sorted order, arrangement of the digits seen that contains every attempt as a subsequence;
    /// null when no arrangement does.
    /// </summary>
    private static string? ShortestPasscodeByBruteForce(IReadOnlyList<string> attempts)
    {
        var digits = attempts.SelectMany(attempt => attempt).Distinct().Order().ToArray();
        return Combinatorics.Permutations(digits)
            .Select(arrangement => new string(arrangement))
            .FirstOrDefault(candidate => attempts.All(attempt => IsSubsequence(attempt, candidate)));
    }

    private static bool IsSubsequence(string attempt, string passcode)
    {
        var position = 0;
        foreach (var digit in attempt)
        {
            position = passcode.IndexOf(digit, position) + 1;
            if (position == 0)
            {
                return false;
            }
        }

        return true;
    }
}
