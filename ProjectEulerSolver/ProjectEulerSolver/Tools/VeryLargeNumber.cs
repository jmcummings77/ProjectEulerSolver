using ProjectEulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Tools
{
    public class VeryLargeNumber
    {
        public BigInteger Value { get; set; }
        public VeryLargeNumber(BigInteger _Value)
        {
            Value = _Value;
        }
        public int DigitCount()
        {
            return ToCharArray().Length;
        }
        public List<int> GetDivisors()
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
        public int GetSumOfDivisors()
        {
            int result = 0;
            foreach(int i in GetDivisors())
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
            BigInteger n = Value - 1;
            BigInteger result = Value;
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
            var digits = ToIntList();
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
        public byte[] ToByteArray()
        {
            return Value.ToByteArray();
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
            var n = Value;
            if (n == 0)
            {
                return new int[1] { 0 };
            }
            var digits = new List<int>();
            for (; n != 0; n /= 10)
            {
                digits.Add((int)n % 10);
            }
            return digits.ToArray();
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
        public bool CanConvertToInt64()
        {
            return Value < Int64.MaxValue;
        }
        public Int64 ToInt64()
        {
            if(CanConvertToInt())
            {
                return (Int64)Value;
            }
            else
            {
                throw new OverflowException();
            }

        }
        public bool IsPrime()
        {
            throw new NotImplementedException();
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