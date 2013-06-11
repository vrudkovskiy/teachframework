using System;

namespace TeachFramework.Exceptions
{
    public class ControlDescriptionDuplicateException : Exception
    {
        public ControlDescriptionDuplicateException() { }

        public ControlDescriptionDuplicateException(string message)
            : base(message)
        { }
    }
}
