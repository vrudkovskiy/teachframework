using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TeachFramework.Interfaces;
using TeachFramework.MathProgramming;

namespace TeachFramework.Controls
{
    public partial class DppView : UserControl, IDataControl
    {
        public const string ControlDescription = "DppView";
        private List<CheckBox> Variables { get; set; }

        public DppView()
        {
            InitializeComponent();
            variablesPanel.AutoScroll = true;
            variablesPanel.BorderStyle = BorderStyle.Fixed3D;
            variablesPanel.HorizontalScroll.Visible = false;
            lpp.ProblemChanged += ProblemChanged;
        }

        private void ProblemChanged(object sender, EventArgs e)
        {
            try
            {
                if (lpp.Value == null) return;
            }
            catch (FormatException)
            {
                return;
            }
            SetWholeVariablesPanel(((LinearProgrammingProblem)lpp.Value).TargetFunction.Arguments);
        }

        private void SetWholeVariablesPanel(IEnumerable<string> variables)
        {
            var y = 10;
            Variables = new List<CheckBox>();
            variablesPanel.Controls.Clear();
            foreach (var variable in variables)
            {
                var checkBox = new CheckBox { Text = variable, Location = new Point(50, y), Size = new Size(40, 20) };
                y += 20;
                variablesPanel.Controls.Add(checkBox);
                Variables.Add(checkBox);
            }
        }

        private void SetWholeVariablesPanel(IEnumerable<string> variables, ICollection<string> wholeVariables)
        {
            var y = 10;
            Variables = new List<CheckBox>();
            variablesPanel.Controls.Clear();
            foreach (var variable in variables)
            {
                var checkBox = new CheckBox
                {
                    Text = variable,
                    Location = new Point(50, y),
                    Size = new Size(40, 20),
                    Checked = wholeVariables.Contains(variable)
                };
                y += 20;
                variablesPanel.Controls.Add(checkBox);
                Variables.Add(checkBox);
            }
        }

        private IEnumerable<string> GetWholeVariables()
        {
            var checkedVariables = new List<string>();
            foreach (var variable in Variables)
                if (variable.Checked)
                    checkedVariables.Add(variable.Text);
            return checkedVariables;
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
            get
            {
                return lpp.Value != null
                    ? new DiscreteProgrammingProblem((LinearProgrammingProblem)lpp.Value, GetWholeVariables())
                    : null;
            }
            set
            {
                if (value == null)
                {
                    lpp.Value = null;
                    return;
                }
                var problem = (DiscreteProgrammingProblem)value;
                lpp.Value = problem;
                SetWholeVariablesPanel(problem.TargetFunction.Arguments, problem.WholeConstraints);
            }
        }

        #endregion

        #region IValidateable Members

        public void ValidateItem()
        {
            lpp.ValidateItem();
        }

        #endregion

        public bool ReadOnly
        {
            set
            {
                lpp.ReadOnly = value;
                variablesPanel.Enabled = !value;
            }
        }
    }
}
