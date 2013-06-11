using System.Collections.Generic;

namespace TeachFramework.MathTypes
{
    /// <summary>
    /// x1 + x2 + ... + xn = some constant 
    /// </summary>
    public class EqualityConstraint : Constraint
    {
        /// <summary>
        /// Represents math limitation(equality)
        /// </summary>
        public EqualityConstraint(ICollection<Variable> leftSide, Fraction rightSide) :
            base(leftSide, "=", rightSide)
        { }

        /// <summary>
        /// Represents copy of math limitation(equality)
        /// </summary>
        public EqualityConstraint(EqualityConstraint limitation) :
            base(limitation)
        { }

        /// <summary>
        /// Adds limitation to current limitation
        /// </summary>
        public void Add(EqualityConstraint limitation)
        {
            foreach (var var in limitation.LeftSide)
                AddVariable(var, false);
            SetRightSide(RightSide + limitation.RightSide);
        }

        /// <summary>
        /// Subtracts limitation from current limitation
        /// </summary>
        public void Subtract(EqualityConstraint limitation)
        {
            foreach (var var in limitation.LeftSide)
                AddVariable(new Variable(var.Coefficient * -1, var.Label, var.Power), false);
            SetRightSide(RightSide - limitation.RightSide);
        }
    }
}
