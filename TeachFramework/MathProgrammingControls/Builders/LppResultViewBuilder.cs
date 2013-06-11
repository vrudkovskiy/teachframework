using System.Collections.Generic;
using TeachFramework.Interfaces;

namespace TeachFramework.Controls
{
    public class LppResultViewBuilder : IDataControlBuilder
    {
        #region IDataControlBuilder Members

        public string Description
        {
            get { return LppResultView.ControlDescription; }
        }

        public IDataControl Create(UiDescriptionItem data)
        {
            var resultView = new LppResultView
            {
                Value = data.CheckRequired ? null : data.Value,
                ControlName = data.Name,
                ReadOnly = !data.Editable
            };
            if (data.CheckRequired)
                resultView.SetVariables((IEnumerable<string>)data.ControlSettings);
            return resultView;
        }

        #endregion
    }
}
