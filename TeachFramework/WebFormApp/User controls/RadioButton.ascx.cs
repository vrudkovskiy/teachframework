using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MathProgramming.User_controls
{
    public partial class RadioButton : System.Web.UI.UserControl, IDataControl
    {
        private System.Web.UI.WebControls.RadioButton _radio;
        private string _name;
        //--------------------------------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            _radio = new System.Web.UI.WebControls.RadioButton();
            ID = "radio" + Guid.NewGuid().ToString();
            Controls.Add(_radio);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetText(string text)
        {
            _radio.Text = text;
        }

        #region Members of IDataControl

        public string ControlName
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Value
        {
            get { return _radio.Checked; }
            set { _radio.Checked = (bool)value; }
        }

        #endregion
    }
}