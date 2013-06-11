using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class LimitationsAreaBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return LimitationsArea.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new LimitationsArea
            {
                ReadOnly = !data.Editable,
                ControlName = data.Name,
                Value = data.CheckRequired ? null : data.Value
            };
        }

        #endregion
    }
}
