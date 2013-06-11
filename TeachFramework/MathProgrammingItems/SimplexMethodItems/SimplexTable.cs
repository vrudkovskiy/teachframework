using System;
using System.Collections.Generic;
using System.Linq;

using TeachFramework.MathTypes;

namespace TeachFramework.MathProgramming
{
    /// <summary>
    /// Sympex-table
    /// </summary>
    public class SimplexTable : IComparable
    {
        private Dictionary<int, string> _allVariables;                      //Dictionary<variableId, variable>
        private Dictionary<int, int> _basisVariables;                       //Dictionary<rowId, variableId>
        private Dictionary<int, Fraction[]> _rows;                          //Dictionary<rowId, coefficient[variableId]>
        private Dictionary<int, Fraction> _freeCoefficients;                //Dictionary<rowId, freeCoefficient>
        private Dictionary<int, MaxCoefficient> _targetFunctionCoefficients;//Dictionary<variableId, coefficient>
        private Dictionary<int, MaxCoefficient> _ratings;                   //Dictionary<variableId, rating>
        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Gets ratings
        /// </summary>
        public List<MaxCoefficient> Ratings
        {
            get { return _ratings == null ? null : _ratings.Values.ToList(); }
        }

        /// <summary>
        /// Gets count of rows in symplex-table
        /// </summary>
        public int RowsCount
        {
            get { return _rows.Count; }
        }

        /// <summary>
        /// Gets collection with all variables contained in symplex-table
        /// </summary>
        public ICollection<string> Variables
        {
            get { return _allVariables.Values; }
        }

        /// <summary>
        /// Gets or sets function's value
        /// </summary>
        public MaxCoefficient FunctionValue { get; set; }
        //======================================================================================
        /// <summary>
        /// Represents symplex-table
        /// </summary>
        public SimplexTable(LppForSimplexMethod problem)
        {
            SetVariables(problem);
            SetBasis(problem);
            SetCoeffientsMatrix(problem);
            SetTargetFunctionCoefficints(problem);
        }

        /// <summary>
        /// Represents copy of symplex-table
        /// </summary>
        public SimplexTable(SimplexTable table)
        {
            _allVariables = new Dictionary<int, string>(table._allVariables);
            // 
            _basisVariables = new Dictionary<int, int>(table._basisVariables);
            //
            _rows = new Dictionary<int, Fraction[]>();
            foreach (var row in table._rows)
            {
                var tmp = new Fraction[row.Value.Count()];
                for (var i = 0; i < row.Value.Count(); i++)
                    tmp[i] = new Fraction(row.Value[i]);
                _rows.Add(row.Key, tmp);
            }
            //
            _freeCoefficients = new Dictionary<int, Fraction>();
            foreach (var fCoef in table._freeCoefficients)
                _freeCoefficients.Add(fCoef.Key, new Fraction(fCoef.Value));
            //
            _targetFunctionCoefficients = new Dictionary<int, MaxCoefficient>();
            foreach (var tfCoef in table._targetFunctionCoefficients)
                _targetFunctionCoefficients.Add(tfCoef.Key, new MaxCoefficient(tfCoef.Value));
            //
            if (table._ratings == null) return;
            _ratings = new Dictionary<int, MaxCoefficient>();
            foreach (var rating in table._ratings)
                _ratings.Add(rating.Key, new MaxCoefficient(rating.Value));
            //
            FunctionValue = ReferenceEquals(table.FunctionValue, null) ? null : new MaxCoefficient(table.FunctionValue);
        }

        /// <summary>
        /// Gets element from table
        /// </summary>
        public Fraction GetMatrixElement(int rowIndex, int variableId)
        {
            return new Fraction(_rows[rowIndex].ElementAt(variableId));
        }

        /// <summary>
        /// Gets variable label by index
        /// </summary>
        public string GetVariable(int index)
        {
            return _allVariables[index];
        }

        /// <summary>
        /// Gets label of basis by row index
        /// </summary>
        public string GetBasisVariableLabel(int rowIndex)
        {
            return _allVariables[_basisVariables[rowIndex]];
        }

        /// <summary>
        /// Gets row, which corresponds to a basis variable
        /// </summary>
        public KeyValuePair<Fraction[], Fraction> GetRow(string basisVar)
        {
            var index = IndexOf(basisVar);
            var rowId = 0;
            foreach (var key in _basisVariables.Keys)
                if (_basisVariables[key] == index)
                {
                    rowId = key;
                    break;
                }
            var tmp = new Fraction[_allVariables.Count];
            foreach (var key in _allVariables.Keys)
                tmp[key] = new Fraction(_rows[rowId][key]);
            return new KeyValuePair<Fraction[], Fraction>(tmp, GetFreeCoefficient(rowId));
        }

        /// <summary>
        /// Gets rating, which corresponds to a basis variable
        /// </summary>
        public MaxCoefficient GetRating(string basisVar)
        {
            return _ratings[IndexOf(basisVar)];
        }

        /// <summary>
        /// Gets free coefficient, which corresponds to specified row
        /// </summary>
        public Fraction GetFreeCoefficient(int rowIndex)
        {
            return new Fraction(_freeCoefficients[rowIndex]);
        }

        /// <summary>
        /// Calculates ratings
        /// </summary>
        public void CalculateRatings()
        {
            _ratings = new Dictionary<int, MaxCoefficient>();
            var rowsCount = _rows.Count;

            foreach (var variableId in _allVariables.Keys)
            {
                var rating = new MaxCoefficient();
                for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                {

                    var tmp = new MaxCoefficient(_targetFunctionCoefficients[_basisVariables[rowIndex]]);
                    var fraction = new Fraction(_rows[rowIndex].ElementAt(variableId));
                    tmp.Multiply(fraction);
                    rating.Add(tmp);
                }
                rating.Subtract(_targetFunctionCoefficients[variableId]);
                _ratings.Add(variableId, rating);
            }
            FunctionValue = CalculateFunctionValue();
        }

        /// <summary>
        /// Multiplies row by fraction
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="fraction"></param>
        public void MultiplyRow(int rowIndex, Fraction fraction)
        {
            foreach (var matrixElem in _rows[rowIndex])
                matrixElem.Multiply(fraction);
            _freeCoefficients[rowIndex].Multiply(fraction);
        }

        /// <summary>
        /// Adds two rows of symplex-table(result puts to first row)(first + num*second)
        /// </summary>
        public void AddRows(int firstIndex, int secondIndex, Fraction num)
        {
            foreach (var varId in _allVariables.Keys)
                _rows[firstIndex].ElementAt(varId).Add(num * _rows[secondIndex].ElementAt(varId));
            _freeCoefficients[firstIndex].Add(num * _freeCoefficients[secondIndex]);
        }

        /// <summary>
        /// Adds cell to simplex-table
        /// </summary>
        public void AddCell(string label)
        {
            if (_allVariables.Values.Contains(label))
                throw new DuplicateWaitObjectException("Variable '" + label + "' has already exist in table");
            var variableIndex = _allVariables.Count;
            _allVariables.Add(variableIndex, label);

            foreach (var rowIndex in _basisVariables.Keys)
            {
                var tmpArray = _rows[rowIndex];
                _rows[rowIndex] = new Fraction[_allVariables.Count];
                for (var i = 0; i < tmpArray.Length; i++)
                    _rows[rowIndex][i] = tmpArray[i];
                _rows[rowIndex][tmpArray.Length] = new Fraction();
            }

            _ratings.Add(IndexOf(label), new MaxCoefficient());

            _targetFunctionCoefficients.Add(variableIndex, new MaxCoefficient());
        }

        /// <summary>
        /// Adds row to table
        /// </summary>
        public void AddRow(string basisVarLabel, Fraction[] row, Fraction freeCoefficient)
        {
            var rowIndex = _rows.Count;
            var basisVarId = IndexOf(basisVarLabel);

            _basisVariables.Add(rowIndex, basisVarId);

            _rows.Add(rowIndex, row);

            _freeCoefficients.Add(rowIndex, freeCoefficient);
        }

        /// <summary>
        /// Changes basis according to solving element - [rowIndex][cellIndex]
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cellIndex"></param>
        public void ChangeBasis(int rowIndex, int cellIndex)
        {
            _basisVariables[rowIndex] = cellIndex;

            MultiplyRow(rowIndex, (1 / GetMatrixElement(rowIndex, cellIndex)));

            for (var i = 0; i < RowsCount; i++)
            {
                if (i == rowIndex) continue;
                var num = GetMatrixElement(i, cellIndex) * -1;
                if (num != 0)
                    AddRows(i, rowIndex, num);
            }
        }

        /// <summary>
        /// Gets basis
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Fraction> GetBasis()
        {
            var basis = new Dictionary<string, Fraction>();
            foreach (var basisVar in _basisVariables)
            {
                var variable = _allVariables[basisVar.Value];
                var value = new Fraction(_freeCoefficients[basisVar.Key]);
                basis.Add(variable, value);
            }
            return basis;
        }

        /// <summary>
        /// Gets function value
        /// </summary>
        public MaxCoefficient CalculateFunctionValue()
        {
            var tmpResult = new MaxCoefficient();
            foreach (var elem in _basisVariables)
            {
                var varCoefficient = new MaxCoefficient(_targetFunctionCoefficients[elem.Value]);
                var varValue = new Fraction(_freeCoefficients[elem.Key]);
                varCoefficient.Multiply(varValue);
                tmpResult.Add(varCoefficient);
            }
            return tmpResult;
        }

        /// <summary>
        /// Sets rows
        /// </summary>
        public void CopyRows(Dictionary<int, string> basisVars, Dictionary<int, Fraction[]> rows, Dictionary<int, Fraction> freeCoefficients)
        {
            _basisVariables = new Dictionary<int, int>();
            foreach (var key in basisVars.Keys)
                _basisVariables.Add(key, IndexOf(basisVars[key]));

            _rows = new Dictionary<int, Fraction[]>(rows);

            _freeCoefficients = new Dictionary<int, Fraction>(freeCoefficients);
        }

        /// <summary>
        /// Sets ratings
        /// </summary>
        public void CopyRatings(Dictionary<int, MaxCoefficient> ratings)
        {
            if (ratings == null)
            {
                _ratings = null;
                return;
            }
            _ratings = new Dictionary<int, MaxCoefficient>(ratings);
        }

        private void SetVariables(LppForSimplexMethod problem)
        {
            _allVariables = new Dictionary<int, string>();
            var index = 0;
            foreach (var str in problem.TargetFunctionArguments)
            {
                _allVariables.Add(index, str);
                index++;
            }
        }

        private void SetBasis(LppForSimplexMethod problem)
        {
            _basisVariables = new Dictionary<int, int>();
            for (var i = 0; i < problem.ConstraintCount; i++)
            {
                var str = problem.GetBasisVariableLabel(i);
                var index = IndexOf(str);
                _basisVariables.Add(i, index);
            }
        }

        private void SetCoeffientsMatrix(LppForSimplexMethod problem)
        {
            _rows = new Dictionary<int, Fraction[]>();
            _freeCoefficients = new Dictionary<int, Fraction>();

            for (var rowIndex = 0; rowIndex < problem.ConstraintCount; rowIndex++)
            {
                var limitation = problem.GetConstraint(rowIndex);
                var row = CreateRow(_allVariables.Count);
                foreach (var var in limitation.LeftSide)
                {
                    var cellIndex = IndexOf(var.Label);
                    row[cellIndex] = new Fraction(var.Coefficient);
                }
                _rows.Add(rowIndex, row);
                _freeCoefficients.Add(rowIndex, limitation.RightSide);
            }
        }

        private void SetTargetFunctionCoefficints(LppForSimplexMethod problem)
        {
            _targetFunctionCoefficients = new Dictionary<int, MaxCoefficient>();
            foreach (var var in problem.TargetFunction.Formula)
            {
                var index = IndexOf(var.Label);
                if (problem.VariablesWithMaxCoefficient.Contains(var.Label))
                {
                    var mCoefficient = new Fraction(1, 1);
                    _targetFunctionCoefficients.Add(index, new MaxCoefficient(mCoefficient, new Fraction()));
                    continue;
                }
                var coefficient = new Fraction(var.Coefficient);
                _targetFunctionCoefficients.Add(index, new MaxCoefficient(new Fraction(), coefficient));
            }
        }

        /// <summary>
        /// Gets variable id
        /// </summary>
        public int IndexOf(string variable)
        {
            var index = -1;
            foreach (var key in _allVariables.Keys)
                if (_allVariables[key] == variable)
                {
                    index = key;
                    break;
                }
            return index;
        }

        private static Fraction[] CreateRow(int length)
        {
            var row = new Fraction[length];
            for (var i = 0; i < length; i++)
                row[i] = new Fraction();
            return row;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var table = (SimplexTable)obj;
            return VariablesEquals(table) && RowsEquals(table) && RatingsAndValueEquals(table) ? 0 : -1;
        }

        private bool VariablesEquals(SimplexTable table)
        {
            if (_allVariables.Count != table._allVariables.Count) return false;

            if (_ratings == null && table._ratings != null
                || _ratings != null && table._ratings == null)
                return false;

            foreach (var varId in _allVariables.Keys)
                if (_allVariables[varId] != table._allVariables[varId] ||
                    _targetFunctionCoefficients[varId] != table._targetFunctionCoefficients[varId] ||
                    (_ratings != null && table._ratings != null) && _ratings[varId] != table._ratings[varId])
                    return false;

            return true;

        }

        private bool RowsEquals(SimplexTable table)
        {
            for (var i = 0; i < _rows.Count; i++)
            {
                if (_basisVariables[i] != table._basisVariables[i]) return false;
                if (_freeCoefficients[i] != table._freeCoefficients[i]) return false;
                if (_rows.Count != table._rows.Count) return false;
                foreach (var varId in _allVariables.Keys)
                    if (_rows[i][varId] != table._rows[i][varId])
                        return false;
            }

            return true;
        }

        private bool RatingsAndValueEquals(SimplexTable table)
        {
            for (var i = 0; i < _ratings.Count; i++)
                if (_ratings[i] != table._ratings[i]) return false;

            return FunctionValue == table.FunctionValue;
        }

        #endregion
    }
}
