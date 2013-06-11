using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class DataLabelBuilder : IDataControlBuilder
    {
        #region [ IDataControlBuilder Members ]

        public string Description
        {
            get { return DataLabel.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new DataLabel(data.Name, (string)data.Value) { Width = ((string)data.Value).Length * 8 + 15 };
        }

        #endregion
    }
}
