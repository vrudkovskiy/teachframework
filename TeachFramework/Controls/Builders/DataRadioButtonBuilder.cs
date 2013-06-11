using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class DataRadioButtonBuilder : IDataControlBuilder
    {
        #region [ IDataControlBuilder Members ]

        public string Description
        {
            get { return DataRadioButton.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new DataRadioButton(data.Name, data.Text)
            {
                Width = data.Name.Length * 10 + 30,
                Value = data.CheckRequired ? false : data.Value
            };
        }

        #endregion
    }
}
