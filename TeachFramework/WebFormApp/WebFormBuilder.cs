using System.Collections.Generic;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    public class WebFormBuilder : IViewBuilder
    {
        private readonly TeachAppPage _page;
        //-------------------------------------------------------------
        public WebFormBuilder(TeachAppPage page)
        {
            _page = page;
        }
        
        public TeachAppPage Page
        {
            get { return _page; }
        }
        //-------------------------------------------------------------

        #region IViewBuilder Members

        public IView Build(IEnumerable<Data> data)
        {
            if(data != null)
                _page.ClearControlsList();
            SetControls(data);
            return _page;
        }

        #endregion

        private static void RegistrateControlsBuilders()
        {
            //ControlsRegistrator.Registrate(new LabelBuilder());
            //ControlsRegistrator.Registrate(new SignedTextBoxBuilder());
            //ControlsRegistrator.Registrate(new DataRadioButtonBuilder());
            //ControlsRegistrator.Registrate(new TargetFunctionBoxBuilder());
            //ControlsRegistrator.Registrate(new LimitationsAreaBuilder());
            //ControlsRegistrator.Registrate(new LppViewBuilder());
            //ControlsRegistrator.Registrate(new StepsContainerBuilder());
        }

        /// <summary>
        /// Adds controls to form
        /// </summary>
        /// <param name="controlData">Controls options</param>
        public void SetControls(IEnumerable<Data> controlData)
        {
            if (controlData == null) return;
            foreach (var data in controlData)
            {
                var control = ControlsRegistrator.ControlsBuilders[data.ControlType].Create(data);
                _page.AddControl(control);
            }
        }

        ///// <summary>
        ///// Adds controls to form
        ///// </summary>
        ///// <param name="controlData">Controls options</param>
        //public void SetControls(List<Data> controlData)
        //{
        //    //_form.ClearControlsList();
        //    //controlY = 5;
        //    if (controlData != null)
        //    {
        //        foreach (Data data in controlData)
        //        {
        //            switch (data.ControlType)
        //            {
        //                case ControlTypes.Label: AddLabel(data); break;
        //                case ControlTypes.TextBox: AddTextBox(data); break;
        //                case ControlTypes.RadioButton: AddRadioButton(data); break;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Adds label to form
        ///// </summary>
        ///// <param name="data">Label options</param>
        //public void AddLabel(Data data)
        //{
        //    var label = new UserControls.DataLabel();

        //    _page.AddControl(label, label);
            
        //    label.ID = data.Name;
        //    label.ControlName = data.Name;
        //    label.Value = data.Value;
        //}

        ///// <summary>
        ///// Adds textBox to form
        ///// </summary>
        ///// <param name="data">TextBox options</param>
        //public void AddTextBox(Data data)
        //{
        //    var textBox = new UserControls.SignedTextBox();

        //    _page.AddControl(textBox, textBox);
            
        //    textBox.ID = data.Name;
        //    textBox.ControlName = data.Name;
        //    textBox.Text = data.Text;
        //    if (!data.IsEditable)
        //        textBox.ReadOnly = true;
        //    if (!data.Check)
        //        textBox.Value = data.Value;
        //}

        ///// <summary>
        ///// Adds radioButton to form
        ///// </summary>
        ///// <param name="data">RadioButton options</param>
        //public void AddRadioButton(Data data)
        //{
        //    var button = new UserControls.DataRadioButton {ID = data.Name, ControlName = data.Name};
        //    button.SetGroupName("radioTEachApp");
        //    button.SetText(data.Text);
        //    if (!data.Check)
        //        button.Value = data.Value;
        //    _page.AddControl(button, button);
        //}
    }
}
