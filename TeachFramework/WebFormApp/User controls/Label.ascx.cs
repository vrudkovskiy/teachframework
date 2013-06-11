using System;
using System.Web.UI;
using TeachFramework.Interfaces;

namespace TeachFramework.UserControls
{
    public partial class DataLabel : UserControl, IDataControl
    {
        public const string ControlDescription = "Label";
        private System.Web.UI.WebControls.Label _label;
        protected void Page_Load(object sender, EventArgs e)
        {
            _label = new System.Web.UI.WebControls.Label();
            Controls.Add(_label);
        }

        #region [ IDataControl Members ]

        public string ControlName { get; set; }

        public object Value
        {
            get { return _label.Text; }
            set { _label.Text = (string)value; }
        }

        public string ControlType
        {
            get { return ControlDescription; }
        }

        public bool ReadOnly
        {
            get { return true; }
            set { }
        }

        #endregion

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
        }

        #endregion
    }
}