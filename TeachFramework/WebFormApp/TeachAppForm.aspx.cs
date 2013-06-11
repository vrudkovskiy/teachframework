using System;
using System.Collections.Generic;
using System.Web.UI;

using TeachFramework.Interfaces;

namespace TeachFramework
{
    public partial class TeachAppPage : Page, IView
    {
        private List<IDataControl> ControlsCollection
        {
            get { return (List<IDataControl>)Session["_controls"]; }
            set { Session["_controls"] = value; }
        }
        //-----------------------------------------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlsCollection = new List<IDataControl>();
            }
            SetPresenter();
            submitButton.Click += SubmitButtonClick;

            if (!IsPostBack) return;
            foreach (var c in ControlsCollection)
                form1.Controls.Add((Control)c);
        }

        public void SubmitButtonClick(object sender, EventArgs e)
        {
            if (SubmitButtonPressed != null)
                SubmitButtonPressed(sender, e);
        }

        protected void AutoButtonClick(object sender, EventArgs e)
        {
            if (AutoButtonPressed != null)
                AutoButtonPressed(sender, e);
        }

        #region IView Members

        public string ViewName
        {
            get { return Title; }
            set { Title = value; }
        }

        public event EventHandler<EventArgs> SubmitButtonPressed;

        public event EventHandler<EventArgs> AutoButtonPressed;

        public UiDescription GetUserData()
        {
            var data = new UiDescription();

            foreach (Control control in form1.Controls)
            {
                Data cData = GetControlData(control);
                if (cData == null)
                    continue;
                data.Add(cData);
            }
            return data;
        }

        public void ShowCorrectData(IEnumerable<Data> correctData)
        {
            foreach (Data data in correctData)
                foreach (IDataControl control in ControlsCollection)
                    if (data.Name == control.ControlName)
                        control.Value = data.Value;
        }

        public void ShowMessage(string message)
        {
            Response.Write(message);
        }

        #endregion

        #region [ IValidateable ]

        public new bool IsValid()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Gets control value
        /// </summary>
        private Data GetControlData(Control control)
        {
            foreach (var dataControl in ControlsCollection)
                if (dataControl.ControlName == control.ID)
                    return new Data { Name = control.ID, ControlType = dataControl.ControlType, Value = dataControl.Value};

            return null;
        }

        /// <summary>
        /// Adds control into form(input parameters - different references to one object)
        /// </summary>
        public void AddControl(IDataControl control)
        {
            //if (!IsPostBack)
            //{
            
                form1.Controls.Add((Control)control);
                ControlsCollection.Add(control);
            //}
        }

        /// <summary>
        /// Clears form
        /// </summary>
        public void ClearControlsList()
        {
            foreach (IDataControl iDataControl in ControlsCollection)
                foreach (Control control in form1.Controls)
                    if (iDataControl.ControlName == control.ID)
                    {
                        form1.Controls.Remove(control);
                        break;
                    }
            //form1.Controls.Clear();
            ControlsCollection.Clear();
        }

        /// <summary>
        /// Initializes or refereshes(IsPostBack) presener
        /// </summary>
        private void SetPresenter()
        {
            var viewBuilder = new WebFormBuilder(this);

            if (!IsPostBack)
            {
                var models = new List<IModel>
                                 {
                                     new Addition(),
                                     new Subtracting(),
                                     new Multiplying(),
                                     new Dividing(),
                                     new Testing(),
                                     new SymplexMethod()
                                 };

                var presenter = new Presenter(viewBuilder, models);
                Session.Contents.Add("presenter", presenter);
            }
            else
                ((Presenter)Session.Contents["presenter"]).RefreshPresenter(viewBuilder);
        }
    }
}
