using System;
using System.Linq;
using System.Collections.Generic;
using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Linear programming problem, which:\n
    /// - contains methods for symplex-method;\n
    /// - contains initial problem
    /// </summary>
    public class LppForSimplexMethod : LinearProgrammingProblem
    {
        /// <summary>
        /// Indicates target function change
        /// </summary>
        public bool IsTargetWasChanged { get; set; }

        /// <summary>
        /// Problem befor changes
        /// </summary>
        public LinearProgrammingProblem InitialProblem { get; private set; }

        /// <summary>
        /// Target function's arguments of changed problem
        /// </summary>
        public ICollection<string> TargetFunctionArguments
        {
            get { return TargetFunction.Arguments; }
        }

        /// <summary>
        /// Labels of variables, which has max coefficient in target function of changed problem 
        /// </summary>
        public List<string> VariablesWithMaxCoefficient { get; private set; }

        /// <summary>
        /// Indexes
        /// </summary>
        public List<int> LessThanConstraintsIndexes { get; private set; }

        /// <summary>
        /// Replaced variables, which didn't have zero constraint(x = x' - x'')
        /// </summary>
        public Dictionary<string, KeyValuePair<string, string>> Replacements { get; private set; }

        #region [ Constractors ]

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public LppForSimplexMethod(LinearProgrammingProblem problem)
            : base(problem)
        {
            InitializeComponents();
            InitialProblem = new LinearProgrammingProblem(problem);
        }

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public LppForSimplexMethod(LppForSimplexMethod problem)
            : base(problem.TargetFunction, problem.ConstraintSystem, problem.ZeroConstraints)
        {
            InitializeComponents();
            InitialProblem = new LinearProgrammingProblem(problem.InitialProblem);
            VariablesWithMaxCoefficient.AddRange(problem.VariablesWithMaxCoefficient);
            LessThanConstraintsIndexes.AddRange(problem.LessThanConstraintsIndexes);
            foreach (var replacement in problem.Replacements)
                Replacements.Add(replacement.Key,
                                 new KeyValuePair<string, string>(replacement.Value.Key, replacement.Value.Value));
            IsTargetWasChanged = problem.IsTargetWasChanged;
        }

        /// <summary>
        /// Initialize components as default values
        /// </summary>
        private void InitializeComponents()
        {
            VariablesWithMaxCoefficient = new List<string>();
            Replacements = new Dictionary<string, KeyValuePair<string, string>>();
            LessThanConstraintsIndexes = new List<int>();
        }

        #endregion

        /// <summary>
        /// Gets result of solving of initial problem by result of solving normalized problem
        /// </summary>
        public LppResult GetInitialProblemResult(SimplexTable table)
        {
            var result = GetNormalizedProblemResult(table);
            if (result.Coordinates == null || result.Value == null)
                return result;

            var point = new Dictionary<string, Fraction>();
            foreach (var variable in InitialProblem.TargetFunction.Arguments)
            {
                if (!Replacements.ContainsKey(variable))
                {
                    point.Add(variable, result.Coordinates[variable]);
                    continue;
                }
                //var positiveVar = result.Coordinates[Replacements[variable].Key];
                //var negativeVar = result.Coordinates[Replacements[variable].Value];
                //point.Add(variable, (positiveVar - negativeVar));
                point.Add(variable, GetReplacedVariableValue(variable, result));
            }

            return new LppResult(point, result.Value * (IsTargetWasChanged ? -1 : 1));
        }

        /// <summary>
        /// Gets result of solving of normalized problem
        /// </summary>
        public LppResult GetNormalizedProblemResult(SimplexTable table)
        {
            for (var i = 0; i < table.Variables.Count; i++)
            {
                if (table.GetRating(table.Variables.ElementAt(i)) <= 0) continue;
                var allNegative = true;
                for (var j = 0; j < table.RowsCount; j++)
                {
                    if (table.GetMatrixElement(j, i) >= 0)
                        allNegative = false;
                }
                if (allNegative)
                    return new LppResult(null, null);
            }
            return GetResult(table, VariablesWithMaxCoefficient);
        }

        private static LppResult GetResult(SimplexTable table, ICollection<string> varsWithMaxCoefficient)
        {
            var point = new Dictionary<string, Fraction>();
            var basis = table.GetBasis();
            var isCorrect = true;
            foreach (var variable in table.Variables)
            {
                if (varsWithMaxCoefficient.Contains(variable) && basis.ContainsKey(variable)
                    && basis[variable] != 0)
                    isCorrect = false;
                point.Add(variable, (basis.ContainsKey(variable) ? basis[variable] : new Fraction()));
            }

            return new LppResult(point, isCorrect ? table.CalculateFunctionValue().FreeCoefficient : null);
        }

        private Fraction GetReplacedVariableValue(string label, LppResult normalizedProblemResult)
        {
            var positiveLab = Replacements[label].Key;
            var negativeLab = Replacements[label].Value;
            var positive = new Fraction();
            var negative = new Fraction();

            if (normalizedProblemResult.Coordinates.Keys.Contains(positiveLab))
                positive += normalizedProblemResult.Coordinates[positiveLab];
            if (normalizedProblemResult.Coordinates.Keys.Contains(negativeLab))
                negative += normalizedProblemResult.Coordinates[negativeLab];

            return positive - negative;
        }

        /// <summary>
        /// Gets label, which target function doesn't contain
        /// </summary>
        public string GetFreeLabel()
        {
            for (var i = 1; i < 1000; i++)
            {
                var freeLabel = "x" + i;
                if (!TargetFunction.Arguments.Contains(freeLabel)) return freeLabel;
            }
            return null;
        }

        /// <summary>
        /// Gets basis variable by constraint index
        /// </summary>
        public string GetBasisVariableLabel(int index)
        {
            foreach (var indicatedConstraintVar in GetConstraint(index).LeftSide)
            {
                if (indicatedConstraintVar.Coefficient != 1) continue;
                var flag = false;
                for (var i = 0; i < ConstraintCount && flag == false; i++)
                {
                    if (i == index) continue;
                    foreach (var variable in GetConstraint(i).LeftSide)
                    {
                        if (variable.Label != indicatedConstraintVar.Label || variable.Coefficient == 0) continue;
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                    return indicatedConstraintVar.Label;
            }
            return null;
        }

        /// <summary>
        /// Adds artifical variable to constraint by index(with target function change)
        /// </summary>
        public void AddArtificialVariable(int constraintIndex)
        {
            var label = GetFreeLabel();

            GetConstraint(constraintIndex).AddVariable(new Variable("1/1", label), false);

            TargetFunction.AddVariableWithMaxCoefficient(label);

            AddZeroConstraint(label);

            VariablesWithMaxCoefficient.Add(label);
        }

        /// <summary>
        /// Adds additional variable to constraint by index(with target function change)
        /// </summary>
        public void AddAdditionalVariable(int constraintIndex)
        {
            var label = GetFreeLabel();

            var constraint = GetConstraint(constraintIndex);
            var coefficient = constraint.Sign == ">=" ? "-1/1" : "1/1";
            //if (constraint.Sign == "<=")
            //    LessThanConstraintsIndexes.Add(constraintIndex);
            constraint.AddVariable(new Variable(coefficient, label), true);

            TargetFunction.AddVariable(new Variable("0/0", label));

            AddZeroConstraint(label);
        }

        /// <summary>
        /// Adds replacement: x = x' - x''
        /// </summary>
        /// <param name="oldVarLabel">x</param>
        /// <param name="newPositiveVarLabel">x'</param>
        /// <param name="newNegativeVarLabel">x''</param>
        public void AddVariableReplacement(string oldVarLabel, string newPositiveVarLabel, string newNegativeVarLabel)
        {
            if (TargetFunction.Arguments.Contains(newPositiveVarLabel)
                    || TargetFunction.Arguments.Contains(newNegativeVarLabel))
                throw new ArgumentException("Problem has already contained replacing variable");

            if (TargetFunction.Arguments.Contains(oldVarLabel))
            {
                var coefficient = TargetFunction.GetVariableCoefficient(oldVarLabel);

                TargetFunction.AddVariable(new Variable(coefficient, newPositiveVarLabel));
                TargetFunction.AddVariable(new Variable(coefficient * -1, newNegativeVarLabel));

                TargetFunction.RemoveVariable(oldVarLabel);
            }

            foreach (var constraint in ConstraintSystem)
                foreach (var variable in constraint.LeftSide)
                {
                    if (variable.Label != oldVarLabel) continue;
                    constraint.AddVariable(new Variable(variable.Coefficient, newPositiveVarLabel), false);
                    constraint.AddVariable(new Variable(variable.Coefficient * -1, newNegativeVarLabel), false);
                    constraint.RemoveVariable(oldVarLabel);
                }

            AddZeroConstraint(newPositiveVarLabel);
            AddZeroConstraint(newNegativeVarLabel);

            CompleteTargetFunction();

            Replacements.Add(oldVarLabel, new KeyValuePair<string, string>(newPositiveVarLabel, newNegativeVarLabel));
        }

        /// <summary>
        /// Adds zero constrain for specified variable
        /// </summary>
        private void AddZeroConstraint(string variableLabel)
        {
            var variable = new Variable("1/1", variableLabel);
            ZeroConstraints.Add(new Constraint(new List<Variable> { variable }, ">=", "0/0"));
        }
    }
}
