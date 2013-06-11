using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class LppViewBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return LppView.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new LppView
            {
                ReadOnly = !data.Editable,
                ControlName = data.Name,
                Value = data.CheckRequired ? null : data.Value
            };
        }

        #endregion
    }
}
