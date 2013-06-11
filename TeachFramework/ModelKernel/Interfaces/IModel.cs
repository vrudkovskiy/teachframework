namespace TeachFramework.Interfaces
{
    /// <summary>
    /// Interface for step models
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Gets model's title
        /// </summary>
        string GetDescription();

        /// <summary>
        /// Executes one model's step
        /// </summary>
        UiDescription ExecuteStep(UiDescription userData);

        /// <summary>
        /// Returns true if all steps have been done
        /// </summary>
        bool IsEnd { get; }

        /// <summary>
        /// Sets initial settings
        /// </summary>
        void Reset();
    }
}
