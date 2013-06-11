using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using TeachFramework.Interfaces;
using TeachFramework.MathProgramming;
using TeachFramework.MathTypes;

namespace TeachFramework.Controls
{
    public partial class LimitationsArea : UserControl, IDataControl
    {
        public const string ControlDescription = "LimitationsArea";
        private const string LimSystemIsNotValidMessage = "Система обмежень введена некоректно";
        public LimitationsArea()
        {
            InitializeComponent();
            limitationsSystem.TextChanged += ConstraintSystemChangedRun;
        }

        public event EventHandler<EventArgs> ConstraintSystemChanged;

        private void ConstraintSystemChangedRun(object sender, EventArgs e)
        {
            if (ConstraintSystemChanged != null)
                ConstraintSystemChanged(sender, e);
        }

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
                var limitations = LppParser.ParseConstraints(limitationsSystem.Text);
                //if (limitations == null)
                //    throw new FormatException("Invalid limitations");
                return limitations;
            }
            set
            {
                limitationsSystem.Text = string.Empty;
                if (value == null) return;
                var limitations = (List<Constraint>)value;
                foreach (var lim in limitations)
                    limitationsSystem.Text += lim + Environment.NewLine;
            }
        }

        public bool ReadOnly
        {
            //get { return limitationsSystem.ReadOnly; }
            set { limitationsSystem.ReadOnly = value; }
        }

        #endregion

        public new int Width
        {
            //get { return 25 + limitationsSystem.Width; }
            set
            {
                base.Width = value;
                limitationsSystem.Width = value - 25;
            }
        }

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
            if (!LppParser.IsValidConstraints(limitationsSystem.Text))
                throw new ValidationException(LimSystemIsNotValidMessage);
        }

        #endregion

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (limitationsSystem.SelectedText != "")
                Clipboard.SetText(limitationsSystem.SelectedText);
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            limitationsSystem.SelectedText = Clipboard.GetText();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (limitationsSystem.SelectedText == "") return;
            Clipboard.SetText(limitationsSystem.SelectedText);
            limitationsSystem.SelectedText = "";
        }
    }
}
