using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
using TeachFramework.Interfaces;
using TeachFramework.MathProgramming;
using TeachFramework.MathTypes;

namespace TeachFramework.Controls
{
    public partial class SimplexTableView : DataGridView, IDataControl
    {
        public const string ControlDescription = "SimplexTableView";
        private const string ValidateExceptionMessage = "Симплекс-таблиця введена некоректно";
        private SimplexTable _table;
        //-------------------------------------------------------------
        public SimplexTableView()
        {
            InitializeComponent();
            Init();
        }

        public SimplexTableView(SimplexTable inputTable)
        {
            InitializeComponent();
            Init();
            Value = new SimplexTable(inputTable);
        }

        public SimplexTableView(SimplexTableViewSettings settings)
        {
            InitializeComponent();
            Init();
            SetHead(settings.Variables);
            SetRows(settings.RowCount, settings.Variables);
        }

        private void Init()
        {
            RowHeadersVisible = false;
            AllowUserToAddRows = false;
            VisibleData = true;
            Width = 600;
            ColumnAdded += ColumnWasAdded;
        }

        public bool VisibleData { get; set; }

        #region [ IDataControl Members ]

        public string ControlType
        {
            get { return ControlDescription; }
        }

        public string ControlName
        {
            get { return Name; }
            set { Name = value; }
        }

        public object Value
        {
            get
            {
                if (_table != null)
                    RefreshTable();
                return _table;
            }
            set
            {
                if (value == null)
                {
                    ClearAll();
                    return;
                }
                Bind((SimplexTable)value);
            }
        }

        public new bool ReadOnly
        {
            get { return base.ReadOnly; }
            set { base.ReadOnly = value; }
        }

        #endregion

        private void RefreshTable()
        {

            var vars = new Dictionary<int, string>();
            var rows = new Dictionary<int, Fraction[]>();
            var freeCoefficients = new Dictionary<int, Fraction>();

            int i;
            for (i = 0; i < Rows.Count - 1; i++)
            {
                vars.Add(i, Rows[i].Cells[0].Value.ToString());
                var tmp = new Fraction[_table.Variables.Count];
                for (var j = 1; j < Rows[i].Cells.Count - 1; j++)
                    tmp[j - 1] = new Fraction(Rows[i].Cells[j].Value.ToString());
                rows.Add(i, tmp);
                freeCoefficients.Add(i, new Fraction(Rows[i].Cells[Rows[i].Cells.Count - 1].Value.ToString()));
            }
            _table.CopyRows(vars, rows, freeCoefficients);

            var ratings = new Dictionary<int, MaxCoefficient>();
            int k;
            for (k = 1; k < Rows[i].Cells.Count - 1; k++)
            {
                if (Rows[i].Cells[k].Value == null)
                {
                    ratings = null;
                    break;
                }
                ratings[k - 1] = new MaxCoefficient(Rows[i].Cells[k].Value.ToString());
            }
            _table.CopyRatings(ratings);
            _table.FunctionValue = Rows[i].Cells[k].Value == null || Rows[i].Cells[k].Value.ToString() == ""
                                       ? null
                                       : new MaxCoefficient(Rows[i].Cells[k].Value.ToString());
        }

        private void Bind(SimplexTable inputTable)
        {
            ClearAll();
            _table = new SimplexTable(inputTable);
            SetHead();
            SetRows();
            SetRatings();
        }

        #region [ Methods for binding ]

        private void SetHead()
        {
            Columns.Add("variablesHead", "basis\\all");
            //var firstColumn = new DataGridViewColumn { DefaultCellStyle = { BackColor = Color.Gray }, HeaderText = "basis\\all" };
            Columns[0].DefaultCellStyle.BackColor = Color.Gray;
            foreach (var var in _table.Variables)
                Columns.Add(var, var);
            Columns.Add("freeCoefficients", "free coeff.");
        }
        private void SetRows()
        {
            var i = 0;
            foreach (var var in _table.GetBasis().Keys)
            {
                Rows.Add();
                var variablesComboBox = new DataGridViewComboBoxCell { DropDownWidth = 25 };
                foreach (var variable in _table.Variables)
                    variablesComboBox.Items.Add(variable);
                variablesComboBox.Value = variablesComboBox.Items[variablesComboBox.Items.IndexOf(var)];
                Rows[i].Cells[0] = variablesComboBox;
                if (!VisibleData) break;
                var row = _table.GetRow(var);
                var j = 1;
                foreach (var fraction in row.Key)
                {
                    Rows[i].Cells[j].Value = fraction.ToString();
                    j++;
                }
                Rows[i].Cells[j].Value = row.Value;
                i++;
            }
        }
        private void SetRatings()
        {
            Rows.Add();
            Rows[Rows.Count - 1].Cells[0].Value = "f";
            if (_table.Ratings == null || !VisibleData) return;
            var i = 1;
            foreach (var rating in _table.Ratings)
            {
                Rows[Rows.Count - 1].Cells[i].Value = rating.ToString();
                i++;
            }
            Rows[Rows.Count - 1].Cells[i].Value = _table.FunctionValue.ToString();
        }

        private void ClearAll()
        {
            Rows.Clear();
            Columns.Clear();
        }

        private void SetHead(IEnumerable<string> variables)
        {
            Columns.Add("variablesHead", "basis\\all");
            //var firstColumn = new DataGridViewColumn { DefaultCellStyle = { BackColor = Color.Gray }, HeaderText = "basis\\all" };
            Columns[0].DefaultCellStyle.BackColor = Color.Gray;
            foreach (var var in variables)
                Columns.Add(var, var);
            Columns.Add("freeCoefficients", "free coeff.");
        }
        private void SetRows(int rowCount, List<string> variables)
        {
            int i;
            for (i = 0; i < rowCount; i++)
            {
                Rows.Add();
                var variablesComboBox = new DataGridViewComboBoxCell { DropDownWidth = 25 };
                foreach (var variable in variables)
                    variablesComboBox.Items.Add(variable);
                Rows[i].Cells[0] = variablesComboBox;
            }
            Rows.Add();
            Rows[i].Cells[0].Value = "f";
        }

        #endregion

        private void ColumnWasAdded(object sender, DataGridViewColumnEventArgs e)
        {
            var column = Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None);
            if (column != null)
                column.Width = 50;
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
            try
            {
                if (_table == null)
                    throw new FormatException("Симплекс-таблиця не введена");
                RefreshTable();
            }
            catch (FormatException)
            {
                throw new ValidationException(ValidateExceptionMessage);
            }
        }

        #endregion
    }
}
