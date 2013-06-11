using System.Collections.Generic;

namespace TeachFramework
{
    /// <summary>
    /// Setting for simplex-table control
    /// </summary>
    public class SimplexTableViewSettings
    {
        /// <summary>
        /// Gets or sets row count
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Gets or sets variable count
        /// </summary>
        public int VariableCount
        {
            get { return Variables.Count; }
        }

        /// <summary>
        /// Gets or sets variables
        /// </summary>
        public List<string> Variables { get; set; }
    }
}
