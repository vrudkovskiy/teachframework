using System;
using System.Globalization;

namespace TeachFramework.MathTypes
{
    /// <summary>
    /// Class contains arithmetic and comparison operations with fractions
    /// </summary>
    public class Fraction : IComparable
    {
        /// <summary>
        /// Gets numerator
        /// </summary>
        public int Numerator { get; private set; }

        /// <summary>
        /// Gets denominator
        /// </summary>
        public int Denominator { get; private set; }

        //==========================================================================================================

        /// <summary>
        /// Represents fraction with value: numerator = 0, denominator = 0
        /// </summary>
        public Fraction() { }

        /// <summary>
        /// Represents fraction with value from input string
        /// </summary>
        /// <param name="value">Input string in format: "n/d", n - numerator, d - denominator</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public Fraction(string value)
        {
            value = value.Replace(" ", "");
            var index = value.IndexOf('/');
            if (index == -1)
            {
                int a;
                if (!int.TryParse(value, out a))
                    throw new FormatException("Fraction(string): Value doesn't contain symbol '/'");
                value += @"/1";
                index = value.IndexOf('/');
            }
            int numerator;
            int denominator;
            var numeratorStr = value.Substring(0, index);
            var denominatorStr = value.Substring(index + 1);
            if (!int.TryParse(numeratorStr, out numerator) || !int.TryParse(denominatorStr, out denominator))
                throw new FormatException(
                    "Fraction(string): Invalid format of numerator or denominator in a value string");
            Numerator = numerator;
            Denominator = denominator;
            Normalize();
        }

        /// <summary>
        /// Represents fraction
        /// </summary>
        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            Normalize();
        }

        /// <summary>
        /// Represents fraction with value from input fraction
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public Fraction(Fraction fraction)
        {
            Numerator = fraction.Numerator;
            Denominator = fraction.Denominator;
            Normalize();
        }
        //============================================================================================================

        /// <summary>
        /// Returns fraction in string format("n/d"), n - numerator, d - denominator
        /// </summary>
        public override string ToString()
        {
            if (Denominator == 0)
                return "0";
            if (Denominator == 1)
                return Numerator.ToString(CultureInfo.InvariantCulture);
            return Numerator + "/" + Denominator;
        }

        #region Fraction to fraction operators (==, !=, >, <, >=, <=, +, -, *, /)

        /// <summary>
        /// Operator equality of fractions
        /// </summary>
        public static bool operator ==(Fraction first, Fraction second)
        {
            return (Compare(first, second) == 0);
        }

        /// <summary>
        /// Operator inequality of fractions
        /// </summary>
        public static bool operator !=(Fraction first, Fraction second)
        {
            return (Compare(first, second) != 0);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if first more than second</returns>
        public static bool operator >(Fraction first, Fraction second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == 1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if first less than second</returns>
        public static bool operator <(Fraction first, Fraction second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == -1);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if first more than or equal second</returns>
        public static bool operator >=(Fraction first, Fraction second)
        {
            return !(first < second);
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if first less than or equal second</returns>
        public static bool operator <=(Fraction first, Fraction second)
        {
            return !(first > second);
        }

        /// <summary>
        /// Operator adding two fractions
        /// </summary>
        public static Fraction operator +(Fraction first, Fraction second)
        {
            var result = new Fraction(first);
            result.Add(second);
            return result;
        }

        /// <summary>
        /// Operator subtracting two fractions
        /// </summary>
        public static Fraction operator -(Fraction first, Fraction second)
        {
            var result = new Fraction(first);
            result.Subtract(second);
            return result;
        }

        /// <summary>
        /// Operator multiplying two fractions
        /// </summary>
        public static Fraction operator *(Fraction first, Fraction second)
        {
            var result = new Fraction(first);
            result.Multiply(second);
            return result;
        }

        /// <summary>
        /// Operator dividing two fractions
        /// </summary>
        public static Fraction operator /(Fraction first, Fraction second)
        {
            var result = new Fraction(first);
            result.Divide(second);
            return result;
        }

        #endregion

        #region Fraction to int operators (==, !=, >, <, >=, <=, +, -, *, /)

        /// <summary>
        /// Operator equality of fraction and number
        /// </summary>
        public static bool operator ==(Fraction fraction, int number)
        {
            if (ReferenceEquals(fraction, null))
                return false;
            return fraction.CompareTo(number) == 0;
        }

        /// <summary>
        /// Operator equality of number and fraction
        /// </summary>
        public static bool operator ==(int number, Fraction fraction)
        {
            return (fraction == number);
        }

        /// <summary>
        /// Operator inequality of fraction and number
        /// </summary>
        public static bool operator !=(Fraction fraction, int number)
        {
            return !(fraction == number);
        }

        /// <summary>
        /// Operator inequality of number and fraction
        /// </summary>
        public static bool operator !=(int number, Fraction fraction)
        {
            return !(fraction == number);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if fraction more than number</returns>
        public static bool operator >(Fraction fraction, int number)
        {
            return (fraction.CompareTo(number) == 1);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if number more than fraction</returns>
        public static bool operator >(int number, Fraction fraction)
        {
            return (fraction.CompareTo(number) == -1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if fraction less than number</returns>
        public static bool operator <(Fraction fraction, int number)
        {
            return (fraction.CompareTo(number) == -1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if number less than fraction</returns>
        public static bool operator <(int number, Fraction fraction)
        {
            return (fraction.CompareTo(number) == 1);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if fraction more than or equal number</returns>
        public static bool operator >=(Fraction fraction, int number)
        {
            return fraction.CompareTo(number) != -1;
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if number more than or equal fraction</returns>
        public static bool operator >=(int number, Fraction fraction)
        {
            return fraction.CompareTo(number) != 1;
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if fraction less than or equal number</returns>
        public static bool operator <=(Fraction fraction, int number)
        {
            return fraction.CompareTo(number) != 1;
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if number less than or equal fraction</returns>
        public static bool operator <=(int number, Fraction fraction)
        {
            return fraction.CompareTo(number) != -1;
        }

        /// <summary>
        /// Operator adding fraction and number
        /// </summary>
        public static Fraction operator +(Fraction fraction, int number)
        {
            var result = new Fraction(fraction);
            result.Add(number);
            return result;
        }

        /// <summary>
        /// Operator adding number and fraction
        /// </summary>
        public static Fraction operator +(int number, Fraction fraction)
        {
            return (fraction + number);
        }

        /// <summary>
        /// Operator subtracting fraction and number
        /// </summary>
        public static Fraction operator -(Fraction fraction, int number)
        {
            return (fraction + (-number));
        }

        /// <summary>
        /// Operator subtracting number and fraction
        /// </summary>
        public static Fraction operator -(int number, Fraction fraction)
        {
            var result = new Fraction(fraction);
            result.Numerator *= -1;
            result.Add(number);
            return result;
        }

        /// <summary>
        /// Operator multiplying fraction and number
        /// </summary>
        public static Fraction operator *(Fraction fraction, int number)
        {
            var result = new Fraction(fraction);
            result.Multiply(number);
            return result;
        }

        /// <summary>
        /// Operator multiplying number and fraction
        /// </summary>
        public static Fraction operator *(int number, Fraction fraction)
        {
            return (fraction * number);
        }

        /// <summary>
        /// Operator dividing fraction and number
        /// </summary>
        public static Fraction operator /(Fraction fraction, int number)
        {
            var result = new Fraction(fraction);
            result.Divide(number);
            return result;
        }

        /// <summary>
        /// Operator dividing number and fraction
        /// </summary>
        public static Fraction operator /(int number, Fraction fraction)
        {
            if (fraction.Numerator == 0)
                throw new DivideByZeroException("Operator /(int, Fraction): divide by zero");
            var result = new Fraction(fraction.Denominator, fraction.Numerator);
            result.Numerator *= number;
            return result;
        }

        #endregion

        /// <summary>
        /// Add number to current fraction
        /// </summary>
        public void Add(int number)
        {
            if (Numerator == 0)
            {
                Numerator = number;
                Denominator = 1;
                Normalize();
                return;
            }
            if (number == 0)
                return;
            Numerator = Numerator + number * Denominator;
            Normalize();
        }

        /// <summary>
        /// Add fraction to current fraction
        /// </summary>
        public void Add(Fraction fraction)
        {
            if (Numerator == 0)
            {
                Numerator = fraction.Numerator;
                Denominator = fraction.Denominator;
                return;
            }
            if (fraction.Numerator == 0)
                return;
            Numerator = Numerator * fraction.Denominator + fraction.Numerator * Denominator;
            Denominator = Denominator * fraction.Denominator;
            Normalize();
        }

        /// <summary>
        /// Sabtract number from current fraction
        /// </summary>
        public void Subtract(int number)
        {
            Add(-number);
        }

        /// <summary>
        /// Sabtract fraction from current fraction
        /// </summary>
        public void Subtract(Fraction fraction)
        {
            if (Numerator == 0)
            {
                Numerator = fraction.Numerator * -1;
                Denominator = fraction.Denominator;
                return;
            }
            if (fraction.Numerator == 0)
                return;
            Numerator = Numerator * fraction.Denominator - fraction.Numerator * Denominator;
            Denominator = Denominator * fraction.Denominator;
            Normalize();
        }

        /// <summary>
        /// Multiply number to current fraction
        /// </summary>
        public void Multiply(int number)
        {
            Numerator = Numerator * number;
            Normalize();
        }

        /// <summary>
        /// Multiply fraction to current fraction
        /// </summary>
        /// <param name="fraction"></param>
        public void Multiply(Fraction fraction)
        {
            Numerator = Numerator * fraction.Numerator;
            Denominator = Denominator * fraction.Denominator;
            Normalize();
        }

        /// <summary>
        /// Divide current fraction to number
        /// </summary>
        public void Divide(int number)
        {
            if (number == 0)
                throw new DivideByZeroException("Divide(int): divide fraction by zero");
            Denominator = Denominator * number;
            Normalize();
        }

        /// <summary>
        /// Divide current fraction to fraction
        /// </summary>
        public void Divide(Fraction fraction)
        {
            if (fraction.Numerator == 0)
                throw new DivideByZeroException("Divide(Fraction): divide fraction by zero");
            Numerator = Numerator * fraction.Denominator;
            Denominator = Denominator * fraction.Numerator;
            Normalize();
        }

        /// <summary>
        /// Method for value comparison of current and input fractions
        /// </summary>
        /// <param name="obj">Object for comparison</param>
        /// <returns>Return true if types and values of input and current fractions are equal</returns>
        /// <exception cref="NullReferenceException"></exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Fraction)) return false;
            return Equals((Fraction)obj);
        }

        /// <summary>
        /// Method for value comparison of current and input fractions
        /// </summary>
        /// <param name="fraction">Fraction for comparison</param>
        /// <returns>Return true if values of input and current fractions are equal</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Equals(Fraction fraction)
        {
            if (ReferenceEquals(null, fraction)) return false;
            if (ReferenceEquals(this, fraction)) return true;
            return fraction.Numerator == Numerator && fraction.Denominator == Denominator;
        }

        /// <summary>
        /// Method for value comparison of fractions
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if first > second
        /// -1: if second > first 
        /// </returns>
        public static int Compare(Fraction first, Fraction second)
        {
            if (ReferenceEquals(first, null))
                return ReferenceEquals(second, null) ? 0 : -1;
            return first.CompareTo(second);
        }

        /// <summary>
        /// Method for value comparison of fractions
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if current > fraction
        /// -1: if fraction > current
        /// </returns>
        public int CompareTo(Fraction fraction)
        {
            if (ReferenceEquals(fraction, null))
                return -1;
            if (Numerator == 0)
                return (fraction.Numerator.CompareTo(0) * -1);
            if (fraction.Numerator == 0)
                return (Numerator.CompareTo(0));
            var thisInDouble = (double)Numerator / Denominator;
            var fractionInDouble = (double)fraction.Numerator / fraction.Denominator;
            return thisInDouble.CompareTo(fractionInDouble);
        }

        /// <summary>
        /// Method for value comparison of current and input objects
        /// </summary>
        /// <param name="number">Int value for comparison</param>
        /// <returns>Return true if values of input and current objects are equal</returns>
        public bool Equals(int number)
        {
            return (CompareTo(number) == 0);
        }

        /// <summary>
        /// Method for value comparison of fraction and number
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if fraction > number
        /// -1: if number > fraction
        /// </returns>
        public static int Compare(Fraction fraction, int number)
        {
            return fraction.CompareTo(number);
        }

        /// <summary>
        /// Method for value comparison of fraction and number
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if number > fraction
        /// -1: if fraction > number
        /// </returns>
        public static int Compare(int number, Fraction fraction)
        {
            return (fraction.CompareTo(number) * -1);
        }

        /// <summary>
        /// Method for value comparison of fraction and number
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if number > current fraction
        /// -1: if current fraction > number
        /// </returns>
        public int CompareTo(int number)
        {
            if (Numerator == 0)
                return (number.CompareTo(0) * -1);
            var thisInDouble = ((double)Numerator / Denominator);
            return thisInDouble.CompareTo(number);
        }

        /// <summary>
        /// Method returns hash code of current object
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Numerator * 397) ^ Denominator;
            }
        }

        /// <summary>
        /// Normilize fraction
        /// </summary>
        private void Normalize()
        {
            var gcd = Gcd(Numerator, Denominator);
            if (gcd != 0)
            {
                Denominator /= gcd;
                Numerator /= gcd;
            }
            if (Denominator < 0)
            {
                Denominator *= -1;
                Numerator *= -1;
            }
            if (Numerator != 0 && Denominator != 0) return;
            Denominator = 0;
            Numerator = 0;
        }

        /// <summary>
        /// Greatest common divisor of two numbers
        /// </summary>
        private static int Gcd(int a, int b)
        {
            while (b != 0)
                b = a % (a = b);
            return Math.Abs(a);
        }

        #region IComparable Members

        /// <summary>
        /// Method for value comparison of fractions
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if current > fraction
        /// -1: if fraction > current
        /// </returns>
        public int CompareTo(object obj)
        {
            return CompareTo((Fraction)obj);
        }

        #endregion
    }
}
