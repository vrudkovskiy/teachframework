using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

using TeachFramework.Interfaces;
using TeachFramework.MathProgramming;
using TeachFramework.MathTypes;

namespace TeachFramework.Controls
{
    public partial class TargetFunctionBox : UserControl, IDataControl
    {
        public const string ControlDescription = "TargetFunctionBox";
        private const string TargetFunctionInvalidMessage = "Функція цілі введена неправильно";
        public TargetFunctionBox()
            : this("default")
        { }
        public TargetFunctionBox(string name)
        {
            InitializeComponent();
            _formula.TextChanged += FormulaChangedRun;
            Name = name;
            _target.Items.Add("min");
            _target.Items.Add("max");
            _target.SelectedIndex = 0;
            _target.DropDownStyle = ComboBoxStyle.DropDownList;
            contextMenuStrip1.Items["copy"].Click += CopyToolStripMenuItemClick;
            contextMenuStrip1.Items["paste"].Click += PasteToolStripMenuItemClick;
            contextMenuStrip1.Items["cut"].Click += CutToolStripMenuItemClick;
        }

        public event EventHandler<EventArgs> FormulaChanged;

        private void FormulaChangedRun(object sender, EventArgs e)
        {
            if (FormulaChanged != null)
                FormulaChanged(sender, e);
        }

        #region [ IDataControl Members ]

        public string ControlType
        {
            get { return ControlDescription; }
        }

        public string ControlName
        {
            get { return Name; }
            //set { Name = value; }
        }

        public bool ReadOnly
        {
            //get { return _formula.ReadOnly; }
            set
            {
                _formula.ReadOnly = value;
                _target.Enabled = !value;
            }
        }

        public object Value
        {
            get
            {
                var text = "f(x)=" + _formula.Text + "->" + _target.Text;
                var targetFunction = LppParser.ParseTargetFunction(text);
                //if (targetFunction == null)
                //    throw new FormatException("Invalid target function");
                return targetFunction;
            }
            set
            {
                if (value == null)
                {
                    _formula.Text = string.Empty;
                    return;
                }
                var targetFunction = (TargetFunction)value;
                var text = targetFunction.ToString();
                var startIndex = text.IndexOf('=') + 1;
                var length = text.IndexOf("->", StringComparison.Ordinal) - startIndex;
                _formula.Text = text.Substring(startIndex, length);
                _target.Text = text.Substring(startIndex + length + 2).Replace(" ", "");
            }
        }

        #endregion

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
            if (!LppParser.IsValidTargetFunction("f(x)=" + _formula.Text + "->" + _target.Text))
                throw new ValidationException(TargetFunctionInvalidMessage);
        }

        #endregion

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_formula.SelectedText != "")
                Clipboard.SetText(_formula.SelectedText);
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            _formula.SelectedText = Clipboard.GetText();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_formula.SelectedText == "") return;
            Clipboard.SetText(_formula.SelectedText);
            _formula.SelectedText = "";
        }
    }
}
