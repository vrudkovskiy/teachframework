using System;

namespace TeachFramework.Exceptions
{
    /// <summary>
    /// Occures when error was in model
    /// </summary>
    public class InvalidModelException : Exception
    {
        /// <summary>
        /// Represents InvalidModelException
        /// </summary>
        /// <param name="message"></param>
        public InvalidModelException(string message)
            : base(message)
        { }
    }
}
