using System;

using TeachFramework.Interfaces;

namespace TeachFramework.UserControls
{
    public partial class SignedTextBox : System.Web.UI.UserControl, IDataControl//, IPostBackDataHandler
    {
        public static string ControlDescription = "SignedTextBox";
        private System.Web.UI.WebControls.TextBox _textBox;
        private System.Web.UI.WebControls.Label _label;
        //-------------------------------------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            _label = new System.Web.UI.WebControls.Label { ID = ID + "label"};
            Controls.Add(_label);
            _textBox = new System.Web.UI.WebControls.TextBox { ID = ID + "textBox" };
            Controls.Add(_textBox);
        }

        public string Text
        {
            set { _label.Text = value; }
        }

        public bool ReadOnly
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }

        #region [ Members of IDataControl ]

        public string ControlName
        {
            get { return ID; }
            set { ID = value; }
        }

        public object Value
        {
            get { return _textBox.Text; }
            set { _textBox.Text = (string)value; }
        }

        public string ControlType
        {
            get { return ControlDescription; }
        }

        #endregion

        //#region IPostBackDataHandler Members

        //public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RaisePostDataChangedEvent()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion

        #region [ IValidateable ]

        public void ValidateItem()
        {
        }

        #endregion
    }
}