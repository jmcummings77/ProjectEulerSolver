using ProjectEulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Tools
{
    public class Number
    {
        public Int64 Value { get; set; }
        public Number(Int64 _Value)
        {
            Value = _Value;
        }
        public List<int> GetPrimeFactors()
        {
            Int64 n = Value;
            var PrimeFactors = new List<int>();
            if (n % 2 == 0)
            {
                while (n % 2 == 0)
                {
                    PrimeFactors.Add(2);
                    n = n / 2;
                }
            }
            for (int i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                if (n % i == 0)
                {
                    while (n % i == 0)
                    {
                        PrimeFactors.Add(i);
                        n = n / i;
                    }
                }
            }
            if(n > 2)
            {
                PrimeFactors.Add((int)n);
            }
            return PrimeFactors;
        }
        public BigInteger GetSumOfDigitsToNthPower(int N)
        {
            BigInteger result = 0;
            var digits = ToBigIntegerList();
            foreach(BigInteger digit in digits)
            {
                result += BigInteger.Pow(digit, N);
            }
            return result;
        }
        public List<int> GetProperDivisors()
        {
            var result = new List<int>();
            result.Add(1);
            for (int i = 2; i < Math.Sqrt(ToInt()) + 1; i++)
            {
                if (ToInt()%i==0)
                {
                    if (ToInt()/i == i)
                    {
                        result.Add(i);
                    }
                    else 
                    {
                        result.Add(i);
                        result.Add(ToInt()/i);
                    }
                }
            }
            return result.Distinct().ToList();
        }
        public string IsPerfectAbundantOrDeficient()
        {
            Int64 divisorSum = GetSumOfDivisors();
            if(divisorSum == Value)
            {
                return "Perfect";
            }
            else if (divisorSum > Value)
            {
                return "Abundant";
            }
            else
            {
                return "Deficient";
            }
        }
        public bool IsAbundant()
        {
            return GetSumOfDivisors() > Value;
        }
        public bool IsSumOfAbundantNumbers()
        {
            if(Value > 28123)
            {
                return true;
            }
            else
            {

            }


            return false;
        }
        public Int64 GetSumOfDivisors()
        {
            Int64 result = 0;
            foreach(int i in GetProperDivisors())
            {
                result += i;
            }
            return result;
        }
        public double GetApproximateSquareRoot()
        {
            return Math.Pow(Math.E, BigInteger.Log(Value) / 2);
        }
        public BigInteger GetFactorial()
        {
            BigInteger n = (BigInteger)Value - 1;
            BigInteger result = (BigInteger)Value;
            while(n > 0)
            {
                result *= n;
                n--;
            }
            return result;
        }
        public BigInteger SumEachDigitInValue()
        {
            var result = new BigInteger();
            result = 0;
            var digits = ToIntArray();
            foreach(int i in digits)
            {
                result += i;
            }
            return result;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
        public char[] ToCharArray()
        {
            string value = Value.ToString();
            return Value.ToString().ToCharArray(0, value.Length);
        }
        public List<char> ToCharList()
        {
            string value = Value.ToString();
            char[] array = Value.ToString().ToCharArray(0, value.Length);
            return new List<char>(array);
        }
        public int[] ToIntArray()
        {
            var values = new List<int>();
            string value = Value.ToString();
            for (int i = 0; i < value.Length; i++)
            {
                values.Add(int.Parse(value.Substring(i, 1)));
            }
            return values.ToArray();
        }
       
        public List<int> ToIntList()
        {
            var values = new List<int>();
            string value = Value.ToString();
            for(int i = 0; i < value.Length; i++)
            {
                values.Add(int.Parse(value.Substring(i,1)));
            }
            return values;
        }
        public Int64[] ToInt64Array()
        {
            var values = new List<Int64>();
            string value = Value.ToString();
            for(int i = 0; i < value.Length; i++)
            {
                values.Add(Int64.Parse(value.Substring(i,1)));
            }
            return values.ToArray();
        }
        public List<Int64> ToInt64List()
        {
            var values = new List<Int64>();
            string value = Value.ToString();
            for(int i = 0; i < value.Length; i++)
            {
                values.Add(Int64.Parse(value.Substring(i,1)));
            }
            return values;
        }
        public BigInteger[] ToBigIntegerArray()
        {
            var values = new List<BigInteger>();
            string value = Value.ToString();
            for(int i = 0; i < value.Length; i++)
            {
                values.Add(BigInteger.Parse(value.Substring(i,1)));
            }
            return values.ToArray();
        }
        public List<BigInteger> ToBigIntegerList()
        {
            var values = new List<BigInteger>();
            string value = Value.ToString();
            for(int i = 0; i < value.Length; i++)
            {
                values.Add(BigInteger.Parse(value.Substring(i,1)));
            }
            return values;
        }
        public int ToInt()
        {
            if(CanConvertToInt())
            {
                return (int)Value;
            }
            else
            {
                throw new OverflowException();
            }
        }
        public bool CanConvertToInt()
        {
            return Value < Int32.MaxValue;
        }
        public bool IsEven()
        {
            return Value % 2 == 0;
        }
        public bool IsOdd()
        {
            return !IsEven();
        }
        public List<Tuple<int, int>> GetFactorPairs()
        {
            var factorPairs = new List<Tuple<int, int>>();

            factorPairs.Add(new Tuple<int, int>(1, ToInt()));

            for (int i = 2; i < Math.Sqrt(ToInt()) + 1; i++)
            {
                if (ToInt() % i == 0)
                {
                    factorPairs.Add(new Tuple<int, int>(i, ToInt()/i));
                }
            }

            return factorPairs;
        }
        public BigInteger ToBigInteger()
        {
            return (Int64)Value;
        }
        public bool IsPrime()
        {
            if(Value < 1)
            {
                return false;
            }
            if(Value < 4)
            {
                return true;
            }
            var primes = GetPrimeFactors();
            foreach(int factor in primes)
            {
                if (factor != 1 && factor != (int)Value)
                {
                    return false;
                }
            }
            return true;
        }
        public string ToWords()
        {
            if(Value > 999999999)
            {
                throw new InvalidCastException();
            }
            else
            {
                return NumberToWords(Value);
            }
        }
        private string NumberToWords(BigInteger number)
        {
            
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(BigInteger.Abs(number));

            string words = "";

            if ((number / 100000000) > 0)
            {
                words += NumberToWords(number / 100000000) + " trillion ";
                number %= 100000000;
            }

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " billion ";
                number %= 10000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                {
                    words += "and ";
                }

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                {
                    words += unitsMap[(int)number];
                }
                else
                {
                    words += tensMap[(int)number / 10];
                    if ((number % 10) > 0)
                    {
                        words += "-" + unitsMap[(int)number % 10];
                    }
                }
            }
            return words;
        }
        private static void Swap(ref int a, ref int b)
        {
            if (a == b)
            {
                return;
            }

            a ^= b;
            b ^= a;
            a ^= b;
        }

        private void GetPermutations(int[] list)
        {
            int x = list.Length - 1;
            GetPermutations(list, 0, x);
        }
        public List<long> Permutations { get; set; }
        private void GetPermutations(int[] list, int recursionDepth, int maxDepth)
        {
            if (recursionDepth == maxDepth)
            {
                string result = "";
                foreach(int i in list)
                {
                    result += i.ToString();
                }
                Permutations.Add(Int64.Parse(result));
            }
            else
            {
                for (int i = recursionDepth; i <= maxDepth; i++)
                {
                    Swap(ref list[recursionDepth], ref list[i]);
                    GetPermutations(list, recursionDepth + 1, maxDepth);
                    Swap(ref list[recursionDepth], ref list[i]);
                }
            }
        }
        public List<long> GetAllDigitPermutations()
        {
            Permutations = new List<long>();
            GetPermutations(ToIntArray());
            return Permutations;
        }
        public int Length()
        {
            return Value.ToString().Length;
        }
        public List<long> GetAllRotations()
        {
            var result = new List<long>();
            int length = Length();
            int[] array = ToIntArray();
            for (int i = 0; i < length; i++)
            {
                string digits = "";
                for (int j = 0; j < length; j++)
                {
                    digits += array[j].ToString();
                }
                result.Add(Int64.Parse(digits));
                array = LeftRotateArray(array);
            }


            return result;
        }
        private int[] LeftRotateArray(int[] array)
        {
            int[] tempArray = new int[array.Length];
            tempArray[array.Length - 1] = array[0];
            for (int i = 0; i < array.Length - 1; i++)
            {
                tempArray[i] = array[i + 1];
            }

            return tempArray;
        }
        public string ToBinary()
        {
            return Convert.ToString(Convert.ToInt32(ToString(), 10), 2);
        }
        public string ToBase2()
        {
            return Convert.ToString(Convert.ToInt32(ToString(), 10), 2);
        }
        public string ToBase8()
        {
            return Convert.ToString(Convert.ToInt32(ToString(), 10), 8);
        }
        public string ToBase16()
        {
            return Convert.ToString(Convert.ToInt32(ToString(), 10), 16);
        }
        public string ToBase10(string NonDecimalNumber, int FromBase)
        {
            if (FromBase == 2 || FromBase == 8 || FromBase == 16)
            {
                return Convert.ToString(Convert.ToInt32(ToString(), FromBase), 10);
            }
            else
            {
                throw new NotSupportedException("Base not supported. Only Base 2, 8, and 16 are supported.");
            }
        }
        public bool IsPalindrome(int InBase)
        {
            switch(InBase)
            {
                case 2:
                    return ToBase2().SequenceEqual(ToBase2().Reverse());
                case 8:
                    return ToBase8().SequenceEqual(ToBase8().Reverse());
                case 10:
                    return ToString().SequenceEqual(ToString().Reverse());
                case 16:
                    return ToBase16().SequenceEqual(ToBase16().Reverse());
                default:
                    throw new NotSupportedException("Base not supported. Only Base 2, 8, and 16 are supported.");
            }
        }
        public bool IsPanDigital()
        {
            int length = Length();
            if(length > 9)
            {
                return false;
            }
            List<int> digits = ToIntList();
            if(digits.Distinct().Count() != digits.Count())
            {
                return false;
            }
            for(int i = 1; i <= length; i++)
            {
                if(!digits.Contains(i))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsTriangleNumber()
        {
            double n = Math.Sqrt(8 * Value + 1);
            return (Math.Floor(n) == n);
        }
        public bool IsPentagonNumber()
        {
            double n = (1 + Math.Sqrt(24 * Value + 1)) / 6;
            return (Math.Floor(n) == n);
        }
        public bool IsHexagonNumber()
        {
            double n = (1 + Math.Sqrt(8 * Value + 1)) / 4;
            return (Math.Floor(n) == n);
        }
        public bool IsHeptagonNumber()
        {
            double n = (3 - Math.Sqrt(40 * Value + 9)) / 10;
            return (Math.Floor(n) == n);
        }
        public bool IsOctagonNumber()
        {
            double n = (1 - Math.Sqrt(3 * Value + 1)) / 3;
            return (Math.Floor(n) == n);
        }
        public bool IsPerfectSquare()
        {
            return (Math.Floor(Math.Sqrt(Value)) == Math.Sqrt(Value));
        }
        private bool IsNatural(double X)
        {
            return (Math.Floor(X) == X);
        }
        public int GetDistinctPrimeFactorsCount()
        {
            long n = Value;
            int result = 0;
            if (n % 2 == 0)
            {
                result++;
                while (n % 2 == 0)
                {
                    n = n / 2;
                }
            }
            for (int i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                if (n % i == 0)
                {
                    result++;
                    while (n % i == 0)
                    {
                        n = n / i;
                    }
                }
            }
            if (n > 2)
            {
                result++;
            }
            return result;
        }
        public bool IsPotentiallyCubic()
        {
            switch (GetDigitalRoot())
            {
                case 1:
                    return true;
                case 8:
                    return true;
                case 9:
                    return true;
                default:
                    return false;
            }
        }
        public int GetDigitalRoot()
        {
            List<int> digits = new List<int>();
            foreach (char digit in Value.ToString().ToCharArray())
            {
                digits.Add(int.Parse(digit.ToString()));
            }
            while (digits.Count() > 1)
            {
                digits = GetSumOfDigits(digits);
            }
            return digits.Sum();
        }
        private static List<int> GetSumOfDigits(List<int> digits)
        {
            long sum = digits.Sum();
            foreach (char digit in sum.ToString().ToCharArray())
            {
                digits.Add(int.Parse(digit.ToString()));
            }
            return digits;
        }
    }
}