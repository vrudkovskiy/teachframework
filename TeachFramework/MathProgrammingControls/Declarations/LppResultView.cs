using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using TeachFramework.Interfaces;
using TeachFramework.MathProgramming;
using TeachFramework.MathTypes;

namespace TeachFramework.Controls
{
    public partial class LppResultView : UserControl, IDataControl
    {
        public const string ControlDescription = "LppResultView";
        public const string ResultIncorrectMessage = "Результат введений некоректно";

        public LppResultView()
        {
            InitializeComponent();
            coordinatesDataGrid.ColumnAdded += ColumnWasAdded;
        }

        private void ColumnWasAdded(object sender, DataGridViewColumnEventArgs e)
        {
            var column = coordinatesDataGrid.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None);
            if (column != null)
                column.Width = 50;
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void SetVariables(IEnumerable<string> variables)
        {
            coordinatesDataGrid.Rows.Clear();
            coordinatesDataGrid.Columns.Clear();

            foreach (var variable in variables)
                coordinatesDataGrid.Columns.Add(variable, variable);
            coordinatesDataGrid.Rows.Add();
        }

        public void SetResult(LppResult result)
        {
            coordinatesDataGrid.Rows.Clear();
            coordinatesDataGrid.Columns.Clear();

            if (result.Coordinates == null)
            {
                tfUnlimitedRadio.Select();
                return;
            }
            if (result.Value == null)
            {
                setIncompatibleRadio.Select();
                return;
            }
            successRadio.Select();

            foreach (var variable in result.Coordinates.Keys)
                coordinatesDataGrid.Columns.Add(variable, variable);
            coordinatesDataGrid.Rows.Add();

            var i = 0;
            foreach (var variable in result.Coordinates.Keys)
                coordinatesDataGrid.Rows[0].Cells[i++].Value = result.Coordinates[variable];

            tfValueTextBox.Text = result.Value.ToString();
        }

        public LppResult GetResult()
        {
            if (tfUnlimitedRadio.Checked)
                return new LppResult(null, null);
            if (setIncompatibleRadio.Checked)
                return new LppResult(new Dictionary<string, Fraction>(), null);
            var coordinates = new Dictionary<string, Fraction>();
            var value = new Fraction(tfValueTextBox.Text);
            for (var i = 0; i < coordinatesDataGrid.Columns.Count; i++)
            {
                coordinates.Add(coordinatesDataGrid.Columns[i].HeaderText,
                    new Fraction(coordinatesDataGrid.Rows[0].Cells[i].Value.ToString()));
            }
            return new LppResult(coordinates, value);
        }

        public bool ReadOnly
        {
            get { return !tfUnlimitedRadio.Enabled; }
            set
            {
                tfUnlimitedRadio.Enabled = setIncompatibleRadio.Enabled = successRadio.Enabled = !value;
                coordinatesDataGrid.ReadOnly = tfValueTextBox.ReadOnly = value;
            }
        }

        #region IDataControl Members

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
            get { return GetResult(); }
            set
            {
                if (value != null)
                    SetResult((LppResult)value);
            }
        }

        #endregion

        #region IValidateable Members

        public void ValidateItem()
        {
            try
            {
                GetResult();
            }
            catch (FormatException)
            {
                throw new ValidationException(ResultIncorrectMessage);
            }
        }

        #endregion

        private void SuccessRadioCheckedChanged(object sender, EventArgs e)
        {
            coordinatesDataGrid.Visible = label1.Visible = tfValueTextBox.Visible = successRadio.Checked;
        }
    }
}
