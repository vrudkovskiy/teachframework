using System;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Class contains arithmetic and comparison operations with very big number(mC*M + fC)
    /// </summary>
    public class MaxCoefficient
    {
        private readonly Fraction _mCoefficient;
        private readonly Fraction _freeCoefficient;
        //-----------------------------------------------------
        /// <summary>
        /// Gets coefficient of M
        /// </summary>
        public Fraction MCoefficient
        {
            get { return new Fraction(_mCoefficient); }
        }

        /// <summary>
        /// Gets free coefficient
        /// </summary>
        public Fraction FreeCoefficient
        {
            get { return new Fraction(_freeCoefficient); }
        }
        //===========================================================================================================

        /// <summary>
        /// Represents maxCoefficient with value: 0M + 0
        /// </summary>
        public MaxCoefficient()
        {
            _mCoefficient = new Fraction();
            _freeCoefficient = new Fraction();
        }

        /// <summary>
        /// Represents maxCoefficient with value from input string
        /// </summary>
        /// <param name="value">Input string in format: "mC*M + fC", mC - mCoefficient(Fraction: n/d), 
        ///                                                          fC - free coefficient(Fraction: n/d),
        ///                                                          M - label(char)</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public MaxCoefficient(string value)
        {
            var index = value.IndexOf('M');
            if (index == -1)
            {
                _mCoefficient = new Fraction();
                _freeCoefficient = new Fraction(value);
                return;
            }

            var mCoefficientStr = value.Substring(0, index);
            var freeCoefficientStr = value.Substring(index + 1);

            if (mCoefficientStr == string.Empty)
                mCoefficientStr = "1/1";
            else if (mCoefficientStr == "-")
                mCoefficientStr = "-1/1";
            if (freeCoefficientStr == string.Empty)
                freeCoefficientStr = "0/0";
            if (mCoefficientStr.IndexOf('/') == -1)
                mCoefficientStr = mCoefficientStr + "/1";
            if (freeCoefficientStr.IndexOf('/') == -1)
                freeCoefficientStr = freeCoefficientStr + "/1";

            _mCoefficient = new Fraction(mCoefficientStr);
            _freeCoefficient = new Fraction(freeCoefficientStr);
        }

        /// <summary>
        /// Represents maxCoefficient
        /// </summary>
        public MaxCoefficient(Fraction mCoefficient, Fraction freeCoefficient)
        {
            _mCoefficient = new Fraction(mCoefficient);
            _freeCoefficient = new Fraction(freeCoefficient);
        }

        /// <summary>
        /// Represents copy of maxCoefficient
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public MaxCoefficient(MaxCoefficient maxCoefficient)
        {
            _mCoefficient = new Fraction(maxCoefficient._mCoefficient);
            _freeCoefficient = new Fraction(maxCoefficient._freeCoefficient);
        }
        //============================================================================================================

        /// <summary>
        /// Returns maxCoefficient in string format("mC*M + fC"), mC - mCoefficient(Fraction: n/d), 
        ///                                                       fC - free coefficient(Fraction: n/d),
        ///                                                       M - label(char)
        /// </summary>
        public override string ToString()
        {
            if (_mCoefficient == 0 && _freeCoefficient == 0)
                return "0";

            var mCoefficientStr = _mCoefficient != 1 && _mCoefficient != -1
                                      ? _mCoefficient + "M"
                                      : _mCoefficient == 1 ? "M" : "-M";
            var freeCoefficientStr = _freeCoefficient.ToString();

            if (!freeCoefficientStr.Contains("-") && _mCoefficient != 0)
                freeCoefficientStr = "+" + freeCoefficientStr;

            var valueStr = string.Empty;

            if (_mCoefficient != 0)
                valueStr += mCoefficientStr;
            if (_freeCoefficient != 0)
                valueStr += freeCoefficientStr;

            return valueStr;
        }

        /// <summary>
        /// Multiplyes fraction to current maxCoefficient
        /// </summary>
        /// <param name="fraction"></param>
        public void Multiply(Fraction fraction)
        {
            _mCoefficient.Multiply(fraction);
            _freeCoefficient.Multiply(fraction);
        }

        /// <summary>
        /// Add number to current maxCoefficient
        /// </summary>
        public void Add(int number)
        {
            _freeCoefficient.Add(number);
        }

        /// <summary>
        /// Add fraction to current maxCoefficient
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Add(Fraction fraction)
        {
            _freeCoefficient.Add(fraction);
        }

        /// <summary>
        /// Add maxCoefficient to current maxCoefficient
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Add(MaxCoefficient maxCoefficient)
        {
            _mCoefficient.Add(maxCoefficient._mCoefficient);
            _freeCoefficient.Add(maxCoefficient._freeCoefficient);
        }

        /// <summary>
        /// Subtract number from current maxCoefficient
        /// </summary>
        public void Subtract(int number)
        {
            _freeCoefficient.Add(-number);
        }

        /// <summary>
        /// Subtract fraction from current maxCoefficient
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Subtract(Fraction fraction)
        {
            _freeCoefficient.Add(fraction * -1);
        }

        /// <summary>
        /// Subtract maxCoefficient from current maxCoefficient
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Subtract(MaxCoefficient maxCoefficient)
        {
            _mCoefficient.Add(maxCoefficient._mCoefficient * -1);
            _freeCoefficient.Add(maxCoefficient._freeCoefficient * -1);
        }

        ///// <summary>
        ///// Method for reference comparison of current and input objects
        ///// </summary>
        ///// <param name="obj">Object for comparison</param>
        ///// <returns>Return true if references of input and current objects are equal</returns>
        ///// <exception cref="NullReferenceException"></exception>
        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}

        /// <summary>
        /// Method for value comparison of current maxCoefficient and input number
        /// </summary>
        /// <param name="number">Number for comparison</param>
        /// <returns>Return true if values of input number and current maxCoefficient are equal</returns>
        public bool Equals(int number)
        {
            return (CompareTo(number) == 0);
        }

        /// <summary>
        /// Method for value comparison of current maxCoefficient and input fraction
        /// </summary>
        /// <param name="fraction">Fraction for comparison</param>
        /// <returns>Return true if values of input fraction and current maxCoefficient are equal</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Equals(Fraction fraction)
        {
            return (CompareTo(fraction) == 0);
        }

        /// <summary>
        /// Method for value comparison of current and input maxCoefficients
        /// </summary>
        /// <param name="maxCoefficient">MaxCoefficient for comparison</param>
        /// <returns>Return true if values of input and current maxCoefficient are equal</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool Equals(MaxCoefficient maxCoefficient)
        {
            if (ReferenceEquals(null, maxCoefficient)) return false;
            if (ReferenceEquals(this, maxCoefficient)) return true;
            return Equals(maxCoefficient._mCoefficient, _mCoefficient) && Equals(maxCoefficient._freeCoefficient, _freeCoefficient);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficient and number
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if maxCoefficient > number
        /// -1: if number > maxCoefficient
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public static int Compare(MaxCoefficient maxCoefficient, int number)
        {
            return maxCoefficient.CompareTo(number);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficient and number
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if number > maxCoefficient
        /// -1: if maxCoefficient > number
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public static int Compare(int number, MaxCoefficient maxCoefficient)
        {
            return (maxCoefficient.CompareTo(number) * -1);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficient and fraction
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if maxCoefficient > fraction
        /// -1: if fraction > maxCoefficient
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public static int Compare(MaxCoefficient maxCoefficient, Fraction fraction)
        {
            return maxCoefficient.CompareTo(fraction);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficient and fraction
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if fraction  > maxCoefficient
        /// -1: if maxCoefficient > fraction
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public static int Compare(Fraction fraction, MaxCoefficient maxCoefficient)
        {
            return (maxCoefficient.CompareTo(fraction) * -1);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficients
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if first > second
        /// -1: if second > first 
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public static int Compare(MaxCoefficient first, MaxCoefficient second)
        {
            if (ReferenceEquals(first, null))
                if (ReferenceEquals(second, null))
                    return 0;
                else
                    throw new NullReferenceException();
            return first.CompareTo(second);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficient and number
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if current maxCoefficient > number
        /// -1: if number > current maxCoefficient
        /// </returns>
        public int CompareTo(int number)
        {
            if (_mCoefficient != 0)
                return _mCoefficient > 0 ? 1 : -1;
            return _freeCoefficient.CompareTo(number);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficient and fraction
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if current maxCoefficient > fraction
        /// -1: if fraction > current maxCoefficient
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public int CompareTo(Fraction fraction)
        {
            if (_mCoefficient != 0)
                return _mCoefficient > 0 ? 1 : -1;
            return _freeCoefficient.CompareTo(fraction);
        }

        /// <summary>
        /// Method for value comparison of maxCoefficients
        /// </summary>
        /// <returns>
        /// 0: if they are equal
        /// 1: if current maxCoefficient > input maxCoefficient
        /// -1: if input maxCoefficient > current maxCoefficient
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        public int CompareTo(MaxCoefficient maxCoefficient)
        {
            if (ReferenceEquals(maxCoefficient, null))
                return -1;
            return _mCoefficient == maxCoefficient._mCoefficient
                ? _freeCoefficient.CompareTo(maxCoefficient._freeCoefficient)
                : _mCoefficient.CompareTo(maxCoefficient._mCoefficient);
        }

        #region [ Compare oparations 'MaxCoefficient - MaxCoefficient' ]

        /// <summary>
        /// Operator equality of fractions
        /// </summary>
        public static bool operator ==(MaxCoefficient first, MaxCoefficient second)
        {
            return (Compare(first, second) == 0);
        }

        /// <summary>
        /// Operator inequality of fractions
        /// </summary>
        public static bool operator !=(MaxCoefficient first, MaxCoefficient second)
        {
            return (Compare(first, second) != 0);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if first more than second</returns>
        public static bool operator >(MaxCoefficient first, MaxCoefficient second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == 1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if first less than second</returns>
        public static bool operator <(MaxCoefficient first, MaxCoefficient second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == -1);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if first more than or equal second</returns>
        public static bool operator >=(MaxCoefficient first, MaxCoefficient second)
        {
            return !(first < second);
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if first less than or equal second</returns>
        public static bool operator <=(MaxCoefficient first, MaxCoefficient second)
        {
            return !(first > second);
        }

        #endregion

        #region [ Compare oparations 'MaxCoefficient - Fraction' ]

        /// <summary>
        /// Operator equality of fractions
        /// </summary>
        public static bool operator ==(MaxCoefficient first, Fraction second)
        {
            return (Compare(first, second) == 0);
        }

        /// <summary>
        /// Operator equality of fractions
        /// </summary>
        public static bool operator ==(Fraction first, MaxCoefficient second)
        {
            return (Compare(first, second) == 0);
        }

        /// <summary>
        /// Operator inequality of fractions
        /// </summary>
        public static bool operator !=(MaxCoefficient first, Fraction second)
        {
            return (Compare(first, second) != 0);
        }

        /// <summary>
        /// Operator inequality of fractions
        /// </summary>
        public static bool operator !=(Fraction first, MaxCoefficient second)
        {
            return (Compare(first, second) != 0);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if first more than second</returns>
        public static bool operator >(MaxCoefficient first, Fraction second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == 1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if first less than second</returns>
        public static bool operator <(MaxCoefficient first, Fraction second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == -1);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if first more than second</returns>
        public static bool operator >(Fraction first, MaxCoefficient second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == 1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if first less than second</returns>
        public static bool operator <(Fraction first, MaxCoefficient second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == -1);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if first more than or equal second</returns>
        public static bool operator >=(MaxCoefficient first, Fraction second)
        {
            return !(first < second);
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if first less than or equal second</returns>
        public static bool operator <=(MaxCoefficient first, Fraction second)
        {
            return !(first > second);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if first more than or equal second</returns>
        public static bool operator >=(Fraction first, MaxCoefficient second)
        {
            return !(first < second);
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if first less than or equal second</returns>
        public static bool operator <=(Fraction first, MaxCoefficient second)
        {
            return !(first > second);
        }

        #endregion

        #region [ Compare oparations 'MaxCoefficient - int' ]

        /// <summary>
        /// Operator equality of fractions
        /// </summary>
        public static bool operator ==(MaxCoefficient first, int second)
        {
            return (Compare(first, second) == 0);
        }

        /// <summary>
        /// Operator equality of fractions
        /// </summary>
        public static bool operator ==(int first, MaxCoefficient second)
        {
            return (Compare(first, second) == 0);
        }

        /// <summary>
        /// Operator inequality of fractions
        /// </summary>
        public static bool operator !=(MaxCoefficient first, int second)
        {
            return (Compare(first, second) != 0);
        }

        /// <summary>
        /// Operator inequality of fractions
        /// </summary>
        public static bool operator !=(int first, MaxCoefficient second)
        {
            return (Compare(first, second) != 0);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if first more than second</returns>
        public static bool operator >(MaxCoefficient first, int second)
        {
            return (first.CompareTo(second) == 1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if first less than second</returns>
        public static bool operator <(MaxCoefficient first, int second)
        {
            return (first.CompareTo(second) == -1);
        }

        /// <summary>
        /// Operator more than
        /// </summary>
        /// <returns>true if first more than second</returns>
        public static bool operator >(int first, MaxCoefficient second)
        {
            return (first.CompareTo(second) == 1);
        }

        /// <summary>
        /// Operator less than
        /// </summary>
        /// <returns>true if first less than second</returns>
        public static bool operator <(int first, MaxCoefficient second)
        {
            if (ReferenceEquals(second, null))
                throw new NullReferenceException();
            return (first.CompareTo(second) == -1);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if first more than or equal second</returns>
        public static bool operator >=(MaxCoefficient first, int second)
        {
            return !(first < second);
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if first less than or equal second</returns>
        public static bool operator <=(MaxCoefficient first, int second)
        {
            return !(first > second);
        }

        /// <summary>
        /// Operator more than or equal
        /// </summary>
        /// <returns>true if first more than or equal second</returns>
        public static bool operator >=(int first, MaxCoefficient second)
        {
            return !(first < second);
        }

        /// <summary>
        /// Operator less than or equal
        /// </summary>
        /// <returns>true if first less than or equal second</returns>
        public static bool operator <=(int first, MaxCoefficient second)
        {
            return !(first > second);
        }

        #endregion

        /// <summary>
        /// Method returns hash code of current object
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_mCoefficient != null ? _mCoefficient.GetHashCode() : 0) * 397) ^ (_freeCoefficient != null ? _freeCoefficient.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Method for value comparison of current and input maxCoefficients
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(MaxCoefficient) && Equals((MaxCoefficient)obj);
        }
    }
}
