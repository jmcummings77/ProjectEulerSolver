using EulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EulerSolver.Tools
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
            PrimeFactors.Add(1);
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
            return PrimeFactors;
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
            for(int i = 0; i < value.Length; i++)
            {
                values.Add(int.Parse(value.Substring(i,1)));
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
    }
}