using System.Numerics;
using System.Text;
using ProjectEulerSolver.Problems;
using ProjectEulerSolver.Tools;
using Xunit;

namespace ProjectEulerSolver.Tests.Parameterized;

/// <summary>
/// The general solvers of problems 31 to 40, checked against the examples in the problem statements,
/// against brute force on small arguments, and on arguments beyond the ones the statements use.
/// </summary>
public class Problems031To040Tests
{
    private static Dictionary<string, string> Arguments(params (string Name, string Value)[] pairs) =>
        pairs.ToDictionary(pair => pair.Name, pair => pair.Value);

    [Fact]
    public void The_runner_sees_the_documented_parameters()
    {
        Assert.Equal("digits", ParameterizedSolver.Signature(new Problem032()));
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem035()));
        Assert.Equal("limit, radix", ParameterizedSolver.Signature(new Problem036()));
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem037()));
        Assert.Equal("digits", ParameterizedSolver.Signature(new Problem038()));
        Assert.Equal("maxPerimeter", ParameterizedSolver.Signature(new Problem039()));
        Assert.Equal("positions", ParameterizedSolver.Signature(new Problem040()));

        Assert.Equal(13, ParameterizedSolver.Solve(new Problem035(), Arguments(("limit", "100"))));
        Assert.Equal(25L, ParameterizedSolver.Solve(new Problem036(), Arguments(("limit", "10"), ("radix", "2")))); // 1 + 3 + 5 + 7 + 9
        Assert.Equal(new BigInteger(5), ParameterizedSolver.Solve(new Problem040(), Arguments(("positions", "1, 10, 100"))));
    }

    // ---- Problem 31: coin sums ----

    [Fact]
    public void Problem031_matches_a_recursive_count_for_small_targets()
    {
        int[] coins = [1, 2, 5, 10, 20, 50];
        for (var target = 0; target <= 60; target++)
        {
            Assert.Equal(new BigInteger(Ways(target, coins.Length - 1)), Problem031.Solve(target, coins));
        }

        // Either use the largest remaining coin once more, or never use it again.
        long Ways(int amount, int largest) =>
            amount == 0 ? 1
            : amount < 0 || largest < 0 ? 0
            : Ways(amount - coins[largest], largest) + Ways(amount, largest - 1);
    }

    [Theory]
    [InlineData(1000, "321335886")]
    [InlineData(10_000, "1133873304647601")]
    [InlineData(100_000, "10056050940818192726001")] // Far beyond a long.
    public void Problem031_beyond_the_statement_matches_an_independent_count(int target, string expected)
    {
        // Expected values come from a memoised recursion over the coins, largest first, run outside this repository.
        Assert.Equal(BigInteger.Parse(expected), Problem031.Solve(target, [1, 2, 5, 10, 20, 50, 100, 200]));
        Assert.Equal(BigInteger.Parse(expected), Problem031.Solve(target, [200, 5, 100, 1, 50, 20, 2, 10])); // Order is irrelevant.
    }

    [Fact]
    public void Problem031_with_coins_of_one_and_two_has_a_closed_form()
    {
        // The number of twos used can be anything from 0 to target / 2, and the ones make up the rest.
        foreach (var target in Enumerable.Range(0, 300).Concat([999_999, 1_000_000]))
        {
            Assert.Equal(new BigInteger(target / 2 + 1), Problem031.Solve(target, [1, 2]));
        }

        Assert.Equal(BigInteger.Zero, Problem031.Solve(7, [10, 3])); // 7 is not a multiple of 3 and 10 is too big.
        Assert.Equal(new BigInteger(2), Problem031.Solve(36, [4, 9])); // Nine fours, or four nines.
    }

    [Fact]
    public void Problem031_rejects_out_of_range_arguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem031.Solve(-1, [1, 2]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem031.Solve(5, [0, 1]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem031.Solve(5, [2, -1]));
    }

    // ---- Problem 32: pandigital products ----

    [Fact]
    public void Problem032_finds_the_identity_from_the_statement() =>
        Assert.Contains(7254, Problem032.PandigitalProducts(9)); // 39 × 186 = 7254

    [Theory]
    [InlineData(4, 12)] // 3 × 4 = 12 is the only identity.
    [InlineData(5, 52)] // 4 × 13 = 52 is the only identity.
    [InlineData(7, 0)] // No identity uses 1 to 7, and an empty sum is zero.
    public void Problem032_small_cases_by_hand(int digits, int expected) =>
        Assert.Equal(expected, Problem032.Solve(digits));

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void Problem032_matches_splitting_every_permutation(int digits)
    {
        var expected = new HashSet<int>();
        foreach (var permutation in Combinatorics.Permutations(Enumerable.Range(1, digits).ToArray()))
        {
            for (var timesAt = 1; timesAt < digits - 1; timesAt++)
            {
                for (var equalsAt = timesAt + 1; equalsAt < digits; equalsAt++)
                {
                    var product = Value(permutation, equalsAt, digits);
                    if (Value(permutation, 0, timesAt) * Value(permutation, timesAt, equalsAt) == product)
                    {
                        expected.Add((int)product);
                    }
                }
            }
        }

        Assert.Equal(expected.Order(), Problem032.PandigitalProducts(digits).Order());
        Assert.Equal(expected.Sum(), Problem032.Solve(digits));

        static long Value(int[] digitsOfIdentity, int from, int to)
        {
            long value = 0;
            for (var i = from; i < to; i++)
            {
                value = value * 10 + digitsOfIdentity[i];
            }

            return value;
        }
    }

    [Theory]
    [InlineData(4, 12)]
    [InlineData(5, 52)]
    [InlineData(6, 162)]
    [InlineData(7, 0)]
    [InlineData(8, 13_458)]
    [InlineData(9, 45_228)]
    public void Problem032_matches_an_independent_search_for_every_digit_count(int digits, int expected) =>
        Assert.Equal(expected, Problem032.Solve(digits)); // Sums found by splitting every permutation, outside this repository.

    [Theory]
    [InlineData(3)]
    [InlineData(10)]
    [InlineData(-1)]
    public void Problem032_rejects_digit_counts_outside_4_to_9(int digits) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem032.Solve(digits));

    // ---- Problem 35: circular primes ----

    [Fact]
    public void Problem035_matches_the_statement() =>
        Assert.Equal(13, Problem035.Solve(100)); // 2, 3, 5, 7, 11, 13, 17, 31, 37, 71, 73, 79 and 97.

    [Fact]
    public void Problem035_matches_rotating_every_prime()
    {
        const int size = 100_000; // Rotations of numbers below 10^5 stay below 10^5.
        var isPrime = Primes.Sieve(size);
        var circular = Enumerable.Range(0, size).Where(n => isPrime[n] && Rotations(n).All(r => isPrime[r])).ToArray();

        // Includes limits such as 20, where 13 and 17 count although 31 and 71 lie beyond the limit.
        foreach (var limit in Enumerable.Range(0, 1500).Concat([1931, 1932, 9999, 10_000, 19_937, 99_371, 99_372, size]))
        {
            Assert.Equal(circular.Count(p => p < limit), Problem035.Solve(limit));
        }

        static IEnumerable<int> Rotations(int n)
        {
            var text = n.ToString();
            return Enumerable.Range(0, text.Length).Select(shift => int.Parse(text[shift..] + text[..shift]));
        }
    }

    [Fact]
    public void Problem035_handles_the_largest_limit()
    {
        // The ten-digit candidates have rotations beyond the int range. No circular prime lies between
        // 999,331 and the 19-digit repunit (OEIS A068652), so the count stays at the 55 found below a million.
        Assert.Equal(55, Problem035.Solve(int.MaxValue));
    }

    [Theory]
    [InlineData(999_331, 54)]
    [InlineData(999_332, 55)] // 999,331 is the last one below the 19-digit repunit.
    [InlineData(10_000_000, 55)]
    public void Problem035_beyond_the_statement_matches_a_sieve_of_every_prime(int limit, int expected) =>
        Assert.Equal(expected, Problem035.Solve(limit)); // Counted by rotating every prime of a sieve up to 10^8, outside this repository.

    [Fact]
    public void Problem035_rejects_a_negative_limit() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem035.Solve(-1));

    // ---- Problem 36: double-base palindromes ----

    [Fact]
    public void Problem036_matches_the_statement() =>
        Assert.Equal(585, Problem036.Solve(586, 2) - Problem036.Solve(585, 2)); // 585 = 1001001001 in binary.

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void Problem036_matches_testing_every_number(int radix)
    {
        const int size = 30_000;
        var palindromes = Enumerable.Range(1, size - 1)
            .Where(n => Digits.IsPalindrome(n.ToString()) && Digits.IsPalindrome(InRadix(n, radix)))
            .ToArray();

        foreach (var limit in Enumerable.Range(0, 1200).Concat([9999, 10_000, 10_001, size]))
        {
            Assert.Equal(palindromes.Where(p => p < limit).Sum(p => (long)p), Problem036.Solve(limit, radix));
        }

        static string InRadix(int n, int radix)
        {
            var text = new StringBuilder();
            for (var rest = n; rest > 0; rest /= radix)
            {
                text.Insert(0, (char)('0' + rest % radix));
            }

            return text.ToString();
        }
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void Problem036_at_the_largest_limit_matches_building_the_palindromes_of_the_other_base(int radix)
    {
        // The solver builds decimal palindromes and tests them in the other base; this goes the other way round.
        const int limit = int.MaxValue;
        long expected = 0;
        for (long half = 1; ; half++)
        {
            var oddLength = Mirror(half / radix);
            if (oddLength >= limit)
            {
                break; // Odd-length palindromes grow with their half, and the even-length one is longer still.
            }

            var evenLength = Mirror(half);
            expected += Digits.IsPalindrome(oddLength.ToString()) ? oddLength : 0;
            expected += evenLength < limit && Digits.IsPalindrome(evenLength.ToString()) ? evenLength : 0;

            long Mirror(long tail)
            {
                var palindrome = half;
                for (; tail > 0; tail /= radix)
                {
                    palindrome = palindrome * radix + tail % radix;
                }

                return palindrome;
            }
        }

        Assert.Equal(expected, Problem036.Solve(limit, radix));
    }

    [Theory]
    [InlineData(1_000_000, 2, 872_187)]
    [InlineData(1_000_000, 3, 1_342_501)]
    [InlineData(1_000_000, 9, 782_868)]
    [InlineData(10_000_000, 7, 29_849_749)]
    [InlineData(20_000_000, 3, 44_192_979)]
    [InlineData(int.MaxValue, 2, 3_899_925_195L)]
    [InlineData(int.MaxValue, 9, 1_071_712_516)]
    public void Problem036_beyond_the_statement_matches_testing_every_number(int limit, int radix, long expected) =>
        Assert.Equal(expected, Problem036.Solve(limit, radix)); // Sums found by testing the digit lists of both bases, outside this repository.

    [Fact]
    public void Problem036_stops_exactly_at_the_limit()
    {
        // 2,147,447,412 is the largest decimal palindrome that fits an int; 585 and 717 are palindromes in base 2.
        Assert.Equal(Problem036.Solve(2_147_447_413, 2), Problem036.Solve(int.MaxValue, 2));
        Assert.Equal(585, Problem036.Solve(586, 2) - Problem036.Solve(585, 2));
        Assert.Equal(717, Problem036.Solve(718, 2) - Problem036.Solve(717, 2));
        Assert.Equal(0, Problem036.Solve(0, 2));
        Assert.Equal(0, Problem036.Solve(1, 2));
        Assert.Equal(1, Problem036.Solve(2, 2));
    }

    [Fact]
    public void Problem036_sums_may_exceed_an_int() =>
        Assert.True(Problem036.Solve(int.MaxValue, 2) > int.MaxValue);

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(1000, 1)]
    [InlineData(1000, 0)]
    [InlineData(1000, 10)]
    public void Problem036_rejects_out_of_range_arguments(int limit, int radix) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem036.Solve(limit, radix));

    // ---- Problem 37: truncatable primes ----

    [Fact]
    public void Problem037_matches_the_statement()
    {
        // 3797, 797, 97, 7 and 3797, 379, 37, 3 are all prime.
        Assert.Equal(3797, Problem037.Solve(3798) - Problem037.Solve(3797));

        // 2, 3, 5 and 7 are not considered truncatable; the first that is, is 23.
        Assert.Equal(0, Problem037.Solve(23));
        Assert.Equal(23, Problem037.Solve(24));
    }

    [Fact]
    public void Problem037_matches_truncating_every_prime()
    {
        const int size = 1_000_000;
        var isPrime = Primes.Sieve(size);
        var truncatable = Enumerable.Range(10, size - 10).Where(n => isPrime[n] && Truncations(n).All(t => isPrime[t])).ToArray();
        Assert.Equal(11, truncatable.Length); // "Only eleven", as the statement says.

        foreach (var limit in Enumerable.Range(0, 4000).Concat([100_000, 739_397, 739_398, size]))
        {
            Assert.Equal(truncatable.Where(p => p < limit).Sum(p => (long)p), Problem037.Solve(limit));
        }

        // The search runs out of right-truncatable primes (the largest is 73,939,133) well below the largest limit.
        Assert.Equal(truncatable.Sum(p => (long)p), Problem037.Solve(int.MaxValue));

        static IEnumerable<int> Truncations(int n)
        {
            var text = n.ToString();
            return Enumerable.Range(1, text.Length - 1).SelectMany(cut => new[] { int.Parse(text[cut..]), int.Parse(text[..cut]) });
        }
    }

    [Fact]
    public void Problem037_counts_each_of_the_eleven_exactly_at_its_own_value()
    {
        // The eleven found by truncating every prime of a sieve up to 10^8, outside this repository.
        int[] known = [23, 37, 53, 73, 313, 317, 373, 797, 3137, 3797, 739_397];
        long sum = 0;
        foreach (var prime in known)
        {
            Assert.Equal(sum, Problem037.Solve(prime)); // The limit is exclusive.
            sum += prime;
            Assert.Equal(sum, Problem037.Solve(prime + 1));
        }

        Assert.Equal(748_317, sum);
        Assert.Equal(sum, Problem037.Solve(100_000_000));
    }

    [Fact]
    public void Problem037_rejects_a_negative_limit() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem037.Solve(-1));

    // ---- Problem 38: pandigital multiples ----

    [Fact]
    public void Problem038_matches_the_statement()
    {
        Assert.Equal(192_384_576, Problem038.ConcatenatedProduct(192, 9)); // 192 × (1, 2, 3)
        Assert.Equal(918_273_645, Problem038.ConcatenatedProduct(9, 9)); // 9 × (1, 2, 3, 4, 5)
        Assert.True(Problem038.Solve(9) >= 918_273_645);
    }

    [Theory]
    [InlineData(2, 12)] // 1 × (1, 2)
    [InlineData(3, 123)] // 1 × (1, 2, 3)
    public void Problem038_small_cases_by_hand(int digits, long expected) =>
        Assert.Equal(expected, Problem038.Solve(digits));

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void Problem038_matches_checking_pandigitals_from_the_largest_down(int digits)
    {
        // 1 × (1, ..., digits) is always a concatenated product, so every digit count has an answer.
        Assert.Equal(LargestByPermutation(), Problem038.Solve(digits));

        long LargestByPermutation()
        {
            // Permutations of descending digits come out in descending order, so the first hit is the largest.
            foreach (var permutation in Combinatorics.Permutations(Enumerable.Range(1, digits).Reverse().ToArray()))
            {
                var text = string.Concat(permutation);

                // A proper prefix as the integer guarantees at least two terms.
                for (var prefixLength = 1; prefixLength < digits; prefixLength++)
                {
                    var x = long.Parse(text[..prefixLength]);
                    var built = new StringBuilder();
                    for (var n = 1; built.Length < digits; n++)
                    {
                        built.Append(x * n);
                    }

                    if (built.ToString() == text)
                    {
                        return long.Parse(text);
                    }
                }
            }

            throw new InvalidOperationException($"No concatenated product is 1 to {digits} pandigital.");
        }
    }

    [Theory]
    [InlineData(2, 12)]
    [InlineData(3, 123)]
    [InlineData(4, 1234)]
    [InlineData(5, 12_345)]
    [InlineData(6, 123_456)]
    [InlineData(7, 1_234_567)]
    [InlineData(8, 78_156_234)] // 78 × (1, 2, 3)
    [InlineData(9, 932_718_654)] // 9327 × (1, 2)
    public void Problem038_matches_an_independent_search_for_every_digit_count(int digits, long expected)
    {
        // Largest values found by concatenating the multiples of every integer as text, outside this repository.
        Assert.Equal(expected, Problem038.Solve(digits));

        // The solver has no "no answer" case because 1 × (1, ..., digits) always qualifies.
        var ascending = Problem038.ConcatenatedProduct(1, digits);
        Assert.True(Digits.IsPandigital(ascending, digits));
        Assert.True(Problem038.Solve(digits) >= ascending);
    }

    [Fact]
    public void Problem038_rejects_concatenations_that_overshoot_the_length()
    {
        // 50 × (1, 2) is 50100: five digits, so 50 cannot give a four-digit answer.
        Assert.Equal(50_100, Problem038.ConcatenatedProduct(50, 4));
        Assert.Equal(78_156_234, Problem038.ConcatenatedProduct(78, 8));
        Assert.Equal(932_718_654, Problem038.ConcatenatedProduct(9327, 9));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(0)]
    public void Problem038_rejects_digit_counts_outside_2_to_9(int digits) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem038.Solve(digits));

    // ---- Problem 39: integer right triangles ----

    [Fact]
    public void Problem039_matches_the_statement()
    {
        // {20, 48, 52}, {24, 45, 51} and {30, 40, 50}.
        Assert.Equal(3, Problem039.TriangleCounts(120)[120]);
        Assert.Equal(12, Problem039.Solve(12)); // 3-4-5 is the only triangle.
    }

    [Fact]
    public void Problem039_matches_trying_every_pair_of_legs()
    {
        const int size = 500;
        var expected = new int[size + 1];
        for (var a = 1; a <= size; a++)
        {
            for (var b = a; a + b <= size; b++)
            {
                var hypotenuse = NumberTheory.ISqrt(a * a + b * b);
                if (hypotenuse * hypotenuse == a * a + b * b && a + b + hypotenuse <= size)
                {
                    expected[a + b + hypotenuse]++;
                }
            }
        }

        Assert.Equal(expected, Problem039.TriangleCounts(size));
        for (var maxPerimeter = 12; maxPerimeter <= size; maxPerimeter++)
        {
            Assert.Equal(FirstMaximum(expected, maxPerimeter), Problem039.Solve(maxPerimeter));
        }
    }

    [Fact]
    public void Problem039_beyond_the_statement_matches_solving_for_the_second_leg()
    {
        const int size = 10_000;
        var expected = Enumerable.Range(0, size + 1).Select(p => TrianglesWithPerimeter(p)).ToArray();
        Assert.Equal(FirstMaximum(expected, size), Problem039.Solve(size));
    }

    [Fact]
    public void Problem039_handles_the_largest_perimeter()
    {
        const int size = 10_000_000;
        var counts = Problem039.TriangleCounts(size);
        foreach (var perimeter in (int[])[12, 510_510, 720_720, 999_999, 1_000_000, 9_999_990, size])
        {
            Assert.Equal(TrianglesWithPerimeter(perimeter), counts[perimeter]);
        }

        var best = Problem039.Solve(size);
        Assert.Equal(FirstMaximum(counts, size), best);
        Assert.Equal(TrianglesWithPerimeter(best), counts[best]);
    }

    [Theory]
    [InlineData(23, 12)] // Only 3-4-5.
    [InlineData(59, 12)] // 12, 24, 30, 36, 40, 48 and 56 have one triangle each; the smallest wins the tie.
    [InlineData(60, 60)] // 15-20-25 and 10-24-26.
    [InlineData(1000, 840)]
    [InlineData(5000, 4620)]
    [InlineData(46_340, 27_720)]
    [InlineData(46_341, 27_720)] // p² no longer fits an int from here on.
    [InlineData(100_000, 55_440)]
    [InlineData(1_000_000, 720_720)]
    [InlineData(6_126_119, 4_084_080)]
    [InlineData(6_126_120, 6_126_120)]
    [InlineData(10_000_000, 6_126_120)]
    public void Problem039_matches_an_independent_divisor_count(int maxPerimeter, int expected)
    {
        // Expected values were computed outside this repository without Euclid's formula: a + b + c = p and
        // a² + b² = c² give (p - a)(p - b) = p² / 2, so the triangles of perimeter p are the divisors u of
        // p² / 2 with p / √2 < u < p.
        Assert.Equal(expected, Problem039.Solve(maxPerimeter));
    }

    [Theory]
    [InlineData(11)]
    [InlineData(0)]
    [InlineData(-12)]
    [InlineData(10_000_001)]
    public void Problem039_rejects_out_of_range_perimeters(int maxPerimeter) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem039.Solve(maxPerimeter));

    /// <summary>
    /// Counts the triangles of one perimeter: a + b + c = p and a² + b² = c² give b = p(p - 2a) / (2(p - a)),
    /// so each shortest side a &lt; p / 3 has at most one partner.
    /// </summary>
    private static int TrianglesWithPerimeter(long p)
    {
        var count = 0;
        for (long a = 1; 3 * a < p; a++)
        {
            var numerator = p * (p - 2 * a);
            var denominator = 2 * (p - a);
            if (numerator % denominator == 0 && numerator / denominator > a)
            {
                count++;
            }
        }

        return count;
    }

    private static int FirstMaximum(int[] counts, int maxPerimeter)
    {
        var best = 12;
        for (var p = 12; p <= maxPerimeter; p++)
        {
            best = counts[p] > counts[best] ? p : best;
        }

        return best;
    }

    // ---- Problem 40: Champernowne's constant ----

    [Fact]
    public void Problem040_matches_the_statement() =>
        Assert.Equal(BigInteger.One, Problem040.Solve([12])); // "the 12th digit of the fractional part is 1"

    [Fact]
    public void Problem040_matches_the_written_out_constant()
    {
        // Ten million digits: ten times as far as the statement goes, and across six changes of number length.
        const int size = 10_000_000;
        var fraction = new StringBuilder(size + 8);
        for (var n = 1; fraction.Length < size; n++)
        {
            fraction.Append(n);
        }

        int[] blockEnds = [38_889, 38_890, 488_889, 488_890, 5_888_889, 5_888_890]; // Where the 4-, 5- and 6-digit numbers end.
        var positions = Enumerable.Range(1, 30_000)
            .Concat(blockEnds)
            .Concat(Enumerable.Range(0, 2000).Select(i => 1 + (int)(i * 4_999L % size)))
            .Concat([size - 1, size]);
        foreach (var position in positions)
        {
            Assert.Equal(new BigInteger(fraction[position - 1] - '0'), Problem040.Solve([position]));
        }

        Assert.Equal(new BigInteger(120), Problem040.Solve([1, 2, 3, 4, 5]));
        Assert.Equal(new BigInteger(81), Problem040.Solve([9, 9])); // A repeated position counts each time.
        Assert.Equal(BigInteger.Zero, Problem040.Solve([9, 11])); // The 11th digit is the 0 of 10.
    }

    [Fact]
    public void Problem040_handles_the_largest_positions()
    {
        // The numbers of one to eight digits fill 788,888,889 positions. The billionth digit is therefore
        // number 211,111,111 of the nine-digit block, the first digit of 100,000,000 + 23,456,790.
        Assert.Equal(new BigInteger(1), Problem040.Solve([1_000_000_000]));

        // Likewise position 2,147,483,647 is the first digit of 100,000,000 + 150,954,973.
        Assert.Equal(new BigInteger(2), Problem040.Solve([int.MaxValue]));
    }

    [Theory]
    [InlineData(68_888_889, 9)] // Last digit of 9,999,999.
    [InlineData(68_888_890, 1)] // First digit of 10,000,000.
    [InlineData(788_888_889, 9)] // Last digit of 99,999,999.
    [InlineData(788_888_890, 1)] // First digit of 100,000,000.
    [InlineData(788_888_898, 0)] // Last digit of 100,000,000.
    [InlineData(788_888_899, 1)] // First digit of 100,000,001.
    [InlineData(999_999_999, 9)] // Last digit of 123,456,789.
    [InlineData(1_234_567_890, 8)] // Inside 149,519,888.
    [InlineData(2_000_000_000, 3)] // Inside 234,567,901.
    [InlineData(2_147_483_646, 2)] // Last digit of 250,954,972.
    public void Problem040_large_positions_match_an_independent_search(int position, int expected) =>
        Assert.Equal(new BigInteger(expected), Problem040.Solve([position])); // Found by bisecting on the digits written by 1..n, outside this repository.

    [Fact]
    public void Problem040_products_may_exceed_a_long() =>
        Assert.Equal(BigInteger.Pow(9, 25), Problem040.Solve(Enumerable.Repeat(9, 25).ToArray()));

    [Fact]
    public void Problem040_rejects_malformed_positions()
    {
        Assert.Throws<ArgumentException>(() => Problem040.Solve([]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem040.Solve([0]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem040.Solve([1, 10, -100]));
    }
}
