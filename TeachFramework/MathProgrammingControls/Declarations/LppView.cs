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
    public partial class LppView : UserControl, IDataControl
    {
        public const string ControlDescription = "LppView";
        private readonly TargetFunctionBox _targetF = new TargetFunctionBox("TargetFunction");
        private readonly LimitationsArea _limitations = new LimitationsArea();

        public event EventHandler<EventArgs> ProblemChanged;

        private void ProblemChangedRun(object sender, EventArgs e)
        {
            if (ProblemChanged != null)
                ProblemChanged(sender, e);
        }

        //-------------------------------------------------------------------
        public LppView()
        {
            InitializeComponent();
            Controls.Add(_targetF);
            _targetF.FormulaChanged += ProblemChangedRun;
            Controls.Add(_limitations);
            _limitations.ConstraintSystemChanged += ProblemChangedRun;
            _targetF.Location = new Point(0, 0);
            _limitations.Location = new Point(0, 25);
            _limitations.Height = 120;
            Height = _targetF.Height + _limitations.Height + 10;
            Width = _targetF.Width;
            _limitations.Width = Width;
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
                if (_targetF.Value == null || _limitations.Value == null)
                    return null;
                return new LinearProgrammingProblem((TargetFunction)_targetF.Value, (List<Constraint>)_limitations.Value);
            }
            set
            {
                if (value == null)
                {
                    _targetF.Value = null;
                    _limitations.Value = null;
                    return;
                }
                var problem = (LinearProgrammingProblem)value;
                _targetF.Value = problem.TargetFunction;
                var limitations = new List<Constraint>(problem.ConstraintSystem);
                limitations.AddRange(problem.ZeroConstraints);
                _limitations.Value = limitations;
            }
        }

        #endregion

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
            var message = string.Empty;
            try
            {
                _targetF.ValidateItem();
            }
            catch (ValidationException exception)
            {
                message = exception.Message + Environment.NewLine;
            }
            try
            {
                _limitations.ValidateItem();
            }
            catch (ValidationException exception)
            {
                message += exception.Message;
            }
            if (message != string.Empty)
                throw new ValidationException(message);
        }

        #endregion

        public bool ReadOnly
        {
            set
            {
                _targetF.ReadOnly = value;
                _limitations.ReadOnly = value;
            }
        }
    }
}
