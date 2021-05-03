using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Tools
{
    public class Fraction
    {
        public BigInteger Numerator { get; private set; }
        public BigInteger Denominator { get; private set; }
        public BigInteger ReducedNumerator { get; private set; }
        public BigInteger ReducedDenominator { get; private set; }
        public BigInteger GreatestCommonDivisor { get; private set; }
        public double ApproximateValue { get; private set; }

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            if (Denominator == 0)
            {
                throw new DivideByZeroException();
            }
            Simplify();
            ApproximateValue = (double)numerator / (double)denominator;
        }
        public void Add(Fraction fraction)
        {
            if(fraction.Denominator == Denominator)
            {
                Numerator += fraction.Numerator;
            }
            else
            {
                Numerator = (Numerator * fraction.Denominator) + (fraction.Numerator * Denominator);
                Denominator = Denominator * fraction.Denominator;
            }
            if (Denominator == 0)
            {
                throw new DivideByZeroException();
            }
            Simplify();
        }
        public void Multiply(Fraction fraction)
        {
            Numerator = Numerator * fraction.Numerator;
            Denominator = Denominator * fraction.Denominator;
            if (Denominator == 0)
            {
                throw new DivideByZeroException();
            }
            Simplify();
        }
        public void DivideBy(Fraction fraction)
        {
            Numerator = Numerator * fraction.Denominator;
            Denominator = Denominator * fraction.Numerator;
            if (Denominator == 0)
            {
                throw new DivideByZeroException();
            }
            Simplify();
        }
        public long GetNumeratorDigitCount()
        {
            return Numerator.ToString().Length;
        }
        public long GetDenominatorDigitCount()
        {
            return Denominator.ToString().Length;
        }
        public void Simplify()
        {
            SetGreatestCommonDivisor();
            if(GreatestCommonDivisor == 1)
            {
                ReducedNumerator = Numerator;
                ReducedDenominator = Denominator;
            }
            else
            {
                ReducedNumerator = WholeDivision(Numerator, GreatestCommonDivisor);
                ReducedDenominator = WholeDivision(Denominator, GreatestCommonDivisor);
            }
        }
        private void SetGreatestCommonDivisor()
        {
            BigInteger a = Numerator;
            BigInteger b = Denominator;
            while (a != b)
            {
                if (a < b)
                {
                    b = b - a;
                }
                else
                {
                    a = a - b;
                }
            }
            GreatestCommonDivisor = a;
        }
        private BigInteger WholeDivision(BigInteger a, BigInteger b)
        {
            BigInteger remainder = a;
            BigInteger quotient = 0;
            while (remainder >= b)
            {
                remainder = remainder - b;
                quotient++;
            }
            return quotient;
        }
    }
    public class SmallFraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }
        public double ApproximateValue { get; private set; }

        public SmallFraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            if (Denominator == 0)
            {
                throw new DivideByZeroException();
            }
            Simplify();
            ApproximateValue = (double)numerator / (double)denominator;
        }
        public void Simplify()
        {
            int GreatestCommonDivisor = GetGreatestCommonDivisor();
            if (GreatestCommonDivisor != 1)
            {
                Numerator = WholeDivision(Numerator, GreatestCommonDivisor);
                Denominator = WholeDivision(Denominator, GreatestCommonDivisor);
            }
             

        }
        private int GetGreatestCommonDivisor()
        {
            int a = Numerator;
            int b = Denominator;
            while (a != b)
            {
                if (a < b)
                {
                    b = b - a;
                }
                else
                {
                    a = a - b;
                }
            }
            return a;
        }
        private int WholeDivision(int a, int b)
        {
            int remainder = a;
            int quotient = 0;
            while (remainder >= b)
            {
                remainder = remainder - b;
                quotient++;
            }
            return quotient;
        }
    }

}
