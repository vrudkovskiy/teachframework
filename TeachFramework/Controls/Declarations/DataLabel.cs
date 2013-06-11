using System.Windows.Forms;
using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public partial class DataLabel : UserControl, IDataControl
    {
        private static int _instanceCount;

        public const string ControlDescription = "Label";

        public DataLabel()
        {
            InitializeComponent();
            _instanceCount++;
            ControlName = "dataLabel" + _instanceCount;
            Value = string.Empty;
        }

        public DataLabel(string name, string text)
        {
            InitializeComponent();
            ControlName = name;
            Value = text;
        }

        #region [ IDataControl Members ]

        public string ControlName
        {
            get { return Name; }
            private set { Name = value; }
        }

        public object Value
        {
            get { return lText.Text; }
            set { lText.Text = (string)value; }
        }

        public string ControlType
        {
            get { return ControlDescription; }
        }

        //public bool ReadOnly
        //{
        //    get { return !Enabled; }
        //    set { Enabled = !value; }
        //}

        #endregion

        #region [ IValidable Members ]

        public void ValidateItem()
        {
        }

        #endregion
    }
}
