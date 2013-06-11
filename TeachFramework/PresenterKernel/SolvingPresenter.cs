using System.Collections.Generic;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    public class SolvingPresenter : Presenter
    {
        private int _stepNumber = -1;

        private UiDescription SolvingHistory { get; set; }

        public SolvingPresenter(IViewBuilder viewBuilder, IEnumerable<IModel> models)
            : base(viewBuilder, models)
        {
            SolvingHistory = new UiDescription();
        }

        protected override UiDescription GetNextStepData(UiDescription previousStepData)
        {
            var nextStepData = new UiDescription();
            var shouldShowHistory = false;

            while (!shouldShowHistory && !CurrentModel.IsEnd)
            {
                _stepNumber++;
                nextStepData = base.GetNextStepData(previousStepData);

                foreach (var dataItem in previousStepData)
                {
                    if (SolvingHistory.Contains(dataItem.Name)) continue;

                    dataItem.Editable = false;
                    dataItem.Name = "Step" + _stepNumber + "-" + dataItem.Name;
                    SolvingHistory.Add(dataItem);
                }

                foreach (var dataItem in nextStepData)
                {
                    dataItem.CheckRequired = false;
                    var itemShouldBeShown = RequiresUserInteraction(dataItem);
                    dataItem.Editable = itemShouldBeShown;

                    if (itemShouldBeShown)
                        shouldShowHistory = true;
                }

                if (!shouldShowHistory)
                    previousStepData = new UiDescription {nextStepData};
            }

            var history = new UiDescription { SolvingHistory, nextStepData };

            if (CurrentModel.IsEnd)
                SolvingHistory.Clear();

            return history;
        }

        private static bool RequiresUserInteraction(UiDescriptionItem uiDescriptionItem)
        {
            if (uiDescriptionItem.Value == null) return true;

            var valueAsString = uiDescriptionItem.Value as string;
            return valueAsString != null && valueAsString == string.Empty;
        }
    }
}
