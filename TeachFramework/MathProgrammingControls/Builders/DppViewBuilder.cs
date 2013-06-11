using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class DppViewBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return DppView.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new DppView
            {
                ReadOnly = !data.Editable,
                ControlName = data.Name,
                Value = data.CheckRequired ? null : data.Value
            };
        }

        #endregion
    }
}
