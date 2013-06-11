using System;

namespace TeachFramework.MathProgramming
{
    public class DualSimplexMethod : SimplexMethod
    {
        public new LppForSimplexMethod Normalize(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            problemCopy = TargetToMinimize(problemCopy);
            problemCopy = ConstraintsToLessThanForm(problemCopy);
            problemCopy = ChangeLessThanConstraints(problemCopy);
            problemCopy = ChangeEqualConstraints(problemCopy);
            problemCopy = ReplaceVariablesWithoutZeroConstraints(problemCopy);

            return problemCopy;
        }

        /// <summary>
        /// Returns true if problem can be solved by simplex-method
        /// </summary>
        public new bool IsNormalized(LppForSimplexMethod problem)
        {
            return IsTargetFunctionMinimized(problem)
                && AreAllConstraintsEqual(problem)
                && DoAllConstraintsHaveBasisVariable(problem)
                && DoAllVariablesHaveZeroConstraint(problem);
        }

        /// <summary>
        /// Solving by simplex-method
        /// </summary>
        public new LppResult Solve(LppForSimplexMethod problem)
        {
            if (!IsNormalized(problem))
                throw new FormatException("Problem must be normalized");

            var table = MakeFirstSimplexTable(problem);

            while (!IsEnd(table))
            {
                table = CalculateRatings(table);

                var solvingElement = GetSolvingElement(table);

                table = ChangeBasis(table, solvingElement);
            }

            return GetResult(table, problem);
        }

        /// <summary>
        /// Solving by simplex-method
        /// </summary>
        public new SimplexTable Solve(SimplexTable table)
        {
            var tableCopy = new SimplexTable(table);

            if (tableCopy.Ratings == null || tableCopy.Ratings.Count == 0)
                tableCopy = CalculateRatings(table);

            while (!IsEnd(tableCopy))
            {
                tableCopy = CalculateRatings(tableCopy);

                var solvingElement = GetSolvingElement(tableCopy);

                tableCopy = ChangeBasis(tableCopy, solvingElement);
            }
            tableCopy = CalculateRatings(tableCopy);

            return tableCopy;
        }

        #region [ Methods for normalizing ]

        /// <summary>
        /// 2nd step of normalizing
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <returns>Changed problem(new object)</returns>
        public LppForSimplexMethod ConstraintsToLessThanForm(LppForSimplexMethod problem)
        {
            var problemCopy = new LppForSimplexMethod(problem);

            foreach (var lim in problemCopy.ConstraintSystem)
                if (lim.Sign == ">=")
                    lim.Multiply(-1);

            return problemCopy;
        }

        #endregion

        #region [ Methods for solving by dual simplex-method ]

        /// <summary>
        /// Gets pair of indexes, that indicates solving element
        /// </summary>
        /// <param name="table">Table with calculated ratings</param>
        public new SolvingElementIndexes GetSolvingElement(SimplexTable table)
        {
            var rowIndex = GetSolvingRowIndex(table);
            return new SolvingElementIndexes { CellIndex = GetSolvingCellIndex(table, rowIndex), RowIndex = rowIndex };
        }

        /// <summary>
        /// Returns index of solving cell(or -1 if table has no positive rating)
        /// </summary>
        /// <param name="table">Table with calculated ratings</param>
        /// <param name="solvingRowIndex">Index of solving row</param>
        private int GetSolvingCellIndex(SimplexTable table, int solvingRowIndex)
        {
            if (solvingRowIndex < 0) return -1;
            var minRatioIndex = -1;
            var minRatio = new MaxCoefficient();
            for (var i = 0; i < table.Variables.Count; i++)
            {
                var matrixElement = table.GetMatrixElement(solvingRowIndex, i);
                if (matrixElement >= 0) continue;
                var rating = table.Ratings[i];
                var currRatio = new MaxCoefficient(rating);
                currRatio.Multiply(1 / matrixElement);
                if (currRatio.CompareTo(minRatio) != -1 && minRatioIndex != -1) continue;
                minRatioIndex = i;
                minRatio = currRatio;
            }
            return minRatioIndex;
        }

        /// <summary>
        /// Returns index of solving row(or -1 if solving row has no positive elements or solvingCellIndex == -1)
        /// </summary>
        private int GetSolvingRowIndex(SimplexTable table)
        {
            var minFreeCoefficientIndex = -1;
            for (var i = 0; i < table.RowsCount; i++)
            {
                if (minFreeCoefficientIndex == -1)
                {
                    if (table.GetFreeCoefficient(i) < 0)
                        minFreeCoefficientIndex = i;
                    continue;
                }
                if (table.GetFreeCoefficient(i) < table.GetFreeCoefficient(minFreeCoefficientIndex))
                    minFreeCoefficientIndex = i;
            }
            return minFreeCoefficientIndex;
        }

        /// <summary>
        /// Returns true if result there
        /// </summary>
        public new bool IsEnd(SimplexTable table)
        {
            return !HaveNegativeFreeCoefficients(table) || HaveNegativeRowsWithoutNegativeElements(table);
        }

        /// <summary>
        /// Gets result of solving
        /// </summary>
        public new LppResult GetResult(SimplexTable table, LppForSimplexMethod problem)
        {
            if (!HaveNegativeFreeCoefficients(table))
                return GetInitialProblemResult(table, problem);

            return HaveNegativeRowsWithoutNegativeElements(table) ? new LppResult(null, null) : null;
        }

        private bool HaveNegativeFreeCoefficients(SimplexTable table)
        {
            foreach (var coefficient in table.GetBasis().Values)
                if (coefficient < 0)
                    return true;
            return false;
        }

        private bool HaveNegativeRowsWithoutNegativeElements(SimplexTable table)
        {
            foreach (var basisVar in table.GetBasis())
            {
                if (basisVar.Value >= 0) continue;
                var haveNegative = false;
                foreach (var tableElement in table.GetRow(basisVar.Key).Key)
                {
                    if (tableElement >= 0) continue;
                    haveNegative = true;
                    break;
                }
                if (!haveNegative) return true;
            }
            return false;
        }

        #endregion
    }
}
