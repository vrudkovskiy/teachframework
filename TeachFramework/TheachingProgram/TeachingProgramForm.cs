using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

using TeachFramework.Interfaces;

namespace TeachFramework
{
    /// <summary>
    /// Form for teaching WinForm application
    /// </summary>
    public partial class WinAppForm : Form, IView
    {
        private const string AreYouSureCaption = "Попередження";
        private const string AreYouSureMessage = "Ви впевнені, що бажаєте закінчити розв’язок?";

        // rename
        private List<IDataControl> DataControls { get; set; }
        private Control.ControlCollection UserControls
        {
            get { return _controlsPanel.Controls; }
            //set { _controlsPanel.Controls = value; }
        }
        //----------------------------------------------------------------------
        /// <summary>
        /// Represents WinForm view
        /// </summary>
        public WinAppForm()
        {
            InitializeComponent();
            DataControls = new List<IDataControl>();
            submitButton.Click += SubmitButtonClick;
        }

        private void SubmitButtonClick(object sender, EventArgs e)
        {
            foreach (var control in DataControls)
                ControlValidate(control);
            
            if (SubmitButtonPressed != null)
                SubmitButtonPressed(sender, e);
        }

        private void AutoButtonClick(object sender, EventArgs e)
        {
            if (AutoButtonPressed != null)
                AutoButtonPressed(sender, e);
        }

        #region [ IView Members ]
        //------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets form's title
        /// </summary>
        public string ViewName
        {
            get { return Text; }
            set { Text = value; }
        }

        /// <summary>
        /// Occures when the submit button was pressed
        /// </summary>
        public event EventHandler<EventArgs> SubmitButtonPressed;

        /// <summary>
        /// Occures when the auto button was pressed
        /// </summary>
        public event EventHandler<EventArgs> AutoButtonPressed;

        /// <summary>
        /// Occures when the reset button was pressed
        /// </summary>
        public event EventHandler<EventArgs> ResetButtonPressed;

        /// <summary>
        /// Gets entered by user informaition from form
        /// </summary>
        /// <returns></returns>
        public UiDescription GetUserData()
        {
            var data = new UiDescription();
            foreach (Control control in UserControls)
            {
                var dataControl = GetDataControl(control);
                data.Add(dataControl.ControlType, dataControl.ControlName, dataControl.Value);
            }
            return data;
        }

        /// <summary>
        /// Adds correct data to controls
        /// </summary>
        /// <param name="correctData"></param>
        public void ShowCorrectData(IEnumerable<UiDescriptionItem> correctData)
        {
            foreach (var data in correctData)
                foreach (var control in DataControls)
                    if (data.Name == control.ControlName)
                        control.Value = data.Value;
        }

        /// <summary>
        /// Shows message in MessageBox
        /// </summary>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Returns false if some data inputed incorrectly
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            foreach (Control control in UserControls)
                if (errorProvider.GetError(control) != string.Empty)
                    return false;
            return true;
        }

        /// <summary>
        /// Resets error provider
        /// </summary>
        public void ResetErrors()
        {
            errorProvider.Clear();
        }

        //------------------------------------------------------------------------
        #endregion

        /// <summary>
        /// Gets DataControl which meets input control
        /// </summary>
        private IDataControl GetDataControl(Control control)
        {
            foreach (var dataControl in DataControls)
                if (dataControl.ControlName == control.Name)
                    return dataControl;
            return null;
        }

        /// <summary>
        /// Adds control into form(input parameters - different references to one object)
        /// </summary>
        public void AddControl(IDataControl dataControl)
        {
            var controlReference = (Control)dataControl;
            UserControls.Add(controlReference);
            DataControls.Add(dataControl);
            controlReference.Validating += ControlValidating;
        }

        private void ControlValidating(object sender, CancelEventArgs e)
        {
            ControlValidate((IDataControl)sender);
        }

        private void ControlValidate(IDataControl control)
        {
            try
            {
                control.ValidateItem();
                errorProvider.SetError((Control)control, string.Empty);
            }
            catch (ValidationException exception)
            {
                errorProvider.SetError((Control)control, exception.Message);
            }
        }

        /// <summary>
        /// Clears form
        /// </summary>
        public void ClearControlsList()
        {
            UserControls.Clear();
            DataControls.Clear();
        }

        private void ResentBtnClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AreYouSureMessage, AreYouSureCaption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes && ResetButtonPressed != null)
                ResetButtonPressed(this, new EventArgs());
        }
    }
}
