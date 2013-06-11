using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class SignedTextBoxBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return SignedTextBox.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new SignedTextBox(data.Name, data.Text)
            {
                ReadOnly = !data.Editable,
                Value = data.CheckRequired ? null : data.Value
            };
        }

        #endregion
    }
}
