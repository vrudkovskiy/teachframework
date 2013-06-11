namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Solving element for simplex-method(has pair of solving element's indexes)
    /// </summary>
    public class SolvingElementIndexes
    {
        /// <summary>
        /// Index of solving cell
        /// </summary>
        public int CellIndex { get; set; }

        /// <summary>
        /// Index of solving row
        /// </summary>
        public int RowIndex { get; set; }
    }
}
