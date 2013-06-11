using System;
using System.Collections.Generic;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Class, which solves discrete programming problems by Gomory's First method
    /// </summary>
    public class GomoryFirst
    {
        private readonly SimplexMethod _simplex = new SimplexMethod();
        private readonly DualSimplexMethod _dualSimplex = new DualSimplexMethod();
        //------------------------------------------------------------------------------------

        /// <summary>
        /// Solving by Gompry's first method
        /// </summary>
        public LppResult Solve(ProblemForGomory problem)
        {
            if (!IsSuitable(problem))
                throw new FormatException("Problem isn't suitable for Gomory's first method");

            SimplexTable table;
            LppForSimplexMethod normalizedForSimplex;

            var result = SolveInitialWeekenedProblem(problem, out normalizedForSimplex, out table);

            while (!IsEnd(result, problem))
            {
                var creativeVar = GetCreativeVariable(table, problem);

                var basisVar = GetFreeBasisVariableLabel(table);
                var cutoff = MakeCutoff(table, creativeVar, problem);

                table = AddCutoff(table, basisVar, cutoff);

                result = SolveWeekenedProblemWithCutoff(problem, table, out table);
            }

            return GetInitialProblemResult(table, normalizedForSimplex);
        }

        /// <summary>
        /// Indicates is problem suitable for solving by Gomory method
        /// </summary>
        public bool IsSuitable(DiscreteProgrammingProblem problem)
        {
            return !HaveNotWholeCoefficient(problem.GetAllConstraints()) && !HaveNotWholeVariable(problem);
        }

        /// <summary>
        /// Indicates is problem suitable for solving by Gomory method
        /// </summary>
        public bool IsSuitable(ProblemForGomory problem)
        {
            return !HaveNotWholeCoefficient(problem.GetAllConstraints()) && !HaveNotWholeVariable(problem);
        }

        /// <summary>
        /// Returns true if weakened problem is normalized
        /// </summary>
        public bool IsWeakenedProblemNormalized(ProblemForGomory problem)
        {
            return _simplex.IsNormalized(problem);
        }

        #region [ Methods for solving ]

        /// <summary>
        /// Solves weakened problem(without whole constraints) by simplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <param name="normalizedProblem"></param>
        /// <param name="lastSimplexTable">Last simplex-table(optimal plan)</param>
        /// <returns></returns>
        public LppResult SolveInitialWeekenedProblem(ProblemForGomory problem,
            out LppForSimplexMethod normalizedProblem, out SimplexTable lastSimplexTable)
        {
            normalizedProblem = _simplex.Normalize(problem);
            lastSimplexTable = _simplex.MakeFirstSimplexTable(normalizedProblem);
            lastSimplexTable = _simplex.Solve(lastSimplexTable);

            return _simplex.GetNormalizedProblemResult(lastSimplexTable, normalizedProblem);
        }

        /// <summary>
        /// Returns true if result of weakened problem solving is result of initial problem solving
        /// </summary>
        public bool IsEnd(LppResult result, ProblemForGomory problem)
        {
            return result.Value == null || IsWhole(result, problem);
        }

        /// <summary>
        /// Gets variable, which indicates creative row
        /// </summary>
        public string GetCreativeVariable(SimplexTable table, ProblemForGomory problem)
        {
            var maxVariables = new List<string>();
            var maxFractionalPart = new Fraction();
            foreach (var basisVariable in table.GetBasis())
            {
                if (!problem.WholeConstraints.Contains(basisVariable.Key))
                    continue;
                var fractionalPart = GetFractionalPart(basisVariable.Value);
                if (fractionalPart > maxFractionalPart)
                {
                    maxVariables.Clear();
                    maxFractionalPart = fractionalPart;
                }
                if (fractionalPart == maxFractionalPart)
                    maxVariables.Add(basisVariable.Key);
            }
            if (maxVariables.Count == 1) return maxVariables[0];

            maxFractionalPart = new Fraction();
            var maxVariable = string.Empty;
            foreach (var variable in maxVariables)
            {
                var sumOfFractionalParts = new Fraction();
                foreach (var rowVariable in table.GetRow(variable).Key)
                    sumOfFractionalParts += GetFractionalPart(rowVariable);
                var ratio = sumOfFractionalParts != 0
                                ? GetFractionalPart(table.GetRow(variable).Value) / sumOfFractionalParts
                                : new Fraction();
                if (ratio <= maxFractionalPart) continue;
                maxFractionalPart = ratio;
                maxVariable = variable;
            }
            return maxVariable;
        }

        /// <summary>
        /// Get free variable label( Si )
        /// </summary>
        public string GetFreeBasisVariableLabel(SimplexTable table)
        {
            var label = string.Empty;
            for (var i = 1; i < 1000; i++)
            {
                label = "S" + i;
                if (!table.Variables.Contains(label)) break;
            }
            return label;
        }

        /// <summary>
        /// Makes cutoff
        /// </summary>
        public KeyValuePair<Fraction[], Fraction> MakeCutoff(SimplexTable table,
            string creativeVar, ProblemForGomory problem)
        {
            var j = 0;
            var creativeRow = table.GetRow(creativeVar);
            var row = new Fraction[creativeRow.Key.Length + 1];

            foreach (var coefficient in creativeRow.Key)
                row[j++] = GetFractionalPart(coefficient) * -1;

            row[j] = new Fraction(1, 1);

            return new KeyValuePair<Fraction[], Fraction>(row, GetFractionalPart(creativeRow.Value) * -1);
        }

        /// <summary>
        /// Adds cutoff to simplex-table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="basisVarLabel">Si</param>
        /// <param name="cutoff">Si - Sum(fractionalPart of coefficients) = - fractionalPart of free coefficient</param>
        public SimplexTable AddCutoff(SimplexTable table, string basisVarLabel,
            KeyValuePair<Fraction[], Fraction> cutoff)
        {
            var tableCopy = new SimplexTable(table);

            tableCopy.AddCell(basisVarLabel);
            tableCopy.AddRow(basisVarLabel, cutoff.Key, cutoff.Value);

            return tableCopy;
        }

        /// <summary>
        /// Solves weakened problem(without whole constraints) by dual simplex-method
        /// </summary>
        public LppResult SolveWeekenedProblemWithCutoff(LppForSimplexMethod problem, SimplexTable firstTable, out SimplexTable lastTable)
        {
            lastTable = _dualSimplex.Solve(firstTable);
            return _dualSimplex.GetNormalizedProblemResult(lastTable, problem);
        }

        /// <summary>
        /// Gets initial problem result
        /// </summary>
        public LppResult GetInitialProblemResult(SimplexTable lastTable, LppForSimplexMethod normalizedForSimplexProblem)
        {
            return _simplex.GetInitialProblemResult(lastTable, normalizedForSimplexProblem);
        }

        #endregion

        #region [ Methods for checking ]

        /// <summary>
        /// Returs true if result is whole
        /// </summary>
        protected bool IsWhole(LppResult result, ProblemForGomory problem)
        {
            foreach (var variable in result.Coordinates)
                if (variable.Value.Denominator != 1 && problem.WholeConstraints.Contains(variable.Key))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns true if problem have varables, which isn't whole
        /// </summary>
        protected bool HaveNotWholeVariable(DiscreteProgrammingProblem problem)
        {
            foreach (var variable in problem.TargetFunction.Arguments)
                if (!problem.WholeConstraints.Contains(variable))
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if problem have varables, which isn't whole
        /// </summary>
        protected bool HaveNotWholeVariable(ProblemForGomory problem)
        {
            foreach (var variable in problem.TargetFunction.Arguments)
                if (!problem.WholeConstraints.Contains(variable))
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if constraint system have not whole coeffucuent
        /// </summary>
        /// <param name="constraintSystem">With zero constraints</param>
        protected bool HaveNotWholeCoefficient(IEnumerable<Constraint> constraintSystem)
        {
            foreach (var constraint in constraintSystem)
            {
                if (constraint.RightSide.Denominator != 1 && constraint.RightSide.Denominator != 0)
                    return true;
                foreach (var variable in constraint.LeftSide)
                    if (variable.Coefficient.Denominator != 1) return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Calculates fractional part of fraction
        /// </summary>
        protected static Fraction GetFractionalPart(Fraction fraction)
        {
            if (fraction == 0) return new Fraction();
            var wholePart = fraction.Numerator < 0
                                ? (fraction.Numerator - fraction.Denominator) / fraction.Denominator
                                : fraction.Numerator / fraction.Denominator;
            return fraction - wholePart;
        }
    }
}
