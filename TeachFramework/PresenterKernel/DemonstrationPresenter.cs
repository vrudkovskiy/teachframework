using System.Collections.Generic;

using TeachFramework.Interfaces;

namespace TeachFramework
{
    public class DemonstrationPresenter : Presenter
    {
        public DemonstrationPresenter(IViewBuilder viewBuilder, IEnumerable<IModel> models)
            : base(viewBuilder, models)
        {
        }

        protected override UiDescription GetNextStepData(UiDescription previousStepData)
        {
            var nextStepData = base.GetNextStepData(previousStepData);
            foreach (var dataItem in nextStepData)
            {
                dataItem.CheckRequired = false;
                dataItem.Editable = dataItem.Value == null || IsValueEmptyString(dataItem.Value);
            }
            return nextStepData;
        }

        private static bool IsValueEmptyString(object value)
        {
            var valueAsString = value as string;
            return valueAsString != null && valueAsString == string.Empty;
        }
    }
}
