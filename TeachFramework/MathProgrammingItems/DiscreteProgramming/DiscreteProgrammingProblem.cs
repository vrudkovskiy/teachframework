using System.Collections.Generic;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Discrete programming problem description
    /// (1. target function, 2. constraint system, 3. zero constraints, 4. whole constraint)
    /// </summary>
    public class DiscreteProgrammingProblem : LinearProgrammingProblem
    {
        /// <summary>
        /// Indicates which variables have 'whole constraint'
        /// </summary>
        public List<string> WholeConstraints { get; set; }

        #region [ Constractors ]

        /// <summary>
        /// Represents discrete programming problem description 
        /// (1. target function, 2. constraint system, 3. zero constraints, 4. whole constraint)
        /// </summary>
        public DiscreteProgrammingProblem(LinearProgrammingProblem linearProblem, IEnumerable<string> wholeConstraints)
            : base(linearProblem)
        {
            WholeConstraints = new List<string>(wholeConstraints);
        }

        /// <summary>
        /// Represents discrete programming problem description 
        /// (1. target function, 2. constraint system, 3. zero constraints, 4. whole constraint)
        /// </summary>
        /// <param name="targetFunction">Target function</param>
        /// <param name="constraintSystem">Constraint system with zero constraints</param>
        /// <param name="wholeConstraints">Variables with whole constraint</param>
        public DiscreteProgrammingProblem(TargetFunction targetFunction, ICollection<Constraint> constraintSystem,
            IEnumerable<string> wholeConstraints)
            : base(targetFunction, constraintSystem)
        {
            WholeConstraints = new List<string>(wholeConstraints);
        }

        /// <summary>
        /// Represents discrete programming problem description 
        /// (1. target function, 2. constraint system, 3. zero constraints, 4. whole constraint)
        /// </summary>
        /// <param name="targetFunction">Target function</param>
        /// <param name="constraints">Constraint system without zero constraints</param>
        /// <param name="zeroConstraints">Zero constraints</param>
        /// <param name="wholeConstraints">Variables with whole constraint</param>
        public DiscreteProgrammingProblem(TargetFunction targetFunction, IEnumerable<Constraint> constraints,
            IEnumerable<Constraint> zeroConstraints, IEnumerable<string> wholeConstraints)
            : base(targetFunction, constraints, zeroConstraints)
        {
            WholeConstraints = new List<string>(wholeConstraints);
        }

        /// <summary>
        /// Represents copy of discrete programming problem description 
        /// (1. target function, 2. constraint system, 3. zero constraints, 4. whole constraint)
        /// </summary>
        public DiscreteProgrammingProblem(DiscreteProgrammingProblem problem)
            : base(problem)
        {
            WholeConstraints = new List<string>(problem.WholeConstraints);
        }

        #endregion
    }
}
