using System;

namespace TeachFramework.Exceptions
{
    public class InvalidControlDataValueException: Exception
    {
        public InvalidControlDataValueException(string controlName)
            :base("У елемент управління під ім'ям '" + controlName + "' були передані несумісні з ним дані")
        {}
    }
}
