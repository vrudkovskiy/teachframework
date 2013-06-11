using System.Windows.Forms;
using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public partial class SignedTextBox : UserControl, IDataControl
    {
        private static int _instanceCount;
        public const string ControlDescription = "SignedTextBox";

        public SignedTextBox()
        {
            InitializeComponent();
            _instanceCount++;
            ControlName = "signedTextBox" + _instanceCount;
            _stbText.Text = string.Empty;
        }

        public SignedTextBox(string name, string sign)
        {
            InitializeComponent();
            ControlName = name;
            _stbText.Text = sign;
        }

        #region [ IDataControl Members ]

        public object Value
        {
            get { return _stbTextBox.Text; }
            set { _stbTextBox.Text = (string)value; }
        }

        public string ControlName
        {
            get { return Name; }
            private set { Name = value; }
        }

        public string ControlType
        {
            get { return ControlDescription; }
        }

        public bool ReadOnly
        {
            //get { return _stbTextBox.ReadOnly; }
            set { _stbTextBox.ReadOnly = value; }
        }

        #endregion

        #region [ IValidable Members ]

        public void ValidateItem()
        {
            //return _stbTextBox.Text != string.Empty;
        }

        #endregion
    }
}
