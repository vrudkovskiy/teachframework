using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class TargetFunctionBoxBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return TargetFunctionBox.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new TargetFunctionBox(data.Name)
            {
                ReadOnly = !data.Editable,
                Value = data.CheckRequired ? null : data.Value
            };
        }

        #endregion
    }
}
