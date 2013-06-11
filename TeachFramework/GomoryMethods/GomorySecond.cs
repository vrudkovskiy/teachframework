using System.Collections.Generic;
using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    public class GomorySecond : GomoryFirst
    {
        /// <summary>
        /// Indicates is problem suitable for solving by Gomory method
        /// </summary>
        public new bool IsSuitable(DiscreteProgrammingProblem problem)
        {
            return !HaveNotWholeCoefficient(problem.GetAllConstraints());
        }

        /// <summary>
        /// Indicates is problem suitable for solving by Gomory method
        /// </summary>
        public new bool IsSuitable(ProblemForGomory problem)
        {
            return !HaveNotWholeCoefficient(problem.GetAllConstraints());
        }

        /// <summary>
        /// Returns true if result of weakened problem solving is result of initial problem solving
        /// </summary>
        public new bool IsEnd(LppResult result, ProblemForGomory problem)
        {
            return result.Value == null || IsWhole(result, problem);
        }

        /// <summary>
        /// Makes cutoff
        /// </summary>
        public new KeyValuePair<Fraction[], Fraction> MakeCutoff(SimplexTable table,
            string creativeVar, ProblemForGomory problem)
        {
            var j = 0;
            var creativeRow = table.GetRow(creativeVar);
            var row = new Fraction[creativeRow.Key.Length + 1];

            foreach (var variable in table.Variables)
                row[j++] = GetCoefficientForCutoff(variable, table, creativeVar, problem.WholeConstraints) * -1;

            row[j] = new Fraction(1, 1);

            return new KeyValuePair<Fraction[], Fraction>(row, GetFractionalPart(creativeRow.Value) * -1);
        }

        private Fraction GetCoefficientForCutoff(string variable, SimplexTable table, string creativeVar,
            ICollection<string> wholeConstraints)
        {
            var varIndex = table.IndexOf(variable);
            var creativeRow = table.GetRow(creativeVar);
            var varCoefFractionalPart = GetFractionalPart(creativeRow.Key[varIndex]);
            var freeCoefFractionalPart = GetFractionalPart(creativeRow.Value);
            if (wholeConstraints.Contains(variable))
                return (varCoefFractionalPart <= freeCoefFractionalPart)
                    ? varCoefFractionalPart
                    : freeCoefFractionalPart / (1 - freeCoefFractionalPart) * (1 - varCoefFractionalPart);
            return (creativeRow.Key[varIndex] >= 0)
                       ? creativeRow.Key[varIndex]
                       : freeCoefFractionalPart / (1 - freeCoefFractionalPart) * (-1 * varCoefFractionalPart);
        }
    }
}
