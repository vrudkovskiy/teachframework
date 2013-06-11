namespace TeachFramework
{
    /// <summary>
    /// Control description
    /// </summary>
    public class UiDescriptionItem
    {
        /// <summary>
        /// True if isn't read only
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Object's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Some text information about control
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// True if control requires checking
        /// </summary>
        public bool CheckRequired { get; set; }

        /// <summary>
        /// Control's value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Control's type
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Additional settings
        /// </summary>
        public object ControlSettings { get; set; }
    }
}
