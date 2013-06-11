using System;
using System.Collections.Generic;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Result of solving the linear programming problem
    /// </summary>
    public class LppResult : IComparable
    {
        //-----------------------------------------------------------------------
        /// <summary>
        /// Gets coordinates' values
        /// </summary>
        public Dictionary<string, Fraction> Coordinates { get; private set; }

        /// <summary>
        /// Gats function value in the "result" point
        /// </summary>
        public Fraction Value { get; private set; }

        //=======================================================================
        /// <summary>
        /// Represents result of solving the linear programming problem\n
        /// (If both input parameters == null - target function unlimited bottom\n
        /// (If point != null and functionValue == null - set of admissible solutions of incompatible)
        /// </summary>
        public LppResult(IDictionary<string, Fraction> point, Fraction functionValue)
        {
            if (ReferenceEquals(point, null))
            {
                Value = null;
                Coordinates = null;
                return;
            }
            Coordinates = new Dictionary<string, Fraction>(point);
            if (ReferenceEquals(functionValue, null))
            {
                Value = null;
                return;
            }
            Value = new Fraction(functionValue);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var result = (LppResult)obj;

            if (Value != result.Value)
                return -1;
            if (Coordinates == null)
                return result.Coordinates == null ? 0 : -1;
            if (result.Coordinates == null)
                return -1;
            if (Value == null && result.Value == null)
                return 0;


            foreach (var coordinate in Coordinates)
            {
                if (!result.Coordinates.ContainsKey(coordinate.Key))
                    return -1;
                if (coordinate.Value != result.Coordinates[coordinate.Key])
                    return -1;
            }

            return 0;
        }

        #endregion
    }
}
