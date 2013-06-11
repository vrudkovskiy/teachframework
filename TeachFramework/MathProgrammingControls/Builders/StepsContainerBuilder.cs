using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class StepsContainerBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return StepsContainer.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            return new StepsContainer(data.Text)
            {
                ReadOnly = !data.Editable,
                ControlName = data.Name,
                Value = data.CheckRequired ? null : data.Value
            };
        }

        #endregion
    }
}
