using System.Web.UI.WebControls;

using TeachFramework.Interfaces;

namespace TeachFramework.UserControls
{
    public partial class DataRadioButton : RadioButton, IDataControl
    {
        public const string ControlDescription = "DataRadioButton";

        public void SetText(string text)
        {
            Text = text;
        }

        public void SetGroupName(string name)
        {
            GroupName = name;
        }

        #region [ Members of IDataControl ]

        public string ControlType
        {
            get { return ControlDescription; }
        }

        public string ControlName
        {
            get { return ID; }
            set { ID = value; }
        }

        public object Value
        {
            get { return Checked; }
            set { Checked = (bool)value; }
        }

        public bool ReadOnly
        {
            get { return !Enabled; }
            set { Enabled = !value; }
        }

        #endregion

        #region [ IValidateable Members ]

        public void ValidateItem()
        {
        }

        #endregion
    }
}