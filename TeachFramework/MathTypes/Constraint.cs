using System;
using System.Collections.Generic;
using System.Globalization;

namespace TeachFramework.MathTypes
{
    /// <summary>
    /// x1 + x2 + ... + xn >= some constant 
    /// </summary>
    public class Constraint : IComparable
    {
        private List<Variable> _leftSide;
        private Fraction _rightSide;
        //----------------------------------------------
        /// <summary>
        /// Gets left side
        /// </summary>
        public ICollection<Variable> LeftSide
        {
            get { return new List<Variable>(_leftSide); }
        }

        /// <summary>
        /// Gets comparison sign
        /// </summary>
        public string Sign { get; private set; }

        /// <summary>
        /// Gets right side
        /// </summary>
        public Fraction RightSide
        {
            get { return new Fraction(_rightSide); }
        }
        //=================================================
        /// <summary>
        /// Represents math Constraint(equality or inequality)
        /// </summary>
        public Constraint(ICollection<Variable> leftSide, string sign, Fraction rightSide)
        {
            if (leftSide.Count == 0)
                throw new FormatException("Formula shouldn't be empty");
            if (sign != ">" && sign != ">=" && sign != "<" && sign != "<=" && sign != "=")
                throw new FormatException("Invalid format of comparison sign");

            _leftSide = new List<Variable>(leftSide);
            Sign = sign;
            _rightSide = new Fraction(rightSide);
        }

        /// <summary>
        /// Represents math Constraint(equality or inequality)
        /// </summary>
        public Constraint(ICollection<Variable> leftSide, string sign, string rightSide)
            : this(leftSide, sign, new Fraction(rightSide))
        { }

        /// <summary>
        /// Represents math Constraint(copy of input Constraint)
        /// </summary>
        public Constraint(Constraint constraint)
        {
            _leftSide = new List<Variable>(constraint._leftSide);
            Sign = constraint.Sign;
            _rightSide = new Fraction(constraint._rightSide);
        }

        /// <summary>
        /// Adds variable with or without sign change
        /// </summary>
        /// <param name="var">Variable</param>
        /// <param name="withSignChange">If true change sign('>=' -> '=', if var.Coefficient less than 0)</param>
        public void AddVariable(Variable var, bool withSignChange)
        {
            if (withSignChange)
            {
                if (var.Coefficient > 0)
                    Sign = Sign == "<=" || Sign == "<" ? "=" : Sign;
                if (var.Coefficient < 0)
                    Sign = Sign == ">=" || Sign == ">" ? "=" : Sign;
            }

            for (var i = 0; i < _leftSide.Count; i++)
                if (_leftSide[i].Label == var.Label && _leftSide[i].Power == var.Power)
                {
                    var tmp = _leftSide[i];
                    _leftSide.Remove(tmp);
                    var newVar = new Variable(var.Coefficient + tmp.Coefficient, var.Label, var.Power);
                    _leftSide.Add(newVar);
                    return;
                }

            _leftSide.Add(new Variable(var));
        }

        /// <summary>
        /// Removes variable by label(without sign change)
        /// </summary>
        public void RemoveVariable(string label)
        {
            foreach (var variable in _leftSide)
                if (variable.Label == label)
                {
                    _leftSide.Remove(variable);
                    break;
                }
        }

        /// <summary>
        /// Adds input constraint to current
        /// </summary>
        /// <param name="constraint">Constraint that must be equal</param>
        /// <exception cref="FormatException">If one of the constraints is not equal</exception>
        public void Add(Constraint constraint)
        {
            if (constraint.Sign != "=" || Sign != "=")
                throw new FormatException("Constraints must be equal constraints");

            var current = new EqualityConstraint(LeftSide, RightSide);
            var input = new EqualityConstraint(constraint.LeftSide, constraint.RightSide);

            current.Add(input);

            _leftSide = new List<Variable>(current.LeftSide);
            _rightSide = current.RightSide;
        }

        /// <summary>
        /// Multiplys Constraint(left and right sides) by number
        /// </summary>
        public void Multiply(int number)
        {
            for (var i = 0; i < _leftSide.Count; i++)
            {
                _leftSide[i] = new Variable(_leftSide[i]);
                _leftSide[i].Multiply(number);
            }

            if (number < 0)
                ChangeSign();

            _rightSide *= number;
        }

        /// <summary>
        /// Multiplys Constraint(left and right sides) by fraction
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Multiply(Fraction fraction)
        {
            for (var i = 0; i < _leftSide.Count; i++)
            {
                _leftSide[i] = new Variable(_leftSide[i]);
                _leftSide[i].Multiply(fraction);
            }
            //foreach (Variable var in this._leftSide)
            //    var.Multiply(fraction);

            if (fraction < 0)
                ChangeSign();

            _rightSide *= fraction;
        }

        /// <summary>
        /// Divides Constraint(left and right sides) by number
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        public void Divide(int number)
        {
            for (var i = 0; i < _leftSide.Count; i++)
            {
                _leftSide[i] = new Variable(_leftSide[i]);
                _leftSide[i].Divide(number);
            }
            //foreach (Variable var in this._leftSide)
            //    var.Divide(number);

            if (number < 0)
                ChangeSign();

            _rightSide /= number;
        }

        /// <summary>
        /// Divides Constraint(left and right sides) by fraction
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public void Divide(Fraction fraction)
        {
            for (var i = 0; i < _leftSide.Count; i++)
            {
                _leftSide[i] = new Variable(_leftSide[i]);
                _leftSide[i].Divide(fraction);
            }
            //foreach (Variable var in this._leftSide)
            //    var.Divide(fraction);

            if (fraction < 0)
                ChangeSign();

            _rightSide /= fraction;
        }

        /// <summary>
        /// Gets Constraint in string format
        /// </summary>
        public override string ToString()
        {
            var str = _leftSide[0].Coefficient.ToString();
            str += _leftSide[0].Label;
            if (_leftSide[0].Power != 1)
                str += "^" + _leftSide[0].Power.ToString(CultureInfo.CurrentCulture);

            for (var i = 1; i < _leftSide.Count; i++)
            {
                if (_leftSide[i].Coefficient < 0)
                    str += " - " + (_leftSide[i].Coefficient.Numerator * -1);
                else
                    str += " + " + _leftSide[i].Coefficient.Numerator;
                if (_leftSide[i].Coefficient.Denominator != 0 && _leftSide[i].Coefficient.Denominator != 1)
                {
                    str += "/";
                    str += _leftSide[i].Coefficient.Denominator.ToString(CultureInfo.CurrentCulture);
                }
                str += _leftSide[i].Label;

                if (_leftSide[i].Power != 1)
                    str += "^" + _leftSide[i].Power.ToString(CultureInfo.CurrentCulture);
            }

            str += " " + Sign + " " + _rightSide;

            return str;
        }

        private void ChangeSign()
        {
            if (Sign == "=") return;
            Sign = (Sign[0] == '<' ? ">" : "<") + (Sign.Length == 2 ? "=" : "");
        }

        /// <summary>
        /// Sets Constraint free constant
        /// </summary>
        protected void SetRightSide(Fraction rightSide)
        {
            _rightSide = new Fraction(rightSide);
        }

        #region IComparable Members

        /// <summary>
        /// Compares current Constraint with input one
        /// </summary>
        /// <returns>'0' - if they are equals, '-1' - if are not</returns>
        public int CompareTo(object obj)
        {
            return AreConstraintsEquals(this, (Constraint)obj) ? 0 : -1;
        }

        #endregion

        /// <summary>
        /// Compares two Constraints
        /// </summary>
        public static bool AreConstraintsEquals(Constraint first, Constraint second)
        {
            if (first.Sign != second.Sign || first.RightSide != second.RightSide)
                return false;

            for (var i = 0; i < 2; i++)
            {
                foreach (var var in first.LeftSide)
                {
                    var sCoeff = GetCoefficientByLabel(second, var.Label);
                    if (sCoeff == null)
                    {
                        if (var.Coefficient == 0)
                            continue;
                        return false;
                    }
                    if (var.Coefficient != sCoeff)
                        return false;
                }
                var tmp = first;
                first = second;
                second = tmp;
            }

            return true;
        }

        private static Fraction GetCoefficientByLabel(Constraint lim, string label)
        {
            foreach (var var in lim.LeftSide)
                if (var.Label == label)
                    return var.Coefficient;
            return null;
        }
    }
}