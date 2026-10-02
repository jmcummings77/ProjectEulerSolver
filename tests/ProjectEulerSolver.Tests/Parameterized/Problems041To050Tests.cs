using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ProjectEulerSolver.Problems;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>
/// The general solvers of problems 41 to 50: the examples from the problem statements, cross-checks against
/// brute force, argument validation, and arguments beyond the statements' own.
/// </summary>
public class Problems041To050Tests
{
    private static Dictionary<string, string> Arguments(params (string Name, string Value)[] pairs) =>
        pairs.ToDictionary(pair => pair.Name, pair => pair.Value);

    private static bool IsPrimeByTrialDivision(long n)
    {
        if (n < 2)
        {
            return false;
        }

        for (long d = 2; d * d <= n; d++)
        {
            if (n % d == 0)
            {
                return false;
            }
        }

        return true;
    }

    private static string SortedDigits(long n)
    {
        var digits = n.ToString().ToCharArray();
        Array.Sort(digits);
        return new string(digits);
    }

    [Fact]
    public void Runner_sees_one_general_solver_per_converted_problem()
    {
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem041()));
        Assert.Equal("words", ParameterizedSolver.Signature(new Problem042()));
        Assert.Equal("maxDigit", ParameterizedSolver.Signature(new Problem043()));
        Assert.Equal("after", ParameterizedSolver.Signature(new Problem045()));
        Assert.Equal("count", ParameterizedSolver.Signature(new Problem047()));
        Assert.Equal("limit, digits", ParameterizedSolver.Signature(new Problem048()));
        Assert.Equal("digits", ParameterizedSolver.Signature(new Problem049()));
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem050()));

        Assert.Equal(2143, ParameterizedSolver.Solve(new Problem041(), Arguments(("limit", "2_143"))));
        Assert.Equal(2, ParameterizedSolver.Solve(new Problem042(), Arguments(("words", "\"SKY\",\"SKIES\",\"A\""))));
        Assert.Equal(new BigInteger(40755), ParameterizedSolver.Solve(new Problem045(), Arguments(("after", "1"))));
        Assert.Equal("0405071317", ParameterizedSolver.Solve(new Problem048(), Arguments(("limit", "10"), ("digits", "10"))));
        Assert.Equal(22212L, ParameterizedSolver.Solve(new Problem043(), Arguments(("maxDigit", "3"))));
        Assert.Equal(644, ParameterizedSolver.Solve(new Problem047(), Arguments(("count", "3"))));
        Assert.Equal(41, ParameterizedSolver.Solve(new Problem050(), Arguments(("limit", "100"))));

        var sequences = Assert.IsAssignableFrom<IReadOnlyList<string>>(ParameterizedSolver.Solve(new Problem049(), Arguments(("digits", "4"))));
        Assert.Equal(["148748178147", "296962999629"], sequences);
    }

    // ---- Problem 41 ----

    private static bool IsPandigital(int n) => SortedDigits(n) == "123456789"[..n.ToString().Length];

    private static int LargestPandigitalPrimeByScanningDown(int limit)
    {
        for (var n = Math.Min(limit, 987_654_321); n >= 1; n--)
        {
            if (IsPandigital(n) && IsPrimeByTrialDivision(n))
            {
                return n;
            }
        }

        return 0;
    }

    [Fact]
    public void Problem041_matches_the_statement_example()
    {
        // "2143 is a 4-digit pandigital and is also prime."
        Assert.Equal(2143, Problem041.Solve(2143));
        Assert.Equal(1423, Problem041.Solve(2142)); // The only smaller pandigital prime.
    }

    [Fact]
    public void Problem041_matches_brute_force_for_every_small_limit()
    {
        var pandigitalPrimes = Enumerable.Range(1, 100_000).Where(n => IsPandigital(n) && IsPrimeByTrialDivision(n)).ToArray();
        Assert.Equal(1423, pandigitalPrimes[0]);

        for (var limit = 0; limit <= 100_000; limit += limit < 5000 ? 1 : 37)
        {
            var expected = pandigitalPrimes.LastOrDefault(prime => prime <= limit);
            if (expected == 0)
            {
                var noAnswer = limit;
                Assert.Throws<InvalidOperationException>(() => Problem041.Solve(noAnswer));
            }
            else
            {
                Assert.Equal(expected, Problem041.Solve(limit));
            }
        }
    }

    [Theory]
    [InlineData(1_234_566)] // Just below the smallest 7-digit pandigital, so the answer drops back to four digits.
    [InlineData(1_234_567)]
    [InlineData(2_000_000)]
    [InlineData(7_652_412)]
    [InlineData(7_652_413)]
    [InlineData(7_654_321)]
    [InlineData(10_000_000)] // An 8-digit limit: no 8-digit pandigital is prime.
    public void Problem041_matches_a_downward_scan_for_larger_limits(int limit) =>
        Assert.Equal(LargestPandigitalPrimeByScanningDown(limit), Problem041.Solve(limit));

    [Fact]
    public void Problem041_accepts_limits_beyond_the_largest_pandigital() =>
        Assert.Equal(Problem041.Solve(987_654_321), Problem041.Solve(int.MaxValue));

    [Fact]
    public void Problem041_validates_its_limit()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem041.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem041.Solve(int.MinValue));
        Assert.Throws<InvalidOperationException>(() => Problem041.Solve(0));
        Assert.Throws<InvalidOperationException>(() => Problem041.Solve(1422));
    }

    // ---- Problem 42 ----

    [Fact]
    public void Problem042_matches_the_statement_example()
    {
        // "The word value for SKY is 19 + 11 + 25 = 55 = t10", and the first triangle numbers are
        // 1, 3, 6, 10, 15, 21, 28, 36, 45, 55.
        Assert.Equal(1, Problem042.Solve(["SKY"]));
        Assert.Equal(7, Problem042.Solve(["A", "B", "C", "AB", "BA", "F", "Z", "J", "SKY", "SKIES"])); // All but B = 2, Z = 26 and SKIES = 63.
        Assert.Equal(0, Problem042.Solve([]));
        Assert.Equal(3, Problem042.Solve(["SKY", "SKY", "SKY"]));
    }

    [Fact]
    public void Problem042_matches_brute_force_on_random_words()
    {
        // More words than the statement's file (about 1,800), and far longer ones.
        var random = new Random(42);
        var words = Enumerable.Range(0, 20_000)
            .Select(_ => new string(Enumerable.Range(0, random.Next(1, 40)).Select(_ => (char)('A' + random.Next(26))).ToArray()))
            .ToArray();

        var triangles = new HashSet<int>();
        for (int n = 1, triangle = 1; triangle <= 26 * 40; n++, triangle += n)
        {
            triangles.Add(triangle);
        }

        var expected = words.Count(word => triangles.Contains(word.Sum(letter => " ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter))));
        Assert.InRange(expected, 1, words.Length - 1);
        Assert.Equal(expected, Problem042.Solve(words));
    }

    [Fact]
    public void Problem042_handles_very_long_words()
    {
        // 2,001,000 is the 2000th triangle number.
        Assert.Equal(1, Problem042.Solve([new string('A', 2_001_000), new string('A', 2_001_001), new string('B', 2_001_000)]));
    }

    [Fact]
    public void Problem042_rejects_malformed_words()
    {
        Assert.Throws<ArgumentException>(() => Problem042.Solve(["SKY", "sky"]));
        Assert.Throws<ArgumentException>(() => Problem042.Solve(["SKY", ""]));
        Assert.Throws<ArgumentException>(() => Problem042.Solve(["TWO WORDS"]));
        Assert.Throws<ArgumentException>(() => Problem042.Solve(["A1"]));
        Assert.Throws<ArgumentException>(() => Problem042.Solve(["É"]));
        Assert.Throws<ArgumentNullException>(() => Problem042.Solve(null!));
    }

    // ---- Problem 43 ----

    private static IEnumerable<string> Arrangements(string prefix, string unused)
    {
        if (unused.Length == 0)
        {
            yield return prefix;
        }

        foreach (var digit in unused)
        {
            foreach (var arrangement in Arrangements(prefix + digit, unused.Replace(digit.ToString(), string.Empty)))
            {
                yield return arrangement;
            }
        }
    }

    [Fact]
    public void Problem043_recognises_the_statement_example()
    {
        // "The number, 1406357289, ... has a rather interesting sub-string divisibility property":
        // 406, 063, 635, 357, 572, 728 and 289 are divisible by 2, 3, 5, 7, 11, 13 and 17.
        Assert.True(Problem043.HasProperty([1, 4, 0, 6, 3, 5, 7, 2, 8, 9]));
        Assert.False(Problem043.HasProperty([1, 4, 0, 6, 3, 5, 7, 2, 9, 8])); // 298 is not divisible by 17.
        Assert.False(Problem043.HasProperty([1, 4, 0, 3, 6, 5, 7, 2, 8, 9])); // 403 is odd.
    }

    [Fact]
    public void Problem043_sums_the_four_digit_case_by_hand()
    {
        // With digits 0..3 the only window is d2d3d4, which is even when d4 is 0 or 2.
        // d4 = 0: the six orders of 1, 2, 3 in front, 10 × (6 × 222) = 13320.
        // d4 = 2: the six orders of 0, 1, 3 in front (leading zeros included), 10 × 888 + 6 × 2 = 8892.
        Assert.Equal(13320 + 8892, Problem043.Solve(3));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Problem043_matches_brute_force(int maxDigit)
    {
        int[] primes = [2, 3, 5, 7, 11, 13, 17];
        var expected = Arrangements(string.Empty, "0123456789"[..(maxDigit + 1)])
            .Where(text => Enumerable.Range(1, text.Length - 3).All(i => int.Parse(text.Substring(i, 3)) % primes[i - 1] == 0))
            .Sum(long.Parse);

        Assert.Equal(expected, Problem043.Solve(maxDigit));
    }

    [Fact]
    public void Problem043_validates_its_largest_digit()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem043.Solve(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem043.Solve(10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem043.Solve(-9));
    }

    // ---- Problem 45 ----

    private static BigInteger SquareRoot(BigInteger n)
    {
        if (n < 2)
        {
            return n;
        }

        // Newton's iteration from a starting point at or above the root.
        var x = BigInteger.One << (int)((n.GetBitLength() + 1) / 2);
        while (true)
        {
            var next = (x + n / x) >> 1;
            if (next >= x)
            {
                return x;
            }

            x = next;
        }
    }

    // x = n(n + 1)/2 exactly when 8x + 1 is the square of 2n + 1.
    private static bool IsTriangular(BigInteger x)
    {
        var root = SquareRoot(8 * x + 1);
        return root * root == 8 * x + 1 && root % 2 == 1 && root >= 3;
    }

    // x = n(3n − 1)/2 exactly when 24x + 1 is the square of 6n − 1.
    private static bool IsPentagonal(BigInteger x)
    {
        var root = SquareRoot(24 * x + 1);
        return root * root == 24 * x + 1 && root % 6 == 5;
    }

    // x = n(2n − 1) exactly when 8x + 1 is the square of 4n − 1.
    private static bool IsHexagonal(BigInteger x)
    {
        var root = SquareRoot(8 * x + 1);
        return root * root == 8 * x + 1 && root % 4 == 3;
    }

    [Fact]
    public void Problem045_matches_the_statement_example()
    {
        // "T285 = P165 = H143 = 40755", and T1 = P1 = H1 = 1 is the only smaller one.
        Assert.Equal(1, Problem045.Solve(0));
        Assert.Equal(40755, Problem045.Solve(1));
        Assert.Equal(40755, Problem045.Solve(40754));
        Assert.Equal(285 * 286 / 2, (int)Problem045.Solve(1));
        Assert.Equal(165 * (3 * 165 - 1) / 2, (int)Problem045.Solve(1));
        Assert.Equal(143 * (2 * 143 - 1), (int)Problem045.Solve(1));
    }

    [Fact]
    public void Problem045_matches_brute_force_over_every_number_up_to_two_billion()
    {
        // Build the three families from their formulas and intersect them.
        const long bound = 2_000_000_000;
        var triangular = new HashSet<long>();
        var pentagonal = new HashSet<long>();
        var common = new List<long>();
        for (long n = 1; n * (n + 1) / 2 <= bound; n++)
        {
            triangular.Add(n * (n + 1) / 2);
        }

        for (long n = 1; n * (3 * n - 1) / 2 <= bound; n++)
        {
            pentagonal.Add(n * (3 * n - 1) / 2);
        }

        for (long n = 1; n * (2 * n - 1) <= bound; n++)
        {
            var hexagonal = n * (2 * n - 1);
            if (triangular.Contains(hexagonal) && pentagonal.Contains(hexagonal))
            {
                common.Add(hexagonal);
            }
        }

        Assert.Equal(3, common.Count);
        long[] bounds = [0, 1, 2, 17, 40_754, 40_755, 40_756, 1_000_000, 1_533_776_804];
        foreach (var after in bounds)
        {
            Assert.Equal(common.First(number => number > after), Problem045.Solve(after));
        }
    }

    [Fact]
    public void Problem045_misses_no_hexagonal_number_up_to_the_fourth_answer()
    {
        // Every hexagonal number up to 6 × 10^13 that is also pentagonal, by exact long arithmetic
        // (24x + 1 stays below 2^53, so the floating-point root is within one of the true root).
        var common = new List<long>();
        for (long n = 1; n * (2 * n - 1) <= 60_000_000_000_000; n++)
        {
            var hexagonal = n * (2 * n - 1);
            var square = 24 * hexagonal + 1;
            var root = (long)Math.Sqrt(square);
            root += root * root > square ? -1 : (root + 1) * (root + 1) <= square ? 1 : 0;
            if (root * root == square && root % 6 == 5)
            {
                common.Add(hexagonal);
            }
        }

        BigInteger after = 0;
        foreach (var expected in common)
        {
            Assert.True(IsTriangular(expected));
            after = Problem045.Solve(after);
            Assert.Equal(expected, after);
        }

        Assert.Equal(4, common.Count);
        Assert.True(Problem045.Solve(after) > 60_000_000_000_000);
    }

    [Fact]
    public void Problem045_keeps_going_past_the_range_of_a_long()
    {
        // OEIS A046180: 1, 40755, 1533776805, 57722156241751, 2172315626468283465, 81752926228785223683195, ...
        Assert.Equal(BigInteger.Parse("2172315626468283465"), Problem045.Solve(57_722_156_241_751));
        Assert.Equal(BigInteger.Parse("81752926228785223683195"), Problem045.Solve(long.MaxValue));

        BigInteger after = 0;
        for (var i = 0; i < 40; i++)
        {
            var next = Problem045.Solve(after);
            Assert.True(next > after);
            Assert.True(IsTriangular(next) && IsPentagonal(next) && IsHexagonal(next), $"{next} is not all three.");
            Assert.Equal(next, Problem045.Solve(next - 1));
            after = next;
        }

        Assert.True(after > BigInteger.Pow(10, 150));
    }

    [Fact]
    public void Problem045_rejects_a_negative_bound() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem045.Solve(-1));

    // ---- Problem 47 ----

    private static int DistinctPrimeFactors(int n)
    {
        var count = 0;
        for (var p = 2; p * p <= n; p++)
        {
            if (n % p == 0)
            {
                count++;
                while (n % p == 0)
                {
                    n /= p;
                }
            }
        }

        return n > 1 ? count + 1 : count;
    }

    [Fact]
    public void Problem047_matches_the_statement_examples()
    {
        // "The first two consecutive numbers to have two distinct prime factors are: 14 = 2 × 7, 15 = 3 × 5."
        Assert.Equal(14, Problem047.Solve(2));

        // "The first three consecutive numbers to have three distinct prime factors are: 644, 645, 646."
        Assert.Equal(644, Problem047.Solve(3));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void Problem047_matches_brute_force(int count)
    {
        var first = 2;
        while (!Enumerable.Range(first, count).All(n => DistinctPrimeFactors(n) == count))
        {
            first++;
        }

        Assert.Equal(first, Problem047.Solve(count));
    }

    [Fact]
    public void Problem047_validates_its_count()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem047.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem047.Solve(-4));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem047.Solve(5));
    }

    // ---- Problem 48 ----

    [Fact]
    public void Problem048_matches_the_statement_example()
    {
        // "The series, 1^1 + 2^2 + 3^3 + ... + 10^10 = 10405071317."
        Assert.Equal("10405071317", Problem048.Solve(10, 11));
        Assert.Equal("0405071317", Problem048.Solve(10, 10));
        Assert.Equal("7", Problem048.Solve(10, 1));
        Assert.Equal("000010405071317", Problem048.Solve(10, 15));
        Assert.Equal("1", Problem048.Solve(1, 1));
    }

    [Fact]
    public void Problem048_matches_the_exact_sum_for_small_limits()
    {
        int[] digitCounts = [1, 2, 5, 10, 18, 19, 20, 37, 150];
        BigInteger sum = 0;
        for (var limit = 1; limit <= 80; limit++)
        {
            sum += BigInteger.Pow(limit, limit);
            foreach (var digits in digitCounts)
            {
                var expected = sum.ToString().PadLeft(digits, '0')[^digits..];
                Assert.Equal(expected, Problem048.Solve(limit, digits));
            }
        }
    }

    [Fact]
    public void Problem048_matches_repeated_multiplication_beyond_the_statement_limit()
    {
        // Nine digits keep every product below 10^18, so plain 64-bit arithmetic is exact.
        const int limit = 5000;
        const ulong modulus = 1_000_000_000;
        ulong sum = 0;
        for (ulong n = 1; n <= limit; n++)
        {
            ulong power = 1;
            for (ulong i = 0; i < n; i++)
            {
                power = power * n % modulus;
            }

            sum = (sum + power) % modulus;
        }

        Assert.Equal(sum.ToString("D9"), Problem048.Solve(limit, 9));
    }

    [Fact]
    public void Problem048_pads_to_the_largest_digit_count()
    {
        var text = Problem048.Solve(3, 1000); // 1 + 4 + 27
        Assert.Equal(new string('0', 998) + "32", text);
    }

    [Fact]
    public void Problem048_matches_the_exact_sum_at_the_largest_digit_count()
    {
        // The whole sum up to 1000^1000 has 3001 digits, so a thousand of them is a real truncation.
        BigInteger sum = 0;
        for (var n = 1; n <= 1000; n++)
        {
            sum += BigInteger.Pow(n, n);
        }

        var text = sum.ToString();
        Assert.Equal(3001, text.Length);
        Assert.Equal(text[^1000..], Problem048.Solve(1000, 1000));
        Assert.Equal(text[^999..], Problem048.Solve(1000, 999));
        Assert.Equal(text[^10..], Problem048.Solve(1000, 10));
    }

    [Theory]
    [InlineData(100_000, 10, "3031782500")]
    [InlineData(300_000, 9, "708622500")]
    [InlineData(20_000, 50, "39559167174122199552621688721540922613012601046500")]
    public void Problem048_matches_an_independent_implementation_for_large_limits(int limit, int digits, string expected) =>
        Assert.Equal(expected, Problem048.Solve(limit, digits)); // Expected values come from a separate arbitrary-precision program.

    [Fact]
    public void Problem048_rejects_combinations_that_would_run_for_hours()
    {
        // Each maximum is fine on its own, but limit × digits² may not exceed 10^11.
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(10_000_000, 1000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(10_000_000, 101));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(100_001, 1000));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(1_000_000, 317));
    }

    [Fact]
    public void Problem048_validates_its_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(0, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(-1, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(10_000_001, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(10, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(10, -3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem048.Solve(10, 1001));
    }

    // ---- Problem 49 ----

    private static List<string> PrimePermutationSequencesByBruteForce(int digits)
    {
        var upper = (int)BigInteger.Pow(10, digits);
        var isPrime = Enumerable.Range(0, upper).Select(n => IsPrimeByTrialDivision(n)).ToArray();
        var primes = Enumerable.Range(upper / 10, upper - upper / 10).Where(n => isPrime[n]).ToArray();
        var keys = primes.ToDictionary(prime => prime, prime => SortedDigits(prime));

        var sequences = new List<string>();
        foreach (var first in primes)
        {
            foreach (var second in primes)
            {
                var third = second + (second - first);
                if (second > first && third < upper && isPrime[third] && keys[first] == keys[second] && keys[first] == keys[third])
                {
                    sequences.Add($"{first}{second}{third}");
                }
            }
        }

        return [.. sequences.OrderBy(BigInteger.Parse)];
    }

    private static void AssertIsPrimePermutationSequence(string sequence, int digits)
    {
        Assert.Equal(3 * digits, sequence.Length);
        var terms = Enumerable.Range(0, 3).Select(i => int.Parse(sequence.Substring(i * digits, digits))).ToArray();
        Assert.All(terms, term => Assert.Equal(digits, term.ToString().Length));
        Assert.All(terms, term => Assert.True(IsPrimeByTrialDivision(term), $"{term} is not prime."));
        Assert.All(terms, term => Assert.Equal(SortedDigits(terms[0]), SortedDigits(term)));
        Assert.True(terms[0] < terms[1]);
        Assert.Equal(terms[1] - terms[0], terms[2] - terms[1]);
    }

    [Fact]
    public void Problem049_matches_the_statement_example()
    {
        // "The arithmetic sequence, 1487, 4817, 8147 ... There are no arithmetic sequences made up of three 1-, 2-,
        // or 3-digit primes, exhibiting this property, but there is one other 4-digit increasing sequence."
        var sequences = Problem049.Solve(4);
        Assert.Equal(2, sequences.Count);
        Assert.Equal("148748178147", sequences[0]);
        AssertIsPrimePermutationSequence(sequences[1], 4);

        Assert.Empty(Problem049.Solve(1));
        Assert.Empty(Problem049.Solve(2));
        Assert.Empty(Problem049.Solve(3));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Problem049_matches_brute_force(int digits) =>
        Assert.Equal(PrimePermutationSequencesByBruteForce(digits), Problem049.Solve(digits));

    [Fact]
    public void Problem049_returns_valid_sequences_in_order_for_six_digits()
    {
        var sequences = Problem049.Solve(6);
        Assert.True(sequences.Count > Problem049.Solve(5).Count);
        Assert.All(sequences, sequence => AssertIsPrimePermutationSequence(sequence, 6));
        for (var i = 1; i < sequences.Count; i++)
        {
            Assert.True(long.Parse(sequences[i - 1]) < long.Parse(sequences[i]), $"Entry {i} is out of order or repeated.");
        }
    }

    [Theory]
    [InlineData(6, 828, "101603106103110603", "993493993943994393", "1990000FFCE6FF380B4D89A81A4193C8BF5D16930F21DCBF4981FFF22D34FDCB")]
    [InlineData(7, 16_520, "100156910510691100569", "998938199938819998381", "165A520269CC4B9D67A011C8A6F232D1452968D5CBC45D55499F4F23CF5C7725")]
    public void Problem049_finds_every_sequence_up_to_the_largest_digit_count(int digits, int count, string first, string last, string sha256)
    {
        // The expected lists come from a separate program that tries every pair of primes with that many digits,
        // which takes minutes at seven digits; the hash pins the complete list, comma-separated, in order.
        var sequences = Problem049.Solve(digits);
        Assert.Equal(count, sequences.Count);
        Assert.Equal(first, sequences[0]);
        Assert.Equal(last, sequences[^1]);
        Assert.Equal(sha256, Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(string.Join(',', sequences)))));
    }

    [Fact]
    public void Problem049_validates_its_digit_count()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem049.Solve(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem049.Solve(-4));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem049.Solve(8));
    }

    // ---- Problem 50 ----

    private static bool[] SieveBelow(int limit)
    {
        var isPrime = Enumerable.Repeat(true, limit).ToArray();
        for (var n = 0; n < limit; n++)
        {
            if (n < 2)
            {
                isPrime[n] = false;
            }
            else if (isPrime[n])
            {
                for (var multiple = 2L * n; multiple < limit; multiple += n)
                {
                    isPrime[multiple] = false;
                }
            }
        }

        return isPrime;
    }

    // Tries every run of consecutive primes whose sum stays below the limit: the longest prime sum wins, the smallest on ties.
    private static int LongestConsecutivePrimeSumByBruteForce(int limit, bool[] isPrime)
    {
        var primes = Enumerable.Range(0, limit).Where(n => isPrime[n]).ToArray();
        var bestLength = 0;
        var best = 0;
        for (var start = 0; start < primes.Length; start++)
        {
            long sum = 0;
            for (var end = start; end < primes.Length && sum + primes[end] < limit; end++)
            {
                sum += primes[end];
                var length = end - start + 1;
                if (isPrime[sum] && (length > bestLength || (length == bestLength && sum < best)))
                {
                    bestLength = length;
                    best = (int)sum;
                }
            }
        }

        return best;
    }

    [Fact]
    public void Problem050_matches_the_statement_examples()
    {
        // "The prime 41, can be written as the sum of six consecutive primes: 41 = 2 + 3 + 5 + 7 + 11 + 13.
        // This is the longest sum of consecutive primes that adds to a prime below one-hundred."
        Assert.Equal(41, Problem050.Solve(100));

        // "The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953."
        Assert.Equal(953, Problem050.Solve(1000));
    }

    [Fact]
    public void Problem050_handles_the_smallest_limits()
    {
        Assert.Equal(2, Problem050.Solve(3));
        Assert.Equal(2, Problem050.Solve(5)); // 2 and 3 tie with one term each; the smaller wins.
        Assert.Equal(5, Problem050.Solve(6)); // 2 + 3
        Assert.Equal(5, Problem050.Solve(17));
        Assert.Equal(17, Problem050.Solve(18)); // 2 + 3 + 5 + 7
    }

    [Fact]
    public void Problem050_matches_brute_force_for_every_small_limit()
    {
        var isPrime = SieveBelow(3000);
        for (var limit = 3; limit <= 3000; limit++)
        {
            Assert.Equal(LongestConsecutivePrimeSumByBruteForce(limit, isPrime), Problem050.Solve(limit));
        }
    }

    [Theory]
    [InlineData(250_000)]
    [InlineData(1_999_993)] // A prime limit.
    [InlineData(5_000_000)]
    public void Problem050_matches_brute_force_for_larger_limits(int limit) =>
        Assert.Equal(LongestConsecutivePrimeSumByBruteForce(limit, SieveBelow(limit)), Problem050.Solve(limit));

    [Theory]
    [InlineData(491, 379)] // 379 is the only prime below 491 with 15 terms.
    [InlineData(492, 379)] // 491 = 7 + 11 + ... + 61 has 15 terms too; the smaller prime wins.
    [InlineData(997_651, 978_037)] // Three primes share 539 terms once the statement's answer is excluded.
    [InlineData(997_652, 997_651)]
    [InlineData(10_000_000, 9_951_191)] // 9,964,597 also has 1587 terms.
    public void Problem050_returns_the_smallest_prime_when_several_tie(int limit, int expected) =>
        Assert.Equal(expected, Problem050.Solve(limit));

    [Fact]
    public void Problem050_handles_the_largest_limit()
    {
        // 4685 terms, found by trying every run of consecutive primes in a separate program.
        Assert.Equal(99_819_619, Problem050.Solve(100_000_000));
    }

    [Fact]
    public void Problem050_validates_its_limit()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem050.Solve(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem050.Solve(100_000_001));
        Assert.Throws<InvalidOperationException>(() => Problem050.Solve(0));
        Assert.Throws<InvalidOperationException>(() => Problem050.Solve(1));
        Assert.Throws<InvalidOperationException>(() => Problem050.Solve(2));
    }
}
