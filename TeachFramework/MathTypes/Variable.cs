using System;
using System.Text.RegularExpressions;

namespace TeachFramework.MathTypes
{
    /// <summary>
    /// Class contains operations with variables
    /// </summary>
    public class Variable : IComparable
    {
        private Fraction _coefficient;

        /// <summary>
        /// Gets copy of coefficient
        /// </summary>
        public Fraction Coefficient
        {
            get { return new Fraction(_coefficient); }
        }

        /// <summary>
        /// Gets copy of label
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Gets copy of power
        /// </summary>
        public int Power { get; private set; }

        //=================================================================================================

        #region [ Constractors ]

        /// <summary>
        /// Represents variable: coefficient = 0, label = "x", power = 1
        /// </summary>
        public Variable()
        {
            _coefficient = new Fraction();
            Label = "x";
            Power = 1;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, label = "x", power = 1
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public Variable(Fraction coefficient)
        {
            _coefficient = new Fraction(coefficient);
            Label = "x";
            Power = 1;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, label = "x", power = 1
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public Variable(string coefficient)
        {
            _coefficient = new Fraction(coefficient);
            Label = "x";
            Power = 1;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, this.label = label, power = 1
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="label">Should be in format: letter+index(x23)</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public Variable(Fraction coefficient, string label)
        {
            _coefficient = new Fraction(coefficient);
            SetLabel(label);
            Power = 1;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, this.label = label, power = 1
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="label">Should be in format: letter+index(x23)</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public Variable(string coefficient, string label)
        {
            _coefficient = new Fraction(coefficient);
            SetLabel(label);
            Power = 1;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, this.label = label, this.power = power
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="label">Should be in format: letter+index(x23)</param>
        /// <param name="power"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public Variable(Fraction coefficient, string label, int power)
        {
            _coefficient = new Fraction(coefficient);
            SetLabel(label);
            if (power <= 0)
                throw new FormatException("Power must be more than zero");
            Power = power;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, this.label = label, this.power = power
        /// </summary>
        /// <param name="coefficient">Must be in format: 'n/d'</param>
        /// <param name="label">Should be in format: letter+index(x23)</param>
        /// <param name="power"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public Variable(string coefficient, string label, int power)
        {
            _coefficient = new Fraction(coefficient);
            SetLabel(label);
            if (power <= 0)
                throw new FormatException("Power must be more than zero");
            Power = power;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, this.label = "x", this.power = power
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public Variable(Fraction coefficient, int power)
        {
            _coefficient = new Fraction(coefficient);
            SetLabel("x");
            if (power <= 0)
                throw new FormatException("Power must be more than zero");
            Power = power;
        }

        /// <summary>
        /// Represents variable: this.coefficient = coefficient, this.label = "x", this.power = power
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public Variable(string coefficient, int power)
        {
            _coefficient = new Fraction(coefficient);
            SetLabel("x");
            if (power <= 0)
                throw new FormatException("Power must be more than zero");
            Power = power;
        }

        /// <summary>
        /// Represents copy of variable
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public Variable(Variable variable)
        {
            _coefficient = new Fraction(variable._coefficient);
            SetLabel(variable.Label);
            Power = variable.Power;
        }

        #endregion

        /// <summary>
        /// Returns variable in string format("cl^p" or "cl" if power = 1), c - coefficient, l - label, p - power
        /// </summary>
        public override string ToString()
        {
            return Power == 1 ? _coefficient + Label : _coefficient + Label + "^" + Power;
        }

        /// <summary>
        /// Calculates variable value
        /// </summary>
        /// <param name="varValue"></param>
        /// <exception cref="NullReferenceException"></exception>
        public Fraction Calculate(Fraction varValue)
        {
            var result = new Fraction(varValue);
            for (var i = 1; i < Power; i++)
                result *= result;
            result *= _coefficient;
            return result;
        }

        /// <summary>
        /// Adds var to current variable
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Add(Variable var)
        {
            if (var.Label != Label || var.Power != Power)
                throw new FormatException("Input variable must be with same labels and powers");
            _coefficient += var._coefficient;
        }

        /// <summary>
        /// Multiplys variable by number
        /// </summary>
        public void Multiply(int number)
        {
            _coefficient *= number;
        }

        /// <summary>
        /// Multiplys variable by fraction
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Multiply(Fraction fraction)
        {
            _coefficient *= fraction;
        }

        /// <summary>
        /// Divides variable by number
        /// </summary>
        public void Divide(int number)
        {
            _coefficient /= number;
        }

        /// <summary>
        /// Divides variable by fraction
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="DivideByZeroException"></exception>
        public void Divide(Fraction fraction)
        {
            _coefficient /= fraction;
        }

        /// <summary>
        /// Sets label
        /// </summary>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private void SetLabel(string label)
        {
            var regex = new Regex("^([A-Za-z]{1}[0-9]*[']{0,2})$");
            if (!regex.IsMatch(label))
                throw new FormatException("A label must be in format: <letter> or <letter><index>");
            Label = label;
        }

        #region IComparable Members

        /// <summary>
        /// Compares current variable with input
        /// </summary>
        /// <returns>'0' - if they are equals, '-1' - if not</returns>
        public int CompareTo(object obj)
        {
            var variable = (Variable)obj;
            return _coefficient == variable.Coefficient && Power == variable.Power ? 0 : -1;
        }

        #endregion
    }
}