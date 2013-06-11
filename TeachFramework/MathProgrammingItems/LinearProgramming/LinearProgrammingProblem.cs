using System;
using System.Collections.Generic;
using System.Linq;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Linear programming problem description(1. target function, 2. limitation system, 3. zero limitations)
    /// </summary>
    public class LinearProgrammingProblem : IComparable
    {
        /// <summary>
        /// Gets target function
        /// </summary>
        public TargetFunction TargetFunction { get; set; }

        /// <summary>
        /// Gets constraint system(without zero constraints)
        /// </summary>
        public List<Constraint> ConstraintSystem { get; private set; }

        /// <summary>
        /// Gets zero constraints
        /// </summary>
        public List<Constraint> ZeroConstraints { get; private set; }

        /// <summary>
        /// Gets count of constraints in constraintSystem(without zero constraints)
        /// </summary>
        public int ConstraintCount
        {
            get { return ConstraintSystem.Count; }
        }

        #region [ Constractors ]

        /// <summary>
        /// Represents linear programming problem(1. target function, 2. constraint system, 3. zero constraints)
        /// </summary>
        /// <param name="targetFunction"></param>
        /// <param name="constraints">Constraint system with zero constraints</param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public LinearProgrammingProblem(TargetFunction targetFunction, ICollection<Constraint> constraints)
        {
            if (!IsLinearFormula(targetFunction.Formula))
                throw new FormatException("Target function must be linear");

            TargetFunction = new TargetFunction(targetFunction);
            SetConstraintSystem(constraints);
            CompleteTargetFunction();
        }

        /// <summary>
        /// Represents linear programming problem(1. target function, 2. constraint system, 3. zero constraints)
        /// </summary>
        /// <param name="targetFunction"></param>
        /// <param name="constraints">Constraint system without zero constraints</param>
        /// <param name="zeroConstraints">Zero constraints</param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public LinearProgrammingProblem(TargetFunction targetFunction, IEnumerable<Constraint> constraints,
            IEnumerable<Constraint> zeroConstraints)
            : this(targetFunction, CombineConstraints(constraints, zeroConstraints))
        { }

        /// <summary>
        /// Represents linear programming problem(1. target function, 2. limitation system, 3. zero limitations)
        /// </summary>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public LinearProgrammingProblem(IList<string> problemDescription)
            : this(LppParser.ParseTargetFunction(problemDescription), LppParser.ParseConstraints(problemDescription))
        { }

        /// <summary>
        /// Represents copy of linear programming problem
        /// </summary>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public LinearProgrammingProblem(LinearProgrammingProblem problem)
            : this(problem.TargetFunction, problem.ConstraintSystem, problem.ZeroConstraints)
        { }

        /// <summary>
        /// Sets constraint system(with zero constraints)
        /// </summary>
        /// <param name="constraints">Constraint system with zero constraints</param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private void SetConstraintSystem(ICollection<Constraint> constraints)
        {
            foreach (var constraint in constraints)
                if (!IsLinearFormula(constraint.LeftSide))
                    throw new FormatException("Constraint system must be linear");

            ConstraintSystem = new List<Constraint>();
            ZeroConstraints = new List<Constraint>();
            foreach (var constraint in constraints)
            {
                if (constraint.LeftSide.Count == 1 && constraint.LeftSide.ElementAt(0).Coefficient == 1
                    && constraint.Sign == ">=" && constraint.RightSide == 0)
                    ZeroConstraints.Add(new Constraint(constraint));
                else
                    ConstraintSystem.Add(new Constraint(constraint));
            }
            if (ConstraintSystem.Count < 2)
                throw new FormatException("Number of constraints in constraint system must be more than 1");
        }

        /// <summary>
        /// Adds missing variables to target function with coefficient '0'(only which are contained in constraint system)
        /// </summary>
        protected void CompleteTargetFunction()
        {
            var combinedConstraintSys = CombineConstraints(ConstraintSystem, ZeroConstraints);
            foreach (var constraint in combinedConstraintSys)
                foreach (var variable in constraint.LeftSide)
                    if (!TargetFunction.Arguments.Contains(variable.Label))
                        TargetFunction.AddVariable(new Variable("0/0", variable.Label));
        }

        #endregion

        /// <summary>
        /// Gets constraint from constraint system by index
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Constraint GetConstraint(int index)
        {
            return ConstraintSystem[index];
        }

        /// <summary>
        /// Gets constraint system(with zero constraints)
        /// </summary>
        public ICollection<Constraint> GetAllConstraints()
        {
            var tmpConstraintSystem = new List<Constraint>(ConstraintSystem);
            tmpConstraintSystem.AddRange(ZeroConstraints);
            return tmpConstraintSystem;
        }

        /// <summary>
        /// Returns true if formula is linear
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private static bool IsLinearFormula(IEnumerable<Variable> formula)
        {
            foreach (var var in formula)
                if (var.Power != 1)
                    return false;
            return true;
        }

        /// <summary>
        /// Combines constraint lists into one new list
        /// </summary>
        /// <returns></returns>
        private static List<Constraint> CombineConstraints(IEnumerable<Constraint> first, IEnumerable<Constraint> second)
        {
            var tmpConstraints = new List<Constraint>(first);
            tmpConstraints.AddRange(second);
            return tmpConstraints;
        }

        //---------------------------------------------------------------------------------------
        #region [ IComparable Members ]

        /// <summary>
        /// Compares current LPP with input one
        /// </summary>
        public int CompareTo(object obj)
        {
            var problem = (LinearProgrammingProblem)obj;
            if (TargetFunction.CompareTo(problem.TargetFunction) != 0)
                return -1;
            if (!ConstraintSystemsEquals(ConstraintSystem, problem.ConstraintSystem))
                return -1;
            if (!ConstraintSystemsEquals(ZeroConstraints, problem.ZeroConstraints))
                return -1;
            return 0;
        }

        private static bool ConstraintSystemsEquals(ICollection<Constraint> first, ICollection<Constraint> second)
        {
            if (first.Count != second.Count)
                return false;
            foreach (var fLim in first)
            {
                var flag = false;
                foreach (var sLim in second)
                {
                    if (fLim.CompareTo(sLim) != 0) continue;
                    flag = true;
                    break;
                }
                if (flag == false)
                    return false;
            }
            return true;
        }

        #endregion

    }
}
