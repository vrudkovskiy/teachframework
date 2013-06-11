using System;
using System.Collections.Generic;

namespace TeachFramework.MathTypes
{
    /// <summary>
    /// F(x) = x1 + x2 + ... + xn -> extremum
    /// </summary>
    public class TargetFunction : IComparable
    {
        private List<Variable> _formula = new List<Variable>();
        private readonly List<string> _arguments = new List<string>();
        private readonly List<string> _variablesWithMaxCoefficient = new List<string>();
        //-------------------------------------------------------------------------------

        /// <summary>
        /// Gets function target
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// Gets function arguments
        /// </summary>
        public ICollection<string> Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets function formula
        /// </summary>
        public ICollection<Variable> Formula
        {
            get { return new List<Variable>(_formula); }
        }
        //===============================================================================

        /// <summary>
        /// Represents target function(f(x) = formula -> target)
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public TargetFunction(ICollection<Variable> formula, string target)
        {
            if (target != "min" && target != "max")
                throw new FormatException("Target must be 'min' or 'max'");
            SetFormula(formula);
            Target = target;
        }

        /// <summary>
        /// Represents target function(f(x) = formula -> target)
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public TargetFunction(ICollection<Variable> formula, string target, IEnumerable<string> variablesWithMaxCoefficient)
        {
            if (target != "min" && target != "max")
                throw new FormatException("Target must be 'min' or 'max'");
            SetFormula(formula);
            Target = target;
            if (variablesWithMaxCoefficient != null)
                _variablesWithMaxCoefficient = new List<string>(variablesWithMaxCoefficient);
        }

        /// <summary>
        /// Represents copy of target function
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        public TargetFunction(TargetFunction targetFunction)
            : this(targetFunction._formula, targetFunction.Target)
        {
            _variablesWithMaxCoefficient = new List<string>(targetFunction._variablesWithMaxCoefficient);

        }

        /// <summary>
        /// Adds variable to formula
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void AddVariable(Variable var)
        {
            if (_arguments.Contains(var.Label))
            {
                for (var i = 0; i < _formula.Count; i++)
                    if (_formula[i].Label == var.Label && _formula[i].Power == var.Power)
                    {
                        var tmp = _formula[i];
                        _formula.Remove(tmp);
                        var newVar = new Variable(var.Coefficient + tmp.Coefficient, var.Label, var.Power);
                        _formula.Add(newVar);
                        return;
                    }
            }
            _formula.Add(new Variable(var.Coefficient, var.Label, var.Power));
            _arguments.Add(var.Label);
        }

        /// <summary>
        /// Removes variable from target function by label
        /// </summary>
        public void RemoveVariable(string variableLabel)
        {
            var removedVariable = new Variable();
            foreach (var variable in _formula)
                if (variable.Label == variableLabel)
                {
                    removedVariable = variable;
                    break;
                }
            _formula.Remove(removedVariable);
            _arguments.Remove(variableLabel);
            if (_variablesWithMaxCoefficient.Contains(variableLabel))
                _variablesWithMaxCoefficient.Remove(variableLabel);
        }

        /// <summary>
        /// Gets coefficient of variable by label
        /// </summary>
        public Fraction GetVariableCoefficient(string label)
        {
            foreach (var variable in _formula)
                if (variable.Label == label)
                    return variable.Coefficient;
            return new Fraction();
        }

        /// <summary>
        /// Adds variable with maximum big coefficient to formula
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void AddVariableWithMaxCoefficient(string label)
        {
            AddVariable(new Variable(new Fraction(999999, 1), label));
            _variablesWithMaxCoefficient.Add(label);
        }

        /// <summary>
        /// Changes target (=> changes variables' sings)
        /// </summary>
        public void ChangeTarget()
        {
            Target = Target == "min" ? "max" : "min";

            foreach (var var in _formula)
                var.Multiply(-1);
        }

        /// <summary>
        /// Returns function in string format
        /// </summary>
        public override string ToString()
        {
            var str = "f(";
            foreach (var tmp in _arguments)
                str += tmp + ", ";
            str = str.Substring(0, str.Length - 2);
            str += ") = ";
            for (var i = 0; i < _formula.Count; i++)
            {
                var var = _formula[i];
                if (_variablesWithMaxCoefficient.Contains(var.Label))
                {
                    str += " + M*" + var.Label;
                    continue;
                }
                if (var.Coefficient >= 0)
                {
                    if (i != 0)
                        str += " + ";
                    str += var.ToString();
                }
                else
                {
                    str += " - " + (var.Coefficient * -1) + var.Label;
                    if (var.Power != 1)
                        str += "^" + var.Power;
                }
            }
            str += " -> " + Target;
            return str;
        }

        /// <summary>
        /// Sets formula
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="FormatException"></exception>
        private void SetFormula(ICollection<Variable> formula)
        {
            if (formula.Count == 0)
                throw new FormatException("Formula can not be empty");
            _formula = new List<Variable>();
            foreach (var var in formula)
                AddVariable(var);
        }

        #region IComparable Members

        /// <summary>
        /// Compares current target function with input one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>'0' - if they are equals, '-1' - if are not</returns>
        public int CompareTo(object obj)
        {
            return IsTargetFunctionsEquals(this, (TargetFunction)obj) ? 0 : -1;
        }

        #endregion

        /// <summary>
        /// Compares two target functions
        /// </summary>
        public static bool IsTargetFunctionsEquals(TargetFunction first, TargetFunction second)
        {
            if (first.Target != second.Target || first.Arguments.Count != second.Arguments.Count)
                return false;
            foreach (var varF in first.Formula)
            {
                var flag = false;
                foreach (var varS in second.Formula)
                {
                    if (varF.Label != varS.Label) continue;
                    if (varF.Coefficient != varS.Coefficient || varF.Power != varS.Power)
                        return false;
                    flag = true;
                    break;
                }
                if (flag == false)
                    return false;
            }
            return true;
        }
    }
}
