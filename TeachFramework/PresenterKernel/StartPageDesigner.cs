using System.Collections.Generic;

using TeachFramework.Interfaces;

namespace TeachFramework
{
    public class StartPageDesigner
    {
        public UiDescription Design(IEnumerable<IModel> models)
        {
            var data = new UiDescription();
            foreach (var model in models)
                data.Add("DataRadioButton", model.GetDescription(), model.GetDescription(), false);
            return data;
        }
    }
}
