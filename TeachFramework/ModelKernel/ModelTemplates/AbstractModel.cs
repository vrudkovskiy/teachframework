using System.Collections.Generic;

using TeachFramework.Exceptions;
using TeachFramework.Interfaces;

namespace TeachFramework
{
    /// <summary>
    /// Basic class for static models
    /// </summary>
    public abstract class AbstractModel : IModel
    {
        /// <summary>
        /// One step
        /// </summary>
        protected delegate UiDescription Step(UiDescription userData);

        /// <summary>
        /// Collection of all model's steps
        /// </summary>
        protected readonly Dictionary<int, Step> Steps = new Dictionary<int, Step>();
        private int _currentStepIndex;

        /// <summary>
        /// Model's title
        /// </summary>
        protected string Description;
        //-----------------------------------------------------------------
        #region [ IModel Members ]

        /// <summary>
        /// Gets model's title
        /// </summary>
        public string GetDescription()
        {
            if (Description == null)
                return "Not named " + GetHashCode();
            return Description;
        }

        /// <summary>
        /// Executes one step from model
        /// </summary>
        public UiDescription ExecuteStep(UiDescription userData)
        {
            if (_currentStepIndex == 0)
                IsEnd = false;

            var data = new UiDescription();
            if (Steps == null || Steps.Count == 0)
            {
                IsEnd = true;
                return data;
            }

            var currentStep = Steps[_currentStepIndex];
            try
            {
                data = currentStep(userData);
            }
            catch (NoSuchDataException exception)
            {
                throw new InvalidModelException(exception.Message);
            }
            _currentStepIndex++;

            if (_currentStepIndex == Steps.Count)
            {
                IsEnd = true;
                _currentStepIndex = 0;
            }
            return data;
        }

        /// <summary>
        /// Returns true if all steps have been done
        /// </summary>
        public bool IsEnd { get; protected set; }

        /// <summary>
        /// Sets initial settings
        /// </summary>
        public void Reset()
        {
            IsEnd = false;
            _currentStepIndex = 0;
        }

        #endregion
    }
}
