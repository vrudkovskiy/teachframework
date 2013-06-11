using System;
using System.Collections.Generic;
using System.Linq;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Сlass, which solves linear programming problems by simplex-method
    /// </summary>
    public class SimplexMethod
    {
        /// <summary>
        /// Nomalizing for simplex-method
        /// </summary>
        public LppForSimplexMethod Normalize(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            problemCopy = TargetToMinimize(problemCopy);
            problemCopy = MakeFreeCoefficientsNonNegative(problemCopy);
            problemCopy = ChangeMoreThanZeroConstraints(problemCopy);
            problemCopy = ChangeLessThanConstraints(problemCopy);
            problemCopy = ChangeMoreThanAndEqualConstraints(problemCopy);
            problemCopy = ReplaceVariablesWithoutZeroConstraints(problemCopy);

            return problemCopy;
        }

        /// <summary>
        /// Solving by simplex-method
        /// </summary>
        public LppResult Solve(LppForSimplexMethod problem)
        {
            if (!IsNormalized(problem))
                throw new FormatException("Problem must be normalized");

            var table = MakeFirstSimplexTable(problem);
            table = CalculateRatings(table);

            while (!IsEnd(table))
            {
                var solvingElement = GetSolvingElement(table);

                table = NextSimplexTable(table, solvingElement);
            }

            return GetResult(table, problem);
        }

        /// <summary>
        /// Solving by simplex-method
        /// </summary>
        public SimplexTable Solve(SimplexTable table)
        {
            var tableCopy = new SimplexTable(table);

            if (tableCopy.Ratings == null || tableCopy.Ratings.Count == 0)
                tableCopy = CalculateRatings(table);

            while (!IsEnd(tableCopy))
            {
                var solvingElement = GetSolvingElement(tableCopy);

                tableCopy = NextSimplexTable(tableCopy, solvingElement);
            }

            return tableCopy;
        }

        /// <summary>
        /// Returns true if problem can be solved by simplex-method
        /// </summary>
        public bool IsNormalized(LppForSimplexMethod problem)
        {
            return IsTargetFunctionMinimized(problem)
                && AreFreeCoefficientsNonNegative(problem)
                && AreAllConstraintsEqual(problem)
                && DoAllConstraintsHaveBasisVariable(problem)
                && DoAllVariablesHaveZeroConstraint(problem);
        }

        #region [ Methods for solving by simplex-method ]

        /// <summary>
        /// Makes first simplex table(without ratings)
        /// </summary>
        public SimplexTable MakeFirstSimplexTable(LppForSimplexMethod problem)
        {
            return new SimplexTable(problem);
        }

        /// <summary>
        /// Returns new simplex-table with calculated ratings
        /// </summary>
        public SimplexTable CalculateRatings(SimplexTable table)
        {
            var tableCopy = new SimplexTable(table);
            tableCopy.CalculateRatings();
            return tableCopy;
        }

        /// <summary>
        /// Gets pair of indexes, that indicates solving element
        /// </summary>
        /// <param name="table">Table with calculated ratings</param>
        public SolvingElementIndexes GetSolvingElement(SimplexTable table)
        {
            var cellIndex = GetSolvingCellIndex(table);
            return new SolvingElementIndexes { CellIndex = cellIndex, RowIndex = GetSolvingRowIndex(table, cellIndex) };
        }

        /// <summary>
        /// Changes basis(returns new object)
        /// </summary>
        public SimplexTable ChangeBasis(SimplexTable table, SolvingElementIndexes solvingElement)
        {
            var tableCopy = new SimplexTable(table);
            if (solvingElement.RowIndex < 0 || solvingElement.CellIndex < 0) return tableCopy;
            tableCopy.ChangeBasis(solvingElement.RowIndex, solvingElement.CellIndex);
            return tableCopy;
        }

        /// <summary>
        /// Changes basis and calculates ratings
        /// </summary>
        public SimplexTable NextSimplexTable(SimplexTable table, SolvingElementIndexes solvingElement)
        {
            var tableCopy = new SimplexTable(table);
            if (solvingElement.RowIndex < 0 || solvingElement.CellIndex < 0) return tableCopy;
            tableCopy.ChangeBasis(solvingElement.RowIndex, solvingElement.CellIndex);
            tableCopy.CalculateRatings();
            return tableCopy;
        }

        /// <summary>
        /// Returns index of solving cell(or -1 if table has no positive rating)
        /// </summary>
        /// <param name="table">Table with calculated ratings</param>
        private int GetSolvingCellIndex(SimplexTable table)
        {
            var maxRatingIndex = -1;
            foreach (var rating in table.Ratings)
            {
                if (maxRatingIndex == -1)
                {
                    if (rating.CompareTo(0) == 1)
                        maxRatingIndex = table.Ratings.IndexOf(rating);
                    continue;
                }
                if (rating.CompareTo(table.Ratings[maxRatingIndex]) == 1)
                    maxRatingIndex = table.Ratings.IndexOf(rating);
            }
            return maxRatingIndex;
        }

        /// <summary>
        /// Returns index of solving row(or -1 if solving row has no positive elements or solvingCellIndex == -1)
        /// </summary>
        private int GetSolvingRowIndex(SimplexTable table, int solvingCellIndex)
        {
            if (solvingCellIndex < 0) return -1;
            var minRatioIndex = -1;
            var minRatio = new Fraction();
            for (var i = 0; i < table.RowsCount; i++)
            {
                var matrixElement = table.GetMatrixElement(i, solvingCellIndex);
                if (matrixElement <= 0) continue;
                var freeCoefficient = table.GetFreeCoefficient(i);
                var currRatio = freeCoefficient / matrixElement;
                if (currRatio >= minRatio && minRatioIndex != -1) continue;
                minRatioIndex = i;
                minRatio = currRatio;
            }
            return minRatioIndex;
        }

        /// <summary>
        /// Gets acceptable solution of normalixed problem
        /// </summary>
        public LppResult GetNormalizedProblemResult(SimplexTable table, LppForSimplexMethod problem)
        {
            return problem.GetNormalizedProblemResult(table);
        }

        /// <summary>
        /// Gets acceptable solution of initial problem
        /// </summary>
        public LppResult GetInitialProblemResult(SimplexTable table, LppForSimplexMethod problem)
        {
            return problem.GetInitialProblemResult(table);
        }

        /// <summary>
        /// Returns true if result there
        /// </summary>
        public bool IsEnd(SimplexTable table)
        {
            return !HavePositiveRatings(table) || HavePositiveCellsWithoutPositiveElements(table);
        }

        /// <summary>
        /// Gets result of solving
        /// </summary>
        public LppResult GetResult(SimplexTable table, LppForSimplexMethod problem)
        {
            if (!HavePositiveRatings(table))
                return GetInitialProblemResult(table, problem);

            return HavePositiveCellsWithoutPositiveElements(table) ? new LppResult(null, null) : null;
        }

        private bool HavePositiveRatings(SimplexTable table)
        {
            foreach (var rating in table.Ratings)
                if (rating > 0)
                    return true;
            return false;
        }

        private bool HavePositiveCellsWithoutPositiveElements(SimplexTable table)
        {
            for (var j = 0; j < table.Variables.Count; j++)
            {
                if (table.GetRating(table.Variables.ElementAt(j)) <= 0) continue;
                var havePositive = false;
                for (var i = 0; i < table.RowsCount; i++)
                {
                    if (table.GetMatrixElement(i, j) < 0) continue;
                    havePositive = true;
                    break;
                }
                if (!havePositive) return true;
            }
            return false;
        }

        #endregion

        #region [ Methods for normalizing ]

        /// <summary>
        /// 1st step of normalizing
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod TargetToMinimize(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            if (problemCopy.TargetFunction.Target != "min")
            {
                problemCopy.TargetFunction.ChangeTarget();
                problemCopy.IsTargetWasChanged = true;
            }

            return problemCopy;
        }

        /// <summary>
        /// 2nd step of normalizing(can be after or befor changing of more than zero constraints)
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod MakeFreeCoefficientsNonNegative(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            foreach (var constraint in problemCopy.ConstraintSystem)
                if (constraint.RightSide < 0)
                    constraint.Multiply(-1);

            return problemCopy;
        }

        /// <summary>
        /// 2nd step of normalizing(can be after or befor changing of free coefficients)
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ChangeMoreThanZeroConstraints(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            foreach (var constraint in problemCopy.ConstraintSystem)
                if (constraint.Sign == ">=" && constraint.RightSide == 0)
                    constraint.Multiply(-1);

            return problemCopy;
        }

        /// <summary>
        /// 3rd step of normalizing
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ChangeLessThanConstraints(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            for (var i = 0; i < problemCopy.ConstraintCount; i++)
                if (problemCopy.GetConstraint(i).Sign == "<=")
                {
                    problemCopy.AddAdditionalVariable(i);
                    problemCopy.LessThanConstraintsIndexes.Add(i);
                }

            return problemCopy;
        }

        /// <summary>
        /// 4th step of normalizing
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ChangeMoreThanAndEqualConstraints(LppForSimplexMethod problem)
        {
            if (ContainsEqualConstraints(problem))
                return ContainsMoreThanConstraints(problem)
                    ? ChangeBothTypesOfConstraints(problem)
                    : ChangeEqualConstraints(problem);
            return ContainsMoreThanConstraints(problem)
                ? ChangeMoreThanConstraints(problem)
                : new LppForSimplexMethod(problem);
        }

        /// <summary>
        /// 5th step of normalizing
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ReplaceVariablesWithoutZeroConstraints(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            var needReplacement = new List<string>();
            foreach (var tFuncArgument in problemCopy.TargetFunction.Arguments)
            {
                var containedZeroConstraint = false;
                foreach (var zeroConstraint in problemCopy.ZeroConstraints)
                    if (zeroConstraint.LeftSide.ElementAt(0).Label == tFuncArgument)
                    {
                        containedZeroConstraint = true;
                        break;
                    }
                if (!containedZeroConstraint)
                    needReplacement.Add(tFuncArgument);
            }

            foreach (var label in needReplacement)
                problemCopy.AddVariableReplacement(label, label + "'", label + "''");

            return problemCopy;
        }

        /// <summary>
        /// Variant of 4th step of normalizing(should be used if constraint system has only more than constraints, 
        /// except less than constraints, which were changed on previous step)
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ChangeMoreThanConstraints(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);
            //1.
            for (var i = 0; i < problemCopy.ConstraintCount; i++)
                if (!problemCopy.LessThanConstraintsIndexes.Contains(i))
                    problemCopy.AddAdditionalVariable(i);
            //2.
            var maxIndex = EqualConstraintWithBiggestFreeCoefficient(problemCopy);
            //3.
            for (var i = 0; i < problemCopy.ConstraintCount; i++)
            {
                if (problemCopy.LessThanConstraintsIndexes.Contains(i) || i == maxIndex) continue;

                var first = problemCopy.GetConstraint(maxIndex);
                var second = problemCopy.GetConstraint(i);
                second.Multiply(-1);
                second.Add(first);
            }
            //4.
            if (problemCopy.GetBasisVariableLabel(maxIndex) == null)
                problemCopy.AddArtificialVariable(maxIndex);

            return problemCopy;
        }

        /// <summary>
        /// Variant of 4th step of normalizing(should be used if constraint system has only equal constraints, 
        /// except less than constraints, which were changed on previous step)
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ChangeEqualConstraints(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            for (var i = 0; i < problemCopy.ConstraintCount; i++)
                if (problemCopy.GetConstraint(i).Sign == "=" && problemCopy.GetBasisVariableLabel(i) == null)
                    problemCopy.AddArtificialVariable(i);

            return problemCopy;
        }

        /// <summary>
        /// Variant of 4th step of normalizing(should be used if constraint system has more than(>=) and equal(=) constraints, 
        /// except less than constraints, which were changed on previous step)
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ChangeBothTypesOfConstraints(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            //1.
            var maxIndex = EqualConstraintWithBiggestFreeCoefficient(problemCopy);
            //2.
            for (var i = 0; i < problemCopy.ConstraintCount; i++)
            {
                var currConstraint = problemCopy.GetConstraint(i);
                if (problemCopy.LessThanConstraintsIndexes.Contains(i) || currConstraint.Sign != ">=")
                    continue;

                var constraintWithMaxCoef = problemCopy.GetConstraint(maxIndex);

                while (currConstraint.RightSide >= constraintWithMaxCoef.RightSide)
                    currConstraint.Divide(2);

                currConstraint.Multiply(-1);
                problemCopy.AddAdditionalVariable(i);

                currConstraint.Add(constraintWithMaxCoef);
            }
            //3.
            for (var i = 0; i < problemCopy.ConstraintCount; i++)
                if (problemCopy.GetBasisVariableLabel(i) == null)
                    problemCopy.AddArtificialVariable(i);

            return problemCopy;
        }

        /// <summary>
        /// Returns true if constraint system contains more than(>=) constraints(except equal constraints, 
        /// which were changed from less than constraints)
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public bool ContainsMoreThanConstraints(LppForSimplexMethod problem)
        {
            return ContainsSuchConstraints(problem, ">=");
        }

        /// <summary>
        /// Returns true if constraint system contains equal(=) constraints
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public bool ContainsEqualConstraints(LppForSimplexMethod problem)
        {
            return ContainsSuchConstraints(problem, "=");
        }

        /// <summary>
        /// Returns true if constraint system contains constraints with such sign
        /// </summary>
        private bool ContainsSuchConstraints(LppForSimplexMethod problem, string sign)
        {
            for (var i = 0; i < problem.ConstraintCount; i++)
            {
                if (problem.LessThanConstraintsIndexes.Contains(i)) continue;
                if (problem.GetConstraint(i).Sign == sign)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns index of constraint with biggest free coefficient
        /// </summary>
        private int EqualConstraintWithBiggestFreeCoefficient(LppForSimplexMethod problem)
        {
            var maxIndex = -1;
            for (var i = 0; i < problem.ConstraintCount; i++)
            {
                var currConstraint = problem.GetConstraint(i);

                if (currConstraint.Sign != "=" || problem.LessThanConstraintsIndexes.Contains(i)) continue;

                if (maxIndex == -1)
                {
                    maxIndex = i;
                    continue;
                }

                if (currConstraint.RightSide > problem.GetConstraint(maxIndex).RightSide)
                    maxIndex = i;
            }
            return maxIndex;
        }

        #endregion

        #region [ Methods for problem checking ]

        /// <summary>
        /// Returns true if target in problem's target function is min
        /// </summary>
        public bool IsTargetFunctionMinimized(LppForSimplexMethod problem)
        {
            return problem.TargetFunction.Target == "min";
        }

        /// <summary>
        /// Returns true if all free coefficients are more than or equal zero
        /// </summary>
        public bool AreFreeCoefficientsNonNegative(LppForSimplexMethod problem)
        {
            foreach (var constraint in problem.ConstraintSystem)
                if (constraint.RightSide < 0)
                    return false;
            return true;
        }

        /// <summary>
        /// Returns true if problem have more than zero constraint
        /// </summary>
        public bool DoProblemHaveMoreThanZeroConstraint(LppForSimplexMethod problem)
        {
            foreach (var constraint in problem.ConstraintSystem)
                if (constraint.Sign == ">=" && constraint.RightSide == 0)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if problem have less than constraints
        /// </summary>
        public bool DoProblemHaveLessThanConstraint(LppForSimplexMethod problem)
        {
            foreach (var constraint in problem.ConstraintSystem)
                if (constraint.Sign == "<=")
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if all constraints in constraint system are equal
        /// </summary>
        public bool AreAllConstraintsEqual(LppForSimplexMethod problem)
        {
            foreach (var constraint in problem.ConstraintSystem)
                if (constraint.Sign != "=")
                    return false;
            return true;
        }

        /// <summary>
        /// Returns true if all constraints in constraint system have basis variable
        /// </summary>
        public bool DoAllConstraintsHaveBasisVariable(LppForSimplexMethod problem)
        {
            for (var i = 0; i < problem.ConstraintCount; i++)
                if (problem.GetBasisVariableLabel(i) == null)
                    return false;
            return true;
        }

        /// <summary>
        /// Returns true if all variables have constraint 'more than zero'
        /// </summary>
        public bool DoAllVariablesHaveZeroConstraint(LppForSimplexMethod problem)
        {
            var zeroConstraintedVariables = new List<string>();
            foreach (var zeroConstraint in problem.ZeroConstraints)
                zeroConstraintedVariables.Add(zeroConstraint.LeftSide.ElementAt(0).Label);
            foreach (var argument in problem.TargetFunctionArguments)
                if (!zeroConstraintedVariables.Contains(argument))
                    return false;
            return true;
        }

        #endregion
    }
}
