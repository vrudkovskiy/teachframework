using TeachFramework.Exceptions;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    /// <summary>
    /// Basic class for dynemic models
    /// </summary>
    public abstract class AbstractDynamicModel : AbstractModel, IModel
    {
        private Step _initialStep;
        //----------------------------------------------------------------
        /// <summary>
        /// Executes one step from model
        /// </summary>
        public new UiDescription ExecuteStep(UiDescription userData)
        {
            var data = new UiDescription();

            if (IsEnd) return data;

            var currentStep = Steps[0];
            try
            {
                data = currentStep(userData);
            }
            catch (NoSuchDataException exception)
            {
                throw new InvalidModelException(exception.Message);
            }
            return data;
        }

        /// <summary>
        /// Sets initial model's step
        /// </summary>
        /// <param name="step"></param>
        protected void SetStartStep(Step step)
        {
            SetNextStep(step);
        }

        /// <summary>
        /// Sets next model's step
        /// </summary>
        /// <param name="step"></param>
        protected void SetNextStep(Step step)
        {
            if (_initialStep == null)
                _initialStep = step;
            Steps.Clear();
            Steps.Add(0, step);
        }

        /// <summary>
        /// Sets initial settings
        /// </summary>
        public new void Reset()
        {
            base.Reset();
            SetNextStep(_initialStep);
        }
    }
}
