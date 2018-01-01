using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using EulerSolver.Tools;

namespace EulerSolver.Tools
{
    public class Fraction
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }
        public long ReducedNumerator { get; private set; }
        public long ReducedDenominator { get; private set; }
        public long GreatestCommonDivisor { get; private set; }

        public Fraction(object numerator, object denominator)
        {
            Numerator = (long)numerator;
            Denominator = (long)denominator;
            if (Denominator == 0)
            {
                throw new DivideByZeroException();
            }
            Simplify();
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
            long a = Numerator;
            long b = Denominator;
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
        private long WholeDivision(long a, long b)
        {
            long remainder = a;
            long quotient = 0;
            while (remainder >= b)
            {
                remainder = remainder - b;
                quotient++;
            }
            return quotient;
        }
    }
}
