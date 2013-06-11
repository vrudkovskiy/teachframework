using System.Collections.Generic;

namespace TeachFramework.MathProgramming
{
    public class ProblemForGomory : LppForSimplexMethod
    {
        /// <summary>
        /// Indicates which variables must be whole
        /// </summary>
        public List<string> WholeConstraints { get; set; }

        /// <summary>
        /// Returns problem copy as discrete programming problem
        /// </summary>
        public DiscreteProgrammingProblem DiscreteProblem
        {
            get { return new DiscreteProgrammingProblem(this, WholeConstraints); }
        }

        //-----------------------------------------------------------------------------
        #region [ Constructors ]

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public ProblemForGomory(LinearProgrammingProblem problem)
            : base(problem)
        {
            InitializeComponents();
        }

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public ProblemForGomory(LppForSimplexMethod problem)
            : base(problem)
        {
            InitializeComponents();
        }

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <param name="wholeConstraints"> </param>
        public ProblemForGomory(LinearProgrammingProblem problem, IEnumerable<string> wholeConstraints)
            : base(problem)
        {
            InitializeComponents();
            WholeConstraints.AddRange(wholeConstraints);
        }

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        public ProblemForGomory(DiscreteProgrammingProblem problem)
            : base(problem)
        {
            InitializeComponents();
            WholeConstraints.AddRange(problem.WholeConstraints);
        }

        /// <summary>
        /// Represents linear programming problem for symplex-method
        /// </summary>
        /// <param name="problem">Initial problem</param>
        /// <param name="wholeConstraints"> </param>
        public ProblemForGomory(LppForSimplexMethod problem, IEnumerable<string> wholeConstraints)
            : base(problem)
        {
            InitializeComponents();
            WholeConstraints.AddRange(wholeConstraints);
        }

        /// <summary>
        /// Initialize components as default values
        /// </summary>
        private void InitializeComponents()
        {
            WholeConstraints = new List<string>();
        }

        #endregion
    }
}
