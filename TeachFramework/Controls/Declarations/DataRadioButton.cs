using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public partial class DataRadioButton : System.Windows.Forms.RadioButton, IDataControl
    {
        public const string ControlDescription = "DataRadioButton";

        public DataRadioButton()
        {
            InitializeComponent();
        }

        public DataRadioButton(string name, string text)
        {
            InitializeComponent();
            ControlName = name;
            base.Text = text;
        }

        #region IDataControl Members

        public string ControlName
        {
            get { return Name; }
            private set { Name = value; }
        }

        public object Value
        {
            get { return Checked; }
            set { Checked = (bool)value; }
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
